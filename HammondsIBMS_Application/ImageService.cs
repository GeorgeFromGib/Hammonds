using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Documents;
using HammondsIBMS_Domain.Repositories;


namespace HammondsIBMS_Application
{
    public interface IDocumentService
    {
        Document GetDocument(int id);
        void DeleteDocument(int id);
    }

    public class DocumentService:ServiceBase, IDocumentService
    {
        private readonly IDocumentRepository _imageRepository;

        public DocumentService(IDocumentRepository imageRepository,IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _imageRepository = imageRepository;
        }

        public Document GetDocument(int id)
        {
            return _imageRepository.GetById(id);
        }

        public void DeleteDocument(int id)
        {
            _imageRepository.Delete(i=>i.DocumentId==id);
        }
    }
}
