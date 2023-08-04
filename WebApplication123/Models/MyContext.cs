using Microsoft.EntityFrameworkCore;
namespace Wing1.Models;

public class MyContext:DbContext{
    public MyContext(DbContextOptions<MyContext>options):base(options){}

    public DbSet<Users>? Users{get; set;}
    public DbSet<Kintai>? Kintai{get; set;}
    public DbSet<Details>? Details{get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //複合キーはHasKeyで設定
        modelBuilder.Entity<Kintai>().HasKey(e => new { e.Userid, e.Date });
        modelBuilder.Entity<Details>().HasKey(e => new { e.Userid, e.Date });
        //modelBuilder.Entity<Users>().HasKey(e => e.Userid);
    }
}