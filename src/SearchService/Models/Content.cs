using MongoDB.Entities;

namespace SearchService.Models;

public class Content : Entity
{
    public string Title { get; set; }
    public string AuthorId { get; set; }
    public string MediaUrl { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Text { get; set; }
}
