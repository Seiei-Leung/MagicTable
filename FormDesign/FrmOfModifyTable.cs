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
    public partial class FrmOfModifyTable : Form
    {
        private string tableName;
        private Worksheet sheet;
        private int activedRowNum;
        private List<FieldOfTable> sourceListOfFields = new List<FieldOfTable>();
        private List<int> listOfModifyIndex = new List<int>();

        public FrmOfModifyTable()
        {
            InitializeComponent();
            var grid = this.reoGridControl1;
            sheet = grid.CurrentWorksheet;
            // 添加右键事件
            sheet.CellMouseDown += rightClickOfCell;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void reload()
        {

            // 删除修改事件
            sheet.BeforeCellEdit -= BeforeCellEdit;
            sourceListOfFields = new List<FieldOfTable>();
            listOfModifyIndex = new List<int>();
            
            // 获取数据表结构
            try
            {
                DataTable resultOfFields = SqlHandle.Common.sqlToDataTable1("select COLUMN_NAME, DATA_TYPE, COLUMN_DEFAULT,CHARACTER_MAXIMUM_LENGTH from information_schema.columns where table_name='" + tableName + "'");
                DataTable resultOfPrimayKey = SqlHandle.Common.sqlToDataTable1("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME='" + tableName + "'");
                string nameOfPrimaryKey = resultOfPrimayKey.Rows[0][0].ToString();
                List<FieldOfTable> listOfFieldOfTable = new List<FieldOfTable>();
                foreach (DataRow rowItem in resultOfFields.Rows)
                {
                    FieldOfTable fieldOfTable = new FieldOfTable();
                    fieldOfTable.nameOfField = rowItem[0].ToString();
                    fieldOfTable.typeOfField = rowItem[1].ToString();
                    fieldOfTable.defaultValue = rowItem[2].ToString().Replace("('", "").Replace("')", "");
                    if (rowItem[1].ToString().Equals("varchar"))
                    {
                        fieldOfTable.typeOfField += "(" + rowItem[3].ToString() + ")";
                    }
                    if (fieldOfTable.nameOfField.Equals(nameOfPrimaryKey))
                    {
                        fieldOfTable.isPrimaryKey = true;
                    }
                    listOfFieldOfTable.Add(fieldOfTable);
                }

                // 定义到全局变量中
                sourceListOfFields = listOfFieldOfTable;

                // 确定范围
                sheet.Resize(listOfFieldOfTable.Count, 4);
                sheet.ColumnHeaders[0].Text = "字段";
                sheet.ColumnHeaders[0].Width = 100;
                sheet.ColumnHeaders[1].Text = "类型";
                sheet.ColumnHeaders[1].Width = 100;
                sheet.ColumnHeaders[2].Text = "主键";
                sheet.ColumnHeaders[2].Width = 100;
                sheet.ColumnHeaders[2].DefaultCellBody = typeof(CheckBoxCell);
                sheet.ColumnHeaders[3].Text = "默认值";
                sheet.ColumnHeaders[3].Width = 100;

                // 填充数据
                for (var indexOfListOfFieldOfTable = 0; indexOfListOfFieldOfTable < listOfFieldOfTable.Count; indexOfListOfFieldOfTable++)
                {
                    FieldOfTable fieldOfTable = listOfFieldOfTable[indexOfListOfFieldOfTable];
                    if (fieldOfTable.isPrimaryKey)
                    {
                        this.sheet.Cells[indexOfListOfFieldOfTable, 0].Style.BackColor = Color.LightGray;
                        this.sheet.Cells[indexOfListOfFieldOfTable, 1].Style.BackColor = Color.LightGray;
                        this.sheet.Cells[indexOfListOfFieldOfTable, 2].Style.BackColor = Color.LightGray;
                        this.sheet.Cells[indexOfListOfFieldOfTable, 3].Style.BackColor = Color.LightGray;
                        this.sheet.Cells[indexOfListOfFieldOfTable, 0].IsReadOnly = true;
                        this.sheet.Cells[indexOfListOfFieldOfTable, 1].IsReadOnly = true;
                        this.sheet.Cells[indexOfListOfFieldOfTable, 2].IsReadOnly = true;
                        this.sheet.Cells[indexOfListOfFieldOfTable, 3].IsReadOnly = true;
                        
                    }
                    sheet[indexOfListOfFieldOfTable, 0] = fieldOfTable.nameOfField;
                    sheet[indexOfListOfFieldOfTable, 1] = fieldOfTable.typeOfField;
                    if (fieldOfTable.isPrimaryKey)
                    {
                        sheet[indexOfListOfFieldOfTable, 2] = true;
                    }
                    sheet[indexOfListOfFieldOfTable, 3] = fieldOfTable.defaultValue;
                }
                // 绑定修改事件
                sheet.BeforeCellEdit += BeforeCellEdit;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        /// <summary>
        /// 单元格修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeforeCellEdit(object sender, CellEventArgs e)
        {
            Cell cell = e.Cell;
            if (cell.Row < sourceListOfFields.Count)
            {
                if (sourceListOfFields[cell.Row].isPrimaryKey)
                {   
                    return;
                }
            }
            bool isExsit = false; 
            for (var i = 0; i < listOfModifyIndex.Count; i++)
            {
                if (listOfModifyIndex[i] == cell.Row)
                {
                    isExsit = true;
                    break;
                }
            }
            if (!isExsit)
            {
                listOfModifyIndex.Add(cell.Row);
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
            if (sheet.FocusPos.Row >= sourceListOfFields.Count)
            {
                sheet.DeleteRows(sheet.FocusPos.Row, 1);
                return;
            }
            if (sourceListOfFields[sheet.FocusPos.Row].isPrimaryKey)
            {
                MessageBox.Show("主键不能删除");
                return;
            }
            
            string nameOfField = sourceListOfFields[sheet.FocusPos.Row].nameOfField;
            
            string sql = getDelSQL(nameOfField);
            if (sourceListOfFields[sheet.FocusPos.Row].defaultValue == null || sourceListOfFields[sheet.FocusPos.Row].defaultValue.Trim().Equals(""))
            {
                sql = "Alter Table " + tableName + " Drop Column " + nameOfField;
            }

            try
            {
                SqlHandle.Common.sqlToDataTable1(sql);
                reload();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        /// <summary>
        /// 提交按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            bool isSuccess = true;
            for (var i = 0; i < listOfModifyIndex.Count; i++)
            {
                var indexOfRow = listOfModifyIndex[i];
                // 如果这是修改表结构
                if (indexOfRow < sourceListOfFields.Count)
                {
                    try
                    {
                        string sql = getDelSQL(sourceListOfFields[indexOfRow].nameOfField);
                        if (sourceListOfFields[indexOfRow].defaultValue == null)
                        {
                            sql = "Alter Table " + tableName + " Drop Column " + sourceListOfFields[indexOfRow].nameOfField;
                            MessageBox.Show(sql);
                        }
                        SqlHandle.Common.sqlToDataTable1(sql);
                        sql = "alter table " + tableName + " add " + this.sheet[indexOfRow, 0] + " " + this.sheet[indexOfRow, 1];
                        if (this.sheet[indexOfRow, 3] != null)
                        {

                            string defaultValue = this.sheet[indexOfRow, 3].ToString();
                            sql += " default '" + defaultValue + "'";
                        }
                        //MessageBox.Show(sql);
                        SqlHandle.Common.sqlToDataTable1(sql);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                        string sql = "alter table " + tableName + " add " + sourceListOfFields[indexOfRow].nameOfField + " " + sourceListOfFields[indexOfRow].typeOfField;
                        if (this.sourceListOfFields[indexOfRow].defaultValue != null)
                        {
                            sql += " default '" + this.sourceListOfFields[indexOfRow].defaultValue + "'";
                        }
                        //MessageBox.Show(sql);
                        SqlHandle.Common.sqlToDataTable1(sql);
                        isSuccess = false;
                        break;
                    }
                }
                // 新增属性
                else
                {
                    try
                    {
                        string sql = "alter table " + tableName + " add " + this.sheet[indexOfRow, 0] + " " + this.sheet[indexOfRow, 1];
                        if (this.sheet[indexOfRow, 3] != null)
                        {
                            sql += " default '" + this.sheet[indexOfRow, 3].ToString() + "'";
                        }
                        //MessageBox.Show(sql);
                        SqlHandle.Common.sqlToDataTable1(sql);
                    }
                    catch (Exception error)
                    {
                        isSuccess = false;
                        MessageBox.Show(error.Message);
                        break;
                    }
                }
            }
            // 重新刷新数据
            reload();
            if (isSuccess)
            {
                MessageBox.Show("修改成功");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOfField"></param>
        /// <returns></returns>
        private string getDelSQL(string nameOfField)
        {
            string sql = @"
                declare @name varchar(8000)
                select @name=b.name from syscolumns a,sysobjects b where a.id=object_id('" + tableName + @"') and b.id=a.cdefault and a.name='" + nameOfField + @"' and b.name like 'DF%'
                exec('alter table " + tableName + @" drop constraint '+@name)
                Alter Table " + tableName + " Drop Column " + nameOfField;
            return sql;
        }

        /// <summary>
        /// 选择修改表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string nameOfTable = this.textBox1.Text.Trim();
            if (nameOfTable.Equals(""))
            {
                MessageBox.Show("请输入表名");
            }
            this.tableName = nameOfTable;

            // 初始化数据
            reload();
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
