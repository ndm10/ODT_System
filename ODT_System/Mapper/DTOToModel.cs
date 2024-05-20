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
        }
    }
}
