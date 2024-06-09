using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.ManageDentist.Models;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Data;
namespace Core.ManageDentist.Repository
{
    public class DentistRepository : IDentistRepository
    {
        private readonly AppDbContext _context;

        public DentistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusinessObject.Models.DentistDetail>> GetAllDentists()
        {
            return await _context.DentistDetails.ToListAsync();
        }

        public async Task<BusinessObject.Models.DentistDetail> GetDentistById(Guid id)
        {
            return await _context.DentistDetails.FindAsync(id);
        }

        public async Task CreateDentist(BusinessObject.Models.DentistDetail dentist)
        {
            _context.DentistDetails.Add(dentist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDentist(BusinessObject.Models.DentistDetail dentist)
        {
            _context.Entry(dentist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDentist(Guid id)
        {
            var dentist = await _context.DentistDetails.FindAsync(id);
            _context.DentistDetails.Remove(dentist);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DentistExists(Guid id)
        {
            return await _context.DentistDetails.AnyAsync(e => e.DentistId == id);
        }
    }
}
