using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HammondsIBMS_Application.DocumentParser;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.DocumentTemplates;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public class DocumentTemplateService : ServiceBase
    {
        private readonly IDocumentTemplateRepository _documentTemplateRepository;

        public DocumentTemplateService(IUnitOfWork unitOfWork, IDocumentTemplateRepository documentTemplateRepository)
            : base(unitOfWork)
        {
            _documentTemplateRepository = documentTemplateRepository;
        }

        public IEnumerable<DocumentTemplate> GetAllDocumentTemplates()
        {
            return _documentTemplateRepository.GetAll();
        }

        public DocumentTemplate GetById(int id)
        {
            return _documentTemplateRepository.GetById(id);
        }

        public void Update(DocumentTemplate docTemplte)
        {
            ValidateDocumentTemplate(docTemplte);
            _documentTemplateRepository.Update(docTemplte);
        }

        private static void ValidateDocumentTemplate(DocumentTemplate documentTemplate)
        {
            var fieldsInBody = Regex.Matches(documentTemplate.Body + " ", @"\$(\S+)\$");

            foreach (var bodyField in fieldsInBody)
            {
                var bodyFieldName = bodyField.ToString();
                if (documentTemplate.GetAllowedFieldsWithDelimeters().All(p => p != bodyFieldName))
                    throw new BusinessRuleException("Body", string.Format(
                        "The field {0} is not in the allowable list of fields for this template",
                        bodyFieldName));
            }
        }

        public DocumentTemplateParser<PurchaseContractDocumentTemplate, PurchaseAccount>
            CreatePurchaseContractDocumentTemplate()
        {
            return
                new DocumentTemplateParser<PurchaseContractDocumentTemplate, PurchaseAccount>(
                    (PurchaseContractDocumentTemplate)_documentTemplateRepository.GetById((int)DocumentTemplateTypes.Sale));
        }

        public DocumentTemplateParser<RentContractDocumentTemplate, RentalAccount> CreateRentContractDocumentTemplate()
        {
            return
                new DocumentTemplateParser<RentContractDocumentTemplate, RentalAccount>(
                    (RentContractDocumentTemplate)_documentTemplateRepository.GetById((int)DocumentTemplateTypes.Rent));
        }
    }

}
