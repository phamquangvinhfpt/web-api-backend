using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models.Clinics
{
    public class ClinicsModel
    {
        public Guid OwnerID { get; set; }
        public required string Name { get; set; }
        public string? Address { get; set; }
    }
}