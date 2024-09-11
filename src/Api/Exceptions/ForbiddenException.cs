namespace Api.Exceptions;

public class ForbiddenException(string detail, string title = "Forbidden")
    : ApiException(detail, title, StatusCodes.Status403Forbidden) { }
