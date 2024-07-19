using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Data;

namespace DAO.ManageDentist
{
    public class DentistDAO
    {
        private static DentistDAO _instance;
        private static AppDbContext _context;
        public DentistDAO()
        {
            _context = _context ?? new AppDbContext();
        }
        public static DentistDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DentistDAO();
                }
                return _instance;
            }
        }

        public async Task<IEnumerable<BusinessObject.Models.DentistDetail>> GetAllDentists()
        {
            try
            {
                return await _context.DentistDetails.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all dentists: {ex.Message}");
                throw;
            }
        }

        public async Task<BusinessObject.Models.DentistDetail> GetDentistById(Guid id)
        {
            try
            {
                return await _context.DentistDetails.FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving dentist by ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task CreateDentist(BusinessObject.Models.DentistDetail dentist)
        {
            try
            {
                _context.DentistDetails.Add(dentist);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating dentist: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateDentist(BusinessObject.Models.DentistDetail dentist)
        {
            try
            {
                var existingDentist = await _context.DentistDetails.FirstOrDefaultAsync(d => d.DentistId == dentist.DentistId);
                if (existingDentist != null)
                {

                    existingDentist.Degree = dentist.Degree;
                    existingDentist.Institute = dentist.Institute;
                    existingDentist.YearOfExperience = dentist.YearOfExperience;
                    existingDentist.Specialization = dentist.Specialization;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"Dentist with ID {dentist.DentistId} not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating dentist: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteDentist(Guid id)
        {
            try
            {
                var dentist = await _context.DentistDetails.FindAsync(id);
                if (dentist == null)
                {
                    throw new KeyNotFoundException($"Dentist with ID {id} not found");
                }
                _context.DentistDetails.Remove(dentist);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting dentist with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DentistExists(Guid id)
        {
            try
            {
                return await _context.DentistDetails.AnyAsync(e => e.DentistId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if dentist exists: {ex.Message}");
                throw;
            }
        }
    }
}
