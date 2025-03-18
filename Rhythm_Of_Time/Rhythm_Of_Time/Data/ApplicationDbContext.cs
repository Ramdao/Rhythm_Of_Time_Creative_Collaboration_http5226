using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Timeline> timelines { get; set; }
    public DbSet<UserTimeline> UsersTimeline { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
