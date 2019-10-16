using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Annotations;
using HammondsIBMS_Domain.BaseInterfaces;
using HammondsIBMS_Domain.Entities.Accounts;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    public abstract class Unit
    {
        [Key]
        public int UnitId { get; set; }
        public int CustomerAccountId { get; set; }
        public virtual CustomerAccount CustomerAccount { get; set; }
        public int StockId { get; set; }
        public virtual Stock.Stock Stock { get; set; }
        public DateTime? RemovalDate { get; set; }

        [DisplayName("Change Date")]
        [DataType(DataType.Date)]
        public DateTime? ChangedDate { get; set; }

        public string Notes { get; set; }

    }
}