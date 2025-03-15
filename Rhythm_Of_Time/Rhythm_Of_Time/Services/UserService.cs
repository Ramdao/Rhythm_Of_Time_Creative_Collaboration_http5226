using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Rhythm_Of_Time.Interfaces;
using Rhythm_Of_Time.Models;
using Rhythm_Of_Time.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Rhythm_Of_Time.Services
{
    public class UserService : IUserServices
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // dependency injection of database context, user manager, and http context accessor
        public UserService(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<UserDto>> List()
        {
            IEnumerable<IdentityUser> Users = await _userManager.GetUsersInRoleAsync("User");

            List<UserDto> UserDtos = new List<UserDto>();
            foreach (IdentityUser User in Users)
            {
                UserDtos.Add(new UserDto()
                {
                    UserId = User.Id,
                    UserName = User.UserName,
                    UserEmail = User.Email,
                    
                });
            }
            
            return UserDtos;
        }
    }
}
