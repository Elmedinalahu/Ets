using Ets.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Ets.Data
{
    public class EtsDbContext : DbContext
    {
        public EtsDbContext(DbContextOptions<EtsDbContext> options) : base(options)
        {

        }
        public DbSet<Exams> Exams { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<ExamResults> ExamResults { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Questions>().HasData(SeedTestData());

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
                entity.Property(e => e.DateOfBirth);
                entity.Property(e => e.Gender).IsRequired().HasMaxLength(1);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
                entity.Property(e => e.DateOfBirth);
                entity.Property(e => e.Gender).IsRequired().HasMaxLength(1);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            });

            modelBuilder.Entity<Questions>(entity =>
            {
                entity.Property(e => e.Question).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Score).IsRequired();
                entity.Property(e => e.Option1).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Option2).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Option3).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Option4).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Answer).IsRequired();
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
            });

            modelBuilder.Entity<Exams>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Author).IsRequired();
            });

            modelBuilder.Entity<ExamResults>(entity =>
            {
                // When deleting a user, set its id as null in the Exam and Exam Requests Table(without deleting those records)
                entity.HasOne(e => e.Students).WithMany(e => e.ExamResults).HasForeignKey(e => e.StudentsId).OnDelete(DeleteBehavior.SetNull);

                //When deleting an exam if u created an additional table to map examid, questionid then delete records for that exam in the exam question table.
                entity.HasOne(e => e.Exams).WithMany(e => e.ExamResults).HasForeignKey(e => e.ExamsId).OnDelete(DeleteBehavior.Cascade);

            });

            //If user already took an exam do not allow to retake or make request.
            //modelBuilder.Entity<ExamResults>().HasIndex(e => new { e.StudentsId, e.ExamsId }).IsUnique();
            modelBuilder.Entity<ExamResults>().HasIndex(e => new { e.ExamsId, e.StudentsId, e.QuestionsId }).IsUnique();
        }

        public List<Questions> SeedTestData()
        {
            var fileName = DateTime.Now.Hour + ".json";
            var questions = new List<Questions>();
            using (StreamReader r = new StreamReader(@"Upload\\Files\\17.json"))
            {
                string json = r.ReadToEnd();
                questions = JsonConvert.DeserializeObject<List<Questions>>(json);
            }
            return questions;
        }
       
    }
}
