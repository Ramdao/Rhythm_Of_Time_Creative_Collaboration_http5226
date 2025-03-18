using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rhythm_Of_Time.Models;

namespace Rhythm_Of_Time.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Timeline> timelines { get; set; }
    public DbSet<UserTimeline> UsersTimeline { get; set; }

    public DbSet<Song> song { get; set; }

    public DbSet<Artist> artist { get; set; }

    public DbSet<Award> award { get; set; }

    public DbSet<Entry> entry { get; set; }

    public DbSet<ArtistSong> artistSongs { get; set; }

    public DbSet<AwardSong> awardSongs { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
