namespace Api.Models;

public record ReadMoviesParameters
{
    public MovieSearchParameters? Search { get; init; }
    public MovieSortingParameters? Sorting { get; init; }
}

public record MovieSearchParameters
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? ActorName { get; init; }
    public string? DirectorName { get; init; }
    public DateOnly? ReleaseDate { get; init; }
}

public record MovieSortingParameters
{
    public IEnumerable<MovieSorting>? MovieSortings { get; init; }
}

public record MovieSorting
{
    public MovieSortingField Field { get; init; }
    public MovieSortingOrder? Order { get; init; }
}

public enum MovieSortingField
{
    Title,
    ReleaseDate,
    Rating,
}

public enum MovieSortingOrder
{
    Ascending,
    Descending,
    Newest,
    Oldest,
    Lowest,
    Highest,
}
