using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api;

namespace Bll.TaoBao
{
    /// <summary>
    /// 提供了店铺查询，店铺自定义类目的查询和更新。
    /// </summary>
    public class ShopOperator : TaoBaoBase
    {
        public ShopOperator() : base() { }
        public ShopOperator(DefaultTopClient client, string sessionKey) : base(client, sessionKey) { }

        private ShopGetRequest shopGetRequest = null;   //卖家店铺的基本信息的请求对象
        /// <summary>
        /// 卖家店铺的基本信息的请求对象
        /// </summary>
        public ShopGetRequest ShopGetRequest
        {
            get
            {
                if (shopGetRequest == null)
                    shopGetRequest = new ShopGetRequest();

                return shopGetRequest;
            }
        }

        private SellercatsListGetRequest sellercatsListGetReq = null;   //卖家自定义商品类目的请求对象

        /// <summary>
        /// 卖家自定义商品类目的请求对象
        /// </summary>
        public SellercatsListGetRequest SellercatsListGetReq
        {
            get 
            {
                if (sellercatsListGetReq == null)
                    sellercatsListGetReq = new SellercatsListGetRequest();
                return sellercatsListGetReq; 
            }
        }

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.33.fz8XsX&path=cid:9-apiId:68
        /// <summary>
        /// 获取卖家店铺的基本信息
        /// <para></para>
        /// </summary>
        /// <param name="nick">昵称</param>
        /// <returns></returns>
        public Shop GetShop(string nick)
        {
            ShopGetRequest.Fields = "sid,cid,title,desc,bulletin,pic_path,created,modified,shop_score";
            ShopGetRequest.Nick = nick;
            ShopGetResponse response = Client.Execute(ShopGetRequest, SessionKey);

            if (response.IsError)
                return null;
            return response.Shop;
        }

        //http://api.taobao.com/apidoc/api.htm?path=cid:9-apiId:65
        /// <summary>
        /// 获取前台展示的店铺内卖家自定义商品类目
        /// </summary>
        /// <param name="nick">昵称</param>
        /// <returns></returns>
        public List<SellerCat> GetSellerCats(string nick)
        {
            SellercatsListGetReq.Nick = nick;
            SellercatsListGetResponse response = Client.Execute(SellercatsListGetReq, SessionKey);
            if (response.IsError)
                return null;
            return response.SellerCats;
        }
    }
}
