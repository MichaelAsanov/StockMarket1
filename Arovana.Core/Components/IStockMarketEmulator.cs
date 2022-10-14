namespace Arovana.StockMarket.Core.Components
{
    /// <summary>
    /// Эмулятор биржи
    /// </summary>
    public interface IStockMarketEmulator
    {
        /// <summary>
        /// Текущий баланс
        /// </summary>
        decimal Balance { get; set; }

        /// <summary>
        /// Покупка
        /// </summary>
        /// <param name="currencyName"></param>
        /// <param name="amount"></param>
        void Buy(string currencyName, decimal amount);

        /// <summary>
        /// Продажа
        /// </summary>
        /// <param name="currencyName"></param>
        /// <param name="amount"></param>
        void Sell(string currencyName, decimal amount);
    }
}
