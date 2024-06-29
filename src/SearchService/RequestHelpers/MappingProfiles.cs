using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<InterviewCreated, Content>();
        CreateMap<InterviewUpdated, Content>();
    }
}