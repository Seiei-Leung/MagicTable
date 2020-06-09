using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Request;
using Top.Api.Domain;
using Top.Api.Response;
using Top.Api;

namespace Bll.TaoBao
{
    /// <summary>
    /// 提供了发货，物流单详情，区域地址和物流公司信息查询功能
    /// </summary>
    public class LogisticsOperator : TaoBaoBase
    {
        public LogisticsOperator() : base() { }
        public LogisticsOperator(DefaultTopClient client, string sessionKey) : base(client, sessionKey) { }

        private LogisticsCompaniesGetRequest logisticsCompaniesGetReq = null;

        private AreasGetRequest areasGetReq = null;
        /// <summary>
        /// 查询地址区域的请求对象
        /// </summary>
        private AreasGetRequest AreasGetReq
        {
            get
            {
                if (areasGetReq == null)
                    areasGetReq = new AreasGetRequest();
                return areasGetReq;
            }
        }

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.44.w3r7lI&path=cid:7-apiId:233
        /// <summary>
        /// taobao.logistics.companies.get 查询物流公司信息
        /// </summary>
        /// <param name="isRecommended">是否查询推荐物流公司.1为是，0为否，其他数值将会返回所有支持电话联系的物流公司</param>
        /// <param name="orderMode">推荐物流公司的下单方式.
        /// <para>LogisticsOrderMode_OFFLINE(电话联系/自己联系),</para>
        /// <para>LogisticsOrderMode_ONLINE(在线下单),</para>
        /// <para>LogisticsOrderMode_ALL(即电话联系又在线下单).</para>
        /// <para>LogisticsOrderMode_NONE(忽略)</para>
        /// <para>注：此参数仅仅用于isRecommended 为1时。就是说对于推荐物流公司才可用.如果不选择此参数将会返回推荐物流中支持电话联系的物流公司.</para></param>
        /// <returns></returns>
        public List<LogisticsCompany> GetLogisticsCompanies(int isRecommended, string orderMode)
        {
            LogisticsCompaniesGetRequest LogisticsCompaniesGetReq = new LogisticsCompaniesGetRequest();
            LogisticsCompaniesGetReq.Fields = "id,code,name,reg_mail_no ";

            if (isRecommended == 1)
            {
                LogisticsCompaniesGetReq.IsRecommended = true;
                if (!orderMode.Equals(""))
                    LogisticsCompaniesGetReq.OrderMode = orderMode;
            }
            else if (isRecommended == 0)
                LogisticsCompaniesGetReq.IsRecommended = false;

            LogisticsCompaniesGetResponse response = Client.Execute(LogisticsCompaniesGetReq);
            LogisticsCompaniesGetReq = null;
            if (response.IsError)
                return null;
            return response.LogisticsCompanies;
        }

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.49.gyZRsY&path=cid:7-apiId:10689#API-tools
        /// <summary>
        /// taobao.logistics.online.confirm 确认发货通知接口
        /// </summary>
        /// <param name="tid">淘宝交易ID</param>
        /// <param name="out_sid">运单号</param>
        /// <returns></returns>
        public bool OnlineCconfirm(long tid, string out_sid)
        {
            LogisticsOnlineConfirmRequest req = new LogisticsOnlineConfirmRequest();
            req.Tid = tid;
            req.OutSid = out_sid;
            LogisticsOnlineConfirmResponse response = Client.Execute(req, SessionKey);
            req = null;
            if (response.IsError)
                return false;
            return response.Shipping.IsSuccess;
        }


        public string OnlineSend(long tid, string out_sid, string company_code)
        {
            LogisticsOnlineSendRequest req = new LogisticsOnlineSendRequest();
            req.Tid = tid;
            req.OutSid = out_sid;
            req.CompanyCode = company_code;
            //req.SenderId = 123456L;
            //req.CancelId = 123456L;
            //req.Feature = "identCode=tid:aaa,bbb;machineCode=tid2:aaa";
            LogisticsOnlineSendResponse response = Client.Execute(req, SessionKey);
            req = null;
            if (response.IsError)
                return response.SubErrMsg;
            return "SUCCESS";
        }


        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.34.6ia2xu&path=cid:7-apiId:59
        /// <summary>
        /// taobao.areas.get 查询地址区域
        /// </summary>
        /// <returns></returns>
        public List<Area> GetAreas()
        {
            AreasGetReq.Fields = "id,type,name,parent_id,zip";
            AreasGetResponse response = Client.Execute(areasGetReq);
            if (response.IsError)
                return null;
            return response.Areas;
        }

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.35.uQx6L2&path=cid:7-apiId:10918
        /// <summary>
        /// taobao.delivery.template.add 新增运费模板
        /// </summary>
        /// <param name="name">运费模板的名称</param>
        /// <param name="assumer">可选值：0,1 说明:0:表示买家承担服务费;1:表示卖家承担服务费 </param>
        /// <param name="valuation">计费方式，可选值：0,1,3 说明:0:表示按宝贝件数计算运费 1:表示按宝贝重量计算运费 3:表示按宝贝体积计算运费</param>
        /// <param name="templateTypes">运费方式平邮 (post),快递公司(express),EMS (ems),货到付款(cod),物流宝保障速递(wlb),家装物流(furniture)结构:value1;value2;value3;value4 如: post;express;ems;cod </param>
        /// <param name="templateDests">邮费子项涉及的地区.</param>
        /// <param name="templateStartStandards">首费标准</param>
        /// <param name="templateStartFees">首费</param>
        /// <param name="TemplateAddStandards">增费标准</param>
        /// <param name="TemplateAddFees">增费</param>
        /// <param name="consignAreaId">卖家发货地址区域ID</param>
        /// <returns></returns>
        public DeliveryTemplate AddDeliveryTemplate(string name, long assumer, long valuation, string templateTypes,
                                    string templateDests, string templateStartStandards, string templateStartFees,
                                    string TemplateAddStandards, string TemplateAddFees, long consignAreaId)
        {
            DeliveryTemplateAddRequest req = new DeliveryTemplateAddRequest();
            req.Name = name;
            req.Assumer = assumer;
            req.Valuation = valuation;
            req.TemplateTypes = templateTypes;
            req.TemplateDests = templateDests;
            req.TemplateStartStandards = templateStartStandards;
            req.TemplateStartFees = templateStartFees;
            req.TemplateAddStandards = TemplateAddStandards;
            req.TemplateAddFees = TemplateAddFees;
            if (!"".Equals(consignAreaId))
                req.ConsignAreaId = consignAreaId;
            DeliveryTemplateAddResponse response = Client.Execute(req, SessionKey);
            req = null;

            if (response.IsError)
                return null;
            req = null;
            return response.DeliveryTemplate;
        }
    }
}
