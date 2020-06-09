using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;

namespace Model
{
    /// <summary>
    /// 创建人：郑志冲
    /// <para>日期：2012-10-18</para>
    /// <para>自定义表单属性类</para>
    /// </summary>
    public class FormProperties : IDisposable
    {

        public void Dispose()
        {
            #region 集合

            if (this.pageSearch != null)
            {
                pageSearch.Clear();
                pageSearch = null;
            }
            if (this.controls_ControlName != null)
            {
                controls_ControlName.Clear();
                controls_ControlName = null;
            }
            if (this.controls_FieldName != null)
            {
                controls_FieldName.Clear();
                controls_FieldName = null;
            }
            if (this.dataTables != null)
            {
                dataTables.Clear();
                dataTables = null;
            }
            if (this.dicBtn != null)
            {
                dicBtn.Clear();
                dicBtn = null;
            }
            if (this.dsPrint != null)
            {
                bsSysModM.Dispose();
            }
            if (this.extendObjects != null)
            {
                extendObjects.Clear();
                extendObjects = null;
            }
            if (this.fileImageList != null)
            {
                fileImageList.Images.Clear();
                fileImageList = null;
            }
            if (this.gcSearch != null)
            {
                gcSearch.Clear();
                gcSearch = null;
            }
            if (this.gvSearch != null)
            {
                gvSearch.Clear();
                gvSearch = null;
            }
            if (this.gvTableAlias != null)
            {
                gvTableAlias.Clear();
                gvTableAlias = null;
            }
            if (this.panelMaster != null)
            {
                panelMaster.Clear();
                panelMaster = null;
            }
            if (this.paramss != null)
            {
                paramss.Clear();
                paramss = null;
            }
            if (this.popupMenus != null)
            {
                popupMenus.Clear();
                popupMenus = null;
            }
            if (this.shortCut != null)
            {
                shortCut.Clear();
                shortCut = null;
            }
            if (this.bands != null)
            {
                bands.Clear();
                bands = null;
            }
            if (this.bars != null)
            {
                bars.Clear();
                bars = null;
            }
            if (this.bsSearch != null)
            {
                bsSearch.Clear();
                bsSearch = null;
            }

            #endregion

            #region BindingSource
            if (this.bsM != null)
            {
                bsM.Dispose();
            }
            if (this.bsSysModD_Dialog != null)
            {
                bsSysModD_Dialog.Dispose();
            }
            if (this.bsSysModI != null)
            {
                bsSysModI.Dispose();
            }
            if (this.bsSysModM != null)
            {
                bsSysModM.Dispose();
            }
            #endregion
            
            #region DataTable
            if (dtAccessory != null)
            {
                dtAccessory.Clear();
                dtAccessory.Dispose();
                dtAccessory = null;
            }
            if (dtM != null)
            {
                dtM.Clear();
                dtM.Dispose();
                dtM = null;
            }
            if (dtSysModAnalyse != null)
            {
                dtSysModAnalyse.Clear();
                dtSysModAnalyse.Dispose();
                dtSysModAnalyse = null;
            }
            if (dtSysModB != null)
            {
                dtSysModB.Clear();
                dtSysModB.Dispose();
                dtSysModB = null;
            }
            if (dtSysModConfig != null)
            {
                dtSysModConfig.Clear();
                dtSysModConfig.Dispose();
                dtSysModConfig = null;
            }
            if (dtSysModConstraints != null)
            {
                dtSysModConstraints.Clear();
                dtSysModConstraints.Dispose();
                dtSysModConstraints = null;
            }
            if (dtSysModD != null)
            {
                dtSysModD.Clear();
                dtSysModD.Dispose();
                dtSysModD = null;
            }
            if (dtSysModD_Dialog != null)
            {
                dtSysModD_Dialog.Clear();
                dtSysModD_Dialog.Dispose();
                dtSysModD_Dialog = null;
            }
            if (dtSysModI != null)
            {
                dtSysModI.Clear();
                dtSysModI.Dispose();
                dtSysModI = null;
            }
            if (dtSysModM != null)
            {
                dtSysModM.Clear();
                dtSysModM.Dispose();
                dtSysModM = null;
            }
            if (dtSysModMu != null)
            {
                dtSysModMu.Clear();
                dtSysModMu.Dispose();
                dtSysModMu = null;
            }
            if (dtSysModMu_Master != null)
            {
                dtSysModMu_Master.Clear();
                dtSysModMu_Master.Dispose();
                dtSysModMu_Master = null;
            }
            if (dtSysModPrint != null)
            {
                dtSysModPrint.Clear();
                dtSysModPrint.Dispose();
                dtSysModPrint = null;
            }
            if (dtSysModS != null)
            {
                dtSysModS.Clear();
                dtSysModS.Dispose();
                dtSysModS = null;
            }
            #endregion

        }


