
using InterviewService.Entities;
using Microsoft.EntityFrameworkCore;

namespace InterviewService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<InterviewDbContext>());
    }

    private static void SeedData(InterviewDbContext context)
    {
        context.Database.Migrate();

        if (context.Interviews != null && context.Interviews.Any())
        {
            System.Console.WriteLine("Already have data - no need to seed");
            return;
        }

        var interviews = new List<Interview>()
        {
            new Interview
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Title = "Futbolcu Röportajı",
                MediaUrl = "https://cdn.pixabay.com/photo/2016/05/06/16/32/car-1376190_960_720.jpg",
                AuthorId = Guid.NewGuid(),
                PublishedAt = DateTime.UtcNow,
                Content = new Content {
                    Id =  Guid.NewGuid(),
                    Text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.",
                }
            },

            new Interview
            {
                Id = Guid.Parse("c8c3ec17-01bf-49db-82aa-1ef80b833a9f"),
                Title = "Ünlü Röportajı",
                MediaUrl = "https://cdn.pixabay.com/photo/2012/05/29/00/43/car-49278_960_720.jpg",
                AuthorId = Guid.NewGuid(),
                PublishedAt = DateTime.UtcNow,
                Content = new Content {
                    Id =  Guid.NewGuid(),
                    Text = "It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                }
            },

            new Interview
            {
                Id = Guid.Parse("bbab4d5a-8565-48b1-9450-5ac2a5c4a654"),
                Title = "Benim Röportajım",
                MediaUrl = "https://cdn.pixabay.com/photo/2012/11/02/13/02/car-63930_960_720.jpg",
                AuthorId = Guid.NewGuid(),
                PublishedAt = DateTime.UtcNow,
                Content = new Content {
                    Id =  Guid.NewGuid(),
                    Text = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.",
                }
            },
        };

        context.AddRange(interviews);

        context.SaveChanges();
    }
}
