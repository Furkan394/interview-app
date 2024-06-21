using AutoMapper;
using InterviewService.Data;
using InterviewService.DTOs;
using InterviewService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewService.Controllers;

[ApiController]
[Route("api/v1/interviews")]
public class InterviewsController : ControllerBase
{
    private readonly InterviewDbContext _dbContext;
    private readonly IMapper _mapper;

    public InterviewsController(InterviewDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<InterviewDTO>>> GetAllInterviews()
    {
        var interviews = await _dbContext.Interviews.Include(x => x.Content).ToListAsync();

        return _mapper.Map<List<InterviewDTO>>(interviews);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InterviewDTO>> GetInterviewById(Guid id)
    {
        var interview = await _dbContext.Interviews.Include(x => x.Content).FirstOrDefaultAsync(x => x.Id == id);

        if (interview is null) return NotFound();

        return _mapper.Map<InterviewDTO>(interview);
    }

    [HttpPost]
    public async Task<ActionResult<InterviewDTO>> Create(CreateInterviewDTO createInterviewDTO)
    {
        var interview = _mapper.Map<Interview>(createInterviewDTO);

        //TODO: add current user

        interview.AuthorId = Guid.NewGuid();

        await _dbContext.Interviews.AddAsync(interview);

        var result = await _dbContext.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the database");

        return CreatedAtAction(nameof(GetInterviewById), new { interview.Id }, _mapper.Map<InterviewDTO>(interview));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateInterviewDTO updateInterviewDTO)
    {
        var interviewToUpdate = await _dbContext.Interviews.Include(x => x.Content).FirstOrDefaultAsync(x => x.Id == id);

        if (interviewToUpdate is null) return NotFound();

        interviewToUpdate.Title = updateInterviewDTO.Title ?? interviewToUpdate.Title;
        interviewToUpdate.MediaUrl = updateInterviewDTO.MediaUrl ?? interviewToUpdate.MediaUrl;
        interviewToUpdate.Content.Text = updateInterviewDTO.Text ?? interviewToUpdate.Content.Text;

        var result = await _dbContext.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the database");

        return Ok(result);


    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var interviewToDelete = await _dbContext.Interviews.FindAsync(id);

        if (interviewToDelete is null) return NotFound();

        _dbContext.Interviews.Remove(interviewToDelete);

        var result = await _dbContext.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the database");

        return Ok();


    }
}