        private DataTable dtSysModConfig; //主档主要信息
        private DataTable dtSysModM;    //主档配置文档
        private DataTable dtSysModB;    //从档页面配置文档
        private DataTable dtSysModD;    //从档显示配置文档
        private DataTable dtSysModD_Dialog;    //从档显示配置文档
        private DataTable dtSysModS;    //条件样式配置文档
        private DataTable dtSysModI;    //用户信息
        private DataTable dtSysModMu;   //右键菜单
        private DataTable dtSysModMu_Master;    //主档扩展按钮
        private BindingSource bsSysModI;
        private DataTable dtSysModPrint; //打印配置文档
        private DataTable dtAccessory;  //附件DataTable
        private DataTable dtSysModConstraints;  //验证约束配置文档
        private DataTable dtSysModAnalyse;  //查询汇总分析配置文档
        private DataTable dtSysModQ; //按时间访问用户列表
        private DataTable dtSysModScript; //执行脚本

        private ImageList fileImageList; //附件图标集合

        private DataSet dsPrint; //打印table数据集
        public DataSet DsPrint
        {
            get
            {
                if (dsPrint == null)
                    dsPrint = new DataSet();
                return dsPrint;
            }
            set { dsPrint = value; }
        }
        private IList<PanelControl> panelMaster = new List<PanelControl>(); //主档控件容器

        private Dictionary<string, Control> controls_FieldName; //主档控件集合，主键为字段名称
        private Dictionary<string, Control> controls_ControlName;   //主档控件集合，主键为控件名称
        private Dictionary<string, BindingSource> bsSearch; //从档集合，主键为页面名称
        private Dictionary<string, DataTable> dataTables; //从档数据表集合，主键为数据表别名
        private Dictionary<string, GridControl> gcSearch;   //从档网格集合，主键为页面名称
        private Dictionary<string, GridView> gvSearch;   //从档Gridview集合，主键为页面名称
        private Dictionary<string, GridView> gvTableAlias;   //从档Gridview集合，主键为数据表别名
        private Dictionary<string, PopupMenu> popupMenus;   //右键菜单
        private Dictionary<string, Bar> bars;   //从档工具条
        private Dictionary<string, object> extendObjects; //扩展方法实例字典
        private Dictionary<string, GridBand> bands; //多表头的聚集表头
        private Dictionary<string, BarButtonItem> shortCut; //多表头的聚集表头
        private Dictionary<string, BarButtonItem> dicBtn = new Dictionary<string, BarButtonItem>(); //工具栏按钮及右键菜单按钮字典，key=按钮名称+数据源别名
        private Dictionary<string, XtraTabPage> pageSearch; //页签集合，主键为页面名称

        private List<DataTable> listPrintTableC; //用户自定义打印table
        public DataTable dtM = null;    //主档表
        public BindingSource bsM = new BindingSource();    //主档bandingSource

        public DataTable dtM2 = null;    //主档表
        public BindingSource bsM2 = new BindingSource();    //主档bandingSource

        public DataTable dtM3 = null;    //主档表
        public BindingSource bsM3 = new BindingSource();    //主档bandingSource

