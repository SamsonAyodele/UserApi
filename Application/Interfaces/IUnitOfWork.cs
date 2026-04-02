using Microsoft.EntityFrameworkCore.Storage;


namespace UserApi.Application.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    Task<int> SaveChangesAsync();

    //NEW TRANSACTION
    Task<IDbContextTransaction> BeginTransactionAsync();
}