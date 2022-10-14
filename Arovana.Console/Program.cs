// See https://aka.ms/new-console-template for more information
using Arovana.StockMarket.Core;
using Arovana.StockMarket.Core.Components;

PricesStockEmulator pricesStockEmulator = new PricesStockEmulator();

//while (true)
//{
//    pricesStockEmulator.CurrencyPricePairs.ForEach(price => Console.WriteLine($"{price.CurrencyName} {price.Price}") );
//    Console.ReadKey();
//}

//pricesStockEmulator.CurrienciesRefreshed += (s, e) =>
//{
//    Console.Clear();
//    pricesStockEmulator.CurrencyPricePairs.ForEach(price => Console.WriteLine($"{price.CurrencyName} {price.Price}"));    
//};

var traderStrategy = new TradeStrategy(pricesStockEmulator);

var stockMarketEmulator = new StockMarketEmulator(pricesStockEmulator, traderStrategy, 100);

Console.ReadKey();