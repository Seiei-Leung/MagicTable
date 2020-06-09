using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Domain;
using Top.Api;
using Top.Api.Util;
using System.IO;
using System.Threading;

namespace Bll.TaoBao
{
    /// <summary>
    /// 提供了订单下载，修改收货地址、修改交易备注等功能
    /// </summary>
    public class TradesOperator : TaoBaoBase
    {        
        public TradesOperator() : base() { }
        public TradesOperator(DefaultTopClient client, string sessionKey) : base(client, sessionKey) { }
        private const long DEFAULT_PAGE_SIZE = 100L;
        private string fields = "seller_nick,buyer_nick,title,type,created,tid,seller_rate,buyer_rate,buyer_message,seller_memo,status,payment,discount_fee,adjust_fee,post_fee,total_fee,pay_time,end_time,modified,consign_time,buyer_obtain_point_fee,point_fee,real_point_fee,received_payment,pic_path,num_iid,num,price,cod_fee,cod_status,shipping_type,receiver_name,receiver_state,receiver_city,receiver_district,receiver_address,receiver_zip,receiver_mobile,receiver_phone,seller_flag,alipay_id,alipay_no,is_lgtype,is_force_wlb,is_brand_sale,buyer_area,has_buyer_message,credit_card_fee,lg_aging_type,lg_aging,step_trade_status,step_paid_fee,mark_desc,has_yfx,yfx_fee,yfx_id,yfx_type,trade_source,orders,service_orders";

        private TradeCloseRequest tradeCloseReq = null;
        /// <summary>
        /// 关闭一笔交易的请求对象
        /// </summary>
        private TradeCloseRequest TradeCloseReq
        {
            get 
            {
                if (tradeCloseReq == null)
                    tradeCloseReq = new TradeCloseRequest();
                return tradeCloseReq; 
            }
        }

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.33.dDQmZG&path=cid:5-apiId:83
        /// <summary>
        /// taobao.trade.close 卖家关闭一笔交易（成功：返回修改时间，否则为错误消息）
        /// </summary>
        /// <param name="tid">主订单或子订单编号</param>
        /// <param name="closeReason">交易关闭原因TradeCloseReason</param>
        /// <returns>成功：返回修改时间，否则为错误消息</returns>
        public string CloseTradeSingle(long tid, string tradeCloseReason)
        {
            TradeCloseReq.Tid = tid;
            TradeCloseReq.CloseReason = tradeCloseReason;
            TradeCloseResponse response = Client.Execute(TradeCloseReq, SessionKey);
            //return Response2DataSet(response);
            if (response.IsError)
            {
                return response.SubErrMsg;
            }
            return response.Trade.Modified;
        }

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.44.EZmR1r&path=cid:5-apiId:46
        /// <summary>
        /// taobao.trades.sold.get 查询卖家已卖出的交易数据（根据创建时间（含时间））
        /// <para>注：只能获取到三个月以内的交易信息</para>
        /// </summary>
        /// <param name="start_created">开始时间</param>
        /// <param name="end_created">结束时间</param>
        /// <param name="buyerNick">买家昵称，空字符串为忽略</param>
        /// <param name="status">TradeStatus类，空字符串为默认值</param>
        /// <param name="type">交易类型列表TradeType，多个可用逗号隔开，空字符串为默认值</param>
        /// <param name="rateStatus">评价状态TradeRateStatus，空字符串为所有评价</param>
        /// <returns></returns>
        public List<Trade> GetTradesSold(DateTime start_created, DateTime end_created, string buyerNick, string status, string type, string rateStatus)
        {
            List<Trade> list = new List<Trade>();
            TradesSoldGetRequest tradesSoldGetReq = null;
            TradesSoldGetResponse response = null;
            long pageNo = 1L;

            do
            {
                if (tradesSoldGetReq == null)
                {
                    tradesSoldGetReq = new TradesSoldGetRequest();
                    tradesSoldGetReq.Fields = fields;
                    tradesSoldGetReq.StartCreated = start_created;
                    tradesSoldGetReq.EndCreated = end_created;

                    if (!status.Equals(""))
                        tradesSoldGetReq.Status = status;
                    if (!type.Equals(""))
                        tradesSoldGetReq.Type = type;
                    if (!buyerNick.Equals(""))
                        tradesSoldGetReq.BuyerNick = buyerNick;
                    if (!rateStatus.Equals(""))
                        tradesSoldGetReq.RateStatus = rateStatus;

                    //TradesSoldGetReq.ExtType = "service";
                    //TradesSoldGetReq.Tag = "time_card";

                    tradesSoldGetReq.PageSize = 100L;
                    tradesSoldGetReq.UseHasNext = true;
                }
                tradesSoldGetReq.PageNo = pageNo++;
                response = Client.Execute(tradesSoldGetReq, SessionKey);
                if (response.IsError)
                    return list;
                if(response.Trades != null)
                    list.AddRange(response.Trades.AsEnumerable());

            } while (response.HasNext);
            return list;
        }


