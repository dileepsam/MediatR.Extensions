namespace MediatR.Behaviours;

public sealed class FluentValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
where TRequest : IRequest<TResponse>
where TResponse : ResultBase, new()
{
  private readonly IEnumerable<IValidator<TRequest>> _validators;

    public FluentValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
          var response = new TResponse();
          response.Errors.Add(new ValidationError(failures));

          // validation failed, abort processing and return the response.
          return response;
        }

        // no validation errors, proceed with handler or next in the pipeline
        return await next();
    }
}

