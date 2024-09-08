using System.Diagnostics.CodeAnalysis;
using Api.Data;
using Api.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public abstract class ServiceBase<TEntity, TDto> : IServiceBase<TEntity, TDto>
    where TEntity : class
{
    protected ApiDbContext _db { get; }
    protected IMapper _am { get; }
    protected IUnitOfWork _uow { get; }
    protected ActionContext _ac { get; }

    public ServiceBase(
        ApiDbContext dbContext,
        IMapper autoMapper,
        IUnitOfWork unitOfWork,
        ActionContext actionContext
    )
    {
        _db = dbContext;
        _am = autoMapper;
        _uow = unitOfWork;
        _ac = actionContext;
    }

    public virtual async Task<TDto> CreateAsync<TCreateDto>(TCreateDto dto)
    {
        var entity = _am.Map<TEntity>(dto);
        _db.Set<TEntity>().Add(entity);
        await _uow.SaveChangesAsync();

        return _am.Map<TDto>(entity);
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync() => await GetAllAsync<TDto>();

    public virtual async Task<IEnumerable<T>> GetAllAsync<T>()
    {
        var query = _db.Set<TEntity>().AsQueryable();
        return await _am.ProjectTo<T>(query).ToListAsync();
    }

    public virtual async Task<TDto> GetByIdAsync(int id) => await GetByIdAsync<TDto>(id);

    public virtual async Task<T> GetByIdAsync<T>(int id)
    {
        var entity = await _db.Set<TEntity>().FindAsync(id);
        if (entity == null)
            NotFound();

        return _am.Map<T>(entity);
    }

    public virtual async Task<TDto> UpdateAsync<TUpdateDto>(int id, TUpdateDto dto) =>
        await UpdateAsync<TDto, TUpdateDto>(id, dto);

    public virtual async Task<TReturn> UpdateAsync<TReturn, TUpdateDto>(int id, TUpdateDto dto)
    {
        var entity = await _db.Set<TEntity>().FindAsync(id);
        if (entity == null)
            NotFound();

        _am.Map(entity, dto);
        await _uow.SaveChangesAsync();

        return _am.Map<TReturn>(entity);
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Set<TEntity>().FindAsync(id);
        if (entity == null)
            NotFound();

        _db.Set<TEntity>().Remove(entity);
        await _uow.SaveChangesAsync();
        return true;
    }

    public virtual async Task<TDto> PartialAsync(int id, JsonPatchDocument patchDocument) =>
        await PartialAsync<TDto>(id, patchDocument);

    public virtual async Task<T> PartialAsync<T>(int id, JsonPatchDocument patchDocument)
    {
        var entity = await _db.Set<TEntity>().FindAsync(id);
        if (entity == null)
            NotFound();

        patchDocument.ApplyTo(entity);

        if (!_ac.ModelState.IsValid)
            BadRequest(_ac.ModelState.ToString() ?? "Model is not valid");

        await _uow.SaveChangesAsync();

        return _am.Map<T>(entity);
    }

    [DoesNotReturn]
    protected static void NotFound(
        string detail = "Entity not found",
        string title = "Not Found"
    ) => throw new NotFoundException(detail, title);

    [DoesNotReturn]
    protected static void BadRequest(string detail, string title = "Bad Request") =>
        throw new BadRequestException(detail, title);

    [DoesNotReturn]
    protected static void Unauthorized(string detail, string title = "Unauthorized") =>
        throw new UnauthorizedException(detail, title);

    [DoesNotReturn]
    protected static void Forbidden(string detail, string title = "Forbidden") =>
        throw new ForbiddenException(detail, title);
}

/*
public abstract class ServiceBase<TEntity, TDto>(
    ApiDbContext dbContext,
    IMapper autoMapper,
    IUnitOfWork unitOfWork,
    ActionContext actionContext
)
    where TEntity : class
{
    protected ApiDbContext _db { get; } = dbContext;
    protected IMapper _am { get; } = autoMapper;
    protected IUnitOfWork _uow { get; } = unitOfWork;
    protected ActionContext _ac { get; } = actionContext;

    public virtual async Task<TDto> CreateAsync<TCreateDto>(TCreateDto dto)
    {
        var entity = _am.Map<TEntity>(dto);
        _db.Set<TEntity>().Add(entity);
        await _uow.SaveChangesAsync();

        return _am.Map<TDto>(entity);
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync() => await GetAllAsync<TDto>();

    public virtual async Task<IEnumerable<T>> GetAllAsync<T>()
    {
        var query = _db.Set<TEntity>().AsQueryable();
        return await _am.ProjectTo<T>(query).ToListAsync();
    }

    public virtual async Task<TDto> GetByIdAsync(int id) => await GetByIdAsync<TDto>(id);

    public virtual async Task<T> GetByIdAsync<T>(int id)
    {
        var entity = await _db.Set<TEntity>().FindAsync(id);
        if (entity == null)
            NotFound();

        return _am.Map<T>(entity);
    }

    public virtual async Task<TDto> UpdateAsync<TUpdateDto>(int id, TUpdateDto dto) =>
        await UpdateAsync<TDto, TUpdateDto>(id, dto);

    public virtual async Task<TReturn> UpdateAsync<TReturn, TUpdateDto>(int id, TUpdateDto dto)
    {
        var entity = await _db.Set<TEntity>().FindAsync(id);
        if (entity == null)
            NotFound();

        _am.Map(entity, dto);
        await _uow.SaveChangesAsync();

        return _am.Map<TReturn>(entity);
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Set<TEntity>().FindAsync(id);
        if (entity == null)
            NotFound();

        _db.Set<TEntity>().Remove(entity);
        await _uow.SaveChangesAsync();
        return true;
    }

    public virtual async Task<TDto> PartialAsync(int id, JsonPatchDocument patchDocument) =>
        await PartialAsync<TDto>(id, patchDocument);

    public virtual async Task<T> PartialAsync<T>(int id, JsonPatchDocument patchDocument)
    {
        var movie = await _db.Movies.FindAsync(id);

        if (movie == null)
            NotFound();

        patchDocument.ApplyTo(movie);

        if (!_ac.ModelState.IsValid)
            BadRequest(_ac.ModelState.ToString() ?? "Model is not valid");

        await _db.SaveChangesAsync();

        return _am.Map<T>(movie);
    }

    [DoesNotReturn]
    protected static void NotFound(
        string detail = "Entity not found",
        string title = "Not Found"
    ) => throw new NotFoundException(detail, title);

    [DoesNotReturn]
    protected static void BadRequest(string detail, string title = "Bad Request") =>
        throw new BadRequestException(detail, title);

    [DoesNotReturn]
    protected static void Unauthorized(string detail, string title = "Unauthorized") =>
        throw new UnauthorizedException(detail, title);

    [DoesNotReturn]
    protected static void Forbidden(string detail, string title = "Forbidden") =>
        throw new ForbiddenException(detail, title);
}
 */
