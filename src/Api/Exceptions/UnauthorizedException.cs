namespace Api.Exceptions;

public class UnauthorizedException(string detail, string title = "Unauthorized")
    : ApiException(detail, title, StatusCodes.Status401Unauthorized) { }
