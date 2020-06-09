using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 常量
    /// </summary>
    public static class ConstDic
    {
        public const string ProcessId = "";


        /// <summary>
        /// 审批记录
        /// 工作项的状态名称
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetCHECKSTATUS(int status)
        {
            string strStatusName = string.Empty;
            switch (status)
            {
                case 0:
                    strStatusName = "未查看";
                    break;
                case 1:
                    strStatusName = "已查看";
                    break;
                case 7:
                    strStatusName = "已完成";
                    break;
                case 9:
                    strStatusName = "已弃审";
                    break;
                case -9:
                    strStatusName = "已拒收";
                    break;
            }

            return strStatusName;

        }

    }

    /// <summary>
    /// 单据提交状态
    /// </summary>
    public enum CHECKSTATUS
    {
        /// <summary>
        /// 提交失败
        /// </summary>
        FAIL = 0,
        /// <summary>
        /// 提交成功
        /// </summary>
        SUCCESS = 1,
        /// <summary>
        /// 暂时不处理
        /// </summary>
        REJECT = 2
    }

    /// <summary>
    /// 单据状态
    /// </summary>
    public enum BILLSTATUS
    {
        /// <summary>初始化状态</summary>
        INITIALIZED = 0,
        /// <summary>运行状态</summary>
        RUNNING = 1,
        /// <summary>挂起</summary>
        SUSPEND = 3,
        /// <summary>已经结束</summary>
        COMPLETED = 7,
        /// <summary>被撤销</summary>
        CANCELED = 9
    }

    /// <summary>
    /// Txt旧系统按钮类型
    /// </summary>
    public enum ButtonTypeTxt
    {
        /// <summary>
        /// Txt审核按钮
        /// </summary>
        check = 0,
        /// <summary>
        /// Txt弃审按钮
        /// </summary>
        uncheck = 1,
        /// <summary>
        /// Txt审批按钮
        /// </summary>
        Approve = 2,
        /// <summary>
        /// Txt反审批按钮
        /// </summary>
        unApprove = 3,
        /// <summary>
        /// Txt确认按钮
        /// </summary>
        Confirm = 4,
        /// <summary>
        /// Txt反确认按钮
        /// </summary>
        unConfirm = 5
    }
}