        public DataTable dtM4 = null;    //主档表
        public BindingSource bsM4 = new BindingSource();    //主档bandingSource

        public DataTable dtMT = null;    //主档表
        public BindingSource bsMT = new BindingSource();    //主档bandingSource

        private Dictionary<string, string> paramss = null;  //参数字典，键为field-P，值为string
        private List<ChangeLogdlDetail> logDetails;// = new List<ChangeLogdlDetail>();
        private List<ChangeLogdlMain> logMains;//= new List<ChangeLogdlMain>();
        public List<DataTable> ListPrintTableC
        {
            get
            {
                if (listPrintTableC == null)
                    listPrintTableC = new List<DataTable>();
                return listPrintTableC;
            }
            set { listPrintTableC = value; }
        }

        public List<ChangeLogdlDetail> LogDetails
        {
            get
            {
                if (logDetails == null)
                    logDetails = new List<ChangeLogdlDetail>();
                return logDetails;
            }
            set { logDetails = value; }
        }

        public List<ChangeLogdlMain> LogMains
        {
            get
            {
                if (logMains == null)
                    logMains = new List<ChangeLogdlMain>();
                return logMains;
            }
            set { logMains = value; }
        }

        /// <summary>
        /// 参数字典，键为field-P，值为string
        /// </summary>
        public Dictionary<string, string> Paramss
        {
            get
            {
                if (paramss == null)
                    paramss = new Dictionary<string, string>();
                return paramss;
            }
            set { paramss = value; }
        }
        private int panelMasterHeight;  //主档面板高度

        /// <summary>
        /// 主档面板高度
        /// </summary>
        public int PanelMasterHeight
        {
            get { return panelMasterHeight; }
            set { panelMasterHeight = value; }
        }


        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2013-04-23</para>
        /// <para>页签集合，主键为页面名称</para>
        /// </summary>
        public Dictionary<string, XtraTabPage> PageSearch
        {
            get
            {
                if (pageSearch == null)
                    pageSearch = new Dictionary<string, XtraTabPage>();
                return pageSearch;
            }
        }

        /// <summary>
        /// 从档数据表集合，主键为数据表别名
        /// </summary>
        public Dictionary<string, DataTable> DataTables
        {
            get
            {
                if (dataTables == null)
                    dataTables = new Dictionary<string, DataTable>();
                return dataTables;
            }
        }

        /// <summary>
        /// 从档Gridview集合，主键为数据表别名
        /// </summary>
        public Dictionary<string, GridView> GvTableAlias
        {
            get
            {
                if (gvTableAlias == null)
                    gvTableAlias = new Dictionary<string, GridView>();
                return gvTableAlias;
            }
            set { gvTableAlias = value; }
        }

        public Dictionary<string, GridBand> Bands
        {
            get
            {
                if (bands == null)
                    bands = new Dictionary<string, GridBand>();
                return bands;
            }
            set { bands = value; }
        }

        public Dictionary<string, BarButtonItem> ShortCut
        {
            get
            {
                if (shortCut == null)
                    shortCut = new Dictionary<string, BarButtonItem>();
                return shortCut;
            }
            set { shortCut = value; }
        }

        public Dictionary<string, BarButtonItem> DicBtn
        {
            get
            {
                if (dicBtn == null)
                    dicBtn = new Dictionary<string, BarButtonItem>();
                return dicBtn;
            }
            set { dicBtn = value; }
        }

        public Dictionary<string, object> ExtendObjects
        {
            get
            {
                if (extendObjects == null)
                    extendObjects = new Dictionary<string, object>();
                return extendObjects;
            }
            set { extendObjects = value; }
        }

        public BindingSource BsSysModI
        {
            get
            {
                if (bsSysModI == null)
                    bsSysModI = new BindingSource();
                if (bsSysModI.DataSource == null)
                    bsSysModI.DataSource = DtSysModI;
                return bsSysModI;
            }
            set { bsSysModI = value; }
        }

