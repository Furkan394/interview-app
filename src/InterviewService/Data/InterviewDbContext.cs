using InterviewService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace InterviewService.Data;

public class InterviewDbContext : DbContext
{
    public InterviewDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Interview> Interviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

}
