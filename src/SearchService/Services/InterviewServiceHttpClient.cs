using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class InterviewServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public InterviewServiceHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<Content>> GetContentsAsync()
    {
        var lastUpdated = await DB.Find<Content, string>()
            .Sort(x => x.Descending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        return await _httpClient.GetFromJsonAsync<List<Content>>(_configuration["InterviewServiceUrl"] + "/api/v1/interviews?date=" + lastUpdated);
    }
}