        public DataTable DtSysModI
        {
            get
            {
                if (dtSysModI == null)
                {
                    dtSysModI = new DataTable();
                    dtSysModI.TableName = "dtSysModI";
                    dtSysModI.Columns.Add("StartDate");
                    dtSysModI.Columns.Add("EndDate");
                    dtSysModI.Columns.Add("UserId");
                    dtSysModI.Columns.Add("UserCode");
                    dtSysModI.Columns.Add("UserName");
                    dtSysModI.Columns.Add("GroupIds");
                    dtSysModI.Columns.Add("GroupCodes");
                    dtSysModI.Columns.Add("GroupTreeCodes");
                    dtSysModI.Columns.Add("GroupCode");
                    dtSysModI.Columns.Add("GroupTreeCode");
                    dtSysModI.Columns.Add("GroupName");
                    dtSysModI.Columns.Add("CPName");
                    dtSysModI.Columns.Add("IP");
                    dtSysModI.Columns.Add("Grade", typeof(int));
                    dtSysModI.Columns.Add("FormName");
                    dtSysModI.Columns.Add("FormCaption");
                    dtSysModI.Columns.Add("Brands");
                    dtSysModI.Columns.Add("Brand");
                    DataRow dr = dtSysModI.NewRow();
                    dtSysModI.Rows.Add(dr);
                }
                return dtSysModI;
            }
            set { dtSysModI = value; }
        }

