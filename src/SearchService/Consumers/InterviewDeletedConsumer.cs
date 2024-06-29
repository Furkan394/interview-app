using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class InterviewDeletedConsumer : IConsumer<InterviewDeleted>
{
    public async Task Consume(ConsumeContext<InterviewDeleted> context)
    {
        System.Console.WriteLine("--> Consuming interview deleted: " + context.Message.Id);

        var result = await DB.DeleteAsync<Content>(context.Message.Id);
        
        if (!result.IsAcknowledged) throw new MessageException(typeof(InterviewUpdated), "Problem deleting interview");
    }
}
