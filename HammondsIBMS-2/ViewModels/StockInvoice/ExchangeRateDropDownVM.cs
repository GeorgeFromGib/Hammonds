namespace HammondsIBMS_2.ViewModels.StockInvoice
{
    public class ExchangeRateDropDownVM
    {
        public int ExchangeRateId { get; set; }
        public string Symbol { get; set; }
        public string Abbreviation { get; set; }
        public string AbbreviationSymbolComb
        {
            get { return Abbreviation + " (" + Symbol + ")"; }
        }
        public string CurrencyName { get; set; }
        public decimal RateToGBP { get; set; }
    }
}