using InterviewService.Entities;
using Microsoft.EntityFrameworkCore;

namespace InterviewService.Data;

public class InterviewDbContext : DbContext
{
    public InterviewDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Interview> Interviews { get; set; }

}
