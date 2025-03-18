using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Interfaces
{
    public interface IUserTimelineService
    {
        Task<ServiceResponse> LinkUserToTimeline(string userId, int timelineId);
        Task<ServiceResponse> UnlinkUserFromTimeline(string userId, int timelineId);

        Task<IEnumerable<UserTimelineDto>> GetTimelinesForUser(string userId);

        Task<IEnumerable<UserTimelineDto>> GetUsersForTimeline(int timelineId);

    }
}
