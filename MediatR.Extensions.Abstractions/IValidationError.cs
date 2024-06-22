using FluentValidation.Results;

namespace MediatR.Extensions.Abstractions;

public interface IValidationError
{
  IReadOnlyList<ValidationFailure> Errors { get; }
}
