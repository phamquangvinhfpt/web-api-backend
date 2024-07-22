using DAO.Requests;
using Repository.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Prescriptions
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IPrescriptionRepository repository;
        public PrescriptionService(IPrescriptionRepository repository)
        {
            this.repository = repository;
        }
        public void CreatePrescription(List<PrescriptionRequest> request, Guid dentalReID, Guid userID)
        {
            repository.CreatePrescription(request, dentalReID, userID);
        }
    }
}
