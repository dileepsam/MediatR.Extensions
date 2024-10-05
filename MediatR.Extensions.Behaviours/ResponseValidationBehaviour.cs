using Microsoft.Extensions.Logging;

namespace MediatR.Extensions.Behaviours;

/// <summary>
/// This behaviour is intended to validate the returned data.
/// eg: in multi tenant application, implement <see cref="IVerifyableResponse"/> for all responses,
/// and add logic to verify that the data returned belongs to the logged in tenant
/// improves security where if any data retrieved is not belonging to the tenant
/// the response will be aborted and an error returned.
/// intended to avoid data breaches and accidental mistakes in retrieval of data
/// </summary>
/// <remarks>
/// Need to be configured as the last behaviour in pipeline.
/// </remarks>
/// <typeparam name="TRequest"><see cref="IRequest{TResponse}"/></typeparam>
/// <typeparam name="TResponse">any type of <see cref="Result{TValue}"/></typeparam>
public class ResponseValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse : ResultBase, new()
{
    private readonly ILogger<TRequest> _logger;

    public ResponseValidationBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // invoke handler, this behaviour should be the last one in pipeline
        TResponse response = await next();

        // Check the result from handler is valid
        if (response is Result<IVerifyableResponse> result
            && result.IsSuccess
            && !result.Value.Verify())
        {
            _logger.LogError($"Unexpected data returned from processing {request}");

            var failedResponse = new TResponse();
            failedResponse.Errors.Add(new ResponseValidationError());

            // response validation failed, return an error.
            return response;
        }

        return response;
    }
}
