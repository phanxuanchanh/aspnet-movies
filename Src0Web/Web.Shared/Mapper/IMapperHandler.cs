namespace Web.Shared.Mapper
{
    public interface IMappingProfile<TSource, TDest>
    {
        TDest Map(TSource source);
    }
}