using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Interfaces
{
    public interface ITimelineService
    {

        Task<IEnumerable<TimelineDto>> List();

        Task<TimelineDto?> FindTimeline(int id);

        Task<ServiceResponse> UpdateTimeline(int id, TimelineDto timelineDto);

        Task<ServiceResponse> AddTimeline(TimelineDto timelineDto);

        Task<ServiceResponse> DeleteTimeline(int id);


       // Task<IEnumerable<TimelineDto>> GetTimelinesForEntry(int entryId);
    }
}
