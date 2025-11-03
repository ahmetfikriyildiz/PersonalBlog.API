namespace PersonalBlog.API.Mappings
{
    public interface IMappingService<TSource, TDestination>
    {
        TDestination Map(TSource source);
        TDestination Map(TSource source, TDestination destination);
        IEnumerable<TDestination> MapCollection(IEnumerable<TSource> source);
    }
}