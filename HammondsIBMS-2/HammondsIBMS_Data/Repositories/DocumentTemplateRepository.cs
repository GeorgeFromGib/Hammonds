using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.DocumentTemplates;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IDocumentTemplateRepository : IRepository<DocumentTemplate>
    {
    }

    public class DocumentTemplateRepository : RepositoryBase<DocumentTemplate>, IDocumentTemplateRepository
    {
        public DocumentTemplateRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
