using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Xml;

using System.IO;

using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.Xml.Linq;

using System.Security.Cryptography;

namespace Common
{
    /// <summary>
    /// 文件操作
    /// </summary>
    public static class FileHandler
    {

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-10-07
        /// 读取客户端的EXCEL文件
        /// </summary>
        /// <param name="Path">EXCEL文件路径</param>
        /// <returns>数据集(DataSet)</returns>
        public static DataSet GetDataFromExcelBySheetName(string Path)
        {
            String strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                     "Data Source=" + Path + ";" +
                     "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等　
            DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

            //包含excel中表名的字符串数组
            string[] strTableNames = new string[dtSheetName.Rows.Count];
            for (int k = 0; k < dtSheetName.Rows.Count; k++)
            {
                strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
            }
            OleDbDataAdapter da = null;
            DataSet ds = new DataSet();
            //从指定的表明查询数据,可先把所有表明列出来供用户选择
            string strExcel = "select * from[" + strTableNames[0] + "]";
            da = new OleDbDataAdapter(strExcel, conn);
            da.Fill(ds);
            /*
            object obj = null;
            
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                obj = null;
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    obj = ds.Tables[0].Rows[i][j];
                }

                if (obj == null || obj.ToString() == "" )  
                    
            }*/

            return ds;
        }

        public static string GetConString()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;

            XElement config = XElement.Load(path + "\\UserConfig.xml");
            string type = GetElementValue(config, "DBConnect");

            config = XElement.Load(path + "\\LocalConifg.xml");
            XElement DBs = config.Element("DBConnect");
            foreach (XElement ele in DBs.Elements("DB"))
            {
                string name = GetAttributeValue(ele, "Name");
                if (type.Equals(name))
                {
                    string str = GetAttributeValue(ele, "Value");
                    config = null;
                    return StringHandler.DesDecrypt(str, "!@#$%^&(");
                }
            }
            config = null;
            return null;
        }


        /// <summary>
        /// 读取EXCEL文件数据
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static DataSet GetDataFromExcel(string Path)
        {
            if (Path.Trim() == "") return null;
            string sheetName = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                String strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" +
                         "Data Source=" + Path + ";" +
                         "Extended Properties='Excel 8.0;IMEX=1;HDR=Yes';";
                //if(Path.EndsWith("xlsx"))
                //    strConn = "Provider=Microsoft.Jet.OLEDB.12.0;" +
                //         "Data Source=" + Path + ";" +
                //         "Extended Properties='Excel 12.0;IMEX=1;HDR=Yes';";

                using (OleDbConnection conn = new OleDbConnection(strConn))
                {

                    conn.Open();
                    DataTable dtData = new DataTable("ExcelImport");
                    dtData.Columns.Add(new DataColumn("工作表", typeof(string)));

                    //返回Excel的架构，包括各个sheet表的名称,类型，创建时间和修改时间等　
                    DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

                    //包含excel中表名的字符串数组
                    string[] strTableNames = new string[dtSheetName.Rows.Count];
                    for (int k = 0; k < dtSheetName.Rows.Count; k++)
                    {
                        strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
                        DataRow dr = dtData.NewRow();
                        dr[0] = strTableNames[k];
                        dtData.Rows.Add(dr);
                    }
                    OleDbDataAdapter da = null;

                    //从指定的表明查询数据,可先把所有表明列出来供用户选择    
                    //if (dtSheetName.Rows.Count > 1)
                    //{
                    //    FrmListBox frm = new FrmListBox("请选择EXCLE的工作簿", dtData);
                    //    frm.ShowDialog();
                    //    if (frm.DialogResult == DialogResult.OK)
                    //    {
                    //        sheetName = frm.SelectValue.ToString();
                    //        string strExcel = "select * from [" + sheetName + "A1:BZ]";
                    //        da = new OleDbDataAdapter(strExcel, conn);
                    //        da.Fill(ds);
                    //    }
                    //}
                    //else
                    {
                        //EXCEL只有一个工作簿
                        string strExcel = "select * from [" + strTableNames[0] + "A1:BZ]";
                        da = new OleDbDataAdapter(strExcel, conn);
                        da.Fill(ds);
                    }

                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("操作失败，请确认关闭上传文件");
                Common.Message.MsgError(ex.Message.Trim());
            }

            return ds;
        }

        

        //public static DataTable OpenExcelTemplate(DevExpress.XtraGrid.Views.Grid.GridView gv, string tableName, string ClassName)
        //{
        //    DataTable dt = new DataTable();
        //    DataSet dsExcelData = new DataSet();

        //    string strName = string.Empty;
        //    strName = SelectFile("选择导入的Excel文件", "Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx|Csv文件(*.csv)|*.csv");

        //    //dt = ReadCsvFile(strName);

