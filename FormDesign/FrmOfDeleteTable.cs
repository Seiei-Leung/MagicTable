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
    public partial class FrmOfDeleteTable : Form
    {
        public FrmOfDeleteTable()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确定删除表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string nameOfTable = this.textBox1.Text.Trim();
            if (nameOfTable.Equals("")) {
                MessageBox.Show("请输入表格名称");
                return;
            }
            DataTable resultOfFields = SqlHandle.Common.sqlToDataTable1("select COLUMN_NAME, DATA_TYPE, COLUMN_DEFAULT,CHARACTER_MAXIMUM_LENGTH from information_schema.columns where table_name='" +  nameOfTable + "'");
            if (resultOfFields.Rows.Count > 0)
            {
                string sql = "drop table " + nameOfTable;
                SqlHandle.Common.sqlToDataTable1(sql);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("当前表格不存在");
            }

          


        }
    }
}
