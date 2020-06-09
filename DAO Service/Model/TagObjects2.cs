using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraTreeList;

namespace Model
{
    public class TagObjects2
    {


        public TagObjects2(DataRow RowConfig,TreeList TreeList)
        {
            this.RowConfig = RowConfig;
            this.TreeList = TreeList;
        }

        /// <summary>
        /// 控件配置行
        /// </summary>
        public DataRow RowConfig { get; set; }

        /// <summary>
        /// 所属的GridView
        /// </summary>
        public TreeList TreeList { get; set; }
    }
}
