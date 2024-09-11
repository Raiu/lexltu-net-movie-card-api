namespace Api.Data;

public class UnitOfWork(ApiDbContext dbContext) : IUnitOfWork
{
    private readonly ApiDbContext _db = dbContext;

    public void Dispose() => _db.Dispose();

    public int SaveChanges() => _db.SaveChanges();

    public async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();
}

public interface IUnitOfWork : IDisposable
{
    int SaveChanges();

    Task<int> SaveChangesAsync();
}
