using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TradingViewWatchlistGenerator
{
    public static class Extensions
    {
        public static void Deconstruct(this string[] input, out string t1, out string t2)
        {
            t1 = input[0];
            t2 = input[1];
        }

        public static IEnumerable<ExchangePair> FilterLeveragedTokens(this IEnumerable<ExchangePair> pairs)
        {
            var suffixes = new[]
            {
                "3SUSDT", "3LUSDT",
                "UPUSDT", "DOWNUSDT",
                "BULLUSDT", "BEARUSDT",
                "1SUSDT", "1LUSDT"
            };

            return pairs.Where(pair =>
                !suffixes.Any(suffix =>
                    pair.GetCleanPairName().Contains(suffix)));
        }

        public static bool ContainsAny<T>(this IEnumerable<T> items, IEnumerable<T> targets)
        {
            return items.Any(targets.Contains);
        }
    }

    class OkexRoot
    {
        public OkexSimpleProduct[] data { get; set; }
    }
    class OkexSimpleProduct {
        public string coinName { get; set; }
        public string ctValCcy { get; set; }
        public string settleCcy { get; set; }
    }
    
    class Program
    {

        private static HttpClient _httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            var binanceUsdtFuturePairs = (await GetMarketPairs("https://sandwichfinance.blob.core.windows.net/files/binancefuturesf_usdt_perpetual_futures.txt")).ToHashSet();
            
            var binanceUsdtPairs = (await GetMarketPairs("https://sandwichfinance.blob.core.windows.net/files/binance_usdt_markets.txt")).ToHashSet();
            var binanceBtcPairs = (await GetMarketPairs("https://sandwichfinance.blob.core.windows.net/files/binance_btc_markets.txt")).ToHashSet();

            var kucoinUsdtPairs = (await GetMarketPairs("https://sandwichfinance.blob.core.windows.net/files/kucoin_usdt_markets.txt")).ToHashSet();
            var huobiUsdtPairs = (await GetMarketPairs("https://sandwichfinance.blob.core.windows.net/files/huobi_usdt_markets.txt")).ToHashSet();
            var bitfinexUsdtPairs = (await GetMarketPairs("https://sandwichfinance.blob.core.windows.net/files/bitfinex_usd_markets.txt")).ToHashSet();
            var poloniexUsdtPairs = (await GetMarketPairs("https://sandwichfinance.blob.core.windows.net/files/poloniex_usdt_markets.txt")).ToHashSet();
            var ftxUsdtPairs = (await GetMarketPairs("https://sandwichfinance.blob.core.windows.net/files/ftx_spot_markets.txt")).Where(x => x.IsUSD()).ToHashSet();

            var okexUsdtFuturesString = await _httpClient.GetStringAsync(
                    "https://www.okx.com/priapi/v5/public/simpleProduct?t=1643581526479&instType=SWAP");
            var okexUsdtFutures = JsonSerializer.Deserialize<OkexRoot>(okexUsdtFuturesString)
                .data
                .Where(x => x.settleCcy == "USDT")
                .Select(x => new ExchangePair()
                {
                    Exchange = "OKEX",
                    Pair = x.ctValCcy+x.settleCcy
                });


            var allFuturesPairs = binanceUsdtFuturePairs
                .Concat(okexUsdtFutures)
                .OrderByDescending(x => x.GetCleanPairName());

            var allUsdtPairs = binanceUsdtPairs
                .Concat(kucoinUsdtPairs)
                .Concat(huobiUsdtPairs)
                .Concat(bitfinexUsdtPairs)
                .Concat(poloniexUsdtPairs)
                .Concat(ftxUsdtPairs)
                .FilterLeveragedTokens()
                .OrderBy(x => x.GetCleanPairName());

            var futuresGroups = allFuturesPairs.GroupBy(x => x.GetCleanPairName());
            var spotGroups = allUsdtPairs.GroupBy(x => x.GetCleanPairName());

            var binanceUsdtSpotOnly = binanceUsdtPairs
                .FilterLeveragedTokens()
                .Where(x => !binanceUsdtFuturePairs.Select(y => y.GetCleanPairName()).Contains(x.GetCleanPairName()));

            var binanceBtcSpotOnly = binanceBtcPairs
                    .Where(x => !binanceUsdtPairs.Select(y => y.GetQuoteCurrency()).Contains(x.GetQuoteCurrency()))
                    .ToList();
            
            //

            var binanceFuturesPairs = new List<ExchangePair>();
            var okexFuturesPairs = new List<ExchangePair>();
            
            foreach (var pair in futuresGroups)
            {
                var exchanges = pair.Select(x => x.Exchange);

                if (exchanges.Contains("BINANCE")) binanceFuturesPairs.Add(pair.First(x => x.Exchange.Contains("BINANCE")));
                else if (exchanges.Contains("OKEX")) okexFuturesPairs.Add(pair.First(x => x.Exchange.Contains("OKEX"))); 
            }
            
            // 

            var binancePairs = new List<ExchangePair>();
            var kucoinPairs = new List<ExchangePair>();
            var huobiPairs = new List<ExchangePair>();
            var bitfinexPairs = new List<ExchangePair>();
            var poloniexPairs = new List<ExchangePair>();
            var ftxPairs = new List<ExchangePair>();

            foreach (var pair in spotGroups)
            {
                var exchanges = pair.Select(x => x.Exchange);

                if (exchanges.Contains("BINANCE")) binancePairs.Add(pair.First(x => x.Exchange.Contains("BINANCE")));
                else if (exchanges.Contains("FTX")) ftxPairs.Add(pair.First(x => x.Exchange.Contains("FTX")));
                else if (exchanges.Contains("KUCOIN")) kucoinPairs.Add(pair.First(x => x.Exchange.Contains("KUCOIN")));
                else if (exchanges.Contains("HUOBI")) huobiPairs.Add(pair.First(x => x.Exchange.Contains("HUOBI")));
                else if (exchanges.Contains("BITFINEX")) bitfinexPairs.Add(pair.First(x => x.Exchange.Contains("BITFINEX")));
                else poloniexPairs.Add(pair.First());
            }

            await File.WriteAllLinesAsync($"{nameof(binanceUsdtFuturePairs)}.txt", binanceUsdtFuturePairs.Select(x => x.ToString()));
            await File.WriteAllLinesAsync($"{nameof(okexFuturesPairs)}.txt", okexFuturesPairs.Select(x => x.ToString()));
            
            await File.WriteAllLinesAsync($"{nameof(binanceBtcSpotOnly)}.txt", binanceBtcSpotOnly.Select(x => x.ToString()));

            await File.WriteAllLinesAsync($"{nameof(binancePairs)}.txt", binancePairs.Select(x => x.ToString()));
            await File.WriteAllLinesAsync($"{nameof(ftxPairs)}.txt", ftxPairs.Select(x => x.ToString()));
            await File.WriteAllLinesAsync($"{nameof(kucoinPairs)}.txt", kucoinPairs.Select(x => x.ToString()));
            await File.WriteAllLinesAsync($"{nameof(huobiPairs)}.txt", huobiPairs.Select(x => x.ToString()));
            await File.WriteAllLinesAsync($"{nameof(bitfinexPairs)}.txt", bitfinexPairs.Select(x => x.ToString()));
            await File.WriteAllLinesAsync($"{nameof(poloniexPairs)}.txt", poloniexPairs.Select(x => x.ToString()));
            await File.WriteAllLinesAsync($"{nameof(binanceUsdtSpotOnly)}.txt", binanceUsdtSpotOnly.Select(x => x.ToString()));


        }

        private static async Task<IEnumerable<ExchangePair>> GetMarketPairs(string marketUrl)
        {
            var rawText = await _httpClient.GetStringAsync(marketUrl);

            var pairs = rawText
                .Split("\n")
                .Select(x =>
                {
                    var (exchange, pair) = x.Split(":");
                    return new ExchangePair()
                    {
                        Exchange = exchange,
                        Pair = pair
                    };
                });

            return pairs;
        }
    }

    public class ExchangePair
    {
        public string Exchange { get; set; }
        public string Pair { get; set; }

        public string GetCleanPairName()
        {
            var pair = Pair.Replace("USDTPERP", "USDT");

            if (pair.EndsWith("USD"))
                pair += "T"; //usdt

            return pair;
        }

        public bool IsUSD()
        {
            return new[] { "USDTPERP", "USDT", "USD" }
                .Any(x => Pair.EndsWith(x));
        }

        public bool IsBTC()
        {
            return Pair.EndsWith("BTC", StringComparison.InvariantCultureIgnoreCase);
        }

        public string GetQuoteCurrency()
        {
            if (IsBTC())
                return Pair.Replace("BTC", "");

            if (IsUSD())
            {
                var index = Pair.IndexOf("USD");

                return Pair.Substring(0, index);
            }

            return Pair;
        }

        public override string ToString()
        {
            return $"{Exchange}:{Pair}";
        }
    }
}

