using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CRM.Infrastructure.Persistences.Interfaces
{
    public interface  IUnitOfWork : IDisposable // = Proveer un mecanismo para liberar objetos en memoria
    {
        IGenericRepository<Client> Client { get; }
        IGenericRepository<DocumentType> DocumentType { get; }
        IGenericRepository<Company> Company { get; }
        IGenericRepository<ActivePause> ActivePause { get; }
        IReservationRepository Reservation { get; }
        IUserRepository User { get; }
        IEmailRepository EmailRepository { get; }
        DbContext Context { get; }

        void SaveChanges();
        Task SaveChangesAsync();

        IDbTransaction BeginTransaction();
        Task CompleteAsync(); // Método para completar transacciones de manera asincrónica
    }
}
