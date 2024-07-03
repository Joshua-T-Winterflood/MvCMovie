namespace MvcMovie.Mediator
{

    //################################################################ Mediator Section #######################################################################
    /// <summary>
    /// Marker interface to represent a request with a void response
    /// </summary>
    public interface IRequest : IBaseRequest { }

    /// <summary>
    /// Marker interface to represent a request with a response
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IRequest<out TResponse> : IBaseRequest { }

    /// <summary>
    /// Allows for generic type constraints of objects implementing IRequest or IRequest{TResponse}
    /// </summary>
    public interface IBaseRequest { }




    //################################################################ IReq Section #######################################################################


    public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }


    //################################################################ Shared Section #######################################################################
    public interface IReportingEntity<T> { }
}