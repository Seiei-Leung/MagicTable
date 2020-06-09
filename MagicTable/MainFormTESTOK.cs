using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using unvell.ReoGrid;
using FormDesign;
using FormDesign.common;


namespace MagicTable
{
    public partial class MainFormTESTOK : RibbonForm
    {

        List<MsgOfModel> listOfMsgOfModel = new List<MsgOfModel>();
        List<PropertyOfColumn> listOfPropertyOfColumn = new List<PropertyOfColumn>();
        private Worksheet sheet;

        //Worksheet wooksheet;
        public MainFormTESTOK()
        {
            InitializeComponent();
            InitLists();
            StartPosition = FormStartPosition.WindowsDefaultBounds;

            var grid = this.reoGridControl1;
            sheet = grid.CurrentWorksheet;

            // ��ȡģ���е�����
            this.reloadTabControlAndReoGrid();

        }

        private Color GetRandomColor(Random r)
        {
            if (r == null)
            {
                r = new Random(DateTime.Now.Millisecond);
            }

            return Color.FromKnownColor((KnownColor)r.Next(1, 150));
        }

        private void InitLists()
        {
            Image[] images = new Image[255];
            RibbonProfessionalRenderer rend = new RibbonProfessionalRenderer();
            BackColor = rend.ColorTable.RibbonBackground;
            Random r = new Random();

            #region Color Squares
            using (GraphicsPath path = RibbonProfessionalRenderer.RoundRectangle(new Rectangle(3, 3, 26, 26), 4))
            {
                using (GraphicsPath outer = RibbonProfessionalRenderer.RoundRectangle(new Rectangle(0, 0, 32, 32), 4))
                {
                    for (int i = 0; i < images.Length; i++)
                    {
                        Bitmap b = new Bitmap(32, 32);

                        using (Graphics g = Graphics.FromImage(b))
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;

                            using (SolidBrush br = new SolidBrush(Color.FromArgb(255, i * (255 / images.Length), 0)))
                            {
                                g.FillPath(br, path);
                            }

                            using (Pen p = new Pen(Color.White, 3))
                            {
                                g.DrawPath(p, path);
                            }

                            g.DrawPath(Pens.Wheat, path);

                            g.DrawString(Convert.ToString(i + 1), Font, Brushes.White, new Point(10, 10));
                        }

                        images[i] = b;

                        RibbonButton btn = new RibbonButton();
                        btn.Image = b;
                        lst.Buttons.Add(btn);
                    }
                }
            }

            //lst.DropDownItems.Add(new RibbonSeparator("Available styles"));
            RibbonButtonList lst2 = new RibbonButtonList();

            for (int i = 0; i < images.Length; i++)
            {
                RibbonButton btn = new RibbonButton();
                btn.Image = images[i];
                lst2.Buttons.Add(btn);
            }
            lst.DropDownItems.Add(lst2);
            lst.DropDownItems.Add(new RibbonButton("Save selection as a new quick style..."));
            lst.DropDownItems.Add(new RibbonButton("Erase Format"));
            lst.DropDownItems.Add(new RibbonButton("Apply style...")); 
            #endregion

            #region Theme Colors

            RibbonButton[] buttons = new RibbonButton[30];
            int square = 16;
            int squares = 4;
            int sqspace = 2;

            for (int i = 0; i < buttons.Length; i++)
            {
                #region Create color squares
                Bitmap colors = new Bitmap((square + sqspace) * squares, square + 1);
                string[] colorss = new string[squares];
                using (Graphics g = Graphics.FromImage(colors))
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Color sqcolor = GetRandomColor(r);
                        colorss[j] = sqcolor.Name;
                        using (SolidBrush b = new SolidBrush(sqcolor))
                        {
                            g.FillRectangle(b, new Rectangle(j * (square + sqspace), 0, square, square));
                        }
                        g.DrawRectangle(Pens.Gray, new Rectangle(j * (square + sqspace), 0, square, square));
                    }
                } 
                #endregion

