using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormDesign
{
    public partial class FrmOfSetModel : Form
    {

        private int idOfModel;
        /// <summary>
        /// 新增模块窗口
        /// </summary>
        public FrmOfSetModel()
        {
            InitializeComponent();
            this.radioButton1.Checked = true;
            this.Name = "新增模块";
            // 绑定事件
            this.button1.Click += new System.EventHandler(this.addModel);
        }

        /// <summary>
        /// 修改模块窗口
        /// </summary>
        /// <param name="nameOfModel"></param>
        public FrmOfSetModel(int id)
        {
            InitializeComponent();
            this.Name = "修改模型";
            // 填充数据
            DataTable dt = SqlHandle.Common.sqlToDataTable1("select id, titleOfModel, classNameOfModel, nameOfMainTable, nameOfDetailTable, typeOfModel, relationField from MsgOfModel where id = '" + id + "'");
            this.textBox1.Text = dt.Rows[0]["titleOfModel"].ToString();
            this.textBox2.Text = dt.Rows[0]["classNameOfModel"].ToString();
            this.textBox3.Text = dt.Rows[0]["nameOfMainTable"].ToString();
            this.textBox4.Text = dt.Rows[0]["nameOfDetailTable"].ToString();
            this.textBox5.Text = dt.Rows[0]["relationField"].ToString();
            this.radioButton1.Checked = (bool)dt.Rows[0]["typeOfModel"];
            this.radioButton2.Checked = !this.radioButton1.Checked;
            this.idOfModel = id;
            this.button1.Click += new System.EventHandler(this.modifyModel);
        }


        /// <summary>
        /// 提交按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addModel(object sender, EventArgs e)
        {
            string titleOfModel = this.textBox1.Text.Trim();
            string classNameOfModel = this.textBox2.Text.Trim();
            string nameOfMainTable = this.textBox3.Text.Trim();
            string nameOfDetailTable = this.textBox4.Text.Trim();
            bool typeOfModel = this.radioButton1.Checked;
            string relationField = this.textBox5.Text.Trim();

            // 检测数据完整性
            if (titleOfModel.Equals("")) {
                MessageBox.Show("模块名称不能为空");
                return;
            }
            if (classNameOfModel.Equals(""))
            {
                MessageBox.Show("模块实例名不能为空");
                return;
            }
            if (nameOfMainTable.Equals(""))
            {
                MessageBox.Show("主档表名不能为空");
                return;
            }
            if (typeOfModel && nameOfDetailTable.Equals(""))
            {
                MessageBox.Show("模块样式为主从，此时从档表名不能为空");
                return;
            }
            int count = (int) SqlHandle.Common.sqlToDataTable1("select count(titleOfModel) count from MsgOfModel where titleOfModel = '" + titleOfModel + "'").Rows[0]["count"];
            if (count > 0)
            {
                MessageBox.Show("该模块已经存在");
                return;
            }
            // 组装 sql
            DateTime time = DateTime.Now;
            string sql = "insert into MsgOfModel (titleOfModel, classNameOfModel, nameOfMainTable, nameOfDetailTable, typeOfModel, relationField, create_time, update_time) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')";
            sql = string.Format(sql, titleOfModel, classNameOfModel, nameOfMainTable, nameOfDetailTable, typeOfModel, relationField, time.ToString(), time.ToString());
            try
            {
                SqlHandle.Common.sqlToDataTable1(sql);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
                        
        }


        /// <summary>
        /// 修改模块按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modifyModel(object sender, EventArgs e)
        {
            string titleOfModel = this.textBox1.Text.Trim();
            string classNameOfModel = this.textBox2.Text.Trim();
            string nameOfMainTable = this.textBox3.Text.Trim();
            string nameOfDetailTable = this.textBox4.Text.Trim();
            bool typeOfModel = this.radioButton1.Checked;
            string relationField = this.textBox5.Text.Trim();

            // 检测数据完整性
            if (titleOfModel.Equals(""))
            {
                MessageBox.Show("模块名称不能为空");
                return;
            }
            if (classNameOfModel.Equals(""))
            {
                MessageBox.Show("模块实例名不能为空");
                return;
            }
            if (nameOfMainTable.Equals(""))
            {
                MessageBox.Show("主档表名不能为空");
                return;
            }
            if (typeOfModel && nameOfDetailTable.Equals(""))
            {
                MessageBox.Show("模块样式为主从，此时从档表名不能为空");
                return;
            }

            // 组装 sql
            DateTime time = DateTime.Now;
            string sql = "update MsgOfModel set titleOfModel = '{0}', classNameOfModel = '{1}', nameOfMainTable = '{2}', nameOfDetailTable = '{3}', typeOfModel = '{4}', relationField = '{5}', update_time = '{6}' where id = '" + idOfModel + "'";
            sql = string.Format(sql, titleOfModel, classNameOfModel, nameOfMainTable, nameOfDetailTable, typeOfModel, relationField, time.ToString());
            try
            {
                SqlHandle.Common.sqlToDataTable1(sql);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
