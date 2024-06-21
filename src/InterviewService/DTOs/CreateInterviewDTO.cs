using System.ComponentModel.DataAnnotations;

namespace InterviewService.DTOs;

public class CreateInterviewDTO
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Text { get; set; }
    public string MediaUrl { get; set; }
}
