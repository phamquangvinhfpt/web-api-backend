﻿using DAO.Requests;
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
        public PrescriptionService() {
            repository = new PrescriptionRepository();
        }
        public void CreatePrescription(List<PrescriptionRequest> request, Guid dentalReID)
        {
            repository.CreatePrescription(request, dentalReID);
        }
    }
}
