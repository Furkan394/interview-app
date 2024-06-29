using Contracts;
using MassTransit;

namespace InterviewService.Consumers;

public class InterviewCreatedFaultConsumer : IConsumer<Fault<InterviewCreated>>
{
    public async Task Consume(ConsumeContext<Fault<InterviewCreated>> context)
    {
        System.Console.WriteLine("--> Consuming faulty creation");

        var exception = context.Message.Exceptions.First();

        if (exception.ExceptionType == "System.ArgumentException") //this is for example
        {
            await context.Publish(context.Message.Message);
        } else 
        {
            System.Console.WriteLine("Not an argument exception");
        }
    }
}
