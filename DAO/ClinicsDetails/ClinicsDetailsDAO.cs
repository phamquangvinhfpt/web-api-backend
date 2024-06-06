using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Data;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DAO.ClinicsDetails
{
    public class ClinicsDetailsDAO
    {
        private static ClinicsDetailsDAO instance = null;
        private readonly AppDbContext _context = null;

        public ClinicsDetailsDAO()
        {
            _context = new AppDbContext();
        }

        public static ClinicsDetailsDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClinicsDetailsDAO();
                }
                return instance;
            }
        }
        public List<ClinicDetail> GetAllClinicDetails()
        {
            try
            {
                return _context.ClinicDetails.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving tours: {ex.Message}");
                throw;
            }
        }

        public ClinicDetail GetClinicDetailById(Guid Id)
        {
            try
            {
                return _context.ClinicDetails.FirstOrDefault(c => c.Id == Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving tour with ID {Id}: {ex.Message}");
                throw;
            }
        }

        public void AddClinicsDetails(ClinicDetail ClinicsDetails)
        {
            try
            {
                _context.ClinicDetails.Add(ClinicsDetails);
                _context.SaveChanges(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding tour: {ex.Message}");
                throw;
            }
        }

        public void UpdateClinicsDetails(ClinicDetail ClinicDetail)
        {
            try
            {
                var existingClinics = _context.ClinicDetails.Find(ClinicDetail.Id);
                if (existingClinics != null)
                {
                    _context.Entry(existingClinics).CurrentValues.SetValues(ClinicDetail);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating tour: {ex.Message}");
                throw;
            }
        }

        public void DeleteClinicsDetails(Guid Id)
        {
            try
            {
                var clinicsdetails = _context.ClinicDetails.FirstOrDefault(c => c.Id == Id);
                if (clinicsdetails != null)
                {
                    _context.ClinicDetails.Remove(clinicsdetails);
                    _context.SaveChanges(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting tour with ID {Id}: {ex.Message}");
                throw;
            }
        }
    }
}