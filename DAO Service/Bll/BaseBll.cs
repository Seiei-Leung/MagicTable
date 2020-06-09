using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using DbFactory;
using System.Data;
using System.Threading;
using System.Runtime.Serialization;

namespace Bll
{
    public class BaseBll
    {
        //public static string ConnStr
        //{
        //    get { return SqlHelper.ConString;}
        //    set { SqlHelper.ConString = value;}
        //}


        //private DataProvider _provider;
        private DalFactory _dal;

        public string UserCode { get; set; }
        //public string UserName { get; set; }
        public string IP { get; set; }

        public BaseBll()
        {
            //_provider = new DataProvider();
            //_dal = _provider.DataFactory;
            _dal = DataProvider.DataFactory;
        }

        public DalFactory GetDal { get { return _dal; } }

        protected DataBaseType GetDataBaseType(dynamic Dal)
        {
            if (Dal.sqlType == "")
                return DataBaseType.MSSQL;
            else if (Dal.sqlType == "MYSQL")
                return DataBaseType.MYSQL;
            else
                return DataBaseType.ORACLE;
        }

    }

    public enum DataBaseType
    {
        [EnumMember]
        MSSQL,
        [EnumMember]
        MYSQL,
        [EnumMember]
        ORACLE
    }
}
