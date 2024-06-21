using AutoMapper;
using InterviewService.DTOs;
using InterviewService.Entities;

namespace InterviewService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Interview, InterviewDTO>().IncludeMembers(x => x.Content);
        CreateMap<Content, InterviewDTO>();
        CreateMap<CreateInterviewDTO, Interview>()
                    .ForMember(d => d.Content, o => o.MapFrom(s => s))
                    .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                    .ForMember(d => d.MediaUrl, o => o.MapFrom(s => s.MediaUrl));
        CreateMap<CreateInterviewDTO, Content>();
    }
}
