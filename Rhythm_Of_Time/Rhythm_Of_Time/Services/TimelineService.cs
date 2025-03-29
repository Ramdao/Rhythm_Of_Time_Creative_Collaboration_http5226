using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Rhythm_Of_Time.Services
{
    public class TimelineService : ITimelineService
    {
        private readonly ApplicationDbContext _context;

        public TimelineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimelineDto>> List()
        {
            // Retrieve all timeline entries
            List<Timeline> timelines = await _context.timelines.ToListAsync();
            List<TimelineDto> TimelineDtos = new List<TimelineDto>();

            foreach (Timeline timeline in timelines)
            {
                // Create new instance of TimelineDto and add to the list
                TimelineDtos.Add(new TimelineDto()
                {
                    timeline_Id = timeline.timeline_Id,
                    timeline_name = timeline.timeline_name,
                    date = timeline.date,
                    description = timeline.description,
                    

                });
            }

            return TimelineDtos;
        }

        public async Task<TimelineDto?> FindTimeline(int id)
        {
            // Find timeline entry based on {id}
            var timeline = await _context.timelines.FirstOrDefaultAsync(c => c.timeline_Id == id);

            if (timeline == null)
            {
                return null;
            }

            return new TimelineDto()
            {
                timeline_Id = timeline.timeline_Id,
                timeline_name = timeline.timeline_name,
                date = timeline.date,
                description = timeline.description,

            };
        }

        public async Task<ServiceResponse> UpdateTimeline(int id, TimelineDto timelineDto)
        {
            ServiceResponse serviceResponse = new();
            // Checking required fields
            if (string.IsNullOrWhiteSpace(timelineDto.timeline_name))
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Timeline name are required.");
                return serviceResponse;
            }

            // Find existing timeline entry in the database
            var timeline = await _context.timelines.FindAsync(id);
            if (timeline == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Timeline entry not found.");
                return serviceResponse;
            }

            // Update properties
            timeline.timeline_name = timelineDto.timeline_name;
            timeline.date = timelineDto.date;
            timeline.description = timelineDto.description;


            try
            {
                // Save changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Updating timeline entry failed.");
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }

        public async Task<ServiceResponse> AddTimeline(TimelineDto timelineDto)
        {
            ServiceResponse serviceResponse = new();
            // Validating data
            if (string.IsNullOrWhiteSpace(timelineDto.timeline_name))
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Timeline name are required.");
                return serviceResponse;
            }
            // Create new instance of timeline
            Timeline timeline = new Timeline()
            {
                timeline_name = timelineDto.timeline_name,
                date = timelineDto.date,
                description = timelineDto.description,

            };

            try
            {
                // Attempts to add timeline entry
                _context.timelines.Add(timeline);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the timeline entry.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = timeline.timeline_Id;
            return serviceResponse;
        }

        public async Task<ServiceResponse> DeleteTimeline(int id)
        {
            ServiceResponse response = new();

            var timeline = await _context.timelines.FindAsync(id);

            if (timeline == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Timeline entry cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                // Attempts to delete timeline entry
                _context.timelines.Remove(timeline);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error encountered while deleting the timeline entry.");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;
            return response;
        }
        //public async Task<IEnumerable<TimelineDto>> GetTimelinesForEntry(int entryId)
        //{
        //    // Fetch all timelines where the entryId matches
        //    var timelines = await _context.timelines
        //        .Where(t => t.entries.Any(e => e.entries_Id == entryId))  // Filters timelines by entryId
        //        .ToListAsync();

        //    // Convert the Timeline entities to DTOs
        //    var timelineDtos = timelines.Select(t => new TimelineDto
        //    {
        //        timeline_Id = t.timeline_Id,
        //        timeline_name = t.timeline_name,
        //        date = t.date,
        //        description = t.description
        //    }).ToList();

        //    return timelineDtos;
        //}

        public async Task<IEnumerable<TimelineDto>> GetTimelinesForEntry(int entryId)
        {
            return await _context.entry
                .Where(e => e.entry_Id == entryId)
                .Join(_context.timelines,
                      e => e.timeline_Id,
                      t => t.timeline_Id,
                      (e, t) => new TimelineDto
                      {
                          timeline_Id = t.timeline_Id,
                          description = t.description
                      })
                .ToListAsync();
        }
    }
}