        //http://api.taobao.com/apidoc/api.htm?path=cid:5-apiId:47
        /// <summary>
        /// taobao.trade.get 获取单笔交易的部分信息(性能高)
        /// </summary>
        /// <param name="tid">交易id</param>
        /// <returns></returns>
        public Trade GetTradeFullInfo(long tid)
        {
            TradeFullinfoGetRequest req = new TradeFullinfoGetRequest();
            req.Fields = fields;
            req.Tid = tid;
            TradeFullinfoGetResponse response = Client.Execute(req, SessionKey);
            if (response.IsError)
                return null;
            return response.Trade;
        }


        //http://api.taobao.com/apidoc/api.htm?path=cid:5-apiId:11117
        /// <summary>
        /// taobao.topats.trades.sold.get 异步获取三个月内已卖出的交易详情(支持超大卖家) 
        /// </summary>
        /// <param name="startDate">订单创建开始时间(yyyyMMdd)</param>
        /// <param name="endDate">订单创建结束时间(yyyyMMdd)</param>
        /// <returns></returns>
        public long GetTradesSoldAsyn(string startDate, string endDate)
        {
            TopatsTradesSoldGetRequest req = new TopatsTradesSoldGetRequest();
            req.Fields = fields;
            req.StartTime = startDate;
            req.EndTime = endDate;
            //req.IsAcookie = true;
            TopatsTradesSoldGetResponse response = Client.Execute(req, SessionKey);

            if (response.IsError)
            {
                if (response.SubErrCode.Equals("isv.task-duplicate"))   //重复请求
                {
                    return long.Parse(StringHandler.GetStrAfter(response.SubErrMsg, "="));
                }
                return 0;
            }
            else
            {
                return response.Task.TaskId;
            }
            
        }

        /// <summary>
        /// 根据任务ID返回订单列表
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public List<Trade> GetResultByTaskId(long taskId)
        {
            string url = GetTaskResultUrl(taskId);
            if (!string.IsNullOrEmpty(url))
                return DownloadAndProcess(url);
            return null;
        }
        
        /// <summary>
        /// 根据任务号获取结果路径
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        private string GetTaskResultUrl(long taskId)
        {
            TopatsResultGetRequest req = new TopatsResultGetRequest();
            req.TaskId = taskId;
            TopatsResultGetResponse rsp = Client.Execute(req);
            if (!rsp.IsError)
            {
                if ("done".Equals(rsp.Task.Status))
                {
                    return rsp.Task.DownloadUrl;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据淘宝指定路径下载订单到本地文件夹，并读取返回订单列表
        /// </summary>
        /// <param name="url">淘宝指定路径</param>
        /// <returns></returns>
        private List<Trade> DownloadAndProcess(string url)
        {
            if (!Directory.Exists(zipPath))
                Directory.CreateDirectory(zipPath);

            if (!Directory.Exists(unzipPaht))
                Directory.CreateDirectory(unzipPaht);

            string zipFile = AtsUtils.Download(url, zipPath);
            List<string> files = AtsUtils.Unzip(zipFile, unzipPaht);

            List<Trade> list = new List<Trade>();

            foreach (string file in files)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        TradeFullinfoGetResponse rsp = TopUtils.ParseResponse<TradeFullinfoGetResponse>(line);

                        if (!rsp.Trade.Status.Equals(ConstTaoBao.TradeStatus_NO_CREATE_PAY) &&   //没有创建支付宝交易
                            !rsp.Trade.Status.Equals(ConstTaoBao.TradeStatus_WAIT_BUYER_PAY))    //等待买家付款
                            list.Add(rsp.Trade);
                    }
                }
            }
            return list;
        }

