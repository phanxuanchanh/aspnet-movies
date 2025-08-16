namespace Web.Shared.Mapper
{
    public interface ITypeMapper<TSource, TTarget>
    {
        TTarget Map(TSource source);
    }
}