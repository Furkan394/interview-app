using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/v1/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Content>>> SearchContents([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Content, Content>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        if (!string.IsNullOrEmpty(searchParams.AuthorId))
        {
            query.Match(x => x.AuthorId == searchParams.AuthorId);
        }

        query = searchParams.OrderBy switch
        {
            "publishedAt" => query.Sort(x => x.Descending(a => a.PublishedAt)),
            _ => query.Sort(x => x.Descending(a => a.CreatedAt))
        };

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