        //    //if (!dsExcelData.Tables.Contains("dtCsv"))
        //    //    dsExcelData.Tables.Add(dt);

        //    dt = ReadExcelByNPOI(strName);

        //    if (!dsExcelData.Tables.Contains("dtExcel") && dt != null)
        //        dsExcelData.Tables.Add(dt);
        //    else
        //        return null;

        //    //Message.MsgAlert(ExcelToCSV(strName));

        //    ////读取EXCEL文件数据,把数据集放在DATASET 里面
        //    //dsExcelData = FileHandler.GetDataFromExcel(strName);

        //    if (dsExcelData == null || dsExcelData.Tables.Count <= 0) return null;

        //    FrmExcelImport frm = new FrmExcelImport(gv, dsExcelData, tableName, ClassName);
        //    frm.ShowDialog();

        //    if (frm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
        //    {
        //        dt = frm.dtReturn;
        //    }
        //    return dt;
        //}

         //<summary>
         //创建人：方君业
         //日期：2012-10-12
         //从gridview导出
         //</summary>
         //<param name="dt">DataTable集合</param>
         //<returns>修改状态</returns>
        //public static bool ExportOutFromGrv(DevExpress.XtraGrid.Views.Grid.GridView grv)
        //{
        //    bool myResult = false;
        //    System.Windows.Forms.SaveFileDialog diaLog = new System.Windows.Forms.SaveFileDialog();
        //    string FileExten = string.Empty;
        //    FileExten = "Microsoft Excel 2003文件(*.xls)|*.xls|Microsoft Excel 2007文件(*.xlsx)|*.xlsx";
        //    FileExten += "|PDF 文件(*.pdf)|*.pdf";

        //    diaLog.Filter = FileExten;
        //    if (diaLog.ShowDialog() == DialogResult.OK)
        //    {
        //        if (diaLog.FilterIndex == 1)
        //            grv.ExportToXls(diaLog.FileName,XlsExportOptions.);
        //        else if (diaLog.FilterIndex == 2)
        //            grv.ExportToXlsx(diaLog.FileName);
        //        else if (diaLog.FilterIndex == 3)
        //            grv.ExportToPdf(diaLog.FileName);

        //        myResult = true;
        //    }
        //    return myResult;
        //}

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-08-28
        ///  将GRIDVIEW 导出EXCEL文件
        /// </summary>
        /// <param name="grv"></param>
        /// <returns></returns>


        
        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-11-21
        /// 打开上传的文件
        /// </summary>
        /// <param name="Filter">文件格式("Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx")</param>
        /// <returns>返回选择的文件</returns>
        public static string SelectFile(string title, string Filter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = title;
            ofd.FileName = "";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Filter = Filter;// "Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx";
            //文件有效性验证ValidateNames，验证用户输入是否是一个有效的Windows文件名
            ofd.ValidateNames = true;
            //验证路径有效性
            ofd.CheckFileExists = true;
            //验证文件有效性
            ofd.CheckPathExists = true;

            string strName = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strName = ofd.FileName;
            }

