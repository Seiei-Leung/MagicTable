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
using Common;


namespace MagicTable
{
    public partial class MainForm : RibbonForm
    {
        public Dictionary<string, FrmBase> dictOfModulesFrm = new Dictionary<string, FrmBase>();
        List<MsgOfModel> listOfMsgOfModel = new List<MsgOfModel>();
        List<PropertyOfColumn> listOfPropertyOfColumn = new List<PropertyOfColumn>();

        public MainForm()
        {
            InitializeComponent();
            InitLists();



            //StartPosition = FormStartPosition.WindowsDefaultBounds;

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


        private void ribbonOrbOptionButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            panel1.Height = this.Size.Height - this.ribbon1.Size.Height - this.ribbon1.Margin.Bottom * 5 - this.ribbon1.Padding.Bottom * 5;
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
        }

        /// <summary>
        /// ģ���޸�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonModuleProp_Click(object sender, EventArgs e)
        {
            int indexOfSelectedTab = this.tabControlEx1.SelectedIndex;
            int idOfSelectedModel = int.Parse(listOfMsgOfModel[indexOfSelectedTab].id);

            FrmOfSetModel frmOfCreateModel = new FrmOfSetModel(idOfSelectedModel);
            DialogResult dialogResult = frmOfCreateModel.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                reloadTabControlAndReoGrid();
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
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonColProp_Click(object sender, EventArgs e)
        {
            int indexOfSelectedTab = this.tabControlEx1.SelectedIndex;
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
        }

        /// <summary>
        /// tabControl �ؼ��ĸĶ��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControlEx1.SelectedIndex == -1)
            {
                return;
            }
            //// ˢ�� �������
            this.reloadReoGrid();
        }

        /// <summary>
        /// ˢ������
        /// </summary>
        private void reloadTabControlAndReoGrid()
        {
            while (tabControlEx1.Controls.Count > 0)
            {
                tabControlEx1.Controls.RemoveAt(0);
            }
            string sql = "select id, titleOfModel, classNameOfModel, nameOfMainTable, nameOfDetailTable, typeOfModel, relationField from MsgOfModel";
            DataTable resultOfMsgOfModel = SqlHandle.Common.sqlToDataTable1(sql);
            listOfMsgOfModel = new List<MsgOfModel>();
            dictOfModulesFrm.Clear();
            foreach (DataRow rowItem in resultOfMsgOfModel.Rows)
            {
                string id = rowItem["id"].ToString();
                string titleOfModel = rowItem["titleOfModel"].ToString();
                string classNameOfModel = rowItem["classnameOfModel"].ToString();
                string nameOfMainTable = rowItem["nameOfMainTable"].ToString();
                string nameOfDetailTable = rowItem["nameOfDetailTable"].ToString();
                bool typeOfModel = (bool)rowItem["typeOfModel"];
                string relationField = rowItem["relationField"].ToString();

                // ��װģ�������б�
                MsgOfModel msgOfModel = new MsgOfModel(id, titleOfModel, classNameOfModel, nameOfMainTable, nameOfDetailTable, typeOfModel, relationField);
                listOfMsgOfModel.Add(msgOfModel);

                // ��װ�洢 pageControl ���ֵ�
                Form fm;
                // ���ӱ�
                if (msgOfModel.typeOfModel)
                {
                    fm = Comm.LoadSysMod(null, "TemplateCustom", "FrmMasterDetail", msgOfModel.classNameOfModel, msgOfModel.titleOfModel, "", false, false, this.tabControlEx1);
                }
                // ����
                else
                {
                    fm = Comm.LoadSysMod(null, "TemplateCustom", "FrmSingle", msgOfModel.classNameOfModel, msgOfModel.titleOfModel, "", false, false, this.tabControlEx1);
                }
                if (dictOfModulesFrm.ContainsKey(msgOfModel.classNameOfModel))
                {
                    dictOfModulesFrm.Remove(msgOfModel.classNameOfModel);
                }
                dictOfModulesFrm.Add(msgOfModel.classNameOfModel, (FrmBase)fm);
            }

            // ˢ�� �������
            this.reloadReoGrid();
        }

