using BusinessObject.Data;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DAO.Clinics
{
    public class ClinicsDAO
    {
        private readonly AppDbContext _context;

        public ClinicsDAO(AppDbContext context)
        {
            _context = context;
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
            var transaction = _context.Database.BeginTransaction();
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

        public void AddClinics(Clinic clinic, Guid userId)
        {
            try
            {
                var existingClinics = _context.Clinics.FirstOrDefault(c => c.Id == clinic.Id);
                if (existingClinics != null)
                {
                    throw new InvalidOperationException($"Clinic with ID {clinic.Id} already exists");
                }
                _context.Clinics.Add(clinic);
                _context.SaveChangesAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding tour: {ex.Message}");
                throw;
            }
        }

        public void UpdateClinics(Clinic clinic, Guid id)
        {
            try
            {
                var existingClinics = _context.Clinics.FirstOrDefault(c => c.Id == clinic.Id);
                if (existingClinics == null)
                {
                    throw new InvalidOperationException($"Clinic with ID {id} does not exist");
                }
                existingClinics.Name = clinic.Name;
                existingClinics.Address = clinic.Address;
                existingClinics.Verified = clinic.Verified;
                _context.Clinics.Update(existingClinics);
                _context.SaveChangesAsync(id);
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
                var clinic = _context.Clinics.FirstOrDefault(c => c.Id == Id);
                if (clinic == null)
                {
                    throw new InvalidOperationException($"Clinic with ID {Id} does not exist");
                }
                _context.Clinics.Remove(clinic);
                _context.SaveChangesAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting tour: {ex.Message}");
                throw;
            }
        }
    }
}