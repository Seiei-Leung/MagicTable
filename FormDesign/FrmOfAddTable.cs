using FormDesign.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.CellTypes;
using unvell.ReoGrid.Events;

namespace FormDesign
{
    public partial class FrmOfAddTable : Form
    {

        private string nameOfTable;
        private Worksheet sheet;
        public int activedRowNum;

        public FrmOfAddTable()
        {
            InitializeComponent();
            var grid = this.reoGridControl1;
            sheet = grid.CurrentWorksheet;
            sheet.Resize(1, 4);
            sheet.ColumnHeaders[0].Text = "字段";
            sheet.ColumnHeaders[0].Width = 100;
            sheet.ColumnHeaders[1].Text = "类型";
            sheet.ColumnHeaders[1].Width = 100;
            sheet.ColumnHeaders[2].Text = "主键";
            sheet.ColumnHeaders[2].Width = 100;
            sheet.ColumnHeaders[2].DefaultCellBody = typeof(CheckBoxCell);
            sheet.ColumnHeaders[3].Text = "默认值";
            sheet.ColumnHeaders[3].Width = 100;

            // 添加右键事件
            this.sheet.CellMouseDown += rightClickOfCell;
        }

        /// <summary>
        /// 提交按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            nameOfTable = this.textBox1.Text.Trim();
            if (nameOfTable.Equals(""))
            {
                MessageBox.Show("没有设置表名");
                return;
            }
            List<FieldOfTable> listOfFieldOfTable = new List<FieldOfTable>();
            bool isHasPrimaryKey = false;
            bool isError = false;
            
            for (var rowIndex = 0; rowIndex < this.sheet.RowCount; rowIndex++)
            {
                if (this.sheet[rowIndex, 0] == null)
                {
                    break;
                }
                if (this.sheet[rowIndex, 1] == null)
                {
                    MessageBox.Show("类型值不能为空！");
                    isError = true;
                    break;
                }
                FieldOfTable fieldOfTable = new FieldOfTable();
                fieldOfTable.nameOfField = this.sheet[rowIndex, 0].ToString();
                fieldOfTable.typeOfField = this.sheet[rowIndex, 1].ToString();
                if (this.sheet[rowIndex, 2] != null && (bool) this.sheet[rowIndex, 2]) {
                    if (isHasPrimaryKey)
                    {
                        MessageBox.Show("不能有两个主键");
                        isError = true;
                        break;
                    }
                    isHasPrimaryKey = true;
                    fieldOfTable.isPrimaryKey = true;
                }
                fieldOfTable.defaultValue = this.sheet[rowIndex, 3] == null ? "null" : "'" + this.sheet[rowIndex, 3].ToString() + "'";
                listOfFieldOfTable.Add(fieldOfTable);
            }
            if (isError)
            {
                return;
            }
            if (!isHasPrimaryKey)
            {
                MessageBox.Show("没有设置主键");
                return;
            }
            // 组装 sql 语句
            string sql = "create table " + nameOfTable + "(";
            foreach (var item in listOfFieldOfTable)
            {
                sql += item.getSQL() + ",";
            }
            sql += ")";
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
        /// 右键按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rightClickOfCell(object sender, CellMouseEventArgs e)
        {
            if (e.Buttons == unvell.ReoGrid.Interaction.MouseButtons.Left)
            {
                return;
            }
            ContextMenuStrip ms = new ContextMenuStrip();

            var cell = sheet.CreateAndGetCell(e.CellPosition);
            activedRowNum = cell.Row;

            ms.Items.Add("新增行");
            ms.Items.Add("删除行");
            ms.Items[0].Click += new EventHandler(insertRow);
            ms.Items[1].Click += new EventHandler(delRow);
            ms.Show(MousePosition.X, MousePosition.Y);
        }

        /// <summary>
        /// 新增行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insertRow(object sender, EventArgs e)
        {
            this.sheet.InsertRows(this.sheet.RowCount, 1);
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delRow(object sender, EventArgs e)
        {
            sheet.DeleteRows(sheet.FocusPos.Row, 1);
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


    
}