            return strName;
        }

        /// <summary>
        /// 创建人：方剑伟
        /// 日期：2018-11-13
        /// <para>设置LocalConfig.xml配置文件的数据库连接</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Xvalue"></param>
        public static void SetDBConnectConfig(string Xvalue)
        {
            //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            //doc.Load(Application.ExecutablePath + ".config");
            //System.Xml.XmlNode node = doc.SelectSingleNode(@"//add[@key='" + name + "']");
            //System.Xml.XmlElement ele = (System.Xml.XmlElement)node;
            //ele.SetAttribute("value", Xvalue);
            //doc.Save(Application.ExecutablePath + ".config");
            try
            {
                //string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\LocalConifg.xml";
                //XElement config = XElement.Load(path);
                //XElement ele = config.Element(name);
                //ele.Value = Xvalue;
                //config.Save(path);
                string path = System.AppDomain.CurrentDomain.BaseDirectory;

                XElement config = XElement.Load(path + "\\UserConfig.xml");
                string type = GetElementValue(config, "DBConnect");
                string savePath = path + "\\LocalConifg.xml";
                config = XElement.Load(path + "\\LocalConifg.xml");
                XElement DBs = config.Element("DBConnect");
                foreach (XElement ele in DBs.Elements("DB"))
                {
                    string name = GetAttributeValue(ele, "Name");
                    if (type.Equals(name))
                    {
                        XName xname = XName.Get("Value");
                        XAttribute xAttribute = ele.Attribute(xname);
                        xAttribute.Value = Xvalue;
                        config.Save(savePath);
                        config = null;

                        //return DesDecrypt(str);
                    }
                }
                config = null;
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2013-01-12
        /// <para>修改配置文件设置</para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Xvalue"></param>
        public static void UpdateConfig(string name, string Xvalue)
        {
            //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            //doc.Load(Application.ExecutablePath + ".config");
            //System.Xml.XmlNode node = doc.SelectSingleNode(@"//add[@key='" + name + "']");
            //System.Xml.XmlElement ele = (System.Xml.XmlElement)node;
            //ele.SetAttribute("value", Xvalue);
            //doc.Save(Application.ExecutablePath + ".config");
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\UserConfig.xml";
                XElement config = XElement.Load(path);
                XElement ele = config.Element(name);
                ele.Value = Xvalue;
                config.Save(path);
            }
            catch (Exception ex)
            {

            }
        }



        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-02-28
        /// 获取XML文档指定节点的值
        /// </summary>
        /// <param name="strXml">字符串XML文档</param>
        /// <param name="selectNode">节点</param>
        /// <returns>XmlNode </returns>
        public static XmlNodeList GetXmlNodeValue(string strXml, string selectNode)
        {
            if (string.IsNullOrEmpty(strXml)) return null;

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(strXml);
            XmlElement root = doc.DocumentElement;

            XmlNodeList NodeList = root.SelectNodes(selectNode);

            return NodeList;
        }

        /// <summary>
        /// 获取XML对象元素的值
        /// </summary>
        /// <param name="xele"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetElementValue(XElement xele, string name)
        {
            XName xname = XName.Get(name);
            XElement xElement = xele.Element(xname);
            if (xElement == null) return String.Empty;
            return xElement.Value;
        }

        /// <summary>
        /// 获取XML对象元素属性的值
        /// </summary>
        /// <param name="xele"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAttributeValue(XElement xele, string name)
        {
            XName xname = XName.Get(name);
            XAttribute xAttribute = xele.Attribute(xname);
            if (xAttribute == null) return String.Empty;
            return xAttribute.Value;
        }

        

        public static DataTable ReadCsvFile(string FilePath)
        {
            String line;
            String[] split = null;
            DataTable table = new DataTable("dtCsv");
            DataRow row = null;
            StreamReader sr = new StreamReader(FilePath, System.Text.Encoding.Default);
            //创建与数据源对应的数据列   
            line = sr.ReadLine();
            split = line.Split(',');
            foreach (String colname in split)
            {
                table.Columns.Add(colname, System.Type.GetType("System.String"));
            }
            //将数据填入数据表   
            int j = 0;
            while ((line = sr.ReadLine()) != null)
            {
                j = 0;

                row = table.NewRow();
                split = line.Split(',');
                foreach (String colname in split)
                {
                    if (!string.IsNullOrEmpty(colname))
                        row[j] = colname;

                    j++;
                }
                table.Rows.Add(row);
            }

            sr.Close();
            return table;
        }

       

        

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2013-12-27
        /// 一次性选择多个文件
        /// </summary>
        /// <param name="title">窗口提示</param>
        /// <param name="Filter">上传文件格式</param>
        /// <param name="IsMultiSelect">是否允许多选</param>
        /// <returns></returns>
        public static string[] MultiSelectFile(string title, string Filter, bool IsMultiSelect)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = title;
            ofd.FileName = "";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Filter = Filter;// "Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx";
            //文件有效性验证ValidateNames，验证用户输入是否是一个有效的Windows文件名
            ofd.ValidateNames = true;
            //验证路径有效性
            ofd.CheckFileExists = true;
            //验证文件有效性
            ofd.CheckPathExists = true;
            ofd.Multiselect = IsMultiSelect;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileNames;
            }

            return null;
        }

        

      
        /// <summary>
        /// 创建人：黎金来
        /// 日期：2015-01-24
        /// RFID扫描串口设置
        /// </summary>
        /// <param name="com">串口</param>
        public static void RfidComSet(string com)
        {
            try
            {
                string FilePath = Application.StartupPath + "\\FormLayOut\\";
                string tranFileName = "rfid";

                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                FilePath += tranFileName + ".xml";

                DataTable dt = new DataTable();
                dt.Columns.Add("com", typeof(System.String));

                DataRow dr = dt.NewRow();
                dr[0] = com;
                dt.Rows.Add(dr);

                dt.TableName = "rfid";

                dt.WriteXmlSchema(FilePath);
                dt.WriteXml(FilePath);
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2015-01-24
        /// 读取RFID串口
        /// </summary>
        /// <returns></returns>
        public static string RfidComGet()
        {
            string FilePath = Application.StartupPath + "\\FormLayOut\\rfid.xml";

            DataTable dt = new DataTable();

            if (File.Exists(FilePath))
            {
                dt.TableName = "rfid";
                dt.ReadXmlSchema(FilePath);
                dt.ReadXml(FilePath);
            }

            if (dt == null || dt.Rows.Count <= 0)
                return "";
            else
                return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        DeleteFolder(d1.FullName);////递归删除子文件夹
                    }
                    Directory.Delete(d);
                }
            }
        }
    }
}
