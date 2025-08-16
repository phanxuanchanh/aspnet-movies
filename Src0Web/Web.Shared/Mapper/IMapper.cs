namespace Web.Shared.Mapper
{
    public interface IMapper
    {
        TTarget Map<TSource, TTarget>(TSource source);
    }
}