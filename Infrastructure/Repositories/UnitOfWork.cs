
using Microsoft.EntityFrameworkCore.Storage;
using UserApi.Application.Interfaces;
using UserApi.Infrastructure.Data;

namespace UserApi.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IUserRepository Users { get; }

    public UnitOfWork(AppDbContext context, IUserRepository users)
    {
        _context = context;
        Users = users;
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    //NEW TRANSACTION
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}