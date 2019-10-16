using System.Collections.Generic;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public class EmployerService:ServiceBase
    {
        private readonly IEmployerRepository _employerRepository;

        public EmployerService(IUnitOfWork unitOfWork,IEmployerRepository employerRepository) : base(unitOfWork)
        {
            _employerRepository = employerRepository;
        }

        public IEnumerable<Employer> GetAllEmployers()
        {
            return _employerRepository.GetAll();
        }

        public Employer GetEmployerById(int id)
        {
            return _employerRepository.GetById(id);
        }

        public void AddEmployer(Employer employer)
        {
            _employerRepository.Add(employer);
        }

        public void UpdateEmployer(Employer employer)
        {
            _employerRepository.Update(employer);
        }
    }
}