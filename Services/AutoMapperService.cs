using AutoMapper;
using Movement_be.Entities;
using Movement_be.Interfaces;
using Movement_be.Models.Dto;

namespace Movement_be.Services
{
    public class AutoMapperService : IAutoMapperService
    {
        public IMapper Mapper { get; }

        public AutoMapperService()
        {
            var config = new MapperConfiguration(InitMapper);
            Mapper = new Mapper(config);
            config.AssertConfigurationIsValid();
        }

        private void InitMapper(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<User, UserDto>()                
                .ForSourceMember(user => user.Password, opt => opt.DoNotValidate())
                .ReverseMap()                
                .ForMember(user => user.Password, opt => opt.Ignore());

        }
    }
}
