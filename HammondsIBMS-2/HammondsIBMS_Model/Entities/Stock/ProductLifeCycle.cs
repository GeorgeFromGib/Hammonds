using System;
using System.Collections.Generic;

namespace HammondsIBMS_Domain.Model.Stock
{
    public class ProductLifeCycle
    {
        public int ProductLifeCycleId { get; set; }
        public string Description { get; set; }
        public string DescriptionAbbreviation { get; set; }
        public bool ContractChangeable { get; set; }
        public ProductLifeCycleActions ProductLifeCycleActions { get; set; }
        public bool NotAttachedStatus { get; set; }
  
    }

    public enum ProductLifeCycleStatus
    {
        InStock = 1,
        Sold = 2,
        Rented = 3,
        ReRental = 4,
        Scrapped = 5,
        Reserved=6,
    }

    [Flags]
    public enum ProductLifeCycleActions
    {
        NoAction=0,
        CanBeSold=1,
        CanBeRented=2,
    }
}