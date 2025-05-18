using CRM.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CRM.Infrastructure.Persistences.Context
{
    public partial class CrmContext : DbContext
    {
        private readonly string _connectionString;
        public CrmContext(string connectionString) {
            _connectionString = connectionString;   
        }

        public CrmContext(DbContextOptions<CrmContext> options) : base(options) { }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<DocumentType> DocumentTypes { get; set; } = null!;

        public virtual DbSet<ActivePause> ActivePause { get; set; } = null!;
        public virtual DbSet<Company> Company { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;

        public virtual DbSet<Role> Roles { get; set; }= null!;
        public virtual DbSet<User> Users { get; set; } = null !;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;

        public Task<IQueryable<T>> ExecuteStoredProcedure<T>(string procedureName, params SqlParameter[] parameters)
        {
            // Build SQL command with placeholders for parameters
            var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
            var sqlCommand = $"EXEC {procedureName} {parameterNames}";

            // Ejecutar el procedimiento almacenado y mapear los resultados al tipo genérico T
            var query = Database
                .SqlQueryRaw<T>(sqlCommand, parameters);

            return Task.FromResult(query.AsQueryable());
        }

        public void ExecuteStoredProcedureAsync(string procedureName, params SqlParameter[] parameters)
        {
            Database.ExecuteSqlRawAsync($"EXEC {procedureName}", parameters);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational : Collation", "Modern_Spanish_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
