using AutoMapper;
using ODT_System.DTO;
using ODT_System.Models;

namespace ODT_System.Mapper
{
    public class DTOToModel : Profile
    {
        public DTOToModel()
        {
            CreateMap<UserLoginDTO, User>();
            CreateMap<UserRegisterDTO, User>();
            CreateMap<ViewProfileDTO, User>();
            CreateMap<PostCreateDTO, Post>();
            CreateMap<StudyTimeDTO, StudyTime>();
            CreateMap<StudyTimeCreateDTO, StudyTime>();
            CreateMap<PostUpdateDTO, Post>();
            CreateMap<StudyTimeUpdateDTO, StudyTime>();
            CreateMap<ChatInBoxDTO, Chat>();
            CreateMap<FeedbackCreateDTO, Feedback>();
        }
    }
}
