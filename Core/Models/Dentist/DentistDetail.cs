namespace Core.ManageDentist.Models
{
    public class DentistDetail
    {
        public Guid DentistId { get; set; }
        public Guid ClinicId { get; set; }
        public string Degree { get; set; } // Bằng cấp (VD: Bác sĩ nha khoa, Thạc sĩ nha khoa, Tiến sĩ nha khoa)
        public string Institute { get; set; } // Trường đào tạo (VD: Đại học Y Hà Nội)
        public int YearOfExperience { get; set; } // Số năm kinh nghiệm (VD: 5 năm)
        public string Specialization { get; set; } // Chuyên ngành (VD: Nha khoa trẻ em)
    }
}