        private BindingSource bsSysModM;
        private BindingSource bsSysModD_Dialog;

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>从档集合，主键为页面名称</para>
        /// </summary>
        public Dictionary<string, BindingSource> BsSearch
        {
            get
            {
                if (bsSearch == null)
                    bsSearch = new Dictionary<string, BindingSource>();
                return bsSearch;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>主档控件集合，主键为控件名称</para>
        /// </summary>
        public Dictionary<string, Control> Controls_ControlName
        {
            get
            {
                if (controls_ControlName == null)
                    controls_ControlName = new Dictionary<string, Control>();
                return controls_ControlName;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>主档控件集合，主键为字段名称</para>
        /// </summary>
        public Dictionary<string, Control> Controls_FieldName
        {
            get
            {
                if (controls_FieldName == null)
                    controls_FieldName = new Dictionary<string, Control>();
                return controls_FieldName;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2013-04-23</para>
        /// <para>从档Gridview集合，主键为页面名称</para>
        /// </summary>
        public Dictionary<string, GridView> GvSearch
        {
            get
            {
                if (gvSearch == null)
                    gvSearch = new Dictionary<string, GridView>();
                return gvSearch;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>从档网格集合，主键为页面名称</para>
        /// </summary>
        public Dictionary<string, GridControl> GcSearch
        {
            get
            {
                if (gcSearch == null)
                    gcSearch = new Dictionary<string, GridControl>();
                return gcSearch;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-12-13</para>
        /// <para>右键菜单</para>
        /// </summary>
        public Dictionary<string, PopupMenu> PopupMenus
        {
            get
            {
                if (popupMenus == null)
                    popupMenus = new Dictionary<string, PopupMenu>();
                return popupMenus;
            }
            set { popupMenus = value; }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>主档控件配置信息</para>
        /// </summary>
        public DataTable DtSysModM
        {
            get
            {
                if (dtSysModM == null)
                    dtSysModM = new DataTable();
                return dtSysModM;
            }
            set { dtSysModM = value; }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>从档显示配置信息</para>
        /// </summary>
        public DataTable DtSysModD_Dialog
        {
            get
            {
                if (dtSysModD_Dialog == null)
                    dtSysModD_Dialog = new DataTable();
                return dtSysModD_Dialog;
            }
            set { dtSysModD_Dialog = value; }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>从档页面配置信息</para>
        /// </summary>
        public DataTable DtSysModB
        {
            get
            {
                if (dtSysModB == null)
                    dtSysModB = new DataTable();
                return dtSysModB;
            }
            set { dtSysModB = value; }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>从档显示配置信息</para>
        /// </summary>
        public DataTable DtSysModD
        {
            get
            {
                if (dtSysModD == null)
                    dtSysModD = new DataTable();
                return dtSysModD;
            }
            set { dtSysModD = value; }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-18</para>
        /// <para>主档主要信息</para>
        /// </summary>
        public DataTable DtSysModConfig
        {
            get
            {
                if (dtSysModConfig == null)
                    dtSysModConfig = new DataTable();
                return this.dtSysModConfig;
            }
            set { dtSysModConfig = value; }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2013-04-21</para>
        /// 验证约束配置文档
        /// </summary>
        public DataTable DtSysModConstraints
        {
            get
            {
                if (dtSysModConstraints == null)
                {
                    dtSysModConstraints = new DataTable();
                }
                return dtSysModConstraints;
            }
            set { dtSysModConstraints = value; }
        }


        /// <summary>
        /// 查询汇总分析配置文档
        /// </summary>
        public DataTable DtSysModAnalyse
        {
            get
            {
                if (dtSysModAnalyse == null)
                    dtSysModAnalyse = new DataTable();
                return dtSysModAnalyse;
            }
            set { dtSysModAnalyse = value; }
        }

        /// <summary>
        /// 按时间访问用户列表
        /// </summary>
        public DataTable DtSysModQ
        {
            get
            {
                if (dtSysModQ == null)
                    dtSysModQ = new DataTable();
                return dtSysModQ;
            }
            set { dtSysModQ = value; }
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        public DataTable DtSysModScript
        {
            get
            {
                if (dtSysModScript == null)
                    dtSysModScript = new DataTable();
                return dtSysModScript;
            }
            set { dtSysModScript = value; }
        }

        public BindingSource BsSysModM
        {
            get
            {
                if (bsSysModM == null)
                    bsSysModM = new BindingSource();
                return bsSysModM;
            }
        }

        public BindingSource BsSysModD_Dialog
        {
            get
            {
                if (bsSysModD_Dialog == null)
                    bsSysModD_Dialog = new BindingSource();
                return bsSysModD_Dialog;
            }
        }

        public IList<PanelControl> PanelMaster
        {
            get
            {
                return panelMaster;
            }
            set { panelMaster = value; }
        }

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2012-12-11</para>
        /// <para>条件样式主要信息</para>
        /// </summary>
        public DataTable DtSysModS
        {
            get
            {
                if (dtSysModS == null)
                    dtSysModS = new DataTable();
                return this.dtSysModS;
            }
            set { dtSysModS = value; }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-12-13</para>
        /// <para>右键菜单</para>
        /// </summary>
        public DataTable DtSysModMu
        {
            get
            {
                if (dtSysModMu == null)
                    dtSysModMu = new DataTable();
                return dtSysModMu;
            }
            set { dtSysModMu = value; }
        }

        /// <summary>
        /// 从档工具条集合
        /// </summary>
        public Dictionary<string, Bar> Bars
        {
            get
            {
                if (bars == null)
                    bars = new Dictionary<string, Bar>();
                return bars;
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-12-17</para>
        /// <para>主档扩展按钮</para>
        /// </summary>
        public DataTable DtSysModMu_Master
        {
            get
            {
                if (dtSysModMu_Master == null)
                    dtSysModMu_Master = new DataTable();
                return dtSysModMu_Master;
            }
            set { dtSysModMu_Master = value; }
        }

        public DataTable DtSysModPrint
        {
            get
            {
                if (dtSysModPrint == null)
                    dtSysModPrint = new DataTable();
                return dtSysModPrint;
            }
            set { dtSysModPrint = value; }
        }

        public ImageList FileImageList
        {
            get
            {
                if (fileImageList == null)
                    fileImageList = new ImageList();
                return fileImageList;
            }
            set { fileImageList = value; }
        }

        public DataTable DtAccessory
        {
            get { return dtAccessory; }
            set { dtAccessory = value; }
        }

    }
}
