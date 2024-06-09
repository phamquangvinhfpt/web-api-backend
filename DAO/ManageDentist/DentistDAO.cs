using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using BusinessObject.Data;

using Microsoft.EntityFrameworkCore;
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
