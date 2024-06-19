using DAO.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Prescriptions
{
    public interface IPrescriptionService
    {
        public void CreatePrescription(List<PrescriptionRequest> request, Guid dentalReID);
    }
}
