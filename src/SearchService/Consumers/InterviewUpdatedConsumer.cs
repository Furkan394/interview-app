using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class InterviewUpdatedConsumer : IConsumer<InterviewUpdated>
{
     private readonly IMapper _mapper;

    public InterviewUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<InterviewUpdated> context)
    {
        System.Console.WriteLine("--> Consuming interview updated: " + context.Message.Id);

        var content = _mapper.Map<Content>(context.Message);

        var result = await DB.Update<Content>()
                .Match(i => i.ID == context.Message.Id)
                .ModifyOnly(x => new
                {
                    x.Title,
                    x.Text,
                    x.MediaUrl
                }, content)
                .ExecuteAsync();

        if (!result.IsAcknowledged) throw new MessageException(typeof(InterviewUpdated), "Problem updating mongodb");
        
    }
}
