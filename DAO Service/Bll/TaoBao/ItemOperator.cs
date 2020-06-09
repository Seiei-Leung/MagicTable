using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api.Request;
using Top.Api.Domain;
using Top.Api.Response;
using Top.Api.Util;
using Top.Api;
using System.Data;
using System.Threading;

namespace Bll.TaoBao
{
    /// <summary>
    /// 提供了商品以及商品相关的sku，邮费增加，修改功能
    /// </summary>
    public class ItemOperator : TaoBaoBase
    {
        public ItemOperator() : base() { }
        public ItemOperator(DefaultTopClient client, string sessionKey) : base(client, sessionKey) { }

        private ItemGetRequest itemGetReq = null;
        /// <summary>
        /// 商品请求对象
        /// </summary>
        private ItemGetRequest ItemGetReq
        {
            get { return itemGetReq; }
            set { itemGetReq = value; }
        }

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.60.4lVnzK&path=cid:4-apiId:18#API-tools
        /// <summary>
        /// taobao.items.onsale.get 获取当前会话用户出售中的商品列表
        /// </summary>
        /// <param name="startModified">修改开始时间</param>
        /// <param name="endModified">修改结束时间</param>
        /// <param name="sellerCids">商品所属的店铺类目列表。按逗号分隔。结构:",cid1,cid2,...,"，如果店铺类目存在二级类目，必须传入子类目cids。</param>
        /// <param name="hasDiscount">是否参与会员折扣。1为是，0为否。其他数值为不过滤该条件 </param>
        /// <param name="hasShowcase">是否橱窗推荐。 1为是，0为否。其他数值为不过滤该条件 </param>
        /// <param name="orderBy">格式为column:asc/desc ，column可选值:list_time(上架时间),delist_time(下架时间),num(商品数量)，modified(最近修改时间);空字符串为默认的上架时间降序(即最新上架排在前面)。</param>
        /// <returns></returns>
        public List<Item> GetItemsOnsale(DateTime startModified, DateTime endModified, string sellerCids, int hasDiscount, int hasShowcase, string orderBy)
        {
            List<Item> list = new List<Item>();
            ItemsOnsaleGetRequest itemsOnsaleGetReq = null;
            ItemsOnsaleGetResponse response = null;
            long pageNo = 1L;

            do
            {
                if (itemsOnsaleGetReq == null)
                {
                    itemsOnsaleGetReq = new ItemsOnsaleGetRequest();
                    itemsOnsaleGetReq.Fields = "num_iid,title,price";
                    //req.Q = "N97";
                    //req.Cid = 1512L;
                    //req.IsTaobao = true;
                    //req.IsEx = true;
                    itemsOnsaleGetReq.PageSize = 200L;
                    if (startModified != null)
                        itemsOnsaleGetReq.StartModified = startModified;
                    if (endModified != null)
                        itemsOnsaleGetReq.EndModified = endModified;
                    if (!sellerCids.Equals(""))
                        itemsOnsaleGetReq.SellerCids = sellerCids;

                    if (hasDiscount == 1)
                        itemsOnsaleGetReq.HasDiscount = true;
                    else if (hasDiscount == 0)
                        itemsOnsaleGetReq.HasDiscount = false;

                    if (hasShowcase == 1)
                        itemsOnsaleGetReq.HasShowcase = true;
                    else if (hasShowcase == 0)
                        itemsOnsaleGetReq.HasShowcase = false;

                    if (!orderBy.Equals(""))
                        itemsOnsaleGetReq.OrderBy = orderBy;
                }
                itemsOnsaleGetReq.PageNo = pageNo++;
                response = Client.Execute(itemsOnsaleGetReq, SessionKey);

                if (response.IsError)
                    return list;

                list.AddRange(response.Items.AsEnumerable());

                if (response.Items.Count < itemsOnsaleGetReq.PageSize)
                    return list;

            } while (list.Count < response.TotalResults);

            return list;
        }

