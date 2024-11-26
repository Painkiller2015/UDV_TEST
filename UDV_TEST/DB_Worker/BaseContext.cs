using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;


namespace UDV_TEST.DB_Worker
{
    public class BaseContext : DbContext
    {
        private const string _dbName = "sqlite.db3";
        private readonly string _dbFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" +_dbName;

        public BaseContext()
        {
        }
        public BaseContext(DbContextOptions<BaseContext> options)
        : base(options)
        {            

        }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<ChatHistory> ChatHistories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string _connectionString = $"Data Source={_dbFile}";

            base.OnConfiguring(optionsBuilder);            
            optionsBuilder.UseSqlite(_connectionString);            

            CheckDatabase(_dbFile);                        
        }        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);

                entity.ToTable("Chats");
            });

            modelBuilder.Entity<ChatHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);

                entity.HasOne(d => d.Chat).WithMany(p => p.Messages).HasForeignKey(e => e.Id_Chat).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable("ChatHistory");
            });
        }
        private void CheckDatabase(string path)
        {
            if (!File.Exists(path))
                File.WriteAllBytes(path, Properties.Resources.sqlite);                        
        }
    }
}
