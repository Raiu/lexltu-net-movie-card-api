using Api.Controllers;
using Api.Models;

namespace Api.Extensions;

public static class SortExtensions
{
    public static IQueryable<Movie> SortMovieNonGeneric(
        this IQueryable<Movie> query,
        MovieSortingParameters? sortingParameters
    )
    {
        if (sortingParameters?.MovieSortings == null)
            return query;

        foreach (var parameter in sortingParameters.MovieSortings)
        {
            switch (parameter.Field)
            {
                case MovieSortingField.Title:
                    query =
                        parameter.Order == MovieSortingOrder.Descending
                            ? query.OrderByDescending(m => m.Title)
                            : query.OrderBy(m => m.Title);
                    break;
                case MovieSortingField.ReleaseDate:
                    query =
                        parameter.Order == MovieSortingOrder.Oldest
                            ? query.OrderByDescending(m => m.ReleaseDate)
                            : query.OrderBy(m => m.ReleaseDate);
                    break;
                case MovieSortingField.Rating:
                    query =
                        parameter.Order == MovieSortingOrder.Lowest
                            ? query.OrderByDescending(m => m.Rating)
                            : query.OrderBy(m => m.Rating);
                    break;
            }
        }

        return query;
    }
}
