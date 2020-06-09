using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbFactory;
using IDAL;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Configuration;
using Model;
using System.Collections;
using System.Runtime.Serialization;
using System.ServiceModel.Security;

namespace Bll
{
    public class AndroidBll : BaseBll
    {
        internal IAndroid iandroid;
        SystemLogBll log = null;

        public AndroidBll()
        {
            iandroid = base.GetDal.iAndroid;
            log = new SystemLogBll();
        }

        #region Method

        public string UploadPicSave(string datetime,string upName,string upPath)
        {
            try
            {
                string strSQL = string.Empty;
                string serialno = System.Guid.NewGuid().ToString();
                strSQL = "insert into androidPicUpload(serialno,uploadTime,uploadName,uploadPath) values ";
                strSQL += "('" + serialno + "','" + datetime + "','" + upName + "','" + upPath + "')";

                return iandroid.ExcuteSQL(strSQL).ToString();
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return "0";
            }
        }

        public string Android_Login(string userCode, string password)
        {
            string sql = "select * from t_SYUsers where Code='" + userCode + "' and PW='" + password + "' and Del<>1";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt.Rows.Count > 0)
                {
                    return "true";
                }
                else
                    return "false";
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return "false";
            }
        }

        public DataTable Login(string parameter1, string parameter2)
        {
            string sql = "select Code value1,Name value2 from t_SYUsers where Code='" + parameter1 + "' and PW='" + parameter2 + "' and Del<>1";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt!=null)
                {
                    return dt;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public string sysName(string code)
        {
            string sqlstr = string.Format("select * from t_SYUsers where Code='{0}'", code);
            try
            {
                //log.SysErrorSave(sqlstr, "192.168.0.51", "ZC-DN-WJ"); //保存看看查询字符串是否有错
                DataSet ds = iandroid.Query(sqlstr);
                if (ds == null || ds.Tables.Count == 0)
                    return "error1";
                DataTable db = ds.Tables[0];
                if (db == null || db.Rows.Count == 0)
                    return "error2";
                return db.Rows[0]["Name"].ToString().Trim();
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return "error456";
            }
        }
        #endregion

        public string MaxTbValue(string sqlStr)
        {
            try
            {
                DataTable dt = iandroid.GetSingle(sqlStr);
                if (dt != null)
                    return dt.Rows[0][0].ToString().Trim();
                else
                    return "";
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return "";
            }
        }
        public DataTable ReturnTable(string sql)
        {
            //if (sql.ToLower().Contains("delete") || sql.ToLower().Contains("update"))
            //{
            //    return null;//存储过程里有update,delete咋办？
            //}
            //else
            //{
                try
                {
                    DataTable dt = iandroid.GetSingle(sql);
                    if (dt != null)
                        return dt;
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                    return null;
                }
            //}
        }

        public DataTable WaitMission2(string parameter1, string parameter2) 
        {
            string sql = "exec sp_SYDeskTop_GetDataNew_Phone '" + parameter1 + "','" + parameter2 + "'";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable WaitMission1(string parameter1)
        {
            string sql = "exec sp_Desktop_GetUnHandlerNew_phone '" + parameter1 + "',''";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable WaitMission3(string parameter1, string parameter2)
        {
            string sql = "exec sp_SYDeskTop_GetDataNew_Phone_Detailed '" + parameter1 + "','"+parameter2+"'";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetWebServiceAddress(string parameter1)
        {
            string sql = "select IPAddress,WebService from t_WebService where CompanyName='" + parameter1 + "' and Del=0";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable RT_WORKITEM() 
        {
            string sql = "select * from T_FF_RT_WORKITEM";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetSupplier()
        {
            string sql = "select * from T1020";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetCustomer()
        {
            string sql = "select simplename,engcode from customer";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable Interface1(string parameter1, string parameter2, string parameter3, string parameter4)
        {
            string sql="";
            if (parameter4 == "QT") 
            {
                sql = "exec sp_IOS_DC_TAB4_1 '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            if (parameter4 == "SAH")
            {
                sql = "exec sp_IOS_DC_TAB4_2 '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            if (parameter4 == "PD")
            {
                sql = "exec sp_IOS_DC_TAB4_3 '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable Interface2(string parameter1, string parameter2, string parameter3, string parameter4)
        {
            string sql = "";
            if (parameter4 == "QT")
            {
                sql = "exec spIOS_Char '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            if (parameter4 == "SAH")
            {
                sql = "exec spIOS_Char_SAH '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            if (parameter4 == "PD")
            {
                sql = "exec spIOS_Char_PD '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            if (parameter4 == "平均数")
            {
                sql = "exec spAndroid_Avg_qt '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            if (parameter4 == "平均SAM")
            {
                sql = "exec spAndroid_Avg_sam '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            if (parameter4 == "落单次数")
            {
                sql = "exec spAndroid_Single_frequency '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable Interface3(string parameter1, string parameter2, string parameter3, string parameter4) 
        {
            string sql = "";
            if (parameter4 == "合格率")
            {
                sql = "exec SP_ProdAnly_ClothInspect_Char '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "'";
            }

            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable Interface5(string parameter1, string parameter2, string parameter3, string parameter4)
        {
            string sql = "exec sp_ChartWithPhone '" + parameter1 + "','" + parameter2 + "','" + parameter3 + "','" + parameter4 + "'";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable Interface4(string parameter1, string parameter2, string parameter3)
        {
            string sql = "";
            if (parameter3 == "合格率_GY")
            {
                sql = "select 年份 y,月份 m,cast((sum(合格)/sum(全部)*100) as numeric(18,0)) valuenum from vBI_SP_ProdAnly_ClothInspect_ALL where 供应商英文代号='" + parameter1 + "' and 年份='" + parameter2 + "' group by 年份,月份";
            }

            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }


        public DataTable TaskItem(string Code)
        {
            string sql = "exec sp_Desktop_GetUnHandlerNew '" + Code + "',''";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                    return dt;
                else
                    return null;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable SearchOrder1(string parameter1)
        {
            string sql = "select orderno value1,custname value2,typename value3,quantity value4,persam value5,closedate value6,photo value7 from workorder where orderno='" + parameter1 + "'";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                    return dt;
                else
                    return null;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable SearchOrder(string parameter1, string parameter2)
        {
            string sql = "select orderno,custname,styleno,typename,persam,season,sellname,closedate,opendate,imageurl,imagewidth,imageheigh from workorder where opendate between '" + parameter1 + "' and '" + parameter2 + "' order by opendate desc";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                    return dt;
                else
                    return null;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable TaskItemList(string ClassName, string Code)
        {
            string sql = "exec sp_SYDeskTop_GetDataNew_Android '" + ClassName + "','','" + Code + "',1";
            try
            {
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                    return dt;
                else
                    return null;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public bool updateimageurl(DataTable parameter1) 
        {
            try 
            {
                string strSQL = "";
                int number = 0;
                for (int i = 0; i < parameter1.Rows.Count; i++) 
                {
                    string orderno = parameter1.Rows[i]["orderno"].ToString();
                    string imageurl = parameter1.Rows[i]["imageurl"].ToString();
                    strSQL = "update workorder set imageurl='" + imageurl + "' where orderno='" + orderno + "'";
                    number += iandroid.ExcuteSQL(strSQL);
                }

                if (number > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) 
            { 
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return false;
            }
        }

        public bool updateimageurl_one(string parameter1, string parameter2, int parameter4, int parameter5)
        {
            try
            {
                string strSQL = "update workorder set imageurl='" + parameter2 + "',imagewidth="+parameter4+",imageheigh="+parameter5+" where orderno='" + parameter1 + "'";
                int number = iandroid.ExcuteSQL(strSQL);
                if (number > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return false;
            }
        }

        public string UploadPic1(string parameter1, string parameter2, string parameter3)
        {
            try
            {
                string strSQL = string.Empty;
                string serialno = System.Guid.NewGuid().ToString();
                strSQL = "insert into androidPicUpload(serialno,uploadTime,uploadName,uploadPath) values ";
                strSQL += "('" + serialno + "','" + parameter1 + "','" + parameter2 + "','" + parameter3 + "')";

                return iandroid.ExcuteSQL(strSQL).ToString();
            }
            catch (SecurityAccessDeniedException e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return "0";
            }
        }

        public DataTable RightSet(string parameter1) 
        {
            string sql = "select Id serialno,UserId value1,ModuleName value2 from t_SYUserRight_Phone where UserId='" + parameter1 + "'";
            try 
            { 
                DataTable dt = iandroid.GetSingle(sql);
                if (dt != null)
                    return dt;
                else
                    return null;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        //public DataTable TestTable(string userCode)
        //{
        //    try
        //    {
        //        string sql = "select * from t_SYUsers where Code=@userCode";
        //        SqlParameter[] param = new SqlParameter[1];
        //        param[0] = new SqlParameter("userCode", userCode);
        //        DataTable dt = iandroid.Query(sql, param);
        //        return dt;
        //    }
        //    catch (Exception e)
        //    {
        //        log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
        //        return null;
        //    }
        //}
    }
}
