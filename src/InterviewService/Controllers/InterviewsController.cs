using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using InterviewService.Data;
using InterviewService.DTOs;
using InterviewService.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewService.Controllers;

[ApiController]
[Route("api/v1/interviews")]
public class InterviewsController : ControllerBase
{
    private readonly InterviewDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public InterviewsController(InterviewDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<InterviewDTO>>> GetAllInterviews(string date)
    {
        var query = _dbContext.Interviews.OrderBy(x => x.PublishedAt).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<InterviewDTO>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InterviewDTO>> GetInterviewById(Guid id)
    {
        var interview = await _dbContext.Interviews.Include(x => x.Content).FirstOrDefaultAsync(x => x.Id == id);

        if (interview is null) return NotFound();

        return _mapper.Map<InterviewDTO>(interview);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<InterviewDTO>> Create(CreateInterviewDTO createInterviewDTO)
    {
        var interview = _mapper.Map<Interview>(createInterviewDTO);

        interview.AuthorId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type.Equals("userId", StringComparison.OrdinalIgnoreCase))?.Value);

        await _dbContext.Interviews.AddAsync(interview);

        var newInterview = _mapper.Map<InterviewDTO>(interview);

        await _publishEndpoint.Publish(_mapper.Map<InterviewCreated>(newInterview));

        var result = await _dbContext.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the database");

        return CreatedAtAction(nameof(GetInterviewById), new { interview.Id }, newInterview);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateInterviewDTO updateInterviewDTO)
    {
        var interviewToUpdate = await _dbContext.Interviews.Include(x => x.Content).FirstOrDefaultAsync(x => x.Id == id);

        if (interviewToUpdate is null) return NotFound();

        if (!CheckUserClaim(interviewToUpdate.AuthorId.ToString())) return Forbid();

        interviewToUpdate.Title = updateInterviewDTO.Title ?? interviewToUpdate.Title;
        interviewToUpdate.MediaUrl = updateInterviewDTO.MediaUrl ?? interviewToUpdate.MediaUrl;
        interviewToUpdate.Content.Text = updateInterviewDTO.Text ?? interviewToUpdate.Content.Text;

        await _publishEndpoint.Publish(_mapper.Map<InterviewUpdated>(interviewToUpdate));

        var result = await _dbContext.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the database");

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var interviewToDelete = await _dbContext.Interviews.FindAsync(id);

        if (interviewToDelete is null) return NotFound();

        if (!CheckUserClaim(interviewToDelete.AuthorId.ToString())) return Forbid();

        _dbContext.Interviews.Remove(interviewToDelete);

        await _publishEndpoint.Publish<InterviewDeleted>(new { id = interviewToDelete.Id.ToString() });

        var result = await _dbContext.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the database");

        return Ok();
    }

    private bool CheckUserClaim(string authorId) 
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type.Equals("userId", StringComparison.OrdinalIgnoreCase))?.Value;

        if (userIdClaim is null || !userIdClaim.Equals(authorId)) return false;

        return true;
    }
}
