using System;
using System.Collections.Generic;
using System.Text;

namespace TradingViewWatchlistGenerator
{
    internal class Class1
    {
    }


    public class ByBitRootObject
    {
        public int ret_code { get; set; }
        public string ret_msg { get; set; }
        public string ext_code { get; set; }
        public Result result { get; set; }
        public object ext_info { get; set; }
        public object token { get; set; }
        public string time_now { get; set; }
    }

    public class Result
    {
        public Inverseperpetual[] InversePerpetual { get; set; }
        public Linearperpetual[] LinearPerpetual { get; set; }
        public Inversefuture[] InverseFutures { get; set; }
        public string allTags { get; set; }
    }

    public class Inverseperpetual
    {
        public int symbol { get; set; }
        public string symbolName { get; set; }
        public string baseCurrency { get; set; }
        public string quoteCurrency { get; set; }
        public int quoteSymbol { get; set; }
        public int priceScale { get; set; }
        public string contractType { get; set; }
        public string maxPrice { get; set; }
        public string minPrice { get; set; }
        public string minQty { get; set; }
        public string maxNewOrderQty { get; set; }
        public int walletBalanceFraction { get; set; }
        public string upnlFraction { get; set; }
        public string tickSize { get; set; }
        public int tickSizeFraction { get; set; }
        public int lotFraction { get; set; }
        public string lotSize { get; set; }
        public int priceFraction { get; set; }
        public string obDepthMergeTimes { get; set; }
        public int indexSort { get; set; }
        public string baseInitialMarginRateE4 { get; set; }
        public string baseMaintenanceMarginRateE4 { get; set; }
        public string stepMaintenanceMarginRateE4 { get; set; }
        public string baseMaxOrdPzValue { get; set; }
        public string stepMaxOrdPzValue { get; set; }
        public string[] section { get; set; }
        public string symbolAlias { get; set; }
        public string contractStatus { get; set; }
        public string quarter { get; set; }
        public string stepInitialMarginRateE4 { get; set; }
        public string symbolTags { get; set; }
        public string startTradingTimeE3 { get; set; }
        public string settleTimeE3 { get; set; }
        public bool isPopular { get; set; }
        public string darkIcon { get; set; }
        public string lightIcon { get; set; }
    }

    public class Linearperpetual
    {
        public int symbol { get; set; }
        public string symbolName { get; set; }
        public string baseCurrency { get; set; }
        public string quoteCurrency { get; set; }
        public int quoteSymbol { get; set; }
        public int priceScale { get; set; }
        public string contractType { get; set; }
        public string maxPrice { get; set; }
        public string minPrice { get; set; }
        public string minQty { get; set; }
        public string maxNewOrderQty { get; set; }
        public int walletBalanceFraction { get; set; }
        public string upnlFraction { get; set; }
        public string tickSize { get; set; }
        public int tickSizeFraction { get; set; }
        public int lotFraction { get; set; }
        public string lotSize { get; set; }
        public int priceFraction { get; set; }
        public string obDepthMergeTimes { get; set; }
        public int indexSort { get; set; }
        public string baseInitialMarginRateE4 { get; set; }
        public string baseMaintenanceMarginRateE4 { get; set; }
        public string stepMaintenanceMarginRateE4 { get; set; }
        public string baseMaxOrdPzValue { get; set; }
        public string stepMaxOrdPzValue { get; set; }
        public string[] section { get; set; }
        public string symbolAlias { get; set; }
        public string contractStatus { get; set; }
        public string quarter { get; set; }
        public string stepInitialMarginRateE4 { get; set; }
        public string symbolTags { get; set; }
        public string startTradingTimeE3 { get; set; }
        public string settleTimeE3 { get; set; }
        public bool isPopular { get; set; }
        public string darkIcon { get; set; }
        public string lightIcon { get; set; }
    }

    public class Inversefuture
    {
        public int symbol { get; set; }
        public string symbolName { get; set; }
        public string baseCurrency { get; set; }
        public string quoteCurrency { get; set; }
        public int quoteSymbol { get; set; }
        public int priceScale { get; set; }
        public string contractType { get; set; }
        public string maxPrice { get; set; }
        public string minPrice { get; set; }
        public string minQty { get; set; }
        public string maxNewOrderQty { get; set; }
        public int walletBalanceFraction { get; set; }
        public string upnlFraction { get; set; }
        public string tickSize { get; set; }
        public int tickSizeFraction { get; set; }
        public int lotFraction { get; set; }
        public string lotSize { get; set; }
        public int priceFraction { get; set; }
        public string obDepthMergeTimes { get; set; }
        public int indexSort { get; set; }
        public string baseInitialMarginRateE4 { get; set; }
        public string baseMaintenanceMarginRateE4 { get; set; }
        public string stepMaintenanceMarginRateE4 { get; set; }
        public string baseMaxOrdPzValue { get; set; }
        public string stepMaxOrdPzValue { get; set; }
        public string[] section { get; set; }
        public string symbolAlias { get; set; }
        public string contractStatus { get; set; }
        public string quarter { get; set; }
        public string stepInitialMarginRateE4 { get; set; }
        public string symbolTags { get; set; }
        public string startTradingTimeE3 { get; set; }
        public string settleTimeE3 { get; set; }
        public bool isPopular { get; set; }
        public string darkIcon { get; set; }
        public string lightIcon { get; set; }
    }
}