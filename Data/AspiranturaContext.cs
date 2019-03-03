using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PGC.Models;
using PGC.Models.ViewModels;

namespace PGC.Data {
    public class AspiranturaContext : DbContext {
        public AspiranturaContext (DbContextOptions<AspiranturaContext> options) : base (options) { }

        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Knowledge> Knowledges { get; set; }
        public DbSet<Aspirant> Aspirants { get; set; }
        public DbSet<Prepod> Prepods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Department> Departments { get; set; }        
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<AspirantOrder> AspirantOrders { get; set; }
        public DbSet<DepartmentSpeciality> DepartmentSpecialities { get; set; }
        public DbSet<PrepodDepartment> PrepodDepartments { get; set; }
        public DbSet<StatusType> Statuses { get; set; }
        
        public DbSet<OrderType> Ordertypes { get; set; }

        // Для того, чтобы таблицы были в единственном числе
        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            // modelBuilder.Entity<Speciality> ().ToTable ("Speciality");
            // modelBuilder.Entity<Knowledge> ().ToTable ("Knowledge");
            // modelBuilder.Entity<Aspirant> ().ToTable ("Aspirant");
            // modelBuilder.Entity<Prepod> ().ToTable ("Prepod");
            // modelBuilder.Entity<Order> ().ToTable ("Order");
            // modelBuilder.Entity<Faculty> ().ToTable ("Faculty");
            // modelBuilder.Entity<Department> ().ToTable ("Department");
            // modelBuilder.Entity<StatusType> ().ToTable ("StatusType");
            // modelBuilder.Entity<Position> ().ToTable ("Position");
            // modelBuilder.Entity<OrderType> ().ToTable ("OrderType");            
            // modelBuilder.Entity<AspirantOrder> ().ToTable ("AspirantOrder");
            // modelBuilder.Entity<DepartmentSpeciality> ().ToTable ("DepartmentSpeciality");
            // modelBuilder.Entity<PrepodDepartment> ().ToTable ("PrepodDepartment");
            
            // табл связи многие ко многим
            modelBuilder.Entity<AspirantOrder> ().HasKey (c => new { c.AspirantId, c.OrderId }); // настраивает составной первичный ключ
            modelBuilder.Entity<DepartmentSpeciality> ().HasKey (c => new { c.DepartmentId, c.SpecialityId }); // настраивает составной первичный ключ
            modelBuilder.Entity<PrepodDepartment> ().HasKey (c => new { c.DepartmentId, c.PrepodId }); // настраивает составной первичный ключ

            modelBuilder.Entity<AspirantOrder> ().HasKey (c => new { c.AspirantId, c.OrderId }); // настраивает составной первичный ключ
        }       
    }
}