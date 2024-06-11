using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Requests
{
    public class MedicalRecordRequest
    {
        [Required]
        public string Symptoms { get; set; } // Triệu chứng
        [Required]
        public string Diagnosis { get; set; } // Chuẩn đoán
        [Required]
        public string Treatment { get; set; } // Điều trị (VD: Răng số 1 bị sâu, trám răng số 1)
    }
}
