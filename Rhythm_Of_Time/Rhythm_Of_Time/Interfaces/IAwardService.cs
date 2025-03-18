using Rhythm_Of_Time.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhythm_Of_Time.Interfaces
{
    public interface IAwardService
    {
        // List all awards
        Task<IEnumerable<AwardDto>> List();

        // Find an award by its ID
        Task<AwardDto?> FindAward(int id);

        // Update an award's information
        Task<ServiceResponse> UpdateAward(int id, AwardDto awardDto);

        // Add a new award
        Task<ServiceResponse> AddAward(AwardDto awardDto);

        // Delete an award by its ID
        Task<ServiceResponse> DeleteAward(int id);
    }
}
