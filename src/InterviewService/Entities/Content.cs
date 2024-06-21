using System.ComponentModel.DataAnnotations.Schema;

namespace InterviewService.Entities;

[Table("Contents")]
public class Content
{
    public Guid Id { get; set; }
    public Interview Interview { get; set; }
    public Guid InterviewId { get; set; }
    public string Text { get; set; }
}
