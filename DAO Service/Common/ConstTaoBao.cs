using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class ConstTaoBao
    {
        # region 推荐物流公司的下单方式LogisticsOrderMode
        /// <summary>
        /// 电话联系/自己联系
        /// </summary>
        public const string LogisticsOrderMode_OFFLINE = "offline";
        /// <summary>
        /// 在线下单
        /// </summary>
        public const string LogisticsOrderMode_ONLINE = "online";
        /// <summary>
        /// 电话联系又在线下单
        /// </summary>
        public const string LogisticsOrderMode_ALL = "all";
        /// <summary>
        /// 忽略
        /// </summary>
        public const string LogisticsOrderMode_NONE = "";
        # endregion

        #region 交易关闭原因TradeCloseReason
        /// <summary>
        /// 买家不想买了
        /// </summary>
        public const string TradeCloseReason_BuyerNotBuy = "买家不想买了";
        /// <summary>
        /// 信息填写错误，重新拍
        /// </summary>
        public const string TradeCloseReason_InfoError = "信息填写错误，重新拍";
        /// <summary>
        /// 卖家缺货
        /// </summary>
        public const string TradeCloseReason_Stockout = "卖家缺货";
        /// <summary>
        /// 同城见面交
        /// </summary>
        public const string TradeCloseReason_MeetTrading = "同城见面交";
        /// <summary>
        /// 其他原因
        /// </summary>
        public const string TradeCloseReason_OtherReasons = "其他原因";
        #endregion

        /// <summary>
        /// 根据描述返回交易状态代码
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string GetTradeStatus(string desc)
        {
            switch (desc)
            {
                case "全部":
                    return "";
                case "没有创建支付宝交易":
                    return TradeStatus_NO_CREATE_PAY;
                case "等待买家付款":
                    return TradeStatus_WAIT_BUYER_PAY;
                case "等待卖家发货,即:买家已付款":
                    return TradeStatus_WAIT_SELLER_SEND_GOODS;
                case "等待买家确认收货,即:卖家已发货":
                    return TradeStatus_WAIT_BUYER_CONFIRM_GOODS;
                case "买家已签收,货到付款专用":
                    return TradeStatus_BUYER_SIGNED;
                case "交易成功":
                    return TradeStatus_FINISHED;
                case "交易关闭":
                    return TradeStatus_CLOSED;
                case "交易被淘宝关闭":
                    return TradeStatus_CLOSED_BY_TAOBAO;
                case "等待买家付款、没有创建支付宝交易":
                    return TradeStatus_ALL_WAIT_PAY;
                case "交易关闭、交易被淘宝关闭":
                    return TradeStatus_ALL_CLOSED;
                case "已付款（已分账），已发货":
                    return TradeStatus_WAIT_BUYER_CONFIRM_GOODS_ACOUNTED;
                case "已付款（已分账），待发货":
                    return TradeStatus_WAIT_SELLER_SEND_GOODS_ACOUNTED;
                default:
                    return "";
            }
        }
        
        #region 交易状态TradeStatus
        /// <summary>
        /// 没有创建支付宝交易
        /// </summary>
        public const string TradeStatus_NO_CREATE_PAY = "TRADE_NO_CREATE_PAY";
        /// <summary>
        /// 等待买家付款
        /// </summary>
        public const string TradeStatus_WAIT_BUYER_PAY = "WAIT_BUYER_PAY";
        /// <summary>
        /// 等待卖家发货,即:买家已付款
        /// </summary>
        public const string TradeStatus_WAIT_SELLER_SEND_GOODS = "WAIT_SELLER_SEND_GOODS";
        /// <summary>
        /// 等待买家确认收货,即:卖家已发货
        /// </summary>
        public const string TradeStatus_WAIT_BUYER_CONFIRM_GOODS = "WAIT_BUYER_CONFIRM_GOODS";
        /// <summary>
        /// 买家已签收,货到付款专用
        /// </summary>
        public const string TradeStatus_BUYER_SIGNED = "TRADE_BUYER_SIGNED";
        /// <summary>
        /// 交易成功
        /// </summary>
        public const string TradeStatus_FINISHED = "TRADE_FINISHED";
        /// <summary>
        /// 交易关闭
        /// </summary>
        public const string TradeStatus_CLOSED = "TRADE_CLOSED";
        /// <summary>
        /// 交易被淘宝关闭
        /// </summary>
        public const string TradeStatus_CLOSED_BY_TAOBAO = "TRADE_CLOSED_BY_TAOBAO";
        /// <summary>
        /// 包含：等待买家付款、没有创建支付宝交易
        /// </summary>
        public const string TradeStatus_ALL_WAIT_PAY = "ALL_WAIT_PAY";
        /// <summary>
        /// 包含：交易关闭、交易被淘宝关闭
        /// </summary>
        public const string TradeStatus_ALL_CLOSED = "ALL_CLOSED";

        /// <summary>
        /// 已付款（已分账），已发货(分销)
        /// </summary>
        public const string TradeStatus_WAIT_BUYER_CONFIRM_GOODS_ACOUNTED = "WAIT_BUYER_CONFIRM_GOODS_ACOUNTED";

        /// <summary>
        /// 已付款（已分账），待发货(分销)
        /// </summary>
        public const string TradeStatus_WAIT_SELLER_SEND_GOODS_ACOUNTED = "WAIT_SELLER_SEND_GOODS_ACOUNTED";

        #endregion

        /// <summary>
        /// 根据描述返回交易类型
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string GetTradeType(string desc)
        {
            switch (desc)
            {
                case "全部":
                    return "";
                case "一口价":
                    return TradeType_FIXED;
                case "拍卖":
                    return TradeType_AUCTION;
                case "一口价、拍卖":
                    return TradeType_GUARANTEE_TRADE;
                case "自动发货":
                    return TradeType_AUTO_DELIVERY;
                case "旺店入门版交易":
                    return TradeType_INDEPENDENT_SIMPLE_TRADE;
                case "旺店标准版交易":
                    return TradeType_INDEPENDENT_SHOP_TRADE;
                case "直冲":
                    return TradeType_EC;
                case "货到付款":
                    return TradeType_COD;
                case "分销":
                    return TradeType_FENXIAO;
                case "游戏装备":
                    return TradeType_GAME_EQUIPMENT;
                case "ShopEX交易":
                    return TradeType_SHOPEX_TRADE;
                case "万网交易":
                    return TradeType_NETCN_TRADE;
                case "统一外部交易":
                    return TradeType_EXTERNAL_TRADE;
                case "万人团":
                    return TradeType_STEP;
                case "代销":
                    return TradeType_AGENT;
                case "经销":
                    return TradeType_DEALER;
                default:
                    return "";
            }
        }

        #region 交易类型列表TradeType
        /// <summary>
        /// 一口价
        /// </summary>
        public const string TradeType_FIXED = "fixed";
        /// <summary>
        /// 拍卖
        /// </summary>
        public const string TradeType_AUCTION = "auction";
        /// <summary>
        /// 一口价、拍卖
        /// </summary>
        public const string TradeType_GUARANTEE_TRADE = "guarantee_trade";
        /// <summary>
        /// 自动发货
        /// </summary>
        public const string TradeType_AUTO_DELIVERY = "auto_delivery";
        /// <summary>
        /// 旺店入门版交易
        /// </summary>
        public const string TradeType_INDEPENDENT_SIMPLE_TRADE = "independent_simple_trade";
        /// <summary>
        /// 旺店标准版交易
        /// </summary>
        public const string TradeType_INDEPENDENT_SHOP_TRADE = "independent_shop_trade";
        /// <summary>
        /// 直冲
        /// </summary>
        public const string TradeType_EC = "ec";
        /// <summary>
        /// 货到付款
        /// </summary>
        public const string TradeType_COD = "cod";
        /// <summary>
        /// 分销
        /// </summary>
        public const string TradeType_FENXIAO = "fenxiao";
        /// <summary>
        /// 游戏装备
        /// </summary>
        public const string TradeType_GAME_EQUIPMENT = "game_equipment";
        /// <summary>
        /// ShopEX交易
        /// </summary>
        public const string TradeType_SHOPEX_TRADE = "shopex_trade";
        /// <summary>
        /// 万网交易
        /// </summary>
        public const string TradeType_NETCN_TRADE = "netcn_trade";
        /// <summary>
        /// 统一外部交易
        /// </summary>
        public const string TradeType_EXTERNAL_TRADE = "external_trade";
        /// <summary>
        /// 万人团
        /// </summary>
        public const string TradeType_STEP = "step";
        /// <summary>
        /// 代销
        /// </summary>
        public const string TradeType_AGENT = "AGENT";
        /// <summary>
        /// 经销
        /// </summary>
        public const string TradeType_DEALER = "DEALER";

        #endregion

        /// <summary>
        /// 根据描述返回评价状态
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static string GetTradeRateStatus(string desc)
        {
            switch (desc)
            {
                case "全部":
                    return "";
                case "买家未评":
                    return TradeRateStatus_UNBUYER;
                case "卖家未评":
                    return TradeRateStatus_UNSELLER;
                case "买家已评，卖家未评":
                    return TradeRateStatus_BUYER_UNSELLER;
                case "买家未评，卖家已评":
                    return TradeRateStatus_UNBUYER_SELLER;
                default:
                    return "";
            }
        }

        #region 评价状态TradeRateStatus
        /// <summary>
        /// 买家未评
        /// </summary>
        public const string TradeRateStatus_UNBUYER = "RATE_UNBUYER";
        /// <summary>
        /// 卖家未评
        /// </summary>
        public const string TradeRateStatus_UNSELLER = "RATE_UNSELLER";
        /// <summary>
        /// 买家已评，卖家未评
        /// </summary>
        public const string TradeRateStatus_BUYER_UNSELLER = "RATE_BUYER_UNSELLER";
        /// <summary>
        /// 买家未评，卖家已评
        /// </summary>
        public const string TradeRateStatus_UNBUYER_SELLER = "RATE_UNBUYER_SELLER";
        #endregion


    }
}
