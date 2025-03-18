using Microsoft.AspNetCore.Identity;
using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Interfaces
{
    public interface IUserServices
    {

        Task<IEnumerable<UserDto>> List();
        Task<UserDto?> FindUser(string id);

        Task<ServiceResponse> UpdateUser(string id, UserDto updatedUser);
        Task<ServiceResponse> DeleteUser(string id);

        Task<ServiceResponse> AddUser(UserDto userDto, string password);

    }
}