        //http://api.taobao.com/apidoc/api.htm?path=cid:5-apiId:128
        /// <summary>
        /// taobao.trades.sold.increment.get 查询卖家已卖出的增量交易数据（根据修改时间）
        /// <para>一次请求只能查询时间跨度为一天的增量交易记录</para>
        /// </summary>
        /// <param name="start_modified">开始时间，格式:yyyy-MM-dd HH:mm:ss</param>
        /// <param name="end_modified">结束时间，格式:yyyy-MM-dd HH:mm:ss</param>
        /// <param name="status">交易状态</param>
        /// <param name="type">交易类型</param>
        /// <returns></returns>
        public List<Trade> GetTradesSoldIncrement(DateTime start_modified, DateTime end_modified, string status, string type, out string errMsg)
        {
            errMsg = "";
            List<Trade> list = new List<Trade>();
            TradesSoldIncrementGetRequest req = new TradesSoldIncrementGetRequest();
            req.Fields = "tid,status";
            req.StartModified = start_modified;
            req.EndModified = end_modified;
            if(!string.IsNullOrEmpty(status))
                req.Status = status;
            if (!string.IsNullOrEmpty(type))
                req.Type = type;
            req.PageSize = DEFAULT_PAGE_SIZE;
            req.UseHasNext = false;

            TradesSoldIncrementGetResponse response = Client.Execute(req, SessionKey);
            if (response.IsError)
            {
                errMsg = response.SubErrMsg;
                return null;
            }

            long pageCount = ((response.TotalResults + req.PageSize - 1) / req.PageSize).Value;
            int recount = 0;    //重复请求次数
            while (pageCount > 0)   //按页反方向查询
            {
                req.PageNo = pageCount;
                req.UseHasNext = true;
                response = Client.Execute(req, SessionKey);
                if (!response.IsError)
                {
                    //list.AddRange(response.Trades.AsEnumerable());
                    foreach (Trade trade in response.Trades)
                    {
                        if (!trade.Status.Equals(ConstTaoBao.TradeStatus_NO_CREATE_PAY) &&   //没有创建支付宝交易
                            !trade.Status.Equals(ConstTaoBao.TradeStatus_WAIT_BUYER_PAY))    //等待买家付款
                        {
                            Trade t = GetTradeFullInfo(trade.Tid);
                            if (t != null)
                                list.Add(t);
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

            //long pageNo = 1L;
            //do
            //{
            //    if (req == null)
            //    {
            //        req = new TradesSoldIncrementGetRequest();
                    
            //    }
            //    req.PageNo = pageNo++;
            //    response = Client.Execute(req, SessionKey);
            //    if (response.IsError)
            //        return null;
            //    if (response.Trades != null)
            //    {
            //        //list.AddRange(response.Trades.AsEnumerable());
            //        foreach (Trade trade in response.Trades)
            //        {
            //            if (!trade.Status.Equals(ConstTaoBao.TradeStatus_NO_CREATE_PAY) &&   //没有创建支付宝交易
            //                !trade.Status.Equals(ConstTaoBao.TradeStatus_WAIT_BUYER_PAY))    //等待买家付款
            //            {
            //                Trade t = GetTradeFullInfo(trade.Tid);
            //                if (t != null)
            //                    list.Add(t);
            //            }
            //        }
            //    }
            //}
            //while (response.HasNext);

        }

        //http://api.taobao.com/apidoc/api.htm?path=cid:5-apiId:49
        /// <summary>
        /// taobao.trade.memo.update 修改一笔交易备注
        /// </summary>
        /// <param name="tid">交易订单编号</param>
        /// <param name="memo">交易备注</param>
        /// <param name="flag">交易备注旗帜</param>
        /// <param name="isReset">是否清空备注</param>
        /// <returns></returns>
        public string UpdateTradeMemo(long tid, string memo, int flag, bool isReset)
        {
            TradeMemoUpdateRequest req = new TradeMemoUpdateRequest();
            req.Tid = tid;
            req.Memo = memo;
            req.Flag = flag;
            req.Reset = isReset;
            TradeMemoUpdateResponse response = Client.Execute(req, SessionKey);
            req = null;
            if (response.IsError)
            {
                return response.SubErrMsg;
            }
            return response.Trade.Modified;
        }

        //http://api.taobao.com/apidoc/api.htm?path=cid:5-apiId:10897
        /// <summary>
        /// taobao.trade.postage.update 修改订单邮费价格
        /// </summary>
        /// <param name="tid">交易订单编号</param>
        /// <param name="post_fee">邮费价格(邮费单位是元）</param>
        /// <returns>返回trade类型，其中包含修改时间modified，修改邮费post_fee，修改后的总费用total_fee和买家实付款payment</returns>
        public string UpdateTradePostage(long tid, string post_fee)
        {
            TradePostageUpdateRequest req = new TradePostageUpdateRequest();
            req.Tid = tid;
            req.PostFee = post_fee;
            TradePostageUpdateResponse response = Client.Execute(req, SessionKey);
            req = null;
            if (response.IsError)
            {
                return response.SubErrMsg;
            }
            return response.Trade.Modified;
        }


    }
}