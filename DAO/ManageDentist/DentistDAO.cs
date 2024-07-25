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
            try
            {
                return await _context.DentistDetails.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all dentists: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<BusinessObject.Models.DentistDetail>> GetAllDentistsByClinicId(Guid id)
        {
            try
            {
                return await _context.DentistDetails.AsNoTracking().Where(d => d.ClinicId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all dentists by clinic ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<BusinessObject.Models.DentistDetail> GetDentistById(Guid id)
        {
            try
            {
                return await _context.DentistDetails.AsNoTracking().FirstOrDefaultAsync(d => d.DentistId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving dentist by ID {id}: {ex.Message}");
                throw;
            }
        }

        public Task CreateDentist(BusinessObject.Models.DentistDetail dentist)
        {
            try
            {
                _context.DentistDetails.Add(dentist);
                _context.SaveChangesAsync();
                return Task.CompletedTask;
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
                if (existingDentist == null)
                {
                    throw new InvalidOperationException($"Dentist with ID {dentist.DentistId} does not exist");
                }
                foreach (var property in _context.Entry(existingDentist).Properties)
                {
                    if (property.Metadata.Name != nameof(BusinessObject.Models.DentistDetail.Id))
                    {
                        property.CurrentValue = _context.Entry(dentist).Property(property.Metadata.Name).CurrentValue;
                    }
                }
                _context.SaveChangesAsync();
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
                var existingDentist = await _context.DentistDetails.AsNoTracking().FirstOrDefaultAsync(d => d.DentistId == id);
                if (existingDentist == null)
                {
                    throw new InvalidOperationException($"Dentist with ID {id} does not exist");
                }
                _context.DentistDetails.Remove(existingDentist);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting dentist: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DentistExists(Guid id)
        {
            try
            {
                return await _context.DentistDetails.AsNoTracking().AnyAsync(d => d.DentistId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if dentist exists: {ex.Message}");
                throw;
            }
        }
    }
}
