using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Services
{
    public class AwardService : IAwardService
    {
        private readonly ApplicationDbContext _context;

        public AwardService(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all awards
        public async Task<IEnumerable<AwardDto>> List()
        {
            var awards = await _context.award.ToListAsync();
            var awardDtos = new List<AwardDto>();

            foreach (var award in awards)
            {
                awardDtos.Add(new AwardDto
                {
                    AwardId = award.AwardId,
                    name = award.name,
                    description = award.description
                });
            }

            return awardDtos;
        }

        // Find an award by its ID
        public async Task<AwardDto?> FindAward(int id)
        {
            var award = await _context.award.FirstOrDefaultAsync(a => a.AwardId == id);
            if (award == null)
            {
                return null;
            }

            return new AwardDto
            {
                AwardId = award.AwardId,
                name = award.name,
                description = award.description
            };
        }

        // Update an award's information
        public async Task<ServiceResponse> UpdateAward(int id, AwardDto awardDto)
        {
            ServiceResponse serviceResponse = new();

            var existingAward = await _context.award.FindAsync(id);
            if (existingAward == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Award not found.");
                return serviceResponse;
            }

            existingAward.name = awardDto.name;
            existingAward.description = awardDto.description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error updating award.");
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }

        // Add a new award
        public async Task<ServiceResponse> AddAward(AwardDto awardDto)
        {
            ServiceResponse serviceResponse = new();

            // Validate award data
            if (string.IsNullOrWhiteSpace(awardDto.name) || string.IsNullOrWhiteSpace(awardDto.description))
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Award name and description are required.");
                return serviceResponse;
            }

            var award = new Award
            {
                name = awardDto.name,
                description = awardDto.description
            };

            _context.award.Add(award);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error adding award.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = award.AwardId;
            return serviceResponse;
        }

        // Delete an award by ID
        public async Task<ServiceResponse> DeleteAward(int id)
        {
            ServiceResponse serviceResponse = new();

            var award = await _context.award.FindAsync(id);
            if (award == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Award not found.");
                return serviceResponse;
            }

            _context.award.Remove(award);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error deleting award.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }
    }
}
