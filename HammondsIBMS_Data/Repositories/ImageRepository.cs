using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Documents;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IDocumentRepository:IRepository<Document>
    {
    }

    public class DocumentRepository:RepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
