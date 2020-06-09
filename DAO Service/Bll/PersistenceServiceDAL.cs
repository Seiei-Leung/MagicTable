/* Copyright 2009 无忧lwz0721@gmail.com
 * @author 无忧lwz0721@gmail.com
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FireWorkflow.Net.Engine;
using FireWorkflow.Net.Engine.Impl;
using FireWorkflow.Net.Engine.Definition;
using FireWorkflow.Net.Kernel;
using FireWorkflow.Net.Kernel.Impl;
using FireWorkflow.Net.Engine.Persistence;

namespace FireWorkflow.Net.Persistence.OracleDAL
{
    public class PersistenceServiceDAL : IPersistenceService
    {
        //string connectionString = "User Id=ISS;Password=webiss;Data Source=ism";
        string connectionString = Microsoft.ApplicationBlocks.Data.SqlHelper.ConString;
        public PersistenceServiceDAL()
        { }
        public PersistenceServiceDAL(String connName)
        {
            //if (String.IsNullOrEmpty(connName)) throw new Exception("没有配置数据库连接字符串。");
            //connectionString = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
            //if (String.IsNullOrEmpty(connectionString)) throw new Exception("没有配置数据库连接字符串。");
            connectionString = Microsoft.ApplicationBlocks.Data.SqlHelper.ConString;
        }

        #region IRuntimeContextAware 成员
        public RuntimeContext RuntimeContext { get; set; }
        public void setRuntimeContext(RuntimeContext ctx)
        {
            this.RuntimeContext = ctx;
        }

        #endregion

        /******************************************************************************/
        /************                                                        **********/
        /************            Process instance 相关的持久化方法            **********/
        /************            Persistence methods for process instance    **********/
        /************                                                        **********/
        /******************************************************************************/
        /// <summary>
        /// 插入或者更新ProcessInstance流程实例
        /// </summary>
        public bool SaveOrUpdateProcessInstance(IProcessInstance processInstance)
        {
            if (String.IsNullOrEmpty(processInstance.Id))
            {
                ((ProcessInstance)processInstance).Id = Guid.NewGuid().ToString().Replace("-", "");
                string insert = "INSERT INTO T_FF_RT_PROCESSINSTANCE ("+
                    "ID, PROCESS_ID, VERSION, NAME, DISPLAY_NAME, "+
                    "STATE, SUSPENDED, CREATOR_ID, CREATED_TIME, STARTED_TIME, "+
                    "EXPIRED_TIME, END_TIME, PARENT_PROCESSINSTANCE_ID, PARENT_TASKINSTANCE_ID"+
                    ") VALUES(@ID,@PROCESS_ID,@VERSION,@NAME,@DISPLAY_NAME, @STATE,@SUSPENDED,@CREATOR_ID,@CREATED_TIME,@STARTED_TIME, @EXPIRED_TIME,@END_TIME,@PARENT_PROCESSINSTANCE_ID,@PARENT_TASKINSTANCE_ID)";
    			SqlParameter[] insertParms = { 
    				OracleHelper.NewOracleParameter("@ID", SqlDbType.VarChar, 50, processInstance.Id), 
    				OracleHelper.NewOracleParameter("@PROCESS_ID", SqlDbType.VarChar, 100, processInstance.ProcessId), 
    				OracleHelper.NewOracleParameter("@VERSION", SqlDbType.Int, processInstance.Version), 
    				OracleHelper.NewOracleParameter("@NAME", SqlDbType.VarChar, 100, processInstance.Name), 
    				OracleHelper.NewOracleParameter("@DISPLAY_NAME", SqlDbType.VarChar, 128, processInstance.DisplayName), 
    				OracleHelper.NewOracleParameter("@STATE", SqlDbType.Int, (int)processInstance.State), 
    				OracleHelper.NewOracleParameter("@SUSPENDED", SqlDbType.Int, OracleHelper.OraBit(processInstance.IsSuspended())), 
    				OracleHelper.NewOracleParameter("@CREATOR_ID", SqlDbType.VarChar, 50, processInstance.CreatorId), 
    				OracleHelper.NewOracleParameter("@CREATED_TIME", SqlDbType.DateTime, 11, processInstance.CreatedTime), 
    				OracleHelper.NewOracleParameter("@STARTED_TIME", SqlDbType.DateTime, 11, processInstance.StartedTime), 
    				OracleHelper.NewOracleParameter("@EXPIRED_TIME", SqlDbType.DateTime, 11, processInstance.ExpiredTime), 
    				OracleHelper.NewOracleParameter("@END_TIME", SqlDbType.DateTime, 11, processInstance.EndTime), 
    				OracleHelper.NewOracleParameter("@PARENT_PROCESSINSTANCE_ID", SqlDbType.VarChar, 50, processInstance.ParentProcessInstanceId), 
    				OracleHelper.NewOracleParameter("@PARENT_TASKINSTANCE_ID", SqlDbType.VarChar, 50, processInstance.ParentTaskInstanceId)
    			};
    			if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
    				return false;
    			else return true;
            }
            else
            {
               string update = "UPDATE T_FF_RT_PROCESSINSTANCE SET "+
                    "PROCESS_ID=@PROCESS_ID, VERSION=@VERSION, NAME=@NAME, DISPLAY_NAME=@DISPLAY_NAME, STATE=@STATE, " +
                    "SUSPENDED=@SUSPENDED, CREATOR_ID=@CREATOR_ID, CREATED_TIME=@CREATED_TIME, STARTED_TIME=@STARTED_TIME, EXPIRED_TIME=@EXPIRED_TIME, " +
                    "END_TIME=@END_TIME, PARENT_PROCESSINSTANCE_ID=@PARENT_PROCESSINSTANCE_ID, PARENT_TASKINSTANCE_ID=@PARENT_TASKINSTANCE_ID WHERE ID=ID";
    			SqlParameter[] updateParms = { 
    				OracleHelper.NewOracleParameter("@PROCESS_ID", SqlDbType.VarChar, 100, processInstance.ProcessId), 
    				OracleHelper.NewOracleParameter("@VERSION", SqlDbType.Int, processInstance.Version), 
    				OracleHelper.NewOracleParameter("@NAME", SqlDbType.VarChar, 100, processInstance.Name), 
    				OracleHelper.NewOracleParameter("@DISPLAY_NAME", SqlDbType.VarChar, 128, processInstance.DisplayName), 
    				OracleHelper.NewOracleParameter("@STATE", SqlDbType.Int, (int)processInstance.State), 
    				OracleHelper.NewOracleParameter("@SUSPENDED", SqlDbType.Int, OracleHelper.OraBit(processInstance.IsSuspended())), 
    				OracleHelper.NewOracleParameter("@CREATOR_ID", SqlDbType.VarChar, 50, processInstance.CreatorId), 
    				OracleHelper.NewOracleParameter("@CREATED_TIME", SqlDbType.DateTime, 11, processInstance.CreatedTime), 
    				OracleHelper.NewOracleParameter("@STARTED_TIME", SqlDbType.DateTime, 11, processInstance.StartedTime), 
    				OracleHelper.NewOracleParameter("@EXPIRED_TIME", SqlDbType.DateTime, 11, processInstance.ExpiredTime), 
    				OracleHelper.NewOracleParameter("@END_TIME", SqlDbType.DateTime, 11, processInstance.EndTime), 
    				OracleHelper.NewOracleParameter("@PARENT_PROCESSINSTANCE_ID", SqlDbType.VarChar, 50, processInstance.ParentProcessInstanceId), 
    				OracleHelper.NewOracleParameter("@PARENT_TASKINSTANCE_ID", SqlDbType.VarChar, 50, processInstance.ParentTaskInstanceId),
    				OracleHelper.NewOracleParameter("@ID", SqlDbType.VarChar, 50, processInstance.Id)
    			};
    			if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
    				return false;
    			else return true;
            }
        }

        /// <summary>
        /// 通过ID获得“活的”ProcessInstance对象。
        /// “活的”是指ProcessInstance.state=INITIALIZED Or ProcessInstance.state=STARTED Or ProcessInstance=SUSPENDED的流程实例
        /// </summary>
        public IProcessInstance FindAliveProcessInstanceById(String id) {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = String.Format("SELECT * FROM T_FF_RT_PROCESSINSTANCE WHERE ID=@ID and  state in ({0},{1})",
                    (int)ProcessInstanceEnum.INITIALIZED,
                    (int)ProcessInstanceEnum.RUNNING);
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@ID", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        IProcessInstance info = OracleDataReaderToInfo.GetProcessInstance(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 通过ID获得ProcessInstance对象。
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IProcessInstance FindProcessInstanceById(String id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "SELECT * FROM T_FF_RT_PROCESSINSTANCE WHERE ID=@ID";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@ID", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        IProcessInstance info = OracleDataReaderToInfo.GetProcessInstance(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 获得操作员发起的工作流实例总数量
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="creatorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <returns></returns>
        public Int32 GetProcessInstanceCountByCreatorId(String creatorId, String publishUser)
        {
            return 0;
        }

        /// <summary>
        /// 获得工作流发布者发起的所有流程定义的工作流实例列表（分页）
        /// </summary>
        /// <param name="publishUser">工作流发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public List<IProcessInstance> FindProcessInstanceListByPublishUser(String publishUser, int pageSize, int pageNumber)
        {
            return FindProcessInstanceListByCreatorId("",publishUser,pageSize,pageNumber);
        }

        /// <summary>
        /// 获得操作员发起的工作流实例列表(运行中)（分页）
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="creatorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public List<IProcessInstance> FindProcessInstanceListByCreatorId(String creatorId, String publishUser, int pageSize, int pageNumber)
        {
            int sum = 0;
            List<IProcessInstance> _IProcessInstances = new List<IProcessInstance>();

            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("a.creator_id", CSharpType.String, creatorId));
            queryField.Add(new QueryFieldInfo("b.publish_user", CSharpType.String, publishUser));
            QueryInfo queryInfo = OracleHelper.GetFormatQuery(queryField);

            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                reader = OracleHelper.ExecuteReader(conn, pageNumber, pageSize, out sum, 
                    "T_FF_RT_PROCESSINSTANCE a,t_ff_df_workflowdef b", 
                    "a.*,b.publish_user",
                    String.Format("a.process_id=b.process_id and a.version=b.version {0}", queryInfo.QueryStringAnd), 
                    "a.created_time desc",
                    queryInfo.ListQueryParameters == null ? null : queryInfo.ListQueryParameters.ToArray());

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        IProcessInstance _IProcessInstance = OracleDataReaderToInfo.GetProcessInstance(reader);
                        _IProcessInstances.Add(_IProcessInstance);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null) reader.Close();
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return _IProcessInstances;
        }

        /// <summary>
        /// 查找并返回同一个业务流程的所有实例
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="processId">The id of the process definition.</param>
        /// <returns>A list of processInstance</returns>
        public List<IProcessInstance> FindProcessInstancesByProcessId(String processId)
        {
            List<IProcessInstance> infos = new List<IProcessInstance>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_processinstance where process_id=@process_id order by created_time";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@process_id", SqlDbType.VarChar, 100, processId)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IProcessInstance info = OracleDataReaderToInfo.GetProcessInstance(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查找并返回同一个指定版本业务流程的所有实例
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        /// <param name="processId">The id of the process definition.</param>
        /// <param name="version">版本号</param>
        /// <returns>A list of processInstance</returns>
        public List<IProcessInstance> FindProcessInstancesByProcessIdAndVersion(String processId, int version)
        {
            List<IProcessInstance> infos = new List<IProcessInstance>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_processinstance where process_id=@process_id and version=@version order by created_time";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@process_id", SqlDbType.VarChar, 100, processId),
                        OracleHelper.NewOracleParameter("@version", SqlDbType.Int, version)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IProcessInstance info = OracleDataReaderToInfo.GetProcessInstance(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 计算活动的子流程实例的数量
        /// </summary>
        /// <param name="taskInstanceId">父TaskInstance的Id</param>
        /// <returns></returns>
        public int GetAliveProcessInstanceCountForParentTaskInstance(String taskInstanceId)
        {
            String select = String.Format("select count(*) from t_ff_rt_processinstance where parent_taskinstance_id=@parent_taskinstance_id and state in({0},{1})",
                (int)ProcessInstanceEnum.INITIALIZED, (int)ProcessInstanceEnum.RUNNING);
            SqlParameter[] selectParms = { 
				OracleHelper.NewOracleParameter("@parent_taskinstance_id", SqlDbType.VarChar, 50, taskInstanceId)
		    };

            return OracleHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// 终止流程实例。将流程实例、活动的TaskInstance、活动的WorkItem的状态设置为CANCELED；并删除所有的token
        /// </summary>
        public bool AbortProcessInstance(ProcessInstance processInstance)
        {
            SqlTransaction transaction = OracleHelper.GetOracleTransaction(this.connectionString);
            try
            {
                // 更新流程状态，设置为canceled
                DateTime now = this.RuntimeContext.CalendarService.getSysDate();
                String processSql = "update t_ff_rt_processinstance set state=" + (int)ProcessInstanceEnum.CANCELED
                        + ",end_time=@end_time where id=@id ";
                int count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, processSql,
                    OracleHelper.NewOracleParameter("@end_time", SqlDbType.DateTime, now),
                    OracleHelper.NewOracleParameter("@id", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }


                // 更新所有的任务实例状态为canceled
                String taskSql = " update t_ff_rt_taskinstance set state=" + (int)TaskInstanceStateEnum.CANCELED
                        + ",end_time=@end_time,can_be_withdrawn=0 " + "  where processinstance_id=@processinstance_id and (state=0 or state=1)";
                count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, taskSql,
                    OracleHelper.NewOracleParameter("@end_time", SqlDbType.DateTime, now),
                    OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 更新所有工作项的状态为canceled
                String workItemSql = " update t_ff_rt_workitem set state="
                        + (int)WorkItemEnum.CANCELED
                        + ",end_time=@end_time  "
                        + " where taskinstance_id in (select a.id  from t_ff_rt_taskinstance a,t_ff_rt_workitem b where a.id=b.taskinstance_id and a.processinstance_id=@processinstance_id ) and (state=0 or state=1) ";
                count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, workItemSql,
                    OracleHelper.NewOracleParameter("@end_time", SqlDbType.DateTime, now),
                    OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 删除所有的token
                String tokenSql = " delete from t_ff_rt_token where processinstance_id=@processinstance_id  ";
                count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, tokenSql,
                    OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 数据库操作成功后，更新对象的状态
                processInstance.State=ProcessInstanceEnum.CANCELED;

                transaction.Commit();
                return true;

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 挂起流程实例
        /// </summary>
        public bool SuspendProcessInstance(ProcessInstance processInstance) {
            SqlTransaction transaction = OracleHelper.GetOracleTransaction(this.connectionString);
            try
            {

                String sql = " update t_ff_rt_processinstance set suspended=1 where id=@id ";
                int count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sql,
                    OracleHelper.NewOracleParameter("@id", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 挂起对应的TaskInstance
                String sql2 = " update t_ff_rt_taskinstance set suspended=1 where processinstance_id=@processinstance_id ";
                count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sql2,
                    OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                processInstance.Suspended=true;

                transaction.Commit();
                return true;

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 恢复流程实例
        /// </summary>
        public bool RestoreProcessInstance(ProcessInstance processInstance)
        {
            SqlTransaction transaction = OracleHelper.GetOracleTransaction(this.connectionString);
            try
            {
                String sql = " update t_ff_rt_processinstance set suspended=0 where id=@id ";
                int count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sql,
                    OracleHelper.NewOracleParameter("@id", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                // 恢复对应的TaskInstance
                String sql2 = " update t_ff_rt_taskinstance set suspended=0 where processinstance_id=@processinstance_id";
                count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sql2,
                    OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, processInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                processInstance.Suspended=false;

                transaction.Commit();
                return true;

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /******************************************************************************/
        /************                                                        **********/
        /************            task instance 相关的持久化方法               **********/
        /************            Persistence methods for task instance       **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// <para>插入或者更新TaskInstance。</para>
        /// <para>Save or update task instance. If the taskInstance.id is null then insert a new task instance record</para>
        /// <para>and generate a new id for it { throw new NotImplementedException(); }</para>
        /// <para>otherwise update the existent one. </para>
        /// </summary>
        public bool SaveOrUpdateTaskInstance(ITaskInstance taskInstance)
        {
            if (String.IsNullOrEmpty(taskInstance.Id))
            {
                ((TaskInstance)taskInstance).Id = Guid.NewGuid().ToString().Replace("-", "");
                string insert = "INSERT INTO T_FF_RT_TASKINSTANCE (" +
                "ID, BIZ_TYPE, TASK_ID, ACTIVITY_ID, NAME, " +
                "DISPLAY_NAME, STATE, SUSPENDED, TASK_TYPE, CREATED_TIME, " +
                "STARTED_TIME, EXPIRED_TIME, END_TIME, ASSIGNMENT_STRATEGY, PROCESSINSTANCE_ID, " +
                "PROCESS_ID, VERSION, TARGET_ACTIVITY_ID, FROM_ACTIVITY_ID, STEP_NUMBER, " +
                "CAN_BE_WITHDRAWN, BIZ_INFO )VALUES(@ID, @BIZ_TYPE, @TASK_ID, @ACTIVITY_ID, @NAME, @DISPLAY_NAME, @STATE, @SUSPENDED, @TASK_TYPE, @CREATED_TIME, @STARTED_TIME, @EXPIRED_TIME, @END_TIME, @ASSIGNMENT_STRATEGY, @PROCESSINSTANCE_ID, @PROCESS_ID, @VERSION, @TARGET_ACTIVITY_ID, @FROM_ACTIVITY_ID, @STEP_numeric, @CAN_BE_WITHDRAWN, @BIZ_INFO)";
    			SqlParameter[] insertParms = { 
    				OracleHelper.NewOracleParameter("@ID", SqlDbType.VarChar, 50, taskInstance.Id), 
    				OracleHelper.NewOracleParameter("@BIZ_TYPE", SqlDbType.VarChar, 250, taskInstance.GetType().Name), 
    				OracleHelper.NewOracleParameter("@TASK_ID", SqlDbType.VarChar, 300, taskInstance.TaskId), 
    				OracleHelper.NewOracleParameter("@ACTIVITY_ID", SqlDbType.VarChar, 200, taskInstance.ActivityId), 
    				OracleHelper.NewOracleParameter("@NAME", SqlDbType.VarChar, 100, taskInstance.Name), 
    				OracleHelper.NewOracleParameter("@DISPLAY_NAME", SqlDbType.VarChar, 128, taskInstance.DisplayName), 
    				OracleHelper.NewOracleParameter("@STATE", SqlDbType.Int, (int)taskInstance.State), 
    				OracleHelper.NewOracleParameter("@SUSPENDED", SqlDbType.Int, OracleHelper.OraBit(taskInstance.IsSuspended())), 
    				OracleHelper.NewOracleParameter("@TASK_TYPE", SqlDbType.VarChar, 10, taskInstance.TaskType), 
    				OracleHelper.NewOracleParameter("@CREATED_TIME", SqlDbType.DateTime, taskInstance.CreatedTime), 
    				OracleHelper.NewOracleParameter("@STARTED_TIME", SqlDbType.DateTime, taskInstance.StartedTime), 
    				OracleHelper.NewOracleParameter("@EXPIRED_TIME", SqlDbType.DateTime, taskInstance.ExpiredTime), 
    				OracleHelper.NewOracleParameter("@END_TIME", SqlDbType.DateTime, taskInstance.EndTime), 
    				OracleHelper.NewOracleParameter("@ASSIGNMENT_STRATEGY", SqlDbType.VarChar, 10, taskInstance.AssignmentStrategy), 
    				OracleHelper.NewOracleParameter("@PROCESSINSTANCE_ID", SqlDbType.VarChar, 50, taskInstance.ProcessInstanceId), 
    				OracleHelper.NewOracleParameter("@PROCESS_ID", SqlDbType.VarChar, 100, taskInstance.ProcessId), 
    				OracleHelper.NewOracleParameter("@VERSION", SqlDbType.Int, taskInstance.Version), 
    				OracleHelper.NewOracleParameter("@TARGET_ACTIVITY_ID", SqlDbType.VarChar, 100, taskInstance.TargetActivityId), 
    				OracleHelper.NewOracleParameter("@FROM_ACTIVITY_ID", SqlDbType.VarChar, 600, ((TaskInstance) taskInstance).FromActivityId), 
    				OracleHelper.NewOracleParameter("@STEP_numeric", SqlDbType.Int, taskInstance.StepNumber), 
    				OracleHelper.NewOracleParameter("@CAN_BE_WITHDRAWN", SqlDbType.Int, OracleHelper.OraBit(((TaskInstance) taskInstance).CanBeWithdrawn)),
    				OracleHelper.NewOracleParameter("@BIZ_INFO", SqlDbType.VarChar, 500, taskInstance.BizInfo)
    			};
    			if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
    				return false;
    			else return true;
            }
            else
            {
                string update = "UPDATE T_FF_RT_TASKINSTANCE SET " +
                "BIZ_TYPE=@2, TASK_ID=@3, ACTIVITY_ID=@4, NAME=@5, DISPLAY_NAME=@6, " +
                "STATE=@7, SUSPENDED=@8, TASK_TYPE=@9, CREATED_TIME=@10, STARTED_TIME=@11, " +
                "EXPIRED_TIME=@12, END_TIME=@13, ASSIGNMENT_STRATEGY=@14, PROCESSINSTANCE_ID=@15, PROCESS_ID=@16, " +
                "VERSION=@17, TARGET_ACTIVITY_ID=@18, FROM_ACTIVITY_ID=@19, STEP_NUMBER=@20, CAN_BE_WITHDRAWN=@21, BIZ_INFO=@22" +
                " WHERE ID=@1";
                SqlParameter[] updateParms = { 
                    OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 250, taskInstance.GetType().Name), 
                    OracleHelper.NewOracleParameter("@3", SqlDbType.VarChar, 300, taskInstance.TaskId), 
                    OracleHelper.NewOracleParameter("@4", SqlDbType.VarChar, 200, taskInstance.ActivityId), 
                    OracleHelper.NewOracleParameter("@5", SqlDbType.VarChar, 100, taskInstance.Name), 
                    OracleHelper.NewOracleParameter("@6", SqlDbType.VarChar, 128, taskInstance.DisplayName), 
                    OracleHelper.NewOracleParameter("@7", SqlDbType.Int, (int)taskInstance.State), 
                    OracleHelper.NewOracleParameter("@8", SqlDbType.Int, OracleHelper.OraBit(taskInstance.IsSuspended())), 
                    OracleHelper.NewOracleParameter("@9", SqlDbType.VarChar, 10, taskInstance.TaskType), 
                    OracleHelper.NewOracleParameter("@10", SqlDbType.DateTime, taskInstance.CreatedTime), 
                    OracleHelper.NewOracleParameter("@11", SqlDbType.DateTime, taskInstance.StartedTime), 
                    OracleHelper.NewOracleParameter("@12", SqlDbType.DateTime, taskInstance.ExpiredTime), 
                    OracleHelper.NewOracleParameter("@13", SqlDbType.DateTime, taskInstance.EndTime), 
                    OracleHelper.NewOracleParameter("@14", SqlDbType.VarChar, 10, taskInstance.AssignmentStrategy), 
                    OracleHelper.NewOracleParameter("@15", SqlDbType.VarChar, 50, taskInstance.ProcessInstanceId), 
                    OracleHelper.NewOracleParameter("@16", SqlDbType.VarChar, 100, taskInstance.ProcessId), 
                    OracleHelper.NewOracleParameter("@17", SqlDbType.Int, taskInstance.Version), 
                    OracleHelper.NewOracleParameter("@18", SqlDbType.VarChar, 100, taskInstance.TargetActivityId), 
                    OracleHelper.NewOracleParameter("@19", SqlDbType.VarChar, 600, ((TaskInstance) taskInstance).FromActivityId), 
                    OracleHelper.NewOracleParameter("@20", SqlDbType.Int, taskInstance.StepNumber), 
                    OracleHelper.NewOracleParameter("@21", SqlDbType.Int, OracleHelper.OraBit(((TaskInstance) taskInstance).CanBeWithdrawn)),
                    OracleHelper.NewOracleParameter("@22", SqlDbType.VarChar, 500, taskInstance.BizInfo),
                    OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, taskInstance.Id)
    			};
                if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }

        /// <summary>
        /// 终止TaskInstance。将任务实例及其所有的“活的”WorkItem变成Canceled状态。
        /// "活的"WorkItem 是指状态等于INITIALIZED、STARTED或者SUSPENDED的WorkItem.
        /// </summary>
        public bool AbortTaskInstance(TaskInstance taskInstance)
        {
            SqlTransaction transaction = OracleHelper.GetOracleTransaction(connectionString);
            try
            {
                String sql = "update t_ff_rt_taskinstance set state=" + (int)TaskInstanceStateEnum.CANCELED + " ,end_time=@1 where id=@2 and (state=0 or state=1)";
                int count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, sql,
                    OracleHelper.NewOracleParameter("@1", SqlDbType.DateTime, this.RuntimeContext.CalendarService.getSysDate()),
                    OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 50, taskInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }


                // 将与之关联的WorkItem取消掉
                String workItemSql = " update t_ff_rt_workitem set state=" + (int)WorkItemEnum.CANCELED + ",end_time=@1  "
                        + " where taskinstance_id =@2 ";
                count = OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, workItemSql,
                    OracleHelper.NewOracleParameter("@1", SqlDbType.DateTime, this.RuntimeContext.CalendarService.getSysDate()),
                    OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 50, taskInstance.Id)
                    );
                if (count <= 0)
                {
                    transaction.Rollback();
                    return false;
                }

                taskInstance.State=TaskInstanceStateEnum.CANCELED;

                transaction.Commit();
                return true;

            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 返回“活的”TaskInstance。
        /// “活的”是指TaskInstance.state=INITIALIZED Or TaskInstance.state=STARTED 。
        /// </summary>
        public ITaskInstance FindAliveTaskInstanceById(String id) {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_rt_taskinstance where id=@1 and  (state=" + (int)TaskInstanceStateEnum.INITIALIZED
                        + " or state=" + (int)TaskInstanceStateEnum.RUNNING + " )";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        ITaskInstance info = OracleDataReaderToInfo.GetTaskInstance(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 获得activity的“活的”TaskInstance的数量
        /// “活的”是指TaskInstance.state=INITIALIZED Or TaskInstance.state=STARTED 。
        /// </summary>
        public int GetAliveTaskInstanceCountForActivity(String processInstanceId, String activityId)
        {
            String select = "select count(*) from T_FF_RT_TASKINSTANCE where " + " (state=" + (int)TaskInstanceStateEnum.INITIALIZED
                + " or state=" + (int)TaskInstanceStateEnum.RUNNING + ")" + " and activity_id=@1 and processinstance_id=@2";
            SqlParameter[] selectParms = { 
				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 200, activityId), 
				OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 50, processInstanceId), 
		    };

            return OracleHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// 返回某个Task已经结束的TaskInstance的数量
        /// “已经结束”是指TaskInstance.state=COMPLETED。
        /// </summary>
        public int GetCompletedTaskInstanceCountForTask(String processInstanceId, String taskId) {
            String select = "select count(*) from T_FF_RT_TASKINSTANCE where state=" + (int)TaskInstanceStateEnum.COMPLETED
                        + " and task_id=@1 and processinstance_id=@2 ";
            SqlParameter[] selectParms = { 
				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 300, taskId), 
				OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 50, processInstanceId), 
		    };

            return OracleHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// Find the task instance by id
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public ITaskInstance FindTaskInstanceById(String id) {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_rt_taskinstance where id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        ITaskInstance info = OracleDataReaderToInfo.GetTaskInstance(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// 查询流程实例的所有的TaskInstance,如果activityId不为空，则返回该流程实例下指定环节的TaskInstance
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public List<ITaskInstance> FindTaskInstancesForProcessInstance(String processInstanceId, String activityId)
        {
            List<ITaskInstance> infos = new List<ITaskInstance>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select;
                SqlParameter[] selectParms;
                if (String.IsNullOrEmpty(activityId))
                {
                    select = "select * from t_ff_rt_taskinstance where processinstance_id=@1 order by step_number,end_time";
                    selectParms = new SqlParameter[]{ 
        				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, processInstanceId)
        		    };
                }
                else
                {
                    select = "select * from t_ff_rt_taskinstance where processinstance_id=@1 and activity_id=@2 order by step_number,end_time";
                    selectParms = new SqlParameter[]{  
        				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, processInstanceId), 
				        OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 200, activityId)
        		    };
                }
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select, selectParms);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ITaskInstance info = OracleDataReaderToInfo.GetTaskInstance(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查询出同一个stepNumber的所有TaskInstance实例
        /// </summary>
        public List<ITaskInstance> FindTaskInstancesForProcessInstanceByStepNumber(String processInstanceId, Int32 stepNumber)
        {
            List<ITaskInstance> infos = new List<ITaskInstance>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select;
                SqlParameter[] selectParms;
                if (stepNumber<0)
                {
                    select = "select * from t_ff_rt_taskinstance where processinstance_id=@1 order by step_number,end_time";
                    selectParms = new SqlParameter[]{ 
        				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, processInstanceId)
        		    };
                }
                else
                {
                    select = "select * from t_ff_rt_taskinstance where processinstance_id=@1 and step_number=@2 order by step_number,end_time";
                    selectParms = new SqlParameter[]{  
        				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, processInstanceId), 
				        OracleHelper.NewOracleParameter("@2", SqlDbType.Int,stepNumber)
        		    };
                }
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select, selectParms);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ITaskInstance info = OracleDataReaderToInfo.GetTaskInstance(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 调用数据库自身的机制所定TaskInstance实例。
        /// 该方法主要用于工单的签收操作，在签收之前先锁定与之对应的TaskInstance。
        /// </summary>
        public bool LockTaskInstance(String taskInstanceId) {
            //if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, "select * from t_ff_rt_taskinstance where id=@1 for update",
            //    OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)) != 1)
            if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, "select * from t_ff_rt_taskinstance where id=@1",
                OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)) != 1)
                return false;
            else return true;
        }


        /******************************************************************************/
        /************                                                        **********/
        /************            workItem 相关的持久化方法                    **********/
        /************            Persistence methods for workitem            **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// 插入或者更新WorkItem
        /// </summary>
        public bool SaveOrUpdateWorkItem(IWorkItem workitem)
        {
            if (String.IsNullOrEmpty(workitem.Id))
            {
                ((WorkItem)workitem).Id = Guid.NewGuid().ToString().Replace("-", ""); ;

                string insert = "INSERT INTO T_FF_RT_WORKITEM (" +
                    "ID, STATE, CREATED_TIME, CLAIMED_TIME, END_TIME, " +
                    "ACTOR_ID, COMMENTS, TASKINSTANCE_ID )VALUES(@1, @2, @3, @4, @5, @6, @7, @8)";
                SqlParameter[] insertParms = { 
    				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, workitem.Id), 
    				OracleHelper.NewOracleParameter("@2", SqlDbType.Int, (int)workitem.State), 
    				OracleHelper.NewOracleParameter("@3", SqlDbType.DateTime, workitem.CreatedTime), 
    				OracleHelper.NewOracleParameter("@4", SqlDbType.DateTime, workitem.ClaimedTime), 
    				OracleHelper.NewOracleParameter("@5", SqlDbType.DateTime, workitem.EndTime), 
    				OracleHelper.NewOracleParameter("@6", SqlDbType.VarChar, 50, workitem.ActorId), 
    				OracleHelper.NewOracleParameter("@7", SqlDbType.VarChar, 1024, workitem.Comments), 
    				OracleHelper.NewOracleParameter("@8", SqlDbType.VarChar, 50, workitem.TaskInstance.Id)
    			};
                if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_RT_WORKITEM SET " +
                    "STATE=@2, CREATED_TIME=@3, CLAIMED_TIME=@4, END_TIME=@5, ACTOR_ID=@6, " +
                    "COMMENTS=@7, TASKINSTANCE_ID=@8" +
                    " WHERE ID=@1";
                SqlParameter[] updateParms = { 
    				OracleHelper.NewOracleParameter("@2", SqlDbType.Int, (int)workitem.State), 
    				OracleHelper.NewOracleParameter("@3", SqlDbType.DateTime, workitem.CreatedTime), 
    				OracleHelper.NewOracleParameter("@4", SqlDbType.DateTime, workitem.ClaimedTime), 
    				OracleHelper.NewOracleParameter("@5", SqlDbType.DateTime, workitem.EndTime), 
    				OracleHelper.NewOracleParameter("@6", SqlDbType.VarChar, 50, workitem.ActorId), 
    				OracleHelper.NewOracleParameter("@7", SqlDbType.VarChar, 1024, workitem.Comments), 
    				OracleHelper.NewOracleParameter("@8", SqlDbType.VarChar, 50, workitem.TaskInstance.Id),
    				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, workitem.Id)
    			};
                if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }

        /// <summary>
        ///  返回任务实例的所有"活的"WorkItem的数量。
        ///  "活的"WorkItem 是指状态等于INITIALIZED、STARTED或者SUSPENDED的WorkItem。
        /// </summary>
        public Int32 GetAliveWorkItemCountForTaskInstance(String taskInstanceId)
        {
            String select = "select count(*) from t_ff_rt_workitem where taskinstance_id=@1 and (state in (0,1,3))";
            SqlParameter[] selectParms = { 
				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, taskInstanceId),
		    };
            return OracleHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// 查询任务实例的所有"已经结束"WorkItem。
        /// 所以必须有关联条件WorkItem.state=IWorkItem.COMPLTED
        /// </summary>
        /// <param name="taskInstanceId">任务实例Id</param>
        public List<IWorkItem> FindCompletedWorkItemsForTaskInstance(String taskInstanceId)
        {
            List<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_workitem where taskinstance_id=@1 and state=" + (int)WorkItemEnum.COMPLETED;
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)
                        );
                    if (reader != null)
                    {
                        ITaskInstance iTaskInstance = FindTaskInstanceById(taskInstanceId);
                        while (reader.Read())
                        {
                            IWorkItem info = OracleDataReaderToInfo.GetWorkItem(reader);
                            ((WorkItem)info).TaskInstance=iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查询某任务实例的所有WorkItem
        /// </summary>
        public List<IWorkItem> FindWorkItemsForTaskInstance(String taskInstanceId)
        {
            List<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_workitem where taskinstance_id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)
                        );
                    if (reader != null)
                    {
                        ITaskInstance iTaskInstance = FindTaskInstanceById(taskInstanceId);
                        while (reader.Read())
                        {
                            IWorkItem info = OracleDataReaderToInfo.GetWorkItem(reader);
                            ((WorkItem)info).TaskInstance=iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 删除处于初始化状态的workitem。
        /// 此方法用于签收Workitem时，删除其他Actor的WorkItem
        /// </summary>
        public bool DeleteWorkItemsInInitializedState(String taskInstanceId)
        {
            String delete = "delete from t_ff_rt_workitem where taskinstance_id=@1 and  state=" + (int)WorkItemEnum.INITIALIZED;
            SqlParameter[] deleteParms = { 
                 OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, taskInstanceId)
			};
            if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, delete, deleteParms) <= 0)
                return false;
            else return true;
        }

        /// <summary>
        /// Find workItem by id
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IWorkItem FindWorkItemById(String id)
        {
            WorkItem workItem;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = " select * from t_ff_rt_workitem where id=@1 ";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            workItem = OracleDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = FindTaskInstanceById(workItem.TaskInstanceId);
                            if (iTaskInstance != null)
                            {
                                ((WorkItem)workItem).TaskInstance=iTaskInstance;
                            }
                            return workItem;
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Find all workitems for task
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public List<IWorkItem> FindWorkItemsForTask(String taskid)
        {
            List<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and b.task_id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 300, taskid)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = OracleDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = OracleDataReaderToInfo.GetTaskInstance(reader); //FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance=iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 根据操作员的Id返回其待办工单。如果actorId==null，则返回系统所有的待办任务
        /// 待办工单是指状态等于INITIALIZED或STARTED工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public List<IWorkItem> FindTodoWorkItems(String actorId)
        {
            return FindTodoWorkItems(actorId,String.Empty);
        }

        /// <summary>
        /// 查找操作员在某个流程实例中的待办工单。
        /// 如果processInstanceId为空，则等价于调用findTodoWorkItems(String actorId)
        /// 待办工单是指状态等于INITIALIZED或STARTED工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public List<IWorkItem> FindTodoWorkItems(String actorId, String processInstanceId)
        {
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("actor_id", CSharpType.String, actorId));
            queryField.Add(new QueryFieldInfo("a.taskinstance_id", CSharpType.String, processInstanceId));
            QueryInfo queryInfo = OracleHelper.GetFormatQuery(queryField);

            List<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select=string.Format("select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and a.state in ({0},{1}){2}",
                    (int)WorkItemEnum.INITIALIZED, (int)WorkItemEnum.RUNNING, queryInfo.QueryStringAnd);
                
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = OracleDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = OracleDataReaderToInfo.GetTaskInstance(reader); //FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance=iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查找操作员在某个流程某个任务上的待办工单。
        /// actorId，processId，taskId都可以为空（null或者""）,为空的条件将被忽略
        /// 待办工单是指状态等于INITIALIZED或STARTED工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public List<IWorkItem> FindTodoWorkItems(String actorId, String processId, String taskId)
        {
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("actor_id", CSharpType.String, actorId));
            queryField.Add(new QueryFieldInfo("process_id", CSharpType.String, processId));
            queryField.Add(new QueryFieldInfo("task_id", CSharpType.String, taskId));
            QueryInfo queryInfo = OracleHelper.GetFormatQuery(queryField);

            List<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = String.Format(
                    "select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and a.state in ({0},{1}){2}",
                    (int)WorkItemEnum.INITIALIZED,
                    (int)WorkItemEnum.RUNNING,
                    queryInfo.QueryStringAnd);

                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = OracleDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = OracleDataReaderToInfo.GetTaskInstance(reader);//FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance=iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 根据操作员的Id返回其已办工单。如果actorId==null，则返回系统所有的已办任务
        /// 已办工单是指状态等于COMPLETED或CANCELED的工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public List<IWorkItem> FindHaveDoneWorkItems(String actorId)
        {
            return FindHaveDoneWorkItems(actorId, string.Empty);
        }

        /// <summary>
        /// 查找操作员在某个流程实例中的已办工单。
        /// 如果processInstanceId为空，则等价于调用findHaveDoneWorkItems(String actorId)
        /// 已办工单是指状态等于COMPLETED或CANCELED的工单
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public List<IWorkItem> FindHaveDoneWorkItems(String actorId, String processInstanceId)
        {
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("actor_id", CSharpType.String, actorId));
            queryField.Add(new QueryFieldInfo("processinstance_id", CSharpType.String, processInstanceId));
            QueryInfo queryInfo = OracleHelper.GetFormatQuery(queryField);

            List<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = String.Format(
                    "select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and a.state in ({0},{1}){2}",
                    (int)WorkItemEnum.COMPLETED,
                    (int)WorkItemEnum.CANCELED,
                    queryInfo.QueryStringAnd);
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = OracleDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = OracleDataReaderToInfo.GetTaskInstance(reader); //FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance=iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 查找操作员在某个流程某个任务上的已办工单。
        /// actorId，processId，taskId都可以为空（null或者""）,为空的条件将被忽略
        ///  已办工单是指状态等于COMPLETED或CANCELED的工单
        ///  (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public List<IWorkItem> FindHaveDoneWorkItems(String actorId, String processId, String taskId)
        {
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("actor_id", CSharpType.String, actorId));
            queryField.Add(new QueryFieldInfo("process_id", CSharpType.String, processId));
            queryField.Add(new QueryFieldInfo("task_id", CSharpType.String, taskId));
            QueryInfo queryInfo = OracleHelper.GetFormatQuery(queryField);

            List<IWorkItem> infos = new List<IWorkItem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = String.Format(
                    "select a.*,b.* from t_ff_rt_workitem a,t_ff_rt_taskinstance b where a.taskinstance_id=b.id and a.state in ({0},{1}){2}",
                    (int)WorkItemEnum.COMPLETED,
                    (int)WorkItemEnum.CANCELED,
                    queryInfo.QueryStringAnd);

                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IWorkItem info = OracleDataReaderToInfo.GetWorkItem(reader);
                            ITaskInstance iTaskInstance = OracleDataReaderToInfo.GetTaskInstance(reader); //FindTaskInstanceById(((WorkItem)info).TaskInstanceId);
                            ((WorkItem)info).TaskInstance=iTaskInstance;
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /******************************************************************************/
        /************                                                        **********/
        /************            token 相关的持久化方法                       **********/
        /************            Persistence methods for token               **********/
        /************                                                        **********/
        /******************************************************************************/


        public bool SaveOrUpdateToken(IToken token)
        {
            if (String.IsNullOrEmpty(token.Id))
            {
                ((Token)token).Id = Guid.NewGuid().ToString().Replace("-", "");
                string insert = "INSERT INTO T_FF_RT_TOKEN (" +
                    "ID, ALIVE, VALUE, NODE_ID, PROCESSINSTANCE_ID, " +
                    "STEP_NUMBER, FROM_ACTIVITY_ID )VALUES(@1, @2, @3, @4, @5, @6, @7)";
                SqlParameter[] insertParms = { 
    				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, token.Id), 
    				OracleHelper.NewOracleParameter("@2", SqlDbType.Int, OracleHelper.OraBit(token.IsAlive)), 
    				OracleHelper.NewOracleParameter("@3", SqlDbType.Int, token.Value), 
    				OracleHelper.NewOracleParameter("@4", SqlDbType.VarChar, 200, token.NodeId), 
    				OracleHelper.NewOracleParameter("@5", SqlDbType.VarChar, 50, token.ProcessInstanceId), 
    				OracleHelper.NewOracleParameter("@6", SqlDbType.Int, token.StepNumber), 
    				OracleHelper.NewOracleParameter("@7", SqlDbType.VarChar, 100, token.FromActivityId)
    			};
                if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_RT_TOKEN SET " +
                    "ALIVE=@2, VALUE=@3, NODE_ID=@4, PROCESSINSTANCE_ID=@5, STEP_NUMBER=@6, " +
                    "FROM_ACTIVITY_ID=@7" +
                    " WHERE ID=@1";
                SqlParameter[] updateParms = { 
					OracleHelper.NewOracleParameter("@2", SqlDbType.Int, OracleHelper.OraBit(token.IsAlive)), 
					OracleHelper.NewOracleParameter("@3", SqlDbType.Int, token.Value), 
					OracleHelper.NewOracleParameter("@4", SqlDbType.VarChar, 200, token.NodeId), 
					OracleHelper.NewOracleParameter("@5", SqlDbType.VarChar, 50, token.ProcessInstanceId), 
					OracleHelper.NewOracleParameter("@6", SqlDbType.Int, token.StepNumber), 
					OracleHelper.NewOracleParameter("@7", SqlDbType.VarChar, 100, token.FromActivityId),
					OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, token.Id)
				};
                if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }

        /// <summary>
        /// 统计流程任意节点的活动Token的数量。对于Activity节点，该数量只能取值1或者0，大于1表明有流程实例出现异常。
        /// </summary>
        public int GetAliveTokenCountForNode(String processInstanceId, String nodeId)
        {
            String select = String.Format("select count(*) from T_FF_RT_TOKEN where alive=1 and processinstance_id=@1 and node_id =@2",
                ProcessInstanceEnum.INITIALIZED, ProcessInstanceEnum.RUNNING);
            SqlParameter[] selectParms = { 
				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, processInstanceId), 
				OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 200, nodeId)
		    };
            return OracleHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// (Engine没有引用到该方法，提供给业务系统使用，20090303)
        /// </summary>
        public IToken FindTokenById(String id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_rt_token where id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        IToken info = OracleDataReaderToInfo.GetToken(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Find all the tokens for process instance ,and the nodeId of the token must equals to the second argument.
        /// </summary>
        /// <param name="processInstanceId">the id of the process instance</param>
        /// <param name="nodeId">if the nodeId is null ,then return all the tokens of the process instance.</param>
        /// <returns></returns>
        public List<IToken> FindTokensForProcessInstance(String processInstanceId, String nodeId)
        {
            if (String.IsNullOrEmpty(processInstanceId)) return null;
            QueryField queryField = new QueryField();
            queryField.Add(new QueryFieldInfo("processinstance_id", CSharpType.String, processInstanceId));
            queryField.Add(new QueryFieldInfo("node_id", CSharpType.String, nodeId));
            QueryInfo queryInfo = OracleHelper.GetFormatQuery(queryField);

            List<IToken> infos = new List<IToken>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = String.Format("select * from t_ff_rt_token{0}", queryInfo.QueryStringWhere);
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select, ((List<SqlParameter>)queryInfo.ListQueryParameters).ToArray());
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            IToken info = OracleDataReaderToInfo.GetToken(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// 删除某个节点的所有token
        /// </summary>
        public bool DeleteTokensForNode(String processInstanceId, String nodeId)
        {
            String delete = "delete from t_ff_rt_token where processinstance_id = @1 and node_id=@2 ";
            SqlParameter[] deleteParms = { 
				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, processInstanceId),
				OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 200, nodeId)
			};
            if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, delete, deleteParms) != 1)
                return false;
            else return true;
        }

        /// <summary>
        /// 删除某些节点的所有token
        /// </summary>
        public bool DeleteTokensForNodes(String processInstanceId, List<String> nodeIdsList)
        {
            SqlTransaction transaction = OracleHelper.GetOracleTransaction(connectionString);
            try
            {
                String delete = "delete from t_ff_rt_token where processinstance_id = @1 and node_id=@2";
                SqlParameter[] deleteParms = { 
    				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, processInstanceId),
    				OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 200, "")
    			};
                foreach (String item in nodeIdsList)
                {
                    deleteParms[1].Value = item;
                    OracleHelper.ExecuteNonQuery(transaction, CommandType.Text, delete, deleteParms);
                }
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
            finally
            {
                if (transaction.Connection.State != ConnectionState.Closed)
                {
                    transaction.Connection.Close();
                    transaction.Dispose();
                }
            }
        }

        /// <summary>
        /// 删除token
        /// </summary>
        public bool DeleteToken(IToken token)
        {
            string delete = "delete from t_ff_rt_token where id=@1";
            SqlParameter[] deleteParms = { 
				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, token.Id)
			};
            if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, delete, deleteParms) <=0)
                return false;
            else return true;
        }

        /******************************************************************************/
        /************                                                        **********/
        /************            存取流程定义文件 相关的持久化方法             **********/
        /************            Persistence methods for workflow definition **********/
        /************                                                        **********/
        /******************************************************************************/

        /// <summary>
        /// Save or update the workflow definition. The version will be increased automatically when insert a new record.
        /// 保存流程定义，如果同一个ProcessId的流程定义已经存在，则版本号自动加1。
        /// </summary>
        public bool SaveOrUpdateWorkflowDefinition(WorkflowDefinition workflowDef) {

            if (String.IsNullOrEmpty(workflowDef.Id))
            {
                Int32 latestVersion = FindTheLatestVersionNumberIgnoreState(workflowDef.ProcessId);
                if (latestVersion > 0)
                {
                    workflowDef.Version=latestVersion + 1;
                }
                else
                {
                    workflowDef.Version=1;
                }
                workflowDef.Id = Guid.NewGuid().ToString().Replace("-", "");
                string insert = "INSERT INTO T_FF_DF_WORKFLOWDEF (" +
                    "ID, DEFINITION_TYPE, PROCESS_ID, NAME, DISPLAY_NAME, " +
                    "DESCRIPTION, VERSION, STATE, UPLOAD_USER, UPLOAD_TIME, " +
                    "PUBLISH_USER, PUBLISH_TIME, PROCESS_CONTENT )VALUES(@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13)";
                
                SqlParameter[] insertParms = { 
					OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, workflowDef.Id), 
					OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 50, workflowDef.DefinitionType), 
					OracleHelper.NewOracleParameter("@3", SqlDbType.VarChar, 100, workflowDef.ProcessId), 
					OracleHelper.NewOracleParameter("@4", SqlDbType.VarChar, 100, workflowDef.Name), 
					OracleHelper.NewOracleParameter("@5", SqlDbType.VarChar, 128, workflowDef.DisplayName), 
					OracleHelper.NewOracleParameter("@6", SqlDbType.VarChar, 1024, workflowDef.Description), 
					OracleHelper.NewOracleParameter("@7", SqlDbType.Int, workflowDef.Version), 
					OracleHelper.NewOracleParameter("@8", SqlDbType.Int, OracleHelper.OraBit(workflowDef.State) ), 
					OracleHelper.NewOracleParameter("@9", SqlDbType.VarChar, 50, workflowDef.UploadUser), 
					OracleHelper.NewOracleParameter("@10", SqlDbType.DateTime, workflowDef.UploadTime), 
					OracleHelper.NewOracleParameter("@11", SqlDbType.VarChar, 50, workflowDef.PublishUser), 
					OracleHelper.NewOracleParameter("@12", SqlDbType.DateTime, workflowDef.PublishTime), 
					OracleHelper.NewOracleParameter("@13", SqlDbType.NVarChar, workflowDef.ProcessContent)
				};
                if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_DF_WORKFLOWDEF SET " +
                    "PROCESS_ID=@3, NAME=@4, DISPLAY_NAME=@5, DESCRIPTION=@6, " +
                    "STATE=@8, UPLOAD_USER=@9, UPLOAD_TIME=@10, PROCESS_CONTENT=@13 " +
                    "WHERE ID=@1";
                SqlParameter[] updateParms = { 
    				OracleHelper.NewOracleParameter("@3", SqlDbType.VarChar, 100, workflowDef.ProcessId), 
    				OracleHelper.NewOracleParameter("@4", SqlDbType.VarChar, 100, workflowDef.Name), 
    				OracleHelper.NewOracleParameter("@5", SqlDbType.VarChar, 128, workflowDef.DisplayName), 
    				OracleHelper.NewOracleParameter("@6", SqlDbType.VarChar, 1024, workflowDef.Description), 
    				OracleHelper.NewOracleParameter("@8", SqlDbType.Int, OracleHelper.OraBit(workflowDef.State)), 
    				OracleHelper.NewOracleParameter("@9", SqlDbType.VarChar, 50, workflowDef.UploadUser), 
    				OracleHelper.NewOracleParameter("@10", SqlDbType.DateTime, workflowDef.UploadTime),
    				OracleHelper.NewOracleParameter("@13", SqlDbType.NVarChar,workflowDef.ProcessContent),
    				OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, workflowDef.Id)
    			};
                if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }

        /// <summary>
        /// Find the workflow definition by id .
        /// 根据纪录的ID返回流程定义
        /// </summary>
        public WorkflowDefinition FindWorkflowDefinitionById(String id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_df_workflowdef where id=@1";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@1", SqlDbType.VarChar, 50, id)
                        );
                    while (reader.Read())
                    {
                        WorkflowDefinition info = OracleDataReaderToInfo.GetWorkflowDefinition(reader);
                        return info;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Find workflow definition by workflow process id and version<br>
        /// 根据ProcessId和版本号返回流程定义
        /// </summary>
        public WorkflowDefinition FindWorkflowDefinitionByProcessIdAndVersionNumber(String processId, int version)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_df_workflowdef where process_id=@processId and version=@version";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@processId", SqlDbType.VarChar, 100, processId),
                        OracleHelper.NewOracleParameter("@version", SqlDbType.Int, version)
                        );
                    while (reader.Read())
                    {
                        WorkflowDefinition info = OracleDataReaderToInfo.GetWorkflowDefinition(reader);
                        return info;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        /// <summary>
        /// Find the latest version of the workflow definition.
        /// 根据processId返回最新版本的有效流程定义
        /// </summary>
        /// <param name="processId">the workflow process id </param>
        /// <returns></returns>
        public WorkflowDefinition FindTheLatestVersionOfWorkflowDefinitionByProcessId(String processId)
        {
            Int32 latestVersion = this.FindTheLatestVersionNumber(processId);
            return this.FindWorkflowDefinitionByProcessIdAndVersionNumber(processId, latestVersion);
        }

        /// <summary>
        /// Find all the workflow definitions for the workflow process id.
        /// 根据ProcessId 返回所有版本的流程定义
        /// </summary>
        public List<WorkflowDefinition> FindWorkflowDefinitionsByProcessId(String processId)
        {
            List<WorkflowDefinition> infos = new List<WorkflowDefinition>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_df_workflowdef where process_id=@processId";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@processId", SqlDbType.VarChar, 100, processId)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            WorkflowDefinition info = OracleDataReaderToInfo.GetWorkflowDefinition(reader);
                            infos.Add(info);
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// Find all of the latest version of workflow definitions.
        /// 返回系统中所有的最新版本的有效流程定义
        /// </summary>
        public List<WorkflowDefinition> FindAllTheLatestVersionsOfWorkflowDefinition()
        {
            List<WorkflowDefinition> infos = new List<WorkflowDefinition>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "SELECT * FROM T_FF_DF_WORKFLOWDEF a "+ 
                    "where version=(select max(version) from T_FF_DF_WORKFLOWDEF b where a.PROCESS_ID=b.PROCESS_ID) ";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,null);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            WorkflowDefinition info = OracleDataReaderToInfo.GetWorkflowDefinition(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        /// <summary>
        /// Find the latest version number
        /// 返回最新的有效版本号
        /// </summary>
        /// <returns>the version number ,null if there is no workflow definition stored in the DB.</returns>
        public int FindTheLatestVersionNumber(String processId)
        {
            String select = "select max(version) from t_ff_df_workflowdef where process_id=@process_id and state=1";
            SqlParameter[] selectParms = { 
				OracleHelper.NewOracleParameter("@process_id", SqlDbType.VarChar, 100, processId)
		    };
            return OracleHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }

        /// <summary>
        /// 返回最新版本号,
        /// </summary>
        public int FindTheLatestVersionNumberIgnoreState(String processId)
        {
            String select = "select max(version) from t_ff_df_workflowdef where process_id=@process_id";
            SqlParameter[] selectParms = { 
				OracleHelper.NewOracleParameter("@process_id", SqlDbType.VarChar, 100, processId)
		    };
            return OracleHelper.ExecuteInt32(this.connectionString, CommandType.Text, select, selectParms);
        }



        /********************************process instance trace info **********************/
        public bool SaveOrUpdateProcessInstanceTrace(ProcessInstanceTrace processInstanceTrace) {
            if (String.IsNullOrEmpty(processInstanceTrace.Id))
            {
                processInstanceTrace.Id = Guid.NewGuid().ToString().Replace("-", "");
                string insert = "INSERT INTO T_FF_HIST_TRACE (" +
                    "ID, PROCESSINSTANCE_ID, STEP_NUMBER, MINOR_NUMBER, TYPE, " +
                    "EDGE_ID, FROM_NODE_ID, TO_NODE_ID )VALUES(@ID, @PROCESSINSTANCE_ID, @STEP_numeric, @MINOR_numeric, @TYPE, @EDGE_ID, @FROM_NODE_ID, @TO_NODE_ID)";
                SqlParameter[] insertParms = { 
				OracleHelper.NewOracleParameter("@ID", SqlDbType.VarChar, 50, processInstanceTrace.Id), 
				OracleHelper.NewOracleParameter("@PROCESSINSTANCE_ID", SqlDbType.VarChar, 50, processInstanceTrace.ProcessInstanceId), 
				OracleHelper.NewOracleParameter("@STEP_numeric", SqlDbType.Int, processInstanceTrace.StepNumber), 
				OracleHelper.NewOracleParameter("@MINOR_numeric", SqlDbType.Int, processInstanceTrace.MinorNumber), 
				OracleHelper.NewOracleParameter("@TYPE", SqlDbType.VarChar, 15, processInstanceTrace.Type.ToString()), 
				OracleHelper.NewOracleParameter("@EDGE_ID", SqlDbType.VarChar, 100, processInstanceTrace.EdgeId), 
				OracleHelper.NewOracleParameter("@FROM_NODE_ID", SqlDbType.VarChar, 100, processInstanceTrace.FromNodeId), 
				OracleHelper.NewOracleParameter("@TO_NODE_ID", SqlDbType.VarChar, 100, processInstanceTrace.ToNodeId)
			};
                if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                    return false;
                else return true;
            }
            else
            {
                string update = "UPDATE T_FF_HIST_TRACE SET " +
                    "PROCESSINSTANCE_ID=@PROCESSINSTANCE_ID, STEP_NUMBER=@STEP_numeric, MINOR_NUMBER=@MINOR_numeric, TYPE=@TYPE, EDGE_ID=@EDGE_ID, " +
                    "FROM_NODE_ID=@FROM_NODE_ID, TO_NODE_ID=@TO_NODE_ID" +
                    " WHERE ID=@ID";
                SqlParameter[] updateParms = { 
                //OracleHelper.NewOracleParameter("@2", SqlDbType.VarChar, 50, processInstanceTrace.ProcessInstanceId), 
                //OracleHelper.NewOracleParameter("@3", SqlDbType.Int, processInstanceTrace.StepNumber), 
                //OracleHelper.NewOracleParameter("@4", SqlDbType.Int, processInstanceTrace.MinorNumber), 
                //OracleHelper.NewOracleParameter("@5", SqlDbType.VarChar, 15, processInstanceTrace.Type), 
                //OracleHelper.NewOracleParameter("@6", SqlDbType.VarChar, 100, processInstanceTrace.EdgeId), 
                //OracleHelper.NewOracleParameter("@7", SqlDbType.VarChar, 100, processInstanceTrace.FromNodeId), 
                //OracleHelper.NewOracleParameter("@8", SqlDbType.VarChar, 100, processInstanceTrace.ToNodeId),

                OracleHelper.NewOracleParameter("@PROCESSINSTANCE_ID", SqlDbType.VarChar, 50, processInstanceTrace.ProcessInstanceId), 
				OracleHelper.NewOracleParameter("@STEP_numeric", SqlDbType.Int, processInstanceTrace.StepNumber), 
				OracleHelper.NewOracleParameter("@MINOR_numeric", SqlDbType.Int, processInstanceTrace.MinorNumber), 
				OracleHelper.NewOracleParameter("@TYPE", SqlDbType.VarChar, 15, processInstanceTrace.Type.ToString()), 
				OracleHelper.NewOracleParameter("@EDGE_ID", SqlDbType.VarChar, 100, processInstanceTrace.EdgeId), 
				OracleHelper.NewOracleParameter("@FROM_NODE_ID", SqlDbType.VarChar, 100, processInstanceTrace.FromNodeId), 
				OracleHelper.NewOracleParameter("@TO_NODE_ID", SqlDbType.VarChar, 100, processInstanceTrace.ToNodeId),
				OracleHelper.NewOracleParameter("@ID", SqlDbType.VarChar, 50, processInstanceTrace.Id)
			};
                if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                    return false;
                else return true;
            }
        }


        /********************************process instance trace info **********************/

        /// <summary>
        /// 根据流程实例ID查找流程实例运行轨迹
        /// </summary>
        /// <param name="processInstanceId">流程实例ID</param>
        /// <returns></returns>
        public List<ProcessInstanceTrace> FindProcessInstanceTraces(String processInstanceId)
        {
            List<ProcessInstanceTrace> infos = new List<ProcessInstanceTrace>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_hist_trace where processinstance_id=@processinstance_id order by step_number,minor_number";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, processInstanceId)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ProcessInstanceTrace info = OracleDataReaderToInfo.GetProcessInstanceTrace(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }


        public List<ProcessInstanceVar> FindProcessInstanceVariable(string processInstanceId)
        {
            List<ProcessInstanceVar> infos = new List<ProcessInstanceVar>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string select = "select * from t_ff_rt_procinst_var where processinstance_id=@processinstance_id";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, processInstanceId)
                        );
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            ProcessInstanceVar info = OracleDataReaderToInfo.GetProcessInstanceVar(reader);
                            infos.Add(info);
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return infos;
        }

        public ProcessInstanceVar FindProcessInstanceVariable(string processInstanceId, string name)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                String select = "select * from t_ff_rt_procinst_var where processinstance_id=@processinstance_id and name=@name";
                SqlDataReader reader = null;
                try
                {
                    reader = OracleHelper.ExecuteReader(connection, CommandType.Text, select,
                        OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, processInstanceId),
                        OracleHelper.NewOracleParameter("@name", SqlDbType.VarChar, 255, name)
                        );
                    while (reader.Read())
                    {
                        ProcessInstanceVar info = OracleDataReaderToInfo.GetProcessInstanceVar(reader);
                        return info;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return null;
        }

        public bool UpdateProcessInstanceVariable(ProcessInstanceVar var)
        {
            string update = "UPDATE T_FF_RT_PROCINST_VAR SET " +
                "VALUE=@value" +
                " WHERE PROCESSINSTANCE_ID=@processinstance_id AND NAME=@name";
            SqlParameter[] updateParms = { 
				OracleHelper.NewOracleParameter("value", SqlDbType.VarChar, 255, var.ValueType+"#"+var.Value), 
				OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, var.ProcessInstanceId),
				OracleHelper.NewOracleParameter("@name", SqlDbType.VarChar, 255, var.Name)
			};
            if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, update, updateParms) != 1)
                return false;
            else return true;
        }

        public bool SaveProcessInstanceVariable(ProcessInstanceVar var)
        {
            string insert = "INSERT INTO T_FF_RT_PROCINST_VAR (" +
                   "PROCESSINSTANCE_ID, VALUE, NAME )VALUES(@processinstance_id, @value, @name)";
            SqlParameter[] insertParms = { 
				OracleHelper.NewOracleParameter("@processinstance_id", SqlDbType.VarChar, 50, var.ProcessInstanceId), 
				OracleHelper.NewOracleParameter("@value", SqlDbType.VarChar, 255, var.ValueType+"#"+var.Value), 
				OracleHelper.NewOracleParameter("@name", SqlDbType.VarChar, 255, var.Name)
			};
            if (OracleHelper.ExecuteNonQuery(connectionString, CommandType.Text, insert, insertParms) != 1)
                return false;
            else return true;
        }

        /******************************** lifw555@gmail.com **********************/

        /// <summary>
        /// 获得操作员所要操作工单的总数量
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <returns></returns>
        public Int32 GetTodoWorkItemsCount(String actorId, String publishUser)
        {
            return 0;
        }

        /// <summary>
        /// 获得操作员所要操作工单列表（分页）
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public List<IWorkItem> FindTodoWorkItems(String actorId, String publishUser, int pageSize, int pageNumber)
        {
            return null;
        }

        /// <summary>
        /// 获得操作员完成的工单总数量
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <returns></returns>
        public Int32 GetHaveDoneWorkItemsCount(String actorId, String publishUser)
        {
            return 0;
        }

        /// <summary>
        /// 获得操作员完成的工单列表（分页）
        /// publishUser如果为null，获取全部
        /// </summary>
        /// <param name="actorId">操作员主键</param>
        /// <param name="publishUser">流程定义发布者</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <param name="pageNumber">当前页数</param>
        /// <returns></returns>
        public List<IWorkItem> FindHaveDoneWorkItems(String actorId, String publishUser, int pageSize, int pageNumber)
        {
            return null;
        }

        

        /// <summary>
        /// 获得工作流发布者发起的所有流程定义的工作流实例总数量
        /// </summary>
        /// <param name="publishUser">工作流发布者</param>
        /// <returns></returns>
        public Int32 GetProcessInstanceCountByPublishUser(String publishUser)
        {
            return 0;
        }



    }
}
