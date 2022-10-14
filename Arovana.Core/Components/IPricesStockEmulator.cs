namespace Arovana.StockMarket.Core
{
    /// <summary>
    /// Эмулятор цен с биржи
    /// </summary>
    public interface IPricesStockEmulator
    {
        /// <summary>
        /// Валюты и цены
        /// </summary>
        public List<CurrencyPricePair> CurrencyPricePairs { get; }
    }
}