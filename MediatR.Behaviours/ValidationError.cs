namespace MediatR.Behaviours;

public class ValidationError : Error
{
  public IReadOnlyList<ValidationFailure> Errors { get; private set; }

  public ValidationError(IReadOnlyList<ValidationFailure> failures)
  {
    Errors = failures;
  }
}