        /// <summary>
        /// taobao.item.get 得到单个商品信息
        /// http://api.taobao.com/apidoc/api.htm?spm=0.0.0.34.VTHL5J&path=cid:4-apiId:20
        /// </summary>
        /// <param name="numIid"></param>
        /// <returns></returns>
        public Item GetItemSingle(long numIid)
        {
            ItemGetReq.Fields = "num_iid,title,price,props_name,props_name,desc,location";
            ItemGetReq.NumIid = numIid;
            //req.TrackIid = "123_track_456";
            ItemGetResponse response = Client.Execute(ItemGetReq, SessionKey);
            //return this.Response2DataSet(response);
            if (response.IsError)
                return null;
            return response.Item;
        }

        //http://api.taobao.com/apidoc/api.htm?spm=0.0.0.30.ZwYZfb&path=cid:3-apiId:161#API-tools
        /// <summary>
        /// 查询商家被授权品牌列表和类目列表
        /// </summary>
        /// <returns></returns>
        public SellerAuthorize GetAuthorizeItemcats()
        {
            ItemcatsAuthorizeGetRequest req = new ItemcatsAuthorizeGetRequest();
            req.Fields = "brand.vid, brand.name, item_cat.cid, item_cat.name, item_cat.status,item_cat.sort_order,item_cat.parent_cid,item_cat.is_parent, xinpin_item_cat.cid, xinpin_item_cat.name, xinpin_item_cat.status, xinpin_item_cat.sort_order, xinpin_item_cat.parent_cid, xinpin_item_cat.is_parent";
            ItemcatsAuthorizeGetResponse response = Client.Execute(req, SessionKey);

            if (response.IsError)
                return null;
            else
                return response.SellerAuthorize;
        }

        public Item AddItem()
        {
            ItemAddRequest addReq = new ItemAddRequest();
            addReq.Num = 30L;
            addReq.Price = "200.07";
            addReq.Type = "fixed";
            addReq.StuffStatus = "new";
            addReq.Title = "Nokia N97全新行货";
            addReq.Desc = "这是一个好商品";
            addReq.LocationState = "浙江";
            addReq.LocationCity = "杭州";
            addReq.ApproveStatus = "onsale";
            addReq.Cid = 1512L;
            addReq.Props = "20000:33564;21514:38489";
            addReq.FreightPayer = "buyer";
            addReq.ValidThru = 7L;
            addReq.HasInvoice = true;
            addReq.HasWarranty = true;
            addReq.HasShowcase = true;
            addReq.SellerCids = "1101,1102,1103";
            addReq.HasDiscount = true;
            addReq.PostFee = "5.07";
            addReq.ExpressFee = "15.07";
            addReq.EmsFee = "25.07";
            DateTime dateTime = DateTime.Parse("2000-01-01 00:00:00");
            addReq.ListTime = dateTime;
            addReq.Increment = "2.50";
            FileItem fItem = new FileItem("fileLocation");
            addReq.Image = fItem;
            addReq.PostageId = 775752L;
            addReq.AuctionPoint = 5L;
            //addReq.PropertyAlias = "pid:vid:别名;pid1:vid1:别名1";
            addReq.InputPids = "pid1,pid2,pid3";
            //addReq.SkuProperties = "pid:vid;pid:vid";
            addReq.SkuQuantities = "2,3,4";
            addReq.SkuPrices = "200.07";
            addReq.SkuOuterIds = "1234,1342";
            addReq.Lang = "zh_CN";
            addReq.OuterId = "12345";
            addReq.ProductId = 123456789L;
            addReq.PicPath = "i7/T1rfxpXcVhXXXH9QcZ_033150.jpg";
            addReq.AutoFill = "time_card";
            addReq.InputStr = "耐克;耐克系列;科比系列;科比系列;2K5,Nike乔丹鞋;乔丹系列;乔丹鞋系列;乔丹鞋系列;";
            addReq.IsTaobao = true;
            addReq.IsEx = true;
            addReq.Is3D = true;
            addReq.SellPromise = true;
            addReq.AfterSaleId = 47758L;
            addReq.CodPostageId = 53899L;
            addReq.IsLightningConsignment = true;
            addReq.Weight = 100L;
            addReq.IsXinpin = false;
            addReq.SubStock = 1L;
            addReq.FoodSecurityPrdLicenseNo = "QS410006010388";
            addReq.FoodSecurityDesignCode = "Q/DHL.001-2008";
            addReq.FoodSecurityFactory = "远东恒天然乳品有限公司";
            addReq.FoodSecurityFactorySite = "台北市仁爱路4段85号";
            addReq.FoodSecurityContact = "00800-021216";
            addReq.FoodSecurityMix = "有机乳糖、有机植物油";
            addReq.FoodSecurityPlanStorage = "常温";
            addReq.FoodSecurityPeriod = "2年";
            addReq.FoodSecurityFoodAdditive = "磷脂 、膨松剂";
            addReq.FoodSecuritySupplier = "深圳岸通商贸有限公司";
            addReq.FoodSecurityProductDateStart = "2012-06-01";
            addReq.FoodSecurityProductDateEnd = "2012-06-10";
            addReq.FoodSecurityStockDateStart = "2012-06-20";
            addReq.FoodSecurityStockDateEnd = "2012-06-30";
            addReq.GlobalStockType = "1";
            addReq.ScenicTicketPayWay = 1L;
            addReq.ScenicTicketBookCost = "5.99";
            addReq.LocalityLifeChooseLogis = "0";
            addReq.LocalityLifeExpirydate = "2012-08-06,2012-08-16";
            addReq.LocalityLifeNetworkId = "5645746";
            addReq.LocalityLifeMerchant = "56879:码商X";
            addReq.LocalityLifeVerification = "101";
            addReq.LocalityLifeRefundRatio = 50L;
            ItemAddResponse response = Client.Execute(addReq, SessionKey);

            return response.Item;
        }

