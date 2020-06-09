using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Card;

namespace Model
{
    /// <summary>
    /// 控件Tag属性绑定的对象
    /// </summary>
    public class TagObjects
    {
        private string fieldName = "";
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get
            {
                return fieldName == null ? "" : fieldName;
            }
            set { fieldName = value; }
        }

        private string fieldName2 = "";
        /// <summary>
        /// 字段名称2
        /// </summary>
        public string FieldName2
        {
            get
            {
                return fieldName2 == null ? "" : fieldName2;
            }
            set { fieldName2 = value; }
        }


        /// <summary>
        /// 下划线
        /// </summary>
        public int Underline { get; set; }

        /// <summary>
        /// 必填
        /// </summary>        
        public int NotNull { get; set; }

        /// <summary>
        /// 空值替代文本
        /// </summary>        
        public string NullText { get; set; }

        /// <summary>
        /// 数字
        /// </summary>        
        public int IsNumber { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string PK { get; set; }
        /// <summary>
        /// 统计配置
        /// </summary>
        public string CountConfig { get; set; }
        /// <summary>
        /// 统计值数组字符串
        /// </summary>
        public string CountValues { get; set; }
        /// <summary>
        /// 公式默认值
        /// </summary>
        public string formula { get; set; }
        /// <summary>
        /// 公式
        /// </summary>
        public string formula2 { get; set; }
        /// <summary>
        /// 是否放在最后执行
        /// </summary>
        public int formula2IsLast { get; set; }

        /// <summary>
        /// 控件配置行
        /// </summary>
        public DataRow RowConfig { get; set; }

        /// <summary>
        /// 所属的GridView
        /// </summary>
        public GridView GridView { get; set; }

        public CardView CardView { get; set; }

        /// <summary>
        /// 约束配置行
        /// </summary>
        public DataRow RowConstraints { get; set; }

        /// <summary>
        /// 上下标规则
        /// </summary>
        public string UpperIndexRule { get; set; }

        /// <summary>
        /// 显示格式
        /// </summary>
        public string FormatStr { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>      
        public string Imgurl { get; set; }

        /// <summary>
        /// 绑定字段，但可修改
        /// </summary>
        public int CanM { get; set; }
    }
}
