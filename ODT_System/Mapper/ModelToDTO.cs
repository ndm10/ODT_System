using AutoMapper;
using ODT_System.DTO;
using ODT_System.Models;

namespace ODT_System.Mapper
{
    public class ModelToDTO : Profile
    {
        public ModelToDTO()
        {
            CreateMap<User, UserLoginDTO>();
            CreateMap<User, UserRegisterDTO>();
        }
    }
}
