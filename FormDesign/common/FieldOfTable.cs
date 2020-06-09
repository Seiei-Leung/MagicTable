using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormDesign.common
{
    /// <summary>
    /// 创建表格字段对象
    /// </summary>
    class FieldOfTable
    {
        public string nameOfField
        {
            get;
            set;
        }
        public string typeOfField
        {
            get;
            set;
        }
        public bool isPrimaryKey
        {
            get;
            set;
        }
        public string defaultValue
        {
            set;
            get;
        }
        public string getSQL()
        {
            string sql = nameOfField;
            sql = sql + " " + typeOfField;
            if (isPrimaryKey)
            {
                sql += " primary key";
                return sql;
            }
            if (defaultValue.Equals("not null"))
            {
                sql += " not null";
            }
            else
            {
                sql += " default " + defaultValue;
            }
            return sql;
        }
    }
}
