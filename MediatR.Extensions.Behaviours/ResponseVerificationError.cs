using MediatR.Extensions.Abstractions;

namespace MediatR.Extensions.Behaviours;

internal sealed class ResponseValidationError : Error, IResponseValidationError;
