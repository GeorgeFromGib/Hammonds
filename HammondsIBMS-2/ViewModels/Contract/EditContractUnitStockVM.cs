using FluentValidation;
using FluentValidation.Attributes;

namespace HammondsIBMS_2.ViewModels.Contract
{
    [Validator(typeof(EditCustomerAccountStockVMValidator))]
    public class EditCustomerAccountStockVM
    {
        public int CustomerAccountId { get; set; }
        public int StockId { get; set; }
        public int OldStockId { get; set; }
        public int? NewStockCustomerAccountId { get; set; }
        public int UnitId { get; set; }

        public int OldStockProductCycleId { get; set; }

        public static EditCustomerAccountStockVM CreateContractUnitStockVM(int customerAccountId,int stockId,int productLifeCycleId,int rentalUnitId=0)
        {
            return new EditCustomerAccountStockVM
                       {
                           CustomerAccountId=customerAccountId,
                           StockId = stockId,
                           OldStockId = stockId,
                           OldStockProductCycleId = productLifeCycleId,
                           UnitId = rentalUnitId
                       };
        }

        
    }

    public class EditCustomerAccountStockVMValidator : AbstractValidator<EditCustomerAccountStockVM>
    {
        public EditCustomerAccountStockVMValidator()
        {
            RuleFor(s => s.StockId).GreaterThan(0).WithMessage("Stock must be selected");
            RuleFor(s => s.NewStockCustomerAccountId).Must((s, m) => (m == null || m == s.CustomerAccountId)).
                WithMessage("Selected stock is already allotted to another Account");
            RuleFor(s => s.StockId).NotEqual(m => m.OldStockId).WithMessage("Same Stock selected");
        }
    }
}