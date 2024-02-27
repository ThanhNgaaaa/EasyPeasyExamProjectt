using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EasyPeasyExam.Models;

public partial class EasyPeasyExamContext : DbContext
{
    public EasyPeasyExamContext()
    {
    }

    public EasyPeasyExamContext(DbContextOptions<EasyPeasyExamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseCategory> CourseCategories { get; set; }

    public virtual DbSet<CourseEnrollment> CourseEnrollments { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamResult> ExamResults { get; set; }

    public virtual DbSet<FlashCardTopic> FlashCardTopics { get; set; }

    public virtual DbSet<Flashcard> Flashcards { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__Answers__D4825024F18EA45F");

            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.ExamId).HasColumnName("ExamID");

            entity.HasOne(d => d.Exam).WithMany(p => p.Answers)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK__Answers__ExamID__59FA5E80");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__Answers__Questio__5AEE82B9");
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasKey(e => e.ChoiceId).HasName("PK__Choices__76F5168622C1947F");

            entity.Property(e => e.ChoiceId).HasColumnName("ChoiceID");
            entity.Property(e => e.ImageChoice)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

            entity.HasOne(d => d.Question).WithMany(p => p.Choices)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__Choices__Questio__4F7CD00D");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAA2B81E72A");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentDate).HasColumnType("datetime");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__Course__5DCAEF64");

            entity.HasOne(d => d.Student).WithMany(p => p.Comments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comments__Studen__5EBF139D");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D7187839B651C");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CourseImage)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CourseName).HasMaxLength(255);
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

            entity.HasOne(d => d.Category).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Courses__Categor__4222D4EF");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Courses__Instruc__412EB0B6");
        });

        modelBuilder.Entity<CourseCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__CourseCa__19093A2BB358FEBE");

            entity.ToTable("CourseCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<CourseEnrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PK__CourseEn__7F6877FBFB88F62A");

            entity.Property(e => e.EnrollmentId).HasColumnName("EnrollmentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseEnrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CourseEnr__Cours__45F365D3");

            entity.HasOne(d => d.Student).WithMany(p => p.CourseEnrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CourseEnr__Stude__44FF419A");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__Document__1ABEEF6F5A398CC0");

            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.Content).HasColumnType("ntext");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Image)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.LessonId).HasColumnName("LessonID");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Lesson).WithMany(p => p.Documents)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK__Documents__Lesso__29221CFB");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__Exams__297521A7218D2CD8");

            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.ExamDescription).HasMaxLength(255);
            entity.Property(e => e.ExamImage)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Instructor).WithMany(p => p.Exams)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Exams__Instructo__49C3F6B7");
        });

        modelBuilder.Entity<ExamResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__ExamResu__97690228F6AEC8C8");

            entity.ToTable("ExamResult");

            entity.Property(e => e.ResultId).HasColumnName("ResultID");
            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.TotalPoint).HasColumnType("decimal(2, 1)");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamResults)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK__ExamResul__ExamI__5629CD9C");

            entity.HasOne(d => d.Student).WithMany(p => p.ExamResults)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__ExamResul__Stude__571DF1D5");
        });

        modelBuilder.Entity<FlashCardTopic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FlashCar__3214EC272C20E4FA");

            entity.ToTable("FlashCardTopic");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TopicName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.FlashCardTopics)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserTopic");
        });

        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Flashcar__3214EC2765AA5DDB");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Content).HasColumnType("ntext");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Topic).WithMany(p => p.Flashcards)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("FK_FlashcardTopic");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACB0738D3E75");

            entity.Property(e => e.LessonId).HasColumnName("LessonID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.LessonTitle).HasMaxLength(255);
            entity.Property(e => e.LessonType).HasMaxLength(100);

            entity.HasOne(d => d.Course).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Lessons__CourseI__160F4887");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06F8CC55E3253");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.Point).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.QuestionImage)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.VideoLink)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Exam).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Questions__Video__4CA06362");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating__FCCDF85C8C926128");

            entity.ToTable("Rating");

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.RatingStar).HasColumnType("decimal(2, 1)");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Rating__UserID__619B8048");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__RefreshT__658FEEEA84EE3FDA");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.Token).IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__RefreshTo__UserI__02FC7413");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A65AC31E3");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC13D67534");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash).IsUnicode(false);
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("PK__UserRole__AF27604F5AA057DB");

            entity.ToTable("UserRole");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRole__RoleID__3A81B327");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRole__UserID__3B75D760");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__Videos__BAE5124A6B8CACF4");

            entity.Property(e => e.VideoId).HasColumnName("VideoID");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(10);
            entity.Property(e => e.LessonId).HasColumnName("LessonID");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Lesson).WithMany(p => p.Videos)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK__Videos__LessonID__18EBB532");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
