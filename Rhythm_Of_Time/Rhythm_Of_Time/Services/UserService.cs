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
            //IEnumerable<IdentityUser> Users = await _userManager.GetUsersInRoleAsync("User");
            IEnumerable<IdentityUser> Users = await _userManager.Users.ToListAsync();

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

        public async Task<UserDto?> FindUser(string id)
        {
            IdentityUser? User = await _userManager.FindByIdAsync(id);

            if (User == null) return null;

            UserDto UserDto = new UserDto()
            {
                UserId = User.Id,
                UserName = User.UserName,
                UserEmail = User.Email,
            };

            return UserDto;
        }

        public async Task<ServiceResponse> UpdateUser(string id, UserDto userDto)
        {
            var response = new ServiceResponse();

           

            // Get user
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("User not found");
                return response;
            }

            // Update fields
            user.UserName = userDto.UserName;
            user.NormalizedUserName = _userManager.NormalizeName(userDto.UserName);
            user.Email = userDto.UserEmail;  // Critical for login
            user.NormalizedEmail = _userManager.NormalizeEmail(userDto.UserEmail);
            user.SecurityStamp = Guid.NewGuid().ToString(); // Force re-login

            // Save
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.AddRange(result.Errors.Select(e => e.Description));
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Updated;
            return response;
        }
        public async Task<ServiceResponse> DeleteUser(string id)
        {
            ServiceResponse response = new();

            // Find user in Identity
            IdentityUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("User cannot be deleted because it does not exist.");
                return response;
            }

            try
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    response.Messages.Add("Error encountered while deleting the User.");
                    return response;
                }
            }
            catch (Exception)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("An unexpected error occurred while deleting the user.");
                return response;
            }

            response.Status = ServiceResponse.ServiceStatus.Deleted;
            return response;
        }

        public async Task<ServiceResponse> AddUser(UserDto userDto, string password)
        {
            ServiceResponse serviceResponse = new();

            // Validate required fields
            if (string.IsNullOrWhiteSpace(userDto.UserName) ||
                string.IsNullOrWhiteSpace(userDto.UserEmail) ||
                string.IsNullOrWhiteSpace(password))
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Username, email, and password are required.");
                return serviceResponse;
            }

            // Check if the email is already taken
            var existingUser = await _userManager.FindByEmailAsync(userDto.UserEmail);
            if (existingUser != null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("A user with this email already exists.");
                return serviceResponse;
            }

            // Create a new IdentityUser
            var newUser = new IdentityUser
            {
                UserName = userDto.UserName,
                Email = userDto.UserEmail
            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, password);
                if (!result.Succeeded)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.AddRange(result.Errors.Select(e => e.Description));
                    return serviceResponse;
                }

                // Assign default role (optional)
                await _userManager.AddToRoleAsync(newUser, "User");

                serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
                serviceResponse.Messages.Add("User successfully created.");
            }
            catch (Exception)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An unexpected error occurred while creating the user.");
            }

            return serviceResponse;
        }


    }
}
