using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class PrescriptionRequest
    {
        public string MedicineName { get; set; } // Tên thuốc
        public string Dosage { get; set; } // Liều lượng
        public string Instructions { get; set; } // Hướng dẫn sử dụng
    }
}
