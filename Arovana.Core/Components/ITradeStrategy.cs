namespace Arovana.StockMarket.Core.Components
{
    /// <summary>
    /// Торговая стратегия
    /// </summary>
    public interface ITradeStrategy
    {
        /// <summary>
        /// Послать сигнал
        /// </summary>
        event EventHandler RaisedSignal;
    }
}
