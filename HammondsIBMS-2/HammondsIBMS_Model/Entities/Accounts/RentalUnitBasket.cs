using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_Domain.Entities.Accounts
{
    public abstract class Basket
    {
        public int BasketId { get; set; }
        public int CustomerAccountId  { get; set; }
        public virtual CustomerAccount CustomerAccount { get; set; }
        public virtual List<OneOffItem> OneOffItems { get; set; }
        public DateTime StartDate { get; set; }
        public bool FullPayment { get; set; }
        public abstract  decimal TotalAmount { get;}



        public decimal TotalOneOffItems
        {
            get
            {
                return (OneOffItems == null ? 0 : OneOffItems.Sum(c => c.TotalCharge));
            }
        }

        protected Basket()
        {
            StartDate = DateTime.Today;
        }
    }

    public class RentalBasket : Basket
    {
        public virtual List<RentedUnit> RentedUnits { get; set; }

        public string ModelDescription
        {
            get
            {
                if (RentedUnits != null)
                {
                    if (RentedUnits.Count(c => c.RemovalDate == null) > 1)
                        return "[Multi Units]";
                    else
                        return RentedUnits.Count(c => c.RemovalDate == null) > 0
                            ? RentedUnits.Single(c => c.RemovalDate == null).Stock.ManufacturerModel
                            : "";
                }
                return "";
            }
        }

        public RentalBasket()
        {
            RentedUnits = new List<RentedUnit>();
            OneOffItems = new List<OneOffItem>();
        }


        public override decimal TotalAmount
        {
            get
            {
                return TotalOneOffItems + RentedUnits.Sum(c => c.Amount + c.Deposit);
            }
        }
    }

    public class PurchaseBasket : Basket
    {
        public virtual List<PurchaseUnit> PurchasedUnits { get; set; }

        public  string ModelDescription
        {
            get
            {
                if (PurchasedUnits != null)
                {
                    if (PurchasedUnits.Count(c => c.RemovalDate == null) > 1)
                        return "[Multi Units]";
                    else
                        return PurchasedUnits.Count(c => c.RemovalDate == null) > 0 ? PurchasedUnits.Single(c => c.RemovalDate == null).Stock.ManufacturerModel : "";
                }
                return "";
            }
        }

        public override decimal TotalAmount
        {
            get { return TotalOneOffItems + PurchasedUnits.Sum(c => c.Total)+TotalContracts; }
        }

        public decimal TotalContracts
        {
            get
            {
                return PurchasedUnits == null ? 0 : PurchasedUnits.Sum(c => c.TotalContracts);
            }
        }
    }
}
