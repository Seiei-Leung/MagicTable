using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Bll
{
    /// <summary>
    /// 打印日期，如果是遇到节假日，打印日期往前推或者往后退。
    /// </summary>
    public class HolidayHandler
    {
        DataTable dtCalendar = null;
        public HolidayHandler(DataTable dtCalendar)
        {
            this.dtCalendar = dtCalendar;
        }

        /// <summary>
        /// 创建人：黎金来
        /// 日期：2014-05-23
        /// 获取打印日期
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public DateTime GetWorkDate(DateTime dtdate)
        {
            //DateTime dt = new DateTime();

            while (IsHolidays(dtdate))
            {
                dtdate = dtdate.AddDays(-1);
            }
            return dtdate;
        }

        /// <summary>
        /// 是否节假日
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private bool IsHolidays(DateTime dateTime)
        {
            int flag = 0;
            return IsHolidays(dateTime, out flag);
        }

        /// <summary>
        /// 是否节假日
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="flag">-1：节假日；1：星期日</param>
        /// <returns></returns>
        private bool IsHolidays(DateTime dateTime, out int flag)
        {
            flag = 0;
            string year = dateTime.Year.ToString();
            string datetime = dateTime.ToString("yyyy-MM-dd");

            string sql = "year=" + year + " and BeginDate<='" + datetime + "' and EndDate>='" + datetime + "'";
            DataRow[] rows = DtFactoryCalendar.Select(sql);
            if (rows != null && rows.Length > 0)
            {
                flag = -1;
                return Convert.ToBoolean(rows[0]["IsHolidays"]);
            }
            string week = dateTime.DayOfWeek.ToString();
            sql = "year=" + year + " and " + week + "=0";
            rows = DtFactoryCalendar.Select(sql);
            if (rows != null && rows.Length > 0)
            {
                flag = 1;
                return true;
            }
            return false;
        }


        private DataTable dtFactoryCalendar = null;//工厂日历
        /// <summary>
        /// 工厂日历
        /// </summary>
        public DataTable DtFactoryCalendar
        {
            get
            {
                if (dtFactoryCalendar == null)
                {
                    dtFactoryCalendar = dtCalendar;
                }
                return dtFactoryCalendar;
            }
            set { dtFactoryCalendar = value; }
        }
    
    }
}
