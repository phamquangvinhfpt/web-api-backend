using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.Data;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
            } finally
            {
                transaction.Dispose();
            }
        }

        public async void AddClinics(Clinic clinic, Guid userId)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var existingClinics = _context.Clinics.FirstOrDefault(c => c.Id == clinic.Id);
                if (existingClinics != null)
                {
                    throw new InvalidOperationException($"Clinic with ID {clinic.Id} already exists");
                }
                _context.Clinics.Add(clinic);
                await _context.SaveChangesAsync(userId);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error adding tour: {ex.Message}");
                throw;
            } finally
            {
                transaction.Dispose();
            }
        }

        public async void UpdateClinics(Clinic clinic, Guid id)
        {
            var transaction = _context.Database.BeginTransaction();
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
                _context.Entry(existingClinics).State = EntityState.Detached;
                _context.Attach(clinic);
                _context.Entry(clinic).State = EntityState.Modified;
                await _context.SaveChangesAsync(id);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error updating tour: {ex.Message}");
                throw;
            } finally
            {
                transaction.Dispose();
            }
        }

        public async void DeleteClinics(Guid Id)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var clinic = _context.Clinics.FirstOrDefault(c => c.Id == Id);
                if (clinic == null)
                {
                    throw new InvalidOperationException($"Clinic with ID {Id} does not exist");
                }
                _context.Clinics.Remove(clinic);
                await _context.SaveChangesAsync(true);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error deleting tour: {ex.Message}");
                throw;
            } finally
            {
                transaction.Dispose();
            }
        }
    }
}