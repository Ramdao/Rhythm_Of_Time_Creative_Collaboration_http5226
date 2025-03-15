using Microsoft.AspNetCore.Identity;
using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Interfaces
{
    public interface IUserServices
    {

        Task<IEnumerable<UserDto>> List();
    }
}
