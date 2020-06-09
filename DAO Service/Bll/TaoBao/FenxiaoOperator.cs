using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Domain;

namespace Bll.TaoBao
{
    public class FenxiaoOperator : TaoBaoBase
    {
        public FenxiaoOperator() : base() { }
        public FenxiaoOperator(DefaultTopClient client, string sessionKey) : base(client, sessionKey) { }
        private const long DEFAULT_PAGE_SIZE = 50L;
        //private string fields = "supplier_memo,fenxiao_id,pay_type,trade_type,distributor_from,id,status,buyer_nick,memo,tc_order_id,receiver,shipping,logistics_company_name,logistics_id,isv_custom_key,isv_custom_value,end_time,supplier_flag,buyer_payment,supplier_from,supplier_username,distributor_username,created,alipay_no,total_fee,post_fee,distributor_payment,snapshot_url,pay_time,consign_time,modified,sub_purchase_orders";


        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.53.41mjwl&path=cid:15-apiId:180#API-tools
        /// <summary>
        /// taobao.fenxiao.orders.get 查询采购单信息
        /// <para>采购单查询的起始时间与结束时间跨度不能超过7天</para>
        /// </summary>
        /// <param name="start_Date">起始时间，注：若purchase_order_id没传，则此参数必传。</param>
        /// <param name="end_Date">结束时间，注：若purchase_order_id没传，则此参数必传。</param>
        /// <param name="purchaseOrderId">采购单编号或分销流水号，注：若其它参数没传，则此参数必传。</param>
        /// <param name="IsByUpdateTime">是否按订单成交时间查询</param>
        /// <returns></returns>
        public List<PurchaseOrder> GetFenxiaoOrders(DateTime start_Date, DateTime end_Date, string purchaseOrderId, bool IsByCreateTime, out string errMsg)
        {
            errMsg = "";
            List<PurchaseOrder> list = new List<PurchaseOrder>();
            FenxiaoOrdersGetRequest req = new FenxiaoOrdersGetRequest();
            req.StartCreated = start_Date;
            req.EndCreated = end_Date;
            req.PageSize = DEFAULT_PAGE_SIZE;
            if (!string.IsNullOrEmpty(purchaseOrderId))
                req.PurchaseOrderId = Convert.ToInt64(purchaseOrderId);
            if (IsByCreateTime)
                req.TimeType = "trade_time_type";
            else
                req.TimeType = "update_time_type";
            //req.Fields = fields;
            FenxiaoOrdersGetResponse response = Client.Execute(req, SessionKey);
            if (response.IsError)
            {
                errMsg = response.SubErrMsg;
                return null;
            }
            else if (response.TotalResults == 0)
                return list;

            long pageCount = ((response.TotalResults + req.PageSize - 1) / req.PageSize).Value;
            int recount = 0;    //重复请求次数
            while (pageCount > 0)   //按页反方向查询
            {
                req.PageNo = pageCount;
                response = Client.Execute(req, SessionKey);
                if (!response.IsError)
                {
                    if(response.PurchaseOrders != null && response.PurchaseOrders.Count > 0)
                        list.AddRange(response.PurchaseOrders.AsEnumerable());

                    pageCount--;
                    recount = 0;
                }
                else
                {
                    recount++;
                    if (recount > 3)    //最多重复请求3次
                        pageCount--;
                }
            }
            return list;
        }

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.47.SUO0RV&path=cid:15-apiId:21630
        /// <summary>
        /// taobao.fenxiao.order.close 供应商关闭采购单
        /// </summary>
        /// <param name="oid">采购单编号</param>
        /// <param name="orderCloseReason">关闭理由</param>
        /// <returns></returns>
        public string CloseFenxiaoOrder(long oid, string orderCloseReason)
        {
            FenxiaoOrderCloseRequest req = new FenxiaoOrderCloseRequest();
            req.Message = orderCloseReason;
            req.PurchaseOrderId = oid;
            //req.SubOrderIds = "123123,12312";
            FenxiaoOrderCloseResponse response = Client.Execute(req, SessionKey);
            req = null;
            if (response.IsError)
                return response.SubErrMsg;
            else
                return "SUCCESS";
        }


        //http://api.taobao.com/apidoc/api.htm?path=cid:15-apiId:11106#API-tools
        /// <summary>
        /// taobao.fenxiao.order.remark.update 修改采购单备注
        /// </summary>
        /// <param name="purchaseOrderId">采购单编号</param>
        /// <param name="memo">备注</param>
        /// <param name="flag">旗帜</param>
        /// <returns></returns>
        public string UpdateFenxiaoOrderRemark(long purchaseOrderId, string memo, long flag)
        {
            FenxiaoOrderRemarkUpdateRequest req = new FenxiaoOrderRemarkUpdateRequest();
            req.PurchaseOrderId = purchaseOrderId;
            req.SupplierMemo = memo;
            req.SupplierMemoFlag = flag;
            FenxiaoOrderRemarkUpdateResponse response = Client.Execute(req, SessionKey);
            req = null;
            if (response.IsError)
                return response.SubErrMsg;
            else
                return "SUCCESS";
        }

    }
}