        /// <summary>
        /// ˢ�� Grid 
        /// </summary>
        private void reloadReoGrid()
        {
            // ��ȡ �������� sql
            string sqlForData = "select ";
            // ��ȡ��Ҫ��ѯ��ʾ�����ݱ����
            int indexOfTabControl = this.tabControlEx1.SelectedIndex;
            MsgOfModel msgOfModel = listOfMsgOfModel[indexOfTabControl];
            listOfPropertyOfColumn = new List<PropertyOfColumn>();

            // ��ȡ��Ӧ�� ��������Ϣ
            string sqlForPropertyOfColumn = "select id, nameOfTable, title, field, width, isFreezed, formula, defaultValue, isReadOnly, isShow from PropertyOfColumn where nameOfTable = '" + msgOfModel.nameOfMainTable + "' and isShow= '1'";
            DataTable resultOfPropertyOfColumn = SqlHandle.Common.sqlToDataTable1(sqlForPropertyOfColumn);

            if (resultOfPropertyOfColumn.Rows.Count == 0)
            {
                return;
            }

            // ��װ����ʽ
            foreach (Control control in tabControlEx1.SelectedTab.Controls)
            {
                string classname = control.GetType().Name;
                string formname = control.Name;
                // ��ȡ��ǰ����� page �ؼ�
                FrmBase objForm = dictOfModulesFrm[formname];
                // ��ȡ������ grid �ؼ�
                Worksheet sheetOfMaster = (objForm.dicGrids["master"] as ReoGridControl).CurrentWorksheet;
                // ��ʽ�� grid �ؼ�
                sheetOfMaster.Reset();
                sheetOfMaster.Resize(1, resultOfPropertyOfColumn.Rows.Count);

                for (var indexOfRow = 0; indexOfRow < resultOfPropertyOfColumn.Rows.Count; indexOfRow++)
                {
                    DataRow row = resultOfPropertyOfColumn.Rows[indexOfRow];

                    // ��װ�������б�
                    PropertyOfColumn propertyOfColumn = new PropertyOfColumn();
                    propertyOfColumn.id = (int)row["id"];
                    propertyOfColumn.idOfModel = int.Parse(msgOfModel.id);
                    propertyOfColumn.nameOfTable = row["nameOfTable"].ToString();
                    propertyOfColumn.title = row["title"].ToString();
                    propertyOfColumn.defaultValue = row["defaultValue"].ToString();
                    propertyOfColumn.width = int.Parse(row["width"].ToString());
                    propertyOfColumn.field = row["field"].ToString();
                    propertyOfColumn.formula = row["formula"].ToString();
                    propertyOfColumn.isShow = (bool)row["isShow"];
                    propertyOfColumn.isFreezed = (bool)row["isFreezed"];
                    propertyOfColumn.isReadOnly = (bool)row["isReadOnly"];

                    listOfPropertyOfColumn.Add(propertyOfColumn);

                    // ���ñ�ͷ
                    sheetOfMaster.ColumnHeaders[indexOfRow].Text = row["title"].ToString();
                    sheetOfMaster.ColumnHeaders[indexOfRow].Width = (ushort)int.Parse(row["width"].ToString());

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
                sqlForData = sqlForData + " from " + msgOfModel.nameOfMainTable;
                DataTable resultOfMainTable = SqlHandle.Common.sqlToDataTable1(sqlForData);
                // ��ʽ�����
                sheetOfMaster.Resize(resultOfMainTable.Rows.Count, resultOfPropertyOfColumn.Rows.Count);
                // ѭ������
                for (int indexOfMainTable = 0; indexOfMainTable < resultOfMainTable.Rows.Count; indexOfMainTable++)
                {

                    // ѭ���ֶ�
                    for (int indexOfPropertyOfColumn = 0; indexOfPropertyOfColumn < listOfPropertyOfColumn.Count; indexOfPropertyOfColumn++)
                    {
                        Cell cell = sheetOfMaster.Cells[indexOfMainTable, indexOfPropertyOfColumn];
                        PropertyOfColumn propertyOfColumn = listOfPropertyOfColumn[indexOfPropertyOfColumn];
                        // �������
                        cell.Data = resultOfMainTable.Rows[indexOfMainTable][propertyOfColumn.field];
                    }
                }

                /*
                
                 * ģ��Ϊ���ӵ�����ȡ�ӵ���Ϣ
                
                 */
                if (classname == "FrmMasterDetail")
                {
                    string sqlForDetail = "select ";
                    listOfPropertyOfColumn.Clear();
                    // ��ȡ��Ӧ�� ��������Ϣ
                    sqlForPropertyOfColumn = "select id, nameOfTable, title, field, width, isFreezed, formula, defaultValue, isReadOnly, isShow from PropertyOfColumn where nameOfTable = '" + msgOfModel.nameOfDetailTable + "' and isShow= '1'";

                    resultOfPropertyOfColumn = SqlHandle.Common.sqlToDataTable1(sqlForPropertyOfColumn);

                    if (resultOfPropertyOfColumn.Rows.Count == 0)
                    {
                        return;
                    }

                    // ��ȡ�ӵ��� grid �ؼ�
                    Worksheet sheetOfDetail = (objForm.dicGrids["detail"] as ReoGridControl).CurrentWorksheet;
                    // ��ʽ�� grid �ؼ�
                    sheetOfDetail.Reset();
                    sheetOfDetail.Resize(1, resultOfPropertyOfColumn.Rows.Count);
                    for (var indexOfRow = 0; indexOfRow < resultOfPropertyOfColumn.Rows.Count; indexOfRow++)
                    {
                        DataRow row = resultOfPropertyOfColumn.Rows[indexOfRow];

                        // ��װ�������б�
                        PropertyOfColumn propertyOfColumn = new PropertyOfColumn();
                        propertyOfColumn.id = (int)row["id"];
                        propertyOfColumn.idOfModel = int.Parse(msgOfModel.id);
                        propertyOfColumn.nameOfTable = row["nameOfTable"].ToString();
                        propertyOfColumn.title = row["title"].ToString();
                        propertyOfColumn.defaultValue = row["defaultValue"].ToString();
                        propertyOfColumn.width = int.Parse(row["width"].ToString());
                        propertyOfColumn.field = row["field"].ToString();
                        propertyOfColumn.formula = row["formula"].ToString();
                        propertyOfColumn.isShow = (bool)row["isShow"];
                        propertyOfColumn.isFreezed = (bool)row["isFreezed"];
                        propertyOfColumn.isReadOnly = (bool)row["isReadOnly"];

                        listOfPropertyOfColumn.Add(propertyOfColumn);

                        // ���ñ�ͷ
                        sheetOfDetail.ColumnHeaders[indexOfRow].Text = row["title"].ToString();
                        sheetOfDetail.ColumnHeaders[indexOfRow].Width = (ushort)int.Parse(row["width"].ToString());

                        // ��װ sql ���
                        if (indexOfRow == (resultOfPropertyOfColumn.Rows.Count - 1))
                        {
                            sqlForDetail += row["field"].ToString();
                        }
                        else
                        {
                            sqlForDetail += row["field"].ToString() + ",";
                        }
                    }
                    // ��װ sql����ȡ��������
                    sqlForDetail = sqlForDetail + " from " + msgOfModel.nameOfDetailTable;
                    DataTable resultOfDetailTable = SqlHandle.Common.sqlToDataTable1(sqlForDetail);
                    // ��ʽ�����
                    sheetOfDetail.Resize(resultOfDetailTable.Rows.Count, resultOfPropertyOfColumn.Rows.Count);

                    // ѭ������
                    for (int indexOfDetailTable = 0; indexOfDetailTable < resultOfDetailTable.Rows.Count; indexOfDetailTable++)
                    {

                        // ѭ���ֶ�
                        for (int indexOfPropertyOfColumn = 0; indexOfPropertyOfColumn < listOfPropertyOfColumn.Count; indexOfPropertyOfColumn++)
                        {
                            Cell cell = sheetOfDetail.Cells[indexOfDetailTable, indexOfPropertyOfColumn];
                            PropertyOfColumn propertyOfColumn = listOfPropertyOfColumn[indexOfPropertyOfColumn];
                            // �������
                            cell.Data = resultOfDetailTable.Rows[indexOfDetailTable][propertyOfColumn.field];
                        }
                    }


                }
            }
        }
    }
}