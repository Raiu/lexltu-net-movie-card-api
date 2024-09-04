using System.Linq.Expressions;
using System.Reflection;
using Api.Controllers;
using Api.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using YamlDotNet.Core.Tokens;

namespace Api.Extensions;

public static class QueryExtensions
{
    public static IQueryable<Movie> FilterMovieNonGeneric(
        this IQueryable<Movie> query,
        MovieSearchParameters? searchParameters
    )
    {
        if (searchParameters == null)
            return query;

        foreach (var property in searchParameters.GetType().GetProperties())
        {
            var value = property.GetValue(searchParameters);
            if (value == null)
                continue;

            if (value is string strValue)
            {
                value = strValue.ToLower();
            }

            switch (property.Name)
            {
                case nameof(searchParameters.Title):
                    query = query.Where(m => m.Title.ToLower().Contains((string)value));
                    break;
                case nameof(MovieSearchParameters.ReleaseDate):
                    query = query.Where(m => m.ReleaseDate == (DateOnly)value);
                    break;
                case nameof(searchParameters.DirectorName):

                    query = query.Where(m =>
                        m.Director.Name.ToLower().Contains((string)value)
                    );
                    break;
                case nameof(MovieSearchParameters.ActorName):

                    query = query.Where(m =>
                        m.Actors.Any(a => a.Name.ToLower().Contains((string)value))
                    );
                    break;
                case nameof(MovieSearchParameters.Genre):
                    query = query.Where(m =>
                        m.Genres.Any(g => g.Name.ToLower().Contains((string)value))
                    );
                    break;
                default:
                    break;
            }
        }

        return query;
    }

    /* public static IQueryable<T> Filter<T>(this IQueryable<T> query, object searchParams)
    {
        if (searchParams == null)
            return query;

        var parameter =  Expression.Parameter(typeof(T), "x");

        foreach (var property in searchParams.GetType().GetProperties()) {
            var value = property.GetValue(searchParams);

            if (value == null)
                continue;
            



        }
    }

    private static Expression Comparison<T>(ParameterExpression parameter, PropertyInfo property, Object value)
    {

    }

    private static Expression DateComparion(MemberExpression member, Object value)
    {

    } */
}
