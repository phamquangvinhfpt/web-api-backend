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
        private readonly AppDbContext _context = null;

        public ClinicsDetailsDAO(AppDbContext context)
        {
            _context = context;
        }

        public List<ClinicDetail> GetAllClinicDetails()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                return _context.ClinicDetails.ToList();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error retrieving tours: {ex.Message}");
                throw;
            }
        }

        public ClinicDetail GetClinicDetailById(Guid Id)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                return _context.ClinicDetails.FirstOrDefault(c => c.Id == Id);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error retrieving tour with ID {Id}: {ex.Message}");
                throw;
            }
        }

        public async void AddClinicsDetails(ClinicDetail ClinicsDetails)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var existingClinics = _context.ClinicDetails.FirstOrDefault(c => c.Id == ClinicsDetails.Id);
                if (existingClinics != null)
                {
                    throw new InvalidOperationException($"Clinic with ID {ClinicsDetails.Id} already exists");
                }
                _context.ClinicDetails.Add(ClinicsDetails);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding tour: {ex.Message}");
                throw;
            }
        }

        public async void UpdateClinicsDetails(ClinicDetail ClinicDetail)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var existingClinics = _context.ClinicDetails.FirstOrDefault(c => c.Id == ClinicDetail.Id);
                if (existingClinics == null)
                {
                    throw new InvalidOperationException($"Clinic with ID {ClinicDetail.Id} does not exist");
                }
                _context.ClinicDetails.Update(ClinicDetail);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating tour: {ex.Message}");
                throw;
            }
        }

        public async void DeleteClinicsDetails(Guid Id)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var existingClinics = _context.ClinicDetails.FirstOrDefault(c => c.Id == Id);
                if (existingClinics == null)
                {
                    throw new InvalidOperationException($"Clinic with ID {Id} does not exist");
                }
                _context.ClinicDetails.Remove(existingClinics);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting tour: {ex.Message}");
                throw;
            }
        }
    }
}