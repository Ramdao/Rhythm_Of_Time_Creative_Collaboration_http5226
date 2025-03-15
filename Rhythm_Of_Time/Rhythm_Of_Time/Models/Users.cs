using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhythm_Of_Time.Models
{
    public class UserDto
    {
        public required string UserId { get; set; }

        public string? UserName { get; set; }

        public string? UserEmail { get; set; }

       
    }

}
