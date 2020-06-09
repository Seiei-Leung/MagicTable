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

namespace Bll
{
    [DataContract]
    public enum Status
    {
        [EnumMember]
        SUCCESS,
        [EnumMember]
        FAIL,
        [EnumMember]
        TRUE,
        [EnumMember]
        FALSE,
        [EnumMember]
        ERROR
    }

    [DataContract]
    public class Excute_Status
    {
        [DataMember]
        public Status status { get; private set; }
        [DataMember]
        public string msg { get; private set; }
        [DataMember]
        public string billno { get; private set; }

        public Excute_Status(Status _status, string _msg, string _billno)
        {
            this.status = _status;
            this.msg = _msg;
            this.billno = _billno;
        }

        public Excute_Status(Status _status, string _msg)
        {
            this.status = _status;
            this.msg = _msg;
            this.billno = "";
        }

    }

    public class AuthorityBll : BaseBll
    {
        internal IAuthority authority;
        SystemLogBll log = null;

        public AuthorityBll()
        {
            authority = base.GetDal.AuthorityDal;
            log = new SystemLogBll();
        }

        #region 其他

        public DataTable GetUserGroup()
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select * from t_SYGroup";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYGroup";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetUsersByGroupID(string gid)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select * from t_SYUsers";
                if (gid.Trim() != string.Empty)
                {
                    sql = sql + " where TypeCode='" + gid.Trim() + "'";
                }
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYUsers";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        public DataTable GetUserRight(string userId)
        {
            try
            {
                //IAuthority authority = base.GetDal.AuthorityDal;
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select c.DisplayName PModName,b.DisplayName CModName,");
                sql.AppendLine("a.* from t_SYUserRights a left join t_SYFunc b on a.sysfunid=b.id left join ");
                sql.AppendLine("t_SYFunp c on b.pid=c.id ");
                if (userId.Trim() != string.Empty)
                {
                    sql.AppendLine(" where userId='" + userId.Trim() + "'");
                }
                DataTable dt = authority.GetSingle(sql.ToString());
                if (dt != null)
                {
                    dt.TableName = "t_SYUserrights";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        public bool SaveData(DataTable dt, out string msgcode)
        {
            try
            {
                //if (dt == null)
                //{
                //    msgcode = "10003";
                //    throw new NoNullAllowedException("Paramter dt not allow null");
                //}
                //if (dt.TableName.Trim() == string.Empty)
                //{
                //    msgcode = "10004";
                //    throw new NoNullAllowedException("TableName not allow null");
                //}
                //authority = base.GetDal.AuthorityDal;
                //msgcode = "00000";
                msgcode = "SUCCESS";
                return authority.SaveData(dt);
                //Microsoft.ApplicationBlocks.Data.SqlHelper.SaveToDatabase(dt);
                //return true;
            }
            catch (Exception ex)
            {
                //msgcode = "FFFFF";
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                msgcode = ex.Message;
                return false;
                //throw;
            }
        }

        public DataTable GetAllRole()
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select * from t_SYRole";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYRole";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }


        public DataTable GetRoleRight(string roleId)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                //string sql = "select * from tRolerights where rolecode='" + rolecode + "'";
                StringBuilder sql = new StringBuilder();
                sql.Append("select a.* ,b.displayname CModName,c.displayname PModName ");
                sql.Append("from t_SYRolerights a ");
                sql.Append("left join t_SYFunc b on b.id=a.sysfunid ");
                sql.Append("left join t_SYFunp c on c.id=b.pid ");
                sql.Append("where roleId='" + roleId + "'");
                DataTable dt = authority.GetSingle(sql.ToString());
                if (dt != null)
                {
                    dt.TableName = "t_SYRolerights";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }
      
        public List<UpdateFileModel> GetUpdateFileList(double version)
        {
            try
            {
                string updateconfig = ConfigurationManager.AppSettings["UpdateConfig"].ToString();
                if (updateconfig == string.Empty)
                {
                    throw new ArgumentNullException("UpdateConfig is not applyed!");
                }
                XElement elements = XElement.Load(updateconfig);
                IEnumerable<UpdateFileModel> temp = (from x in elements.Elements()
                                                     where (double)x.Attribute("version") > version
                                                     select new UpdateFileModel { FileName = x.Value, Loc = x.Attribute("loc").Value, DownLoadUrl = x.Attribute("href").Value }
                                                     );
                temp = from x in temp
                       group x by new { x.FileName, x.Loc, x.DownLoadUrl } into g
                       select new UpdateFileModel { FileName = g.Key.FileName, Loc = g.Key.Loc, DownLoadUrl = g.Key.DownLoadUrl };
                List<UpdateFileModel> models = new List<UpdateFileModel>();
                UpdateFileModel model = null;
                foreach (UpdateFileModel item in temp)
                {
                    model = new UpdateFileModel();
                    model.FileName = item.FileName;
                    model.Loc = item.Loc;
                    model.DownLoadUrl = item.DownLoadUrl;
                    models.Add(model);
                }
                return models;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }
       
        public DataTable GetSysModManageBySerialno(string serialno)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModManage where 1<>1";
                else
                    sql = "select * from t_SYModManage where serialno='" + serialno + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManage";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                throw;
            }
        }  

        public DataTable OpenDataSingle(string sqlstr, string table)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = sqlstr;
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = table;
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod + "    查询SQL:" + sqlstr, this, ex, UserCode, IP);
                DataTable dt = new DataTable();
                dt.Columns.Add("A");
                DataRow dr = dt.NewRow();
                dr["A"] = ex.Message;
                dt.Rows.Add(dr);
                dt.TableName = "异常";
                return dt;
                //return null;
            }
        }

        public DataSet OpenDataSet(string sqlstr)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = sqlstr;
                DataSet ds = authority.Query(sql);
                ds.DataSetName = "SUCCESS";
                return ds;
            }
            catch (Exception ex)
            {
                DataSet ds = new DataSet();
                ds.DataSetName = ex.Message;
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return ds;
            }
        }

        #endregion

        #region 用户、角色权限配置

        public string DeleteUserRights(string UserId)
        {
            try
            {
                if (UserId.Trim() == string.Empty)
                {
                    throw new ArgumentException("UserCode not allow empty");
                }
                //authority = base.GetDal.AuthorityDal;
                string sql = "exec sp_SYDeleteUserRights '@UserId'";
                sql = sql.Replace("@UserId", UserId);
                DataTable dt = authority.GetSingle(sql);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return "";
                //throw;
            }
        }

        public string InsertUserRights(string userId, string funId)
        {
            try
            {
                if (userId.Trim() == string.Empty || funId.Trim() == string.Empty)
                {
                    throw new ArgumentException("UserId or FunId not allow empty");
                }
                //authority = base.GetDal.AuthorityDal;
                string sql = "exec sp_SYInsertUserRights '@UserId','@FunId'";
                sql = sql.Replace("@UserId", userId);
                sql = sql.Replace("@FunId", funId);
                DataTable dt = authority.GetSingle(sql);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return "";
                //throw;
            }
        }

        public string DeleteRoleRights(string roleId)
        {
            try
            {
                if (roleId.Trim() == string.Empty)
                {
                    throw new ArgumentException("roleId not allow empty");
                }
                //authority = base.GetDal.AuthorityDal;
                string sql = "exec sp_SYDeleteRoleRights '@RoleId'";
                sql = sql.Replace("@RoleId", roleId);
                DataTable dt = authority.GetSingle(sql);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return "";
                //throw;
            }
        }

        public string InsertRoleRights(string roleId, string funId)
        {
            try
            {
                if (roleId.Trim() == string.Empty || funId.Trim() == string.Empty)
                {
                    throw new ArgumentException("roleId or FunId not allow empty");
                }
                //authority = base.GetDal.AuthorityDal;
                string sql = "exec sp_SYInsertRoleRights '@RoleId','@FunId'";
                sql = sql.Replace("@RoleId", roleId);
                sql = sql.Replace("@FunId", funId);
                DataTable dt = authority.GetSingle(sql);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return "";
                //throw;
            }
        }

        public bool SaveRoleRight(DataTable dt)
        {
            return authority.InsertDataBatch(dt, new string[] { "RoleId", "FunId" });
        }

        public bool SaveUserRight(DataTable dt)
        {
            return authority.InsertDataBatch(dt, new string[] { "UserId", "FunId" });
        }


        #endregion

        #region 登录

        public DataTable GetUserGroupByUserCode(string userCode)
        {
            try
            {
                //string sql = "select c.treecode,c.code,c.Id from t_SYUserGroup a inner join t_SYUsers b on a.userid=b.id"
                //    + " inner join t_SYGroup c on a.groupid=c.id where b.code=@userCode "
                //    + " union select b.treecode,b.treecode,b.Id from t_SYUsers a inner join t_SYGroup b on a.typecode=b.treecode where a.code=@userCode";

                string sql = "SELECT DISTINCT * FROM ("
                    + " select c.treecode,c.code,c.Id,c.Brand from t_SYGroupGroup a inner join t_SYGroup b on a.GId=b.id"
                    + " inner join t_SYGroup c on a.groupid=c.id AND c.Del=0"
                    + " inner join t_syusers d ON b.treeCode=d.TypeCode"
                    + " where d.code=@userCode"
                    + " UNION"
                    + " select c.treecode,c.code,c.Id,c.Brand from t_SYUserGroup a inner join t_SYUsers b on a.userid=b.id"
                    + " inner join t_SYGroup c on a.groupid=c.id AND c.Del=0 where b.code=@userCode"
                    + " UNION"
                    + " select b.treecode,b.code,b.Id,b.Brand from t_SYUsers a inner join t_SYGroup b on a.typecode=b.treecode AND b.Del=0 where a.code=@userCode"
                    + " UNION"
                    + " select b.treecode,b.Code,b.Id,b.Brand from t_SYUsers a inner join "
                    + " (select a.treecode treecodeb,b.* from t_SYGroup a inner join t_SYGroup b on a.TreeCode=b.PCode) b on a.typecode=b.treecodeb"
                    + " AND b.Del=0 where a.code=@userCode"
                    + " ) a";

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("userCode", userCode);
                DataTable dt = authority.Query(sql, param);
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        public DataTable GetUserAllRights(string uid)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select * from v_SYUserrights where userId=@uid and Id not in (select FunId from t_SYUserRightRemove where userId=@uid1)";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("uid", uid);
                param[1] = new SqlParameter("uid1", uid);
                DataTable dt = authority.Query(sql, param);
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
            //throw new NotImplementedException();
        }

        public DataTable GetUserMod(string userId)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select * from v_SYUserMod where UserId=@userId and isnull(Hide,0)=0 and FunId not in (select FunId from t_SYUserRightRemove where userId=@userId1) order by Indexes";
                //string sql = "select a.Code,b.UserId,b.FuncId,c.* from t_SYUsers a inner join t_SYUserFunc b on a.id=b.userid inner join t_SYFunction c on b.FuncId=c.id where a.Id=@UserId and (RightType='file' or RightType='mod')";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("userId", userId);
                param[1] = new SqlParameter("userId1", userId);
                DataTable dt = authority.Query(sql, param);
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public bool Login(string uid, string pwd, string IP, string key, DateTime clienttime, out string msgcode, double clientversion)
        {
            try
            {
                //double version = double.Parse(ConfigurationManager.AppSettings["Version"].ToString());
                //if (version>clientversion)
                //{
                //    msgcode = "10003";
                //    return false;
                //}
                //TimeSpan ts = DateTime.Now - clienttime;
                //if (Math.Abs(ts.Minutes) > 1)
                //{
                //    msgcode = "10001";
                //    return false;
                //}
                string struid = StringHandler.DesDecrypt(uid, key);
                string strpwd = StringHandler.DesDecrypt(pwd, key);
                //authority = base.GetDal.AuthorityDal;
                string sql = "select * from t_SYUsers where Code=@uid and PW=@pwd and Del<>1";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("uid", struid);
                param[1] = new SqlParameter("pwd", strpwd);
                DataTable userdt = authority.Query(sql, param);
                if (userdt == null)
                {
                    msgcode = "10002";
                    return false;
                }
                msgcode = "00000";

                if (userdt.Rows.Count > 0)
                {
                    UserCode = struid;
                    this.IP = IP;
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                msgcode = "FFFFF";
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return false;
                //throw;
            }
        }

        public bool Login2(string uid, string pwd, string IP, string key, DateTime clienttime, out string msgcode)
        {
            try
            {
                string struid = StringHandler.DesDecrypt(uid, key);
                string sql = "select * from t_SYUsers where Code=@uid and PW=@pwd and Del<>1";
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("uid", struid);
                param[1] = new SqlParameter("pwd", pwd);
                DataTable userdt = authority.Query(sql, param);
                if (userdt == null)
                {
                    msgcode = "10002";
                    return false;
                }
                msgcode = "00000";

                if (userdt.Rows.Count > 0)
                {
                    UserCode = struid;
                    this.IP = IP;
                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                msgcode = "FFFFF";
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return false;
                //throw;
            }
        }

        public DataTable GetUsersByCode(string userCode)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select * from t_SYUsers where Code=@userCode";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("userCode", userCode);
                DataTable dt = authority.Query(sql, param);
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// <para>创建人：方君业</para>
        /// <para>日期：2012-10-25</para>
        /// <para>通过用户Id获取用户角色</para>
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户角色DataTable</returns>
        public DataTable GetUserRoleByUserId(string userId)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select a.*,b.Code RoleCode,b.Name RoleName from t_SYUserRole a inner join t_SYRole b on a.RoleId=b.Id where a.UserId=@userId";
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("userId", userId);
                DataTable dt = authority.Query(sql, param);
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public DataTable GetAllUser()
        {
            try
            {
                string sql = "select * from t_SYUsers where Del<>1";
                DataTable dt = authority.GetSingle(sql);
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        #endregion

        # region 主从表配置

        public DataTable GetSysModManage_ConfigBySerialno(string serialno)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModManage_Config where 1<>1";
                else
                    sql = "select * from t_SYModManage_Config where serialno='" + serialno + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManage_Config";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public DataTable GetSysModManagedl_MBySerialno(string serialno)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModManagedl_M where 1<>1";
                else
                    sql = "select * from t_SYModManagedl_M where serialno='" + serialno + "' order by SNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManagedl_M";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public DataTable GetSysModManagedl_BBySerialno(string serialno)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModManagedl_B where 1<>1";
                else
                    sql = "select * from t_SYModManagedl_B where serialno='" + serialno + "'  order by PageNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManagedl_B";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public DataTable GetSysModManagedl_DBySerialno(string serialno)
        {
            try
            {
                authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModManagedl_D where 1<>1";
                else
                    sql = "select * from t_SYModManagedl_D where PGuid in (select Guid from t_SYModManagedl_B where serialno='" + serialno + "')  order by SNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManagedl_D";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public DataTable GetSysModManagedl_DragBySerialno(string serialno)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModManagedl_Drag where 1<>1";
                else
                    sql = "select * from t_SYModManagedl_Drag where PGuid in (select Guid from t_SYModManagedl_B where serialno='" + serialno + "')";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManagedl_Drag";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public DataTable GetSysModManageByModType(string modType)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (modType == "")
                    sql = "select * from t_SYModManage where 1<>1";
                else
                    sql = "select * from t_SYModManage where ModType='" + modType + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManage";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        /// <summary>
        /// <para>创建人：方君业</para>
        /// <para>日期：2012-10-12</para>
        /// <para>检查是否存在重复模块实例名</para>
        /// </summary>
        /// <param name="className">实例名</param>
        /// <param name="v">序列号</param>
        /// <returns>存在重复模块实例名返回真</returns>
        public bool ModManageCheckSameClass(string className, string serialno)
        {
            try
            {
                bool myResult = false;
                //authority = base.GetDal.AuthorityDal;
                string sql = "select 1 from t_SYModManage where ClassName='" + className + "' and serialno<>'" + serialno + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt.Rows.Count > 0)
                    myResult = true;
                return myResult;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return false;
            }
        }

        /// <summary>
        /// <para>创建人：方君业</para>
        /// <para>日期：2012-10-22</para>
        /// <para>获取自定义主从表单后规则信息bySerialno</para>
        /// </summary>
        /// <param name="serialno">外键</param>
        /// <returns>DataTable</returns>
        public DataTable GetSysModManage_BillnoRuleBySerialno(string serialno)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModManage_BillnoRule where 1<>1";
                else
                    sql = "select * from t_SYModManage_BillnoRule where serialno='" + serialno + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManage_BillnoRule";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        # endregion

        # region 基础档案表配置

        public DataTable GetSysModSingle_ConfigBySerialno(string serialno)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModSingle_Config where 1<>1";
                else
                    sql = "select * from t_SYModSingle_Config where serialno='" + serialno + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModSingle_Config";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public DataTable GetSysModSingledl_BBySerialno(string serialno)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModSingledl_B where 1<>1";
                else
                    sql = "select * from t_SYModSingledl_B where serialno='" + serialno + "'  order by PageNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModSingledl_B";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public DataTable GetSysModSingledl_DBySerialno(string serialno)
        {
            try
            {
                authority = base.GetDal.AuthorityDal;
                string sql = "";
                if (serialno == "")
                    sql = "select * from t_SYModSingledl_D where 1<>1";
                else
                    sql = "select * from t_SYModSingledl_D where PGuid in (select Guid from t_SYModSingledl_B where serialno='" + serialno + "')  order by SNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModSingledl_D";
                }
                return dt;

            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        #endregion

        #region 主从表模版

        /// <summary>
        /// 清空集合对象
        /// </summary>
        /// <param name="baseObjects"></param>
        /// <returns></returns>
        public bool ClearBaseObjeces(IDictionary<string, DataTable[]> baseObjects)
        {
            try
            {
                lock (baseObjects)
                {
                    if (!baseObjects.IsReadOnly)
                    {
                        baseObjects.Clear();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return false;
            }
        }

        public DataTable[] GetSyModData_Manage(string className)
        {
             try
             {
                IList<DataTable> dts = new List<DataTable>();
                DataTable dt = null;
                dt = GetSyModManage_ConfigByClass(className);
                dts.Add(dt);

                dt = OpenDataSingle("select a.ModName,a.ClassName,b.* from t_SYModManage a inner join t_SYModManagedl_P b on a.serialno=b.serialno where a.ClassName='" + className + "' order by PageNo", "t_SYModManagedl_P");
                dts.Add(dt);

                dt = OpenDataSingle("select a.ModName,a.ClassName,b.PageNo,b.PageName,b.PageCaption,b.SearchPage,c.* from t_SYModManage a inner join t_SYModManagedl_P b on a.serialno=b.serialno inner join t_SYModManagedl_M c on b.guid=c.pguid where a.ClassName='" + className + "' order by PageNo,SNo", "t_SYModManagedl_M");
                dts.Add(dt);

                dt = GetSyModManagedl_BByClass(className);
                dts.Add(dt);

                dt = GetSyModManagedl_DByClass(className);
                dts.Add(dt);

                dt = GetSyModManagedl_DragByClass(className);
                dt.TableName = "t_SYModManagedl_Drag_D";
                dts.Add(dt);

                dt = OpenDataSingle("select a.ModName,a.ClassName,b.PageName,b.PageCaption,b.DetailTableNick,c.* from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno inner join t_SYModManagedl_Mu c on b.guid=c.pguid where a.ClassName='" + className + "' order by SNo", "t_SYModManagedl_Mu");
                dts.Add(dt);

                dt = OpenDataSingle("select a.ModName,a.ClassName,b.* from t_SYModManage a inner join v_SYModManagedl_S b on a.serialno=b.serialno where a.ClassName='" + className + "'", "t_SYModManagedl_S");
                dts.Add(dt);

                dt = OpenDataSingle("select a.ModName,a.ClassName,b.* from t_SYModManage a inner join t_SYModManagedl_Print b on a.serialno=b.serialno where a.ClassName='" + className + "'", "t_SYModManagedl_Print");
                dts.Add(dt);

                dt = OpenDataSingle("select mu.* from t_SYModManagedl_Mu as mu" +
                                    " left join t_SYModManage_Config c on mu.PGuid=c.Guid" +
                                    " left join t_SYModManage as m on c.Serialno=m.Serialno" +
                                    " where classname='" + className + "'" +
                                    " order by sno", "t_SYModManagedl_Master_Mu");
                dts.Add(dt);

                return dts.ToArray<DataTable>();
             }
             catch (Exception e)
             {
                 log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                 return null;
             }
        }

        public DataTable GetSyModManageByClass(string className)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select * from t_SYModManage where classname='" + className + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManage";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetSyModManage_ConfigByClass(string className)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select a.Modname,a.Classname,b.* from t_SYModManage a left join t_SYModManage_Config b on a.serialno=b.serialno where a.classname='" + className + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManage_Config";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetSyModManagedl_MByClass(string className)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select b.ModName,b.ClassName,a.* from t_SYModManagedl_M a left join t_SYModManage b on a.serialno=b.serialno where b.classname='" + className + "' order by SNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManagedl_M";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetSyModManagedl_BByClass(string className)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select a.ModName,a.ClassName,b.* from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno where a.ClassName='" + className + "' order by PageNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManagedl_B";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetSyModManagedl_DByClass(string className)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select a.ModName,a.ClassName,b.PageName,b.PageCaption,b.SearchPage,b.DetailTableNick,c.* from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno inner join v_SYModManagedl_D c on b.guid=c.pguid where a.ClassName='" + className + "' order by SNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManagedl_D";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetSyModManagedl_DragByClass(string className)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select a.ModName,a.ClassName,b.PageName,c.* from t_SYModManage a inner join t_SYModManagedl_B b on a.serialno=b.serialno inner join t_SYModManagedl_Drag c on b.guid=c.pguid where a.ClassName='" + className + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModManagedl_D";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        #endregion

        #region 基础档案表模版

        public DataTable[] GetSyModData_Single(string className)
        {
            try
            {
                IList<DataTable> dts = new List<DataTable>();
                DataTable dt = null;

                dt = GetSyModSingle_ConfigByClass(className);
                dts.Add(dt);

                dt = GetSyModSingledl_BByClass(className);
                dts.Add(dt);

                dt = GetSyModSingledl_DByClass(className);
                dts.Add(dt);

                dt = OpenDataSingle("select a.ModName,a.ClassName,b.* from t_SYModManage a inner join v_SYModManagedl_S b on a.serialno=b.serialno where a.ClassName='" + className + "'", "t_SYModManagedl_S");
                dts.Add(dt);

                dt = OpenDataSingle("select mu.* from t_SYModManagedl_Mu as mu" +
                                               " left join t_SYModSingle_Config c on mu.PGuid=c.Guid" +
                                               " left join t_SYModManage as m on c.Serialno=m.Serialno" +
                                               " where classname='" + className + "'" +
                                               " order by sno", "t_SYModManagedl_Mu");
                dts.Add(dt);

                dt = OpenDataSingle("select a.ModName,a.ClassName,b.* from t_SYModManage a inner join t_SYModManagedl_Print b on a.serialno=b.serialno where a.ClassName='" + className + "'", "t_SYModManagedl_Print");
                dts.Add(dt);

                return dts.ToArray<DataTable>();
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
            }
        }

        public DataTable GetSyModSingle_ConfigByClass(string className)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select a.Modname,a.Classname,b.* from t_SYModManage a left join t_SYModSingle_Config b on a.serialno=b.serialno where a.classname='" + className + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModSingle_Config";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetSyModSingledl_BByClass(string className)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select a.ModName,a.ClassName,b.* from t_SYModManage a inner join t_SYModSingledl_B b on a.serialno=b.serialno where a.ClassName='" + className + "' order by PageNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModSingledl_B";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        public DataTable GetSyModSingledl_DByClass(string className)
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select a.ModName,a.ClassName,b.PageNo,b.PageName,b.PageCaption,b.SearchPage,c.* from t_SYModManage a inner join t_SYModSingledl_B b on a.serialno=b.serialno inner join t_SYModSingledl_D c on b.guid=c.pguid where a.ClassName='" + className + "' order by PageNo,SNo";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYModSingledl_D";
                }
                return dt;
            }
            catch (Exception ex)
            {
                log.SysErrorSave(log.CurrMethod, this, ex, UserCode, IP);
                return null;
            }
        }

        /// <summary>
        /// <para>创建人：方君业</para>
        /// <para>日期：2012-10-17</para>
        /// <para>判断节点是否存在列表数据</para>
        /// </summary>
        /// <param name="table">列表表名</param>
        /// <param name="treeNodeCode">节点Code</param>
        /// <returns>存在列表数据返回真</returns>
        public bool ModSingleNodeHasList(string table,string treeNodeCode)
        {
            try
            {
                bool myResult = false;
                //authority = base.GetDal.AuthorityDal;
                string sql = "select 1 from " + table + " where TypeCode like '" + treeNodeCode + "%'";
                DataTable dt = authority.GetSingle(sql);
                if (dt.Rows.Count > 0)
                    myResult = true;
                return myResult;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return false;
            }
        }

        /// <summary>
        /// <para>创建人：方君业</para>
        /// <para>日期：2012-10-17</para>
        /// <para>判断节点是否存在子节点</para>
        /// </summary>
        /// <param name="table">树表表名</param>
        /// <param name="treeNodeCode">节点Code</param>
        /// <returns>存在子节点返回真</returns>
        public bool ModSingleNodeHasChild(string table, string treeNodeCode)
        {
            try
            {
                bool myResult = false;
                //authority = base.GetDal.AuthorityDal;
                string sql = "select 1 from " + table + " where PCode ='" + treeNodeCode + "'";
                DataTable dt = authority.GetSingle(sql);
                if (dt.Rows.Count > 0)
                    myResult = true;
                return myResult;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return false;
            }
        }

        /// <summary>
        /// <para>创建人：方君业</para>
        /// <para>日期：2012-10-17</para>
        /// <para>生成新增子节点Code</para>
        /// </summary>
        /// <param name="treetable">树表表名</param>
        /// <param name="pCode">父节点</param>
        /// <returns>返回节点Code</returns>
        public string ModSingleGetTreeCode(string treeTable, string pCode)
        {
            try
            {
                string myResult = "";
                //authority = base.GetDal.AuthorityDal;
                int len = pCode.Length + 3;
                string sql = "select cast(max(TreeCode) as bigint)+1 TreeCode from " + treeTable + " where len(TreeCode)=" + Convert.ToString(len) + " and pcode like '" + pCode + "%'";
                DataTable dt = authority.GetSingle(sql);
                if (dt.Rows[0]["TreeCode"].ToString() != "")
                    myResult = dt.Rows[0]["TreeCode"].ToString();
                else
                    myResult = pCode + "001";
                return myResult;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return "";
            }
        }

        #endregion
       
        /// <summary>
        /// 创建人：黎金来
        /// 日期：2012-12-13
        /// 根据条件，查询系统模块信息
        /// </summary>
        /// <param name="condiction">查询条件，在前面加where</param>
        /// <returns></returns>
        public DataTable GetAllModules(string condiction)
        {            
            string strSQL = " SELECT * FROM t_SYFun " + condiction;
            DataTable dtData = authority.GetSingle(strSQL);
            return dtData;        
        }

        #region 其他

        public DataTable GetControlProperties()
        {
            try
            {
                //authority = base.GetDal.AuthorityDal;
                string sql = "select * from t_SYControlProperties";
                DataTable dt = authority.GetSingle(sql);
                if (dt != null)
                {
                    dt.TableName = "t_SYControlProperties";
                }
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        public DataTable ExecSproc(string sql)
        {
            try
            {
                if (sql.Trim() == string.Empty)
                {
                    throw new ArgumentException("sql not allow empty");
                }
                DataTable dt = authority.GetSingle(sql);
                return dt;
            }
            catch (Exception e)
            {
                log.SysErrorSave(log.CurrMethod + "    查询SQL:" + sql, this, e, UserCode, IP);
                return null;
                //throw;
            }
        }

        #endregion
    }
        
}
