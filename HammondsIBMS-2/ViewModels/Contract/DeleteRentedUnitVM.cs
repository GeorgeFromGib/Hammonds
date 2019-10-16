using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Contract
{
    public class DeleteRentedUnitVM
    {
        public int UnitId { get; set; }

        [DisplayName("Rented Date")]
        [DataType(DataType.Date)]
        public DateTime RentedDate { get; set; }

        [DisplayName("Removed Date")]
        [DataType(DataType.Date)]
        public DateTime RemovalDate { get; set; }

        [DisplayName("Paid Until")]
        [DataType(DataType.Date)]
        public DateTime? PaidUntil { get; set; }

        [DisplayName("Stock")]
        public string StockDescription { get; set; }

        [DisplayName("Deposit")]
        [DataType(DataType.Currency)]
        public decimal Deposit { get; set; }

        [DisplayName("Return Deposit?")]
        public bool ReturnDeposit { get; set; }

        [DisplayName("Recovered Stock Status")]
        public int ProductCycleLifeId { get; set; }

        [DisplayName("Removal Notes")]
        public string RemovalNotes { get; set; }

        //public static DeleteRentedUnitVM CreateDeleteRentedUnitVM(int rentedUnitId,DateTime removalDate,int productLifeCycle,string stockDescription)
        //{
        //    return new DeleteRentedUnitVM
        //               {
        //                   RentedUnitId = rentedUnitId,
        //                   RemovalDate = removalDate,
        //                   ProductCycleLifeId = productLifeCycle,
        //                   StockDescription = stockDescription
        //               };
        //}
    }
}