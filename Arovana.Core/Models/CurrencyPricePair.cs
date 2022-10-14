using System.ComponentModel;

namespace Arovana.StockMarket.Core
{
    public class CurrencyPricePair : INotifyPropertyChanged
    {
        public string CurrencyName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal _price;
        public decimal Price 
        { 
            get { return _price; }
            set
            {
                var increased = (IncreasedEnum)value.CompareTo(_price);
                _price = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new IncreasedPropertyChangedEventArgs(nameof(Price), increased, value));
                }
            }
        }
    }

    public class IncreasedPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public IncreasedPropertyChangedEventArgs(string propertyName, IncreasedEnum increased, decimal price) : base(propertyName)
        {
            Increased = increased;
            Price = price;
        }

        public IncreasedEnum Increased { get; }

        public decimal Price { get; }
    }

    public enum IncreasedEnum
    {
        Increased = 1, 
        NotChanged = 0, 
        Decreased = -1, 
    }
}