        /// <summary>
        /// 同步淘宝库存
        /// 将本地库存表更新到淘宝网站上的数据库
        /// http://api.taobao.com/apidoc/api.htm?spm=0.0.0.0.tOXHES&path=cid:4-apiId:163
        /// </summary>
        /// <returns></returns>
        public bool UpdateTBStock()
        {
            //ItemUpdateRequest req = new ItemUpdateRequest();
            ////req.NumIid = "2000040899440";
            ////////req.Cid = 1512L;
            ////////req.Props = "20000:33564;21514:38489";
            //req.Num = 154L;
            //req.Price = "150";
            //req.Title = "沙箱测试东方儿女男装秋季休闲";
            //req.Desc = "中山通伟公司在2014年倾力打造的女装款式，值得大家期待";
            //req.LocationState = "广东";
            //req.LocationCity = "中山";


            //req.PostFee = "5.07";
            //req.ExpressFee = "15.07";
            //req.EmsFee = "25.07";
            //DateTime dateTime = DateTime.Parse("2013-01-01 00:00:00");
            //req.ListTime = dateTime;
            //req.Increment = "10.50";
            ////FileItem fItem = new FileItem("fileLocation");
            ////req.Image = fItem;
            //req.StuffStatus = "new";
            //req.AuctionPoint = 5L;
            ////req.PropertyAlias = "pid:vid:别名;pid1:vid1:别名1";
            ////req.InputPids = "pid1,pid2,pid3";
            ////req.SkuQuantities = "2,3,4";
            ////req.SkuPrices = "10.00,5.00";
            ////req.SkuProperties = "pid:vid;pid:vid";
            //req.SellerCids = "1105";
            //req.PostageId = 775752L;
            //req.OuterId = "12345";
            //req.ProductId = 123456789L;
            //req.PicPath = "i7/T1rfxpXcVhXXXH9QcZ_033150.jpg";
            //req.AutoFill = "time_card";
            //req.SkuOuterIds = "1234,1342";
            //req.IsTaobao = true;
            //req.IsEx = true;
            //req.Is3D = true;
            //req.IsReplaceSku = true;
            //req.InputStr = "耐克;耐克系列;科比系列;科比系列;2K5,Nike乔丹鞋;乔丹系列;乔丹鞋系列;乔丹鞋系列;json5";
            //req.Lang = "zh_CN";
            //req.HasDiscount = true;
            //req.HasShowcase = true;
            //req.ApproveStatus = "onsale";
            //req.FreightPayer = "buyer";
            //req.ValidThru = 7L;
            //req.HasInvoice = true;
            //req.HasWarranty = true;
            //req.AfterSaleId = 47745L;
            //req.SellPromise = true;
            //req.CodPostageId = 53899L;
            //req.IsLightningConsignment = true;
            //req.Weight = 100L;
            //req.IsXinpin = true;
            //req.SubStock = 1L;
            //req.FoodSecurityPrdLicenseNo = "QS410006010388";
            //req.FoodSecurityDesignCode = "Q/DHL.001-2008";
            //req.FoodSecurityFactory = "远东恒天然乳品有限公司";
            //req.FoodSecurityFactorySite = "台北市仁爱路4段85号";
            //req.FoodSecurityContact = "00800-021216";
            //req.FoodSecurityMix = "有机乳糖、有机植物油";
            //req.FoodSecurityPlanStorage = "常温";
            //req.FoodSecurityPeriod = "2年";
            //req.FoodSecurityFoodAdditive = "磷脂 、膨松剂";
            //req.FoodSecuritySupplier = "深圳岸通商贸有限公司";
            //req.FoodSecurityProductDateStart = "2012-06-01";
            //req.FoodSecurityProductDateEnd = "2012-06-10";
            //req.FoodSecurityStockDateStart = "2012-06-20";
            //req.FoodSecurityStockDateEnd = "2012-06-30";
            //req.GlobalStockType = "1";
            ////req.SkuSpecIds = "123,123,1243";
            //req.ItemSize = "bulk:8";
            //req.ItemWeight = "10";
            ////req.ChangeProp = "162707:28335:28335,28338";
            ////req.SellPoint = "2013新款 时尚 前卫";
            ////req.DescModules = "[{"module_id":123,"module_name":"模特图","type":"cat_mod","content":"模特要漂亮一点拜托"},{"module_id":null,"module_name":"店主最漂亮","type":"usr_mod","content":"老娘全网最美"}]";
            ////req.FoodSecurityHealthProductNo = "卫食健字(1997)第167号";
            //req.EmptyFields = "food_security.plan_storage,food_security.period";
            //req.LocalityLifeExpirydate = "2012-08-06,2012-08-16";
            //req.LocalityLifeNetworkId = "5645746";
            //req.LocalityLifeMerchant = "56879:码商X";
            //req.LocalityLifeVerification = "1";
            //req.LocalityLifeRefundRatio = 50L;
            //req.LocalityLifeChooseLogis = "0";
            ////req.LocalityLifeOnsaleAutoRefundRatio = 80L;
            ////req.LocalityLifeRefundmafee = "b";
            //req.ScenicTicketPayWay = 1L;
            //req.ScenicTicketBookCost = "5.99";
            ////req.PaimaiInfoMode = 1L;
            //req.PaimaiInfoDeposit = 20L;
            //req.PaimaiInfoInterval = 5L;
            //req.PaimaiInfoReserve = "11";
            //req.PaimaiInfoValidHour = 2L;
            //req.PaimaiInfoValidMinute = 22L;

            //ScitemUpdateRequest req = new ScitemUpdateRequest();
            //req.OuterCode = "SJX100";
            //req.ItemType = 0;


            //ITopClient client = new DefaultTopClient(url, appkey, appsecret);
            //ItemSkusGetRequest req = new ItemSkusGetRequest();
            //req.Fields = "sku_id,num_iid";
            //req.NumIids = "2000040577359";
            //ItemSkusGetResponse response =Client.Execute(req, SessionKey);
            //ProductsGetRequest req = new ProductsGetRequest();

            //req.Fields = "product_id,tsc,cat_name,name,outer_id";
            //req.Nick = "sandbox_b_01";
            //req.PageNo = 1L;
            //req.PageSize = 400L;
            //ProductsGetResponse response = Client.Execute(req);

            //ItemsCustomGetRequest req = new ItemsCustomGetRequest();
            //req.OuterId = "120037200804,120030119215";
            //req.Fields = "num_iid,sku,item_img,prop_img,outer_id ";
            //ItemsCustomGetResponse response = Client.Execute(req, SessionKey);

            //ItemUpdateResponse response = Client.Execute(req, SessionKey);

            DataTable dt = new DataTable();

            dt.Columns.Add("outer_id",typeof(long));
            dt.Columns.Add("qty", typeof(long));
            dt.Columns.Add("GoodsName", typeof(string));

            DataRow dr = dt.NewRow();
            dr["outer_id"] = 120037200804;
            dr["qty"] = 115;
            dr["GoodsName"] = "男装圆领长袖T恤";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["outer_id"] = 120030119215;
            dr1["qty"] = 114;
            dr1["GoodsName"] = "男装衬衣领长袖T恤T恤";
            dt.Rows.Add(dr1);
            
            dt.AcceptChanges();

            UpdateTaoBaoStock(dt);

            //if (response.IsError)
            //{                 
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            
            return true;
        }

