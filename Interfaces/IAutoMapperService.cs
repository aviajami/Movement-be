using AutoMapper;

namespace Movement_be.Interfaces
{
    public interface IAutoMapperService
    {
        IMapper Mapper { get; }
    }
}
