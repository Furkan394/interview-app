
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDB", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Content>()
            .Key(x => x.Title, KeyType.Text)
            .Key(x => x.Text, KeyType.Text)
            .Key(x => x.MediaUrl, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Content>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<InterviewServiceHttpClient>();

        var contents = await httpClient.GetContentsAsync();

        System.Console.WriteLine(contents.Count + " returned from the interview service");

        if (contents.Count > 0) await DB.SaveAsync(contents);

    }
}
