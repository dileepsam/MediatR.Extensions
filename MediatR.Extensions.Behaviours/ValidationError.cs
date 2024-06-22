using MediatR.Extensions.Abstractions;

namespace MediatR.Extensions.Behaviours;

internal sealed class ValidationError : Error, IValidationError
{
  public IReadOnlyList<ValidationFailure> Errors { get; private set; }

  public ValidationError(IReadOnlyList<ValidationFailure> failures)
  {
    Errors = failures;
  }
}

