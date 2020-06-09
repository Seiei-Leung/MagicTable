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
    public class RefundsOperator : TaoBaoBase
    {
        public RefundsOperator() : base() { }
        public RefundsOperator(DefaultTopClient client, string sessionKey) : base(client, sessionKey) { }
        private const long DEFAULT_PAGE_SIZE = 100L;
        private const string fields = "refund_id,alipay_no,tid,oid,buyer_nick,seller_nick,total_fee,status,created,refund_fee,good_status,has_good_return,payment,reason,desc,num_iid,title,price,num,good_return_time,company_name,sid,address, shipping_type,refund_remind_timeout";

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.35.U6vnDw&path=cid:10125-apiId:52
        /// <summary>
        /// taobao.refunds.receive.get 查询卖家收到的退款列表
        /// </summary>
        /// <param name="start_Date">修改开始时间</param>
        /// <param name="end_Date">修改结束时间</param>
        /// <returns></returns>
        public List<Refund> GetRefundsReceive(DateTime start_Date, DateTime end_Date, out string errMsg)
        {
            errMsg = "";
            List<Refund> list = new List<Refund>();
            RefundsReceiveGetRequest req = new RefundsReceiveGetRequest();
            req.Fields = "refund_id,modified";
            //req.Status = "WAIT_SELLER_AGREE";
            //req.BuyerNick = "hz0799";
            req.Type = "ec,fixed,auction,auto_delivery,cod,independent_shop_trade,independent_simple_trade,shopex_trade,netcn_trade,external_trade,hotel_trade,fenxiao,game_equipment,instant_trade,b2c_cod,super_market_trade,super_market_cod_trade,alipay_movie,taohua,waimai,nopaid";
            req.StartModified = start_Date;
            req.EndModified = end_Date;
            //req.PageNo = 1L;
            req.PageSize = DEFAULT_PAGE_SIZE;
            //req.UseHasNext = false;
            RefundsReceiveGetResponse response = Client.Execute(req, SessionKey);
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
                //req.UseHasNext = true;
                response = Client.Execute(req, SessionKey);
                if (!response.IsError)
                {
                    //    list.AddRange(response.Refunds.AsEnumerable());

                    if (response.Refunds != null && response.Refunds.Count > 0)
                        foreach(Refund r in response.Refunds)
                        {
                            if ("WAIT_SELLER_AGREE".Equals(r.Status))   //买家已经申请退款，等待卖家同意
                                continue;
                            Refund refund = GetRefundSingle(r.RefundId);
                            if (refund != null)
                            {
                                refund.Modified = r.Modified;
                                list.Add(refund);
                            }
                        }

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


        public Refund GetRefundSingle(long refundId)
        {
            RefundGetRequest req = new RefundGetRequest();
            req.Fields = fields;
            req.RefundId = refundId;
            RefundGetResponse response = Client.Execute(req, SessionKey);
            req = null;
            if (response.IsError)
            {
                return null;
            }
            return response.Refund;
        }

        //http://api.taobao.com/apidoc/api.htm?path=cid:10125-apiId:10480
        /// <summary>
        /// taobao.refund.refuse卖家拒绝退款
        /// <para>卖家拒绝单笔退款交易，要求如下：</para>
        /// <para>1.传入的refund_id和相应的tid, oid必须匹配</para>
        /// <para>2.如果一笔订单只有一笔子订单，则tid必须与oid相同</para>
        /// <para>3.只有卖家才能执行拒绝退款操作</para>
        /// <para>4.以下三种情况不能退款：卖家未发货；7天无理由退换货；网游订单</para>
        /// </summary>
        /// <param name="refund_id">退款单号</param>
        /// <param name="refuse_message">拒绝退款时的说明信息，长度2-200</param>
        /// <param name="tid">退款记录对应的交易订单号</param>
        /// <param name="oid">退款记录对应的交易子订单号</param>
        /// <param name="refuse_fileName">退款凭证文件名称</param>
        /// <param name="refuse_proof">拒绝退款时的退款凭证，一般是卖家拒绝退款时使用的发货凭证，最大长度130000字节，支持的图片格式：GIF, JPG, PNG</param>
        /// <
        public string RefuseRefund(long refund_id, string refuse_message, long tid, long oid, string refuse_fileName, byte[] refuse_proof)
        {
            RefundRefuseRequest req=new RefundRefuseRequest();
            req.RefundId = refund_id;
            req.RefuseMessage = refuse_message;
            req.Tid = tid;
            req.Oid = oid;
            //Top.Api.Util.FileItem fItem = new Top.Api.Util.FileItem(filePath);
            if (!string.IsNullOrEmpty(refuse_fileName) && refuse_proof != null)
            {
                req.RefuseProof = new Top.Api.Util.FileItem(refuse_fileName, refuse_proof);
            }

            RefundRefuseResponse response = Client.Execute(req, SessionKey);
            req = null;
            if (response.IsError)
            {
                return response.SubErrMsg;
            }
            return response.Refund.Status;
        }
    }
}
