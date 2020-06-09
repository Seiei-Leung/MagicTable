using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Model
{
    [Serializable]
    public class DepartmentEntity
    {
        public DepartmentEntity()
        {
        }

        public DepartmentEntity(int id, int deptId, string deptName, int superiorId, string remark)
        {
            _id = id;
            _deptId = deptId;
            _deptName = deptName;
            _superiorId = superiorId;
            _remark = remark;
        }

        private int _id;
        /// <summary>
        /// 数据id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _deptId;
        /// <summary>
        /// 部门id
        /// </summary>
        public int DeptId
        {
            get { return _deptId; }
            set { _deptId = value; }
        }

        private string _deptName;
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName
        {
            get { return _deptName; }
            set { _deptName = value; }
        }

        private int _superiorId;
        /// <summary>
        /// 父id
        /// </summary>
        public int SuperiorId
        {
            get { return _superiorId; }
            set { _superiorId = value; }
        }

        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
