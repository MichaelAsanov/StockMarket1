namespace Arovana.StockMarket.Core.Components
{
    /// <summary>
    /// <inheritdoc cref="IStockMarketEmulator"/>
    /// </summary>
    public class StockMarketEmulator : IStockMarketEmulator
    {
        /// <summary>
        /// <inheritdoc cref="IStockMarketEmulator.Balance"/>
        /// </summary>
        public decimal Balance { get; set; }
        private readonly IPricesStockEmulator _pricesStockEmulator;
        private readonly ITradeStrategy _tradeStrategy;

        public StockMarketEmulator(IPricesStockEmulator pricesStockEmulator, ITradeStrategy tradeStrategy, decimal balance)
        {
            _pricesStockEmulator = pricesStockEmulator;
            _tradeStrategy = tradeStrategy;
            Balance = balance;

            //принимаем сигналы от стратегии
            //_tradeStrategy.RaisedSignal += (s, e) =>
            //{
            //    var args = e as TradeSignalEventArgs;

            //    var signalType = args.SignalType;
            //    var currencyName = args.CurrencyPricePair.CurrencyName;
            //    var amount = args.CurrencyPricePair.Price;

            //    if (signalType == TradeStrategySignalTypes.Buy)
            //        Buy(currencyName, amount);

            //    else if(signalType == TradeStrategySignalTypes.Sell)
            //        Sell(currencyName, amount);
            //};

            _tradeStrategy.RaisedSignal += RaisedSignal;
        }

        /// <summary>
        /// Принимает сигнал от стратегии и покупает/продает
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RaisedSignal(object sender, EventArgs e)
        {
            var args = e as TradeSignalEventArgs;

            var signalType = args.SignalType;
            var currencyName = args.CurrencyPricePair.CurrencyName;
            var amount = args.CurrencyPricePair.Price;

            if (signalType == TradeStrategySignalTypes.Buy)
                Buy(currencyName, amount);

            else if (signalType == TradeStrategySignalTypes.Sell)
                Sell(currencyName, amount);
        }

        //private void RaisedSignal(object sender, TradeSignalEventArgs args)
        //{
        //    var signalType = args.SignalType;
        //    var currencyName = args.CurrencyPricePair.CurrencyName;
        //    var amount = args.CurrencyPricePair.Price;

        //    if (signalType == TradeStrategySignalTypes.Buy)
        //        Buy(currencyName, amount);

        //    else if (signalType == TradeStrategySignalTypes.Sell)
        //        Sell(currencyName, amount);
        //}

        private readonly object _locker = new object();

        /// <summary>
        /// <inheritdoc cref="IStockMarketEmulator.Buy(string, decimal)"/>
        /// </summary>
        /// <param name="currencyName"></param>
        /// <param name="amount"></param>
        public void Buy(string currencyName, decimal amount)
        {
            lock (_locker)
            {
                if (Balance >= amount)
                {
                    Balance -= amount;
                    Console.WriteLine($"{currencyName} куплена");
                    Console.WriteLine($"Баланс: {Balance}");
                }
                else
                {
                    Console.WriteLine($"Недостаточно средств на покупку валюты {currencyName}");
                    Console.WriteLine($"Баланс: {Balance}");
                }
            }
        }

        /// <summary>
        /// <inheritdoc cref="Sell(string, decimal)"/>
        /// </summary>
        /// <param name="currencyName"></param>
        /// <param name="amount"></param>
        public void Sell(string currencyName, decimal amount)
        {
            lock (_locker)
            {
                Balance += amount;
                Console.WriteLine($"{currencyName} продана");
                Console.WriteLine($"Баланс: {Balance}");
            }
        }
    }
}
