using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Context;
using CRM.Infrastructure.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CRM.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly CrmContext _context;
        private readonly IConfiguration _configuration;

        public IGenericRepository<DocumentType> _documentType = null!;
        public IGenericRepository<Client> _client = null!;
        public IGenericRepository<Company> _company = null!;
        public IGenericRepository<ActivePause> _activePause = null!;
        public IReservationRepository _reservation = null!;
        public IUserRepository _user = null!;
        public IEmailRepository _emailRepository = null!;

        public UnitOfWork(CrmContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IGenericRepository<DocumentType> DocumentType => _documentType ?? new GenericRepository<DocumentType>(_context);
        public IGenericRepository<Client> Client => _client ?? new GenericRepository<Client>(_context);
        public IGenericRepository<Company> Company => _company ?? new GenericRepository<Company>(_context);
        public IGenericRepository<ActivePause> ActivePause => _activePause ?? new GenericRepository<ActivePause>(_context);
        public IReservationRepository Reservation => _reservation ?? new ReservationRepository(_context);
        public IUserRepository User => _user ?? new UserRepository(_context);
        public IEmailRepository EmailRepository => _emailRepository ?? new EmailRepository(_configuration);


        public DbContext Context => _context;


        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
