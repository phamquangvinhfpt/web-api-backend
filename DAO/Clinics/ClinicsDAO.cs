using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Data;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DAO.Clinics
{
    public class ClinicsDAO
    {
        private static ClinicsDAO instance = null;
        private readonly AppDbContext _context = null;

        public ClinicsDAO()
        {
            _context = new AppDbContext();
        }

        public static ClinicsDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClinicsDAO();
                }
                return instance;
            }
        }
        public List<Clinic> GetAllClinics()
        {
            try
            {
                return _context.Clinics.Include("ClinicDetails").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving tours: {ex.Message}");
                throw;
            }
        }

        public Clinic GetClinicsById(Guid Id)
        {
            try
            {
                return _context.Clinics.Include("ClinicDetails").FirstOrDefault(c => c.Id == Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving tour with ID {Id}: {ex.Message}");
                throw;
            }
        }

        public void AddClinics(Clinic clinic)
        {
            try
            {
                _context.Clinics.Add(clinic);
                _context.SaveChanges(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding tour: {ex.Message}");
                throw;
            }
        }

        public void UpdateClinics(Clinic clinic)
        {
            try
            {
                var existingClinics = _context.Clinics.Find(clinic.Id);
                if (existingClinics != null)
                {
                    _context.Entry(existingClinics).CurrentValues.SetValues(clinic);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating tour: {ex.Message}");
                throw;
            }
        }

        public void DeleteClinics(Guid Id)
        {
            try
            {
                var clinics = _context.Clinics.FirstOrDefault(c => c.Id == Id);
                if (clinics != null)
                {
                    _context.Clinics.Remove(clinics);
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