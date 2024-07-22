// DAO/ManageDentist/DentistDAO.cs
using BusinessObject.Data;
using Microsoft.EntityFrameworkCore;

namespace DAO.ManageDentist
{
    public class DentistDAO
    {
        private readonly AppDbContext? _context;
        public DentistDAO(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusinessObject.Models.DentistDetail>> GetAllDentists()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                return await _context.DentistDetails.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error retrieving all dentists: {ex.Message}");
                throw;
            }
        }

        public async Task<BusinessObject.Models.DentistDetail> GetDentistById(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                return await _context.DentistDetails.AsNoTracking().FirstOrDefaultAsync(d => d.DentistId == id);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error retrieving dentist by ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task CreateDentist(BusinessObject.Models.DentistDetail dentist)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.DentistDetails.Add(dentist);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error creating dentist: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateDentist(BusinessObject.Models.DentistDetail dentist)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var existingDentist = _context.DentistDetails.FirstOrDefaultAsync(d => d.DentistId == dentist.DentistId);
                if (existingDentist == null)
                {
                    throw new InvalidOperationException($"Dentist with ID {dentist.DentistId} does not exist");
                }
                _context.DentistDetails.Update(dentist);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error updating dentist: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteDentist(Guid id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingDentist = await _context.DentistDetails.AsNoTracking().FirstOrDefaultAsync(d => d.DentistId == id);
                if (existingDentist == null)
                {
                    throw new InvalidOperationException($"Dentist with ID {id} does not exist");
                }
                _context.DentistDetails.Remove(existingDentist);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error deleting dentist: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DentistExists(Guid id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                return await _context.DentistDetails.AsNoTracking().AnyAsync(d => d.DentistId == id);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error checking if dentist exists: {ex.Message}");
                throw;
            }
        }
    }
}
