using AutoMapper;

namespace HotpServer.Utilities
{
    public class MapperBuilder
    {
        public static IMapper CreateMapper<TSource, TDestination>()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
