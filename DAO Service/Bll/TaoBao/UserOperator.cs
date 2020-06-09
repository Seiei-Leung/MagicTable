using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Request;
using Top.Api.Response;
using System.Data;
using Top.Api.Domain;
using Top.Api;

namespace Bll.TaoBao
{
    /// <summary>
    /// 提供了用户基本信息查询功能
    /// </summary>
    class UserOperator : TaoBaoBase
    {
        public UserOperator() : base() { }
        public UserOperator(DefaultTopClient client, string sessionKey) : base(client, sessionKey) { }

        private UserSellerGetRequest sellerGetReq = null;
        /// <summary>
        /// 卖家请求对象
        /// </summary>
        private UserSellerGetRequest SellerGetReq
        {
            get
            {
                if (sellerGetReq == null)
                    sellerGetReq = new UserSellerGetRequest();
                return sellerGetReq;
            }
        }

        private UserGetRequest userGetReq = null;
        /// <summary>
        /// 用户请求对象
        /// </summary>
        private UserGetRequest UserGetReq
        {
            get
            {
                if (userGetReq == null)
                    userGetReq = new UserGetRequest();
                return userGetReq;
            }
        }

        /// <summary>
        /// taobao.user.seller.get 查询卖家用户信息
        /// <para>http://api.taobao.com/apidoc/dataStruct.htm?path=cid:1-dataStructId:3-apiId:21349-invokePath:user</para>
        /// </summary>
        /// <returns></returns>
        public User GetSeller()
        {
            SellerGetReq.Fields = "user_id,nick,sex,seller_credit,type,has_more_pic,item_img_num,item_img_size,prop_img_num,prop_img_size,auto_repost,promoted_type,status,alipay_bind,consumer_protection,avatar,liangpin,sign_food_seller_promise,has_shop,is_lightning_consignment,has_sub_stock,is_golden_seller,vip_info,magazine_subscribe,vertical_market,online_gaming";
            UserSellerGetResponse response = Client.Execute(SellerGetReq, SessionKey);

            if (response.IsError)
                return null;
            return response.User;
        }

        /// <summary>
        /// taobao.user.get 获取单个用户信息
        /// <para>http://api.taobao.com/apidoc/api.htm?path=cid:1-apiId:1#API-tools</para>
        /// </summary>
        /// <param name="nick">用户昵称</param>
        /// <returns></returns>
        public User GetUser(string nick)
        {
            UserGetReq.Fields = "uid,user_id,nick,buyer_credit,location,email,avatar";
            UserGetReq.Nick = nick;
            UserGetResponse response = Client.Execute(UserGetReq, SessionKey);
            //return Response2String(response);
            //return Response2DataSet(response);
            if (response.IsError)
                return null;
            return response.User;
        }
    }
}
