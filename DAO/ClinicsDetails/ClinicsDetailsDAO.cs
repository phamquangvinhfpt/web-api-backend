using BusinessObject.Data;
using BusinessObject.Models;

namespace DAO.ClinicsDetails
{
    public class ClinicsDetailsDAO
    {
        private readonly AppDbContext? _context = null;

        public ClinicsDetailsDAO(AppDbContext context)
        {
            _context = context;
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
                var existingClinics = _context.ClinicDetails.FirstOrDefault(c => c.Id == ClinicsDetails.Id);
                if (existingClinics != null)
                {
                    throw new InvalidOperationException($"Clinic with ID {ClinicsDetails.Id} already exists");
                }
                _context.ClinicDetails.Add(ClinicsDetails);
                _context.SaveChangesAsync();
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
                var existingClinics = _context.ClinicDetails.FirstOrDefault(c => c.Id == ClinicDetail.Id);
                if (existingClinics == null)
                {
                    throw new InvalidOperationException($"Clinic with ID {ClinicDetail.Id} does not exist");
                }
                _context.ClinicDetails.Update(ClinicDetail);
                _context.SaveChangesAsync();
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
                var existingClinics = _context.ClinicDetails.FirstOrDefault(c => c.Id == Id);
                if (existingClinics == null)
                {
                    throw new InvalidOperationException($"Clinic with ID {Id} does not exist");
                }
                _context.ClinicDetails.Remove(existingClinics);
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting tour: {ex.Message}");
                throw;
            }
        }
    }
}