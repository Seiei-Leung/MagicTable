using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Domain;
using Top.Api;
using System.Data;

namespace Bll.TaoBao
{
    /// <summary>
    /// 淘宝接口操作类
    /// </summary>
    public class TaoBaoOperator
    {
        const string format = "json";

        private string appKey = "test";
        private string appSecret = "test";
        private string serverUrl = "http://gw.api.tbsandbox.com/router/rest";

        private DefaultTopClient client = null;
        private string sessionKey = "";

        private string nick = "";

        private UserOperator userOp = null; //用户操作类
        private ShopOperator shopOp = null; //店铺操作类
        private LogisticsOperator logisticsOp = null;   //物流操作类
        private ItemOperator itemOp;    //商品操作类
        private TradesOperator tradeOp = null;  //交易订单操作类
        private FenxiaoOperator fenxiaoOp = null;   //分销操作类
        private RefundsOperator refundsOp = null;   //退款操作类
        
        public string AppKey
        {
            get { return appKey; }
            set { appKey = value; }
        }

        public string AppSecret
        {
            get { return appSecret; }
            set { appSecret = value; }
        }

        public string ServerUrl
        {
            get { return serverUrl; }
            set { serverUrl = value; }
        }

        public string SessionKey
        {
            get { return sessionKey; }
            set { sessionKey = value; }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }

        /// <summary>
        /// 请求url
        /// </summary>
        public string ServerUrl1
        {
            get { return serverUrl; }
            set { serverUrl = value; }
        }

        /// <summary>
        /// 交易订单操作类
        /// </summary>
        public TradesOperator TradeOp
        {
            get
            {
                if (tradeOp == null)
                    tradeOp = new TradesOperator(client, sessionKey);
                return tradeOp; 
            }
        }

        /// <summary>
        /// 分销操作类
        /// </summary>
        public FenxiaoOperator FenxiaoOp
        {
            get 
            {
                if (fenxiaoOp == null)
                    fenxiaoOp = new FenxiaoOperator(client, sessionKey);
                return fenxiaoOp; 
            }
            set { fenxiaoOp = value; }
        }

        /// <summary>
        /// 退款操作类
        /// </summary>
        public RefundsOperator RefundsOp
        {
            get
            {
                if (refundsOp == null)
                    refundsOp = new RefundsOperator(client, sessionKey);
                return refundsOp; 
            }
            set { refundsOp = value; }
        }

        /// <summary>
        /// 用户操作类
        /// </summary>
        internal UserOperator UserOp
        {
            get
            {
                if (userOp == null)
                    userOp = new UserOperator(client, sessionKey);
                return userOp;
            }
        }

        /// <summary>
        /// 店铺操作类
        /// </summary>
        public ShopOperator ShopOp
        {
            get
            {
                if (shopOp == null)
                    shopOp = new ShopOperator(client, sessionKey);
                return shopOp;
            }
        }

        /// <summary>
        /// 物流操作类
        /// </summary>
        public LogisticsOperator LogisticsOp
        {
            get
            {
                if (logisticsOp == null)
                    logisticsOp = new LogisticsOperator(client, sessionKey);
                return logisticsOp;
            }
        }

        /// <summary>
        /// 商品操作类
        /// </summary>
        public ItemOperator ItemOp
        {
            get
            {
                if (itemOp == null)
                    itemOp = new ItemOperator(client, sessionKey);
                return itemOp;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appkey"></param>
        /// <param name="appsecret"></param>
        /// <param name="sessionkey"></param>
        public TaoBaoOperator(string appkey, string appsecret, string sessionkey, string serverUrl)
        {
            this.appKey = appkey;
            this.appSecret = appsecret;
            this.sessionKey = sessionkey;
            this.serverUrl = serverUrl;
            SetClient(serverUrl, appKey, appSecret);
        }


        public void SetClient(string serverUrl, string appkey, string appsecret)
        {
            client = new DefaultTopClient(serverUrl, appKey, appSecret, format);
        }

        public User GetSeller()
        {
            return UserOp.GetSeller();
        }

        public Shop GetShop(string nick)
        {
            return ShopOp.GetShop(nick);
        }

        public List<Area> GetAreas()
        {
            return LogisticsOp.GetAreas();
        }

        public List<LogisticsCompany> GetLogisticsCompanies()
        {
            return LogisticsOp.GetLogisticsCompanies(-1, ConstTaoBao.LogisticsOrderMode_NONE);
        }

        public List<SellerCat> GetSellerCats(string nick)
        {
            return ShopOp.GetSellerCats(nick);
        }

        public List<Item> GetItemsOnsale(DateTime startModified, DateTime endModified, string sellerCids, int hasDiscount, int hasShowcase, string orderBy)
        {
            return ItemOp.GetItemsOnsale(startModified, endModified, sellerCids, hasDiscount, hasShowcase, orderBy);
        }

        public SellerAuthorize GetItemcats()
        {
            return ItemOp.GetAuthorizeItemcats();
        }
         
        public List<Trade> GetTradesSold(DateTime start_created, DateTime end_created, string buyerNick, string status, string type, string rateStatus)
        {
            return TradeOp.GetTradesSold(start_created, end_created, buyerNick, status, type, rateStatus);
        }

        public List<Trade> GetTradesSoldIncrement(DateTime start_created, DateTime end_created, string status, string type, out string errMsg)
        {
            return TradeOp.GetTradesSoldIncrement(start_created, end_created, status, type, out errMsg);
        }

        public bool LogisticsOnlineCconfirm(long tid, string out_sid)
        {
            return LogisticsOp.OnlineCconfirm(tid, out_sid);
        }

        public string OnlineSend(long tid, string out_sid, string company_code)
        {
            return LogisticsOp.OnlineSend(tid, out_sid, company_code);
        }

        public string UpdateTradeMemo(long tid, string memo, int flag, bool isReset)
        {
            return TradeOp.UpdateTradeMemo(tid, memo, flag, isReset);
        }

        public string UpdateTradePostage(long tid, string post_fee)
        {
            return TradeOp.UpdateTradePostage(tid, post_fee);
        }

        public string CloseTradeSingle(long tid, string tradeCloseReason)
        {
            return TradeOp.CloseTradeSingle(tid, tradeCloseReason);
        }

        public List<PurchaseOrder> GetFenxiaoOrders(DateTime start_Date, DateTime end_Date, string purchaseOrderId, bool IsByCreateTime, out string errMsg)
        {
            return FenxiaoOp.GetFenxiaoOrders(start_Date, end_Date, purchaseOrderId, IsByCreateTime, out errMsg);
        }

        public string CloseFenxiaoOrder(long oid, string orderCloseReason)
        {
            return FenxiaoOp.CloseFenxiaoOrder(oid, orderCloseReason);
        }

        public string UpdateFenxiaoOrderRemark(long purchaseOrderId, string memo, long flag)
        {
            return FenxiaoOp.UpdateFenxiaoOrderRemark(purchaseOrderId, memo, flag);
        }

        public List<Refund> GetRefundsReceive(DateTime start_Date, DateTime end_Date, out string errMsg)
        {
            errMsg = "";
            return RefundsOp.GetRefundsReceive(start_Date, end_Date, out errMsg);
        }

        public string RefuseRefund(long refund_id, string refuse_message, long tid, long oid, string refuse_fileName, byte[] refuse_proof)
        {
            return refundsOp.RefuseRefund(refund_id, refuse_message, tid, oid, refuse_fileName, refuse_proof);
        }

        public string UpdateTaoBaoStock(DataTable dt)
        {
            return ItemOp.UpdateTaoBaoStock(dt);
        }
    }
}
