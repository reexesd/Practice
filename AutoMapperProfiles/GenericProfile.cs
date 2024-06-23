using AutoMapper;

namespace Practice.AutoMapperProfiles
{
    public class GenericProfile<TSource, TResult> : Profile 
        where TSource : class
        where TResult : class
    {
        public GenericProfile() 
        {
            CreateMap<TSource, TResult>();
            CreateMap<TResult, TSource>();
        }
    }
}