                buttons[i] = new RibbonButton(colors);
                buttons[i].Text = string.Join(", ", colorss); ;
                buttons[i].MaxSizeMode = RibbonElementSizeMode.Medium;
                buttons[i].MinSizeMode = RibbonElementSizeMode.Medium;
            }
            RibbonButtonList blst = new RibbonButtonList(buttons);
            blst.FlowToBottom = false;
            blst.ItemsSizeInDropwDownMode = new Size(1, 10);
            itemColors.DropDownItems.Insert(0, blst);
            itemColors.DropDownResizable = true;

            #endregion

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void ribbonOrbOptionButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            panel1.Height = this.Size.Height - this.ribbon1.Size.Height - this.ribbon1.Margin.Bottom * 5 - this.ribbon1.Padding.Bottom * 5;
        }

        private void ribbonButton5_Click(object sender, EventArgs e)
        {
            FrmOfAddTable frmOfAddTable = new FrmOfAddTable();
            


        }

        /// <summary>
        /// �������ݱ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreateTable_Click(object sender, EventArgs e)
        {
            FrmOfAddTable frmOfAddTable = new FrmOfAddTable();
            DialogResult dialogResult = frmOfAddTable.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// ������ģ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNewModule_Click(object sender, EventArgs e)
        {
            FrmOfSetModel frmOfCreateModel = new FrmOfSetModel();
            DialogResult dialogResult = frmOfCreateModel.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                reloadTabControlAndReoGrid();
            }
            else
            {

            }
        }

        /// <summary>
        /// ģ���޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonModuleProp_Click(object sender, EventArgs e)
        {
            int indexOfSelectedTab = this.tabControl1.SelectedIndex;
            int idOfSelectedModel = int.Parse(listOfMsgOfModel[indexOfSelectedTab].id);

            FrmOfSetModel frmOfCreateModel = new FrmOfSetModel(idOfSelectedModel);
            DialogResult dialogResult = frmOfCreateModel.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                reloadTabControlAndReoGrid();
            }
            else
            {

            }
        }

        /// <summary>
        /// ����޸ı�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAlterTable_Click(object sender, EventArgs e)
        {
            FrmOfModifyTable frmOfModifyTable = new FrmOfModifyTable();
            DialogResult dialogResult = frmOfModifyTable.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// ɾ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelTable_Click(object sender, EventArgs e)
        {
            FrmOfDeleteTable frmOfDeleteTable = new FrmOfDeleteTable();
            DialogResult dialogResult = frmOfDeleteTable.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {

            }
            else
            {

            }
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonColProp_Click(object sender, EventArgs e)
        {
            int indexOfSelectedTab = this.tabControl1.SelectedIndex;
            MsgOfModel msgOfModel = listOfMsgOfModel[indexOfSelectedTab];
            int idOfSelectedModel = int.Parse(listOfMsgOfModel[indexOfSelectedTab].id);
            List<string> listOfTableName = new List<string>();
            listOfTableName.Add(msgOfModel.nameOfMainTable);
            if (msgOfModel.nameOfDetailTable != null && !msgOfModel.nameOfDetailTable.Equals(""))
            {
                listOfTableName.Add(msgOfModel.nameOfDetailTable);
            }

            FrmOfColumnProperty frmOfColumnProperty = new FrmOfColumnProperty(idOfSelectedModel, listOfTableName);
            DialogResult dialogResult = frmOfColumnProperty.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                this.reloadTabControlAndReoGrid();
            }
            else
            { 
            
            }
        }

        /// <summary>
        /// tabControl �ؼ��ĸĶ��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == -1)
            {
                return;
            }
            // ˢ�� �������
            this.reloadReoGrid();
        }

        /// <summary>
        /// ˢ������
        /// </summary>
        private void reloadTabControlAndReoGrid()
        {
            string sql = "select id, titleOfModel, classNameOfModel, nameOfMainTable, nameOfDetailTable, typeOfModel, relationField from MsgOfModel";
            DataTable resultOfMsgOfModel = SqlHandle.Common.sqlToDataTable1(sql);
            listOfMsgOfModel = new List<MsgOfModel>();
            this.tabControl1.TabPages.Clear();
            foreach (DataRow rowItem in resultOfMsgOfModel.Rows) {
                string id = rowItem["id"].ToString();
                string titleOfModel = rowItem["titleOfModel"].ToString();
                string classNameOfModel = rowItem["classnameOfModel"].ToString();
                string nameOfMainTable = rowItem["nameOfMainTable"].ToString();
                string nameOfDetailTable = rowItem["nameOfDetailTable"].ToString();
                bool typeOfModel = (bool) rowItem["typeOfModel"];
                string relationField = rowItem["relationField"].ToString();

                MsgOfModel msgOfModel = new MsgOfModel(id, titleOfModel, classNameOfModel, nameOfMainTable, nameOfDetailTable, typeOfModel, relationField);
                
                listOfMsgOfModel.Add(msgOfModel);
                TabPage page = new TabPage();
                page.Text = msgOfModel.titleOfModel;
                
                this.tabControl1.TabPages.Add(page);
            }

            // ˢ�� �������
            this.reloadReoGrid();
        }

        /// <summary>
        /// ˢ�� Grid 
        /// </summary>
        private void reloadReoGrid()
        {
            string sqlForData = "select ";
            // ��ȡ��Ҫ��ѯ��ʾ�����ݱ����
            int indexOfTabControl = this.tabControl1.SelectedIndex;
            MsgOfModel msgOfModel = listOfMsgOfModel[indexOfTabControl];
            string nameOfMainTable = msgOfModel.nameOfMainTable;
            listOfPropertyOfColumn = new List<PropertyOfColumn>();
            
            // ��ȡ��Ӧ�� ��������Ϣ
            string sqlForPropertyOfColumn = "select id, nameOfTable, title, field, width, isFreezed, formula, defaultValue, isReadOnly, isShow from PropertyOfColumn where nameOfTable = '" + nameOfMainTable + "' and isShow= '1'";
            DataTable resultOfPropertyOfColumn = SqlHandle.Common.sqlToDataTable1(sqlForPropertyOfColumn);
            
            // ��װ����ʽ
            sheet.Reset();
            sheet.Resize(1, resultOfPropertyOfColumn.Rows.Count);
            for (var indexOfRow = 0; indexOfRow < resultOfPropertyOfColumn.Rows.Count; indexOfRow++)
            {
                DataRow row = resultOfPropertyOfColumn.Rows[indexOfRow];

                // ��װ�������б�
                PropertyOfColumn propertyOfColumn = new PropertyOfColumn();
                propertyOfColumn.id = (int) row["id"];
                propertyOfColumn.idOfModel = int.Parse(msgOfModel.id);
                propertyOfColumn.nameOfTable = row["nameOfTable"].ToString();
                propertyOfColumn.title = row["title"].ToString();
                propertyOfColumn.defaultValue = row["defaultValue"].ToString();
                propertyOfColumn.width = int.Parse(row["width"].ToString());
                propertyOfColumn.field = row["field"].ToString();
                propertyOfColumn.formula = row["formula"].ToString();
                propertyOfColumn.isShow = (bool) row["isShow"];
                propertyOfColumn.isFreezed = (bool)row["isFreezed"];
                propertyOfColumn.isReadOnly = (bool)row["isReadOnly"];

                listOfPropertyOfColumn.Add(propertyOfColumn);

                // ���ñ�ͷ
                sheet.ColumnHeaders[indexOfRow].Text = row["title"].ToString();
                sheet.ColumnHeaders[indexOfRow].Width = (ushort) int.Parse(row["width"].ToString());

                // ��װ sql ���
                if (indexOfRow == (resultOfPropertyOfColumn.Rows.Count - 1))
                {
                    sqlForData += row["field"].ToString();
                }
                else
                {
                    sqlForData += row["field"].ToString() + ",";
                }
            }

            // ��װ sql����ȡ��������
            sqlForData = sqlForData + " from " + nameOfMainTable;
            DataTable resultOfMainTable = SqlHandle.Common.sqlToDataTable1(sqlForData);
            // ��ʽ�����
            sheet.Resize(resultOfMainTable.Rows.Count, resultOfPropertyOfColumn.Rows.Count);
            // ѭ������
            for (int indexOfMainTable = 0; indexOfMainTable < resultOfMainTable.Rows.Count; indexOfMainTable++)
            {
                
                // ѭ���ֶ�
                for (int indexOfPropertyOfColumn = 0; indexOfPropertyOfColumn < listOfPropertyOfColumn.Count; indexOfPropertyOfColumn++)
                {
                    Cell cell = this.sheet.Cells[indexOfMainTable, indexOfPropertyOfColumn];
                    PropertyOfColumn propertyOfColumn = listOfPropertyOfColumn[indexOfPropertyOfColumn];
                    // �������
                    cell.Data = resultOfMainTable.Rows[indexOfMainTable][propertyOfColumn.field];
                    
                }

            }





        }

    }
}