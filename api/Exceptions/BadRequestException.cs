namespace Api.Exceptions;

public class BadRequestException(string detail, string title = "Bad Request")
    : ApiException(detail, title, StatusCodes.Status400BadRequest) { }
