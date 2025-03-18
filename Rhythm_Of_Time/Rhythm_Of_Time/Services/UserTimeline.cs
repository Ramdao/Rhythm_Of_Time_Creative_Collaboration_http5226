using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Data;
using Rhythm_Of_Time.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rhythm_Of_Time.Data.Migrations;

namespace Rhythm_Of_Time.Services
{
    public class UserTimeline : IUserTimelineService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserTimeline(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Get all timelines for a specific user
        public async Task<IEnumerable<UserTimelineDto>> GetTimelinesForUser(string userId)  // Change userId type to string
        {
            List<UserTimelineDto> result = new();

            // Check if the user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return result; // Return empty list if user not found
            }

            try
            {
                result = await _context.UsersTimeline
                    .Where(ut => ut.user_id == userId)  // Use string for user_id
                    .Join(_context.timelines,
                          ut => ut.timeline_Id,
                          t => t.timeline_Id,
                          (ut, t) => new UserTimelineDto
                          {
                              usertime_Id = ut.usertime_Id,
                              timeline_Id = t.timeline_Id,
                              user_id = ut.user_id,
                              timeline_name = t.timeline_name,
                              user_email = user.Email  // Add user email to the DTO
                          })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
               
            }

            return result;
        }

        // Get all users linked to a specific timeline
        public async Task<IEnumerable<UserTimelineDto>> GetUsersForTimeline(int timelineId)
        {
            List<UserTimelineDto> result = new();

            // Check if the timeline exists
            bool timelineExists = await _context.timelines.AnyAsync(t => t.timeline_Id == timelineId);
            if (!timelineExists)
            {
                return result; // Return empty list if timeline not found
            }

            try
            {
                result = await _context.UsersTimeline
                    .Where(ut => ut.timeline_Id == timelineId)
                    .Join(_context.Users,  // Use IdentityUser table instead of custom users table
                          ut => ut.user_id,
                          u => u.Id,  // IdentityUser uses Id for the primary key
                          (ut, u) => new UserTimelineDto
                          {
                              usertime_Id = ut.usertime_Id,
                              timeline_Id = ut.timeline_Id,
                              user_id = u.Id,  // user_id will now be the IdentityUser Id

                          })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception if needed
            }

            return result;
        }

        // Link a user to a timeline
        public async Task<ServiceResponse> LinkUserToTimeline(string userId, int timelineId)  // userId is now a string
        {
            ServiceResponse serviceResponse = new();

            // Check if both user and timeline exist
            var user = await _userManager.FindByIdAsync(userId);
            bool timelineExists = await _context.timelines.AnyAsync(t => t.timeline_Id == timelineId);

            if (user == null || !timelineExists)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (user == null)
                {
                    serviceResponse.Messages.Add("User was not found.");
                }
                if (!timelineExists)
                {
                    serviceResponse.Messages.Add("Timeline was not found.");
                }
                return serviceResponse;
            }

            // Check if the relationship already exists to avoid duplicates
            bool linkExists = await _context.UsersTimeline.AnyAsync(ut => ut.user_id == userId && ut.timeline_Id == timelineId);
            if (linkExists)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("This user is already linked to the specified timeline.");
                return serviceResponse;
            }

            try
            {
                // Create a new entry in the UserTimeline join table
                Rhythm_Of_Time.Models.UserTimeline userTimeline = new()
                {
                    user_id = userId,
                    timeline_Id = timelineId
                };

                await _context.UsersTimeline.AddAsync(userTimeline);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an issue linking the user to the timeline.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            return serviceResponse;
        }

        // Unlink a user from a timeline
        public async Task<ServiceResponse> UnlinkUserFromTimeline(string userId, int timelineId)  // userId is now a string
        {
            ServiceResponse serviceResponse = new();

            // Find the existing link
            Rhythm_Of_Time.Models.UserTimeline? userTimeline = await _context.UsersTimeline
                .FirstOrDefaultAsync(ut => ut.user_id == userId && ut.timeline_Id == timelineId);

            if (userTimeline == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("The user is not linked to this timeline.");
                return serviceResponse;
            }

            try
            {
                _context.UsersTimeline.Remove(userTimeline);
                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
                serviceResponse.Messages.Add("User successfully unlinked from the timeline.");
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an issue unlinking the user from the timeline.");
                serviceResponse.Messages.Add(ex.Message);
            }

            return serviceResponse;
        }
    }
}
