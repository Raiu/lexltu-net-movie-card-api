namespace Api.Exceptions;

public class DuplicateException(string detail, string title = "Duplicate")
    : ApiException(detail, title, StatusCodes.Status409Conflict) { }
