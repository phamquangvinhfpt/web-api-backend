using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class AddPrescription
    {
        public Guid dentalId { get; set; }
        public List<PrescriptionRequest> Prescriptions { get; set; }
    }
}
