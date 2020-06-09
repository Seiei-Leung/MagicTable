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
    public partial class FrmOfColumnProperty : Form
    {

        private Worksheet sheet;
        private string currentTableName;
        private int idOfModel;
        private List<string> listOfTableName = new List<string>();
        private int activedRowNum;

        public FrmOfColumnProperty(int idOfModel, List<string> listOfTableName)
        {
            InitializeComponent();

            var grid = this.reoGridControl1;
            sheet = grid.CurrentWorksheet;
            // 获取下拉表格列表
            this.idOfModel = idOfModel;
            this.listOfTableName = listOfTableName;
            foreach (string tableName in listOfTableName)
            {
                this.comboBox1.Items.Add(tableName);
            }

            resetSheet(1);
            currentTableName = "";
            // 添加右键事件
            sheet.CellMouseDown += rightClickOfCell;
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
            if (this.sheet.Cells[activedRowNum, 1].Data == null)
            {
                sheet.DeleteRows(sheet.FocusPos.Row, 1);
                return;
            }
            string nameOfField = this.sheet.Cells[activedRowNum, 1].Data.ToString();

            string sql = "DELETE FROM PropertyOfColumn where nameOfTable = '" + currentTableName + "' and field = '" + nameOfField + "'";
            SqlHandle.Common.sqlToDataTable1(sql);
            // 不强行添加行数，当表格的行数比原来的行数少一行的时候会有 bug
            sheet.InsertRows(this.sheet.RowCount, 100);
            resetSheet(1);
            reload();
        }

        /// <summary>
        /// 获取表的属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 检测数据库是否存在该表
            currentTableName = this.comboBox1.SelectedItem.ToString();
            if (currentTableName.Equals(""))
            {
                MessageBox.Show("请输入表名");
                return;
            }

            // 不强行添加 100 行的时候会有 bug
            sheet.InsertRows(this.sheet.RowCount, 100);
            resetSheet(1);
            DataTable resultForIsExist = SqlHandle.Common.sqlToDataTable1("select COLUMN_NAME, DATA_TYPE, COLUMN_DEFAULT,CHARACTER_MAXIMUM_LENGTH from information_schema.columns where table_name='" + currentTableName + "'");
            if (resultForIsExist.Rows.Count == 0)
            {
                MessageBox.Show("数据库没有该表");
                return;
            }
            reload();
        }

        /// <summary>
        /// 提交修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentTableName.Equals(""))
            {
                MessageBox.Show("需要先选择表");
            }
            for (int i = 0; i < sheet.Rows; i++)
            {
                string sql = "select Count(id) count from PropertyOfColumn where nameOfTable = '" + currentTableName + "' and field = '" + sheet.Cells[i, 1].Data.ToString() + "'";
                DataTable resultOfCount = SqlHandle.Common.sqlToDataTable1(sql);
                string nameOfField = sheet.Cells[i, 1].Data != null ? sheet.Cells[i, 1].Data.ToString() : "";
                string title = sheet.Cells[i, 0].Data != null ? sheet.Cells[i, 0].Data.ToString() : "";
                string width = sheet.Cells[i, 2].Data != null ? sheet.Cells[i, 2].Data.ToString() : "100";
                string isFreezed = sheet.Cells[i, 3].Data != null ? sheet.Cells[i, 3].Data.ToString() : "";
                isFreezed = isFreezed == "True" ? "1" : "0";
                string formula = sheet.Cells[i, 4].Data != null ? sheet.Cells[i, 4].Data.ToString() : "";
                string defaultValue = sheet.Cells[i, 5].Data != null ? sheet.Cells[i, 5].Data.ToString() : "";
                string isReadOnly = sheet.Cells[i, 6].Data != null ? sheet.Cells[i, 6].Data.ToString() : "";
                isReadOnly = isReadOnly == "True" ? "1" : "0";
                string isShow = sheet.Cells[i, 7].Data != null ? sheet.Cells[i, 7].Data.ToString() : "";
                isShow = isShow == "True" ? "1" : "0";
                DateTime now = DateTime.Now;

                // 如果字段为空，则忽略
                if (nameOfField.Equals(""))
                {
                    break;
                }
                // 更新数据
                if ((int)resultOfCount.Rows[0]["count"] == 1)
                {
                    sql = "update PropertyOfColumn set title = '" + title + "', width = '" + width + "', isFreezed = '" + isFreezed + "', formula = '" + formula + "', defaultValue = '" + defaultValue + "', isReadOnly = '" + isReadOnly + "', isShow = '" + isShow + "', update_time = '" + now.ToString() + "' where nameOfTable ='" + currentTableName + "' and field = '" + nameOfField + "'";
                    SqlHandle.Common.sqlToDataTable1(sql);
                }
                // 新增数据
                else
                {
                    sql = "insert into PropertyOfColumn (idOfModel, nameOfTable, title, field, width, isFreezed, formula, defaultValue, isReadOnly, isShow, create_time, update_time) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}')";
                    sql = string.Format(sql, idOfModel, currentTableName, title, nameOfField, width, isFreezed, formula, defaultValue, isReadOnly, isShow, now.ToString(), now.ToString());
                    SqlHandle.Common.sqlToDataTable1(sql);
                }
                
            }
            MessageBox.Show("提交成功");
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void reload()
        {

            // 获取数据
            string sql = "select id, idOfModel, nameOfTable, title, field, width, isFreezed, formula, defaultValue, isReadOnly, isShow from PropertyOfColumn where nameOfTable = '" + currentTableName + "'";
            DataTable DataTableOfpropertyOfColumn = SqlHandle.Common.sqlToDataTable1(sql);
            // 如果当前没有数据
            if (DataTableOfpropertyOfColumn.Rows.Count == 0)
            {
                sql = "select COLUMN_NAME from information_schema.columns where table_name='" + currentTableName + "'";
                DataTable resultOfFields = SqlHandle.Common.sqlToDataTable1(sql);
                this.resetSheet(resultOfFields.Rows.Count);
                for (int i = 0; i < resultOfFields.Rows.Count; i++)
                {
                    sheet.Cells[i, 1].Data = resultOfFields.Rows[i][0].ToString();
                    sheet.Cells[i, 1].IsReadOnly = true;
                    sheet.Cells[i, 2].Data = "100";
                    sheet.Cells[i, 3].Data = false;
                    sheet.Cells[i, 6].Data = true;
                    sheet.Cells[i, 7].Data = true;
                    
                }
            }
            // 填充数据
            else
            {
                this.resetSheet(DataTableOfpropertyOfColumn.Rows.Count);
                for (var i = 0; i < DataTableOfpropertyOfColumn.Rows.Count; i++)
                {
                    sheet.Cells[i, 0].Data = DataTableOfpropertyOfColumn.Rows[i]["title"];
                    sheet.Cells[i, 1].Data = DataTableOfpropertyOfColumn.Rows[i]["field"];
                    sheet.Cells[i, 1].IsReadOnly = true;
                    sheet.Cells[i, 2].Data = DataTableOfpropertyOfColumn.Rows[i]["width"];
                    sheet.Cells[i, 3].Data = (bool)DataTableOfpropertyOfColumn.Rows[i]["isFreezed"];
                    sheet.Cells[i, 4].Data = DataTableOfpropertyOfColumn.Rows[i]["formula"];
                    sheet.Cells[i, 5].Data = DataTableOfpropertyOfColumn.Rows[i]["defaultValue"];
                    sheet.Cells[i, 6].Data = (bool)DataTableOfpropertyOfColumn.Rows[i]["isReadOnly"];
                    sheet.Cells[i, 7].Data = (bool)DataTableOfpropertyOfColumn.Rows[i]["isShow"];

                }
            }
        }


        /// <summary>
        /// 刷新表格格式
        /// </summary>
        private void resetSheet(int countOfColumn)
        {
            sheet.Reset();
            sheet.Resize(countOfColumn, 8);
            sheet.ColumnHeaders[0].Text = "标题";
            sheet.ColumnHeaders[0].Width = 100;
            sheet.ColumnHeaders[1].Text = "列名";
            sheet.ColumnHeaders[1].Width = 100;
            sheet.ColumnHeaders[2].Text = "宽度";
            sheet.ColumnHeaders[2].Width = 100;
            sheet.ColumnHeaders[3].Text = "冻结";
            sheet.ColumnHeaders[3].Width = 100;
            sheet.ColumnHeaders[3].DefaultCellBody = typeof(CheckBoxCell);
            sheet.ColumnHeaders[4].Text = "公式";
            sheet.ColumnHeaders[4].Width = 100;
            sheet.ColumnHeaders[5].Text = "默认值";
            sheet.ColumnHeaders[5].Width = 100;
            sheet.ColumnHeaders[6].Text = "只读";
            sheet.ColumnHeaders[6].Width = 100;
            sheet.ColumnHeaders[6].DefaultCellBody = typeof(CheckBoxCell);
            sheet.ColumnHeaders[7].Text = "是否显示";
            sheet.ColumnHeaders[7].Width = 100;
            sheet.ColumnHeaders[7].DefaultCellBody = typeof(CheckBoxCell);
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
