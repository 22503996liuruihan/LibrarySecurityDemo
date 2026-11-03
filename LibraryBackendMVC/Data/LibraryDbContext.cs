using LibraryBackendMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBackendMVC.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Borrowal> Borrowals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 配置外键关系
        modelBuilder.Entity<Borrowal>()
            .HasOne(b => b.Member)
            .WithMany(m => m.Borrowals)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Borrowal>()
            .HasOne(b => b.Item)
            .WithMany(i => i.Borrowals)
            .HasForeignKey(b => b.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // 种子数据：管理员账号（暂时注释，避免主键冲突）
        /*
        modelBuilder.Entity<Member>().HasData(
            new Member
            {
                Id = 1,
                UserName = "admin",
                Password = "123456",
                Email = "admin@library.com",
                Role = "Admin",
                CreateTime = new DateTime(2026, 11, 3)
            }
        );
        */
    }
}