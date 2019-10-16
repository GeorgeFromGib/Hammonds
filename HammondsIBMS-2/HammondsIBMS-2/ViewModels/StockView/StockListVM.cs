namespace HammondsIBMS_2.ViewModels.StockView
{
    public class StockVM
    {
        public int StockId { get; set; }
        public string ManufacturerName { get; set; }
        public int ModelId { get; set; }
        public string ModelCode { get; set; }
        public string SerialNumber { get; set; }
        public string Identifier { get; set; }
        public decimal LandedCost { get; set; }
        public string ProductLifeCycle { get; set; }
        public string InvoiceStatus { get; set; }
    }
}