        /// <summary>
        /// 更新淘网店上的库存
        ///  http://api.taobao.com/apidoc/api.htm?spm=0.0.0.0.tOXHES&path=cid:4-apiId:163
        /// </summary>
        /// <param name="dtProduct">从本地数据库取出来的商品库存数</param>
        /// <returns></returns>
        public string UpdateTaoBaoStock(DataTable dtProduct)
        {
            //List<string> listResult = new List<string>();
            StringBuilder sbResult = new StringBuilder();
            
            if (dtProduct == null || dtProduct.Rows.Count <= 0) return "请选择需要同步的商品";
            string strouter_id = string.Empty;

            foreach (DataRow dr in dtProduct.Rows)
            {
                if (dr["outer_id"].ToString() != "")
                {
                    if (string.IsNullOrEmpty(strouter_id))
                    {
                        strouter_id = dr["outer_id"].ToString();
                    }
                    else {
                        strouter_id +=","+ dr["outer_id"].ToString();                      
                    }
                }     
            }

            //读取商品信息
            ItemsCustomGetRequest req = new ItemsCustomGetRequest();
            req.OuterId = strouter_id;
            //req.OuterId = "120037200804";
            req.Fields = "num_iid,sku,item_img,prop_img,outer_id";
            ItemsCustomGetResponse response = Client.Execute(req, SessionKey);

            if (!response.IsError)
            {
                if (response.Items == null || response.Items.Count <= 0) return "请将本地商品资料与淘宝商品的商家编码关联起来";

                foreach (DataRow dr in dtProduct.Rows)
                {
                    foreach (Item item in response.Items)
                    {
                        //根据商品外部ID（即是商品编码）更新库存
                        if (item.OuterId.Trim() == dr["outer_id"].ToString().Trim())
                        {
                            ItemUpdateRequest reqItem = new ItemUpdateRequest();
                            reqItem.NumIid = item.NumIid;

                            try
                            {
                                reqItem.Num = long.Parse(dr["qty"].ToString());
                            }
                            catch
                            {
                                reqItem.Num = 0;
                            }

                            //将库存数提交到淘宝的网站
                            ItemUpdateResponse itemReponse = Client.Execute(reqItem, SessionKey);

                            if (!itemReponse.IsError)
                            {
                                sbResult.Append("更新成功，产品名称：" + dr["GoodsName"].ToString() + "(" + dr["outer_id"].ToString() + ")\n\r");
                            }
                            else
                            {
                                sbResult.Append("更新失败，产品名称：" + dr["GoodsName"].ToString() + "(" + dr["outer_id"].ToString() + ")" + ",原因:" + itemReponse.ErrMsg+"\n\r");
                            }
                            itemReponse = null;

                            Thread.Sleep(500);
                            break;
                        }
                    }
                }
            }
            else
            {
                sbResult.Append(response.ErrMsg);
              
            }

            return sbResult.ToString();
        }
    }
}
