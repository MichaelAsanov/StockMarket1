namespace Arovana.StockMarket.Core
{
    /// <summary>
    /// <inheritdoc cref="IPricesStockEmulator"/>
    /// </summary>
    public class PricesStockEmulator : IPricesStockEmulator
    {
        private readonly List<CurrencyPricePair> _currencyPricePairs;

        /// <summary>
        /// <inheritdoc cref="IPricesStockEmulator.CurrencyPricePairs"/>
        /// </summary>
        public List<CurrencyPricePair> CurrencyPricePairs => _currencyPricePairs;
        public PricesStockEmulator()
        {
            _currencyPricePairs = new List<CurrencyPricePair>()
            {
                new CurrencyPricePair() { CurrencyName = "USD", Price = 60.0M },
                new CurrencyPricePair() { CurrencyName = "EUR", Price = 70.0M },
                new CurrencyPricePair() { CurrencyName = "JPY", Price = 0.4297M },
            };            

            //случайно генерируем новую цену каждую секунду
            var timer = new Timer(GenerateNewPrices, "", 5000, 1000) { };
            
            InitVals();
        }

        /// <summary>
        /// Для каждой валюты - сколько-то секунд цена растет, затем падает или наоборот
        /// </summary>
        class Values
        {
            /// <summary>
            /// Знак, куда двжидется курс валюты (1 - +, -1 - -) (генерирется случайно)
            /// </summary>
            public int Sign { get; set; }

            /// <summary>
            /// Период в секундах, втечение которого курс валюты движется в одну сторону (генерируется случйно)
            /// </summary>
            public int Times { get; set; }

            /// <summary>
            /// момент (в секундах) (Для каждой валюты свой). При достижении до Times, обнуляется, затем Times случйным образом перегенерируется
            /// </summary>
            public int I { get; set; }
        }
        private List<Values> _values;

        /// <summary>
        /// Генерирует знак изменения валюты (1 - вверх, -1 - вниз)
        /// </summary>
        /// <returns></returns>
        private int GenerateSign()
        {
            var random = new Random();
            return random.Next(2) > 0 ? 1 : -1;
        }

        /// <summary>
        /// Генерирует длину периода (в секундах, втечение которого валюта двжиется в одну сторону)
        /// </summary>
        /// <returns></returns>
        private int GenerateTimes()
        {
            var random = new Random();
            return random.Next(10);
        }

        private void InitVals(int? i = null)
        {
            var random = new Random();

            if (i.HasValue)
            {
                _values[i.Value].I = 0;
                _values[i.Value].Sign = GenerateSign();
                _values[i.Value].Times = GenerateTimes();
            }
            else
            {
                _values = _currencyPricePairs.Select(x => new Values { I = 0, Sign = GenerateSign(), Times = GenerateTimes() }).ToList();
            }
        }

        private static readonly object _locker = new object();

        /// <summary>
        /// Генерирует новые цены. Выполняется каждую секунду.
        /// </summary>
        /// <param name="state"></param>
        private void GenerateNewPrices(object state)
        {
            lock (_locker)
            {
                var random = new Random();

                int i = 0;
                _currencyPricePairs.ForEach(x =>
                {
                    if (_values[i].I == _values[i].Times)
                        InitVals(i);

                    var delta = (decimal)random.NextDouble();
                    
                    if (_values[i].Sign < 0 && x.Price - delta >= 0) delta *= -1;

                    x.Price += delta;

                    ++_values[i].I;
                    ++i;
                });
                
                if (CurrienciesRefreshed != null)
                {
                    CurrienciesRefreshed(this, EventArgs.Empty);
                }
            }            
        }

        public event EventHandler CurrienciesRefreshed;
    }
}
