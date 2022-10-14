namespace Arovana.StockMarket.Core.Components
{
    /// <summary>
    /// <inheritdoc cref="ITradeStrategy"/>
    /// </summary>
    public class TradeStrategy : ITradeStrategy
    {
        private readonly IPricesStockEmulator _pricesStockEmulator;
        public TradeStrategy(IPricesStockEmulator pricesStockEmulator)
        {
            _pricesStockEmulator = pricesStockEmulator;

            //при каждом изменении курса валюты - выдаем сигнал покупка/продажа
            _pricesStockEmulator.CurrencyPricePairs.ForEach(p =>
            {
                //p.PropertyChanged += (s, e) =>
                //{
                //    var eventArgs = e as IncreasedPropertyChangedEventArgs;
                //    var currencyPrice = s as CurrencyPricePair;

                //    this.RaiseSignal(new TradeSignalEventArgs(currencyPrice, (TradeStrategySignalTypes)eventArgs.Increased));
                //};

                p.PropertyChanged += RaiseSignal;
            });
        }

        /// <summary>
        /// Посылает сигнал
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RaiseSignal(object sender, EventArgs e)
        {
            var eventArgs = e as IncreasedPropertyChangedEventArgs;
            var currencyPrice = sender as CurrencyPricePair;

            this.RaiseSignal(new TradeSignalEventArgs(currencyPrice, (TradeStrategySignalTypes)eventArgs.Increased));
        }

        //private void RaiseSignal(object sender, IncreasedPropertyChangedEventArgs eventArgs)
        //{
        //    var currencyPrice = sender as CurrencyPricePair;

        //    this.RaiseSignal(new TradeSignalEventArgs(currencyPrice, (TradeStrategySignalTypes)eventArgs.Increased));
        //}

        /// <summary>
        /// Послыает сигнал
        /// </summary>
        /// <param name="e"></param>
        private void RaiseSignal(TradeSignalEventArgs e)
        {
            if(RaisedSignal != null)
            {
                RaisedSignal(this, e);
            }
        }

        /// <summary>
        /// <inheritdoc cref="ITradeStrategy.RaisedSignal"/>
        /// </summary>
        public event EventHandler RaisedSignal;
    }

    /// <summary>
    /// Тип сигнала
    /// </summary>
    public enum TradeStrategySignalTypes
    {
        /// <summary>
        /// Покупка
        /// </summary>
        Buy = 1,

        /// <summary>
        /// Держать позицию
        /// </summary>
        KeepPosition = 0,

        /// <summary>
        /// Продажа
        /// </summary>
        Sell = -1
    }

    public class TradeSignalEventArgs : EventArgs 
    {        
        public TradeSignalEventArgs(CurrencyPricePair currencyPricePair, TradeStrategySignalTypes signalType)
        {
            CurrencyPricePair = currencyPricePair;
            SignalType = signalType;
        }

        /// <summary>
        /// Валюта
        /// </summary>
        public CurrencyPricePair CurrencyPricePair { get; }

        /// <summary>
        /// Тип сигнала
        /// </summary>
        public TradeStrategySignalTypes SignalType { get; }
    }
}
