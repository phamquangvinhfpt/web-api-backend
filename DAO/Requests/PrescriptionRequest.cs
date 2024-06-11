using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class PrescriptionRequest
    {
        [Required]
        public string MedicineName { get; set; } // Tên thuốc
        [Required]
        public string Dosage { get; set; } // Liều lượng
        [Required]
        public string Instructions { get; set; } // Hướng dẫn sử dụng
    }
}
