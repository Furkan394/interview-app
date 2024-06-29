using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class InterviewCreatedConsumer : IConsumer<InterviewCreated>
{
    private readonly IMapper _mapper;

    public InterviewCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<InterviewCreated> context)
    {
        System.Console.WriteLine("--> Consuming interview created: " + context.Message.Id);

        var content = _mapper.Map<Content>(context.Message);

        await content.SaveAsync();
    }
}
