namespace Web.Shared.RequestDispatcher
{
    public interface IRequest<TResponse> { }

    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest request);
    }
}
