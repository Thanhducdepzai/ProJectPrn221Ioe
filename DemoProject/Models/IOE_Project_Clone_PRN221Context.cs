using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectIoePrn.Models

{
    public partial class IOE_Project_Clone_PRN221Context : DbContext
    {
        public IOE_Project_Clone_PRN221Context()
        {
        }

        public IOE_Project_Clone_PRN221Context(DbContextOptions<IOE_Project_Clone_PRN221Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<District> Districts { get; set; } = null!;
        public virtual DbSet<Grade> Grades { get; set; } = null!;
        public virtual DbSet<IndividualResultDetail> IndividualResultDetails { get; set; } = null!;
        public virtual DbSet<LevelOfSchool> LevelOfSchools { get; set; } = null!;
        public virtual DbSet<Part> Parts { get; set; } = null!;
        public virtual DbSet<PresentPartResultDetail> PresentPartResultDetails { get; set; } = null!;
        public virtual DbSet<Province> Provinces { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Round> Rounds { get; set; } = null!;
        public virtual DbSet<School> Schools { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<TypeOfQuestion> TypeOfQuestions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("value")); }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");
                entity.Property(e => e.AdminId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("admin_id");

                entity.Property(e => e.AdminDob)
                    .HasColumnType("date")
                    .HasColumnName("admin_dob");

                entity.Property(e => e.AdminGmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("admin_gmail")
                    .IsFixedLength();

                entity.Property(e => e.AdminName)
                    .HasMaxLength(50)
                    .HasColumnName("admin_name");

                entity.Property(e => e.AdminPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("admin_password")
                    .IsFixedLength();

                entity.Property(e => e.AdminUsername)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("admin_username")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => e.AnwserId)
                    .HasName("PK__Answer__4B78FF676C4C43AF");

                entity.ToTable("Answer");

                entity.Property(e => e.AnwserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("anwser_id ");


                entity.Property(e => e.AnswerText)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("answer_text")
                    .IsFixedLength();

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer.question_id");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.DistricId)

                    .HasName("PK__District__17728AEE2BB99170");

                entity.ToTable("District");

                entity.Property(e => e.DistricId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("distric_id");

                entity.Property(e => e.DistricName)
                    .HasMaxLength(50)
                    .HasColumnName("distric_name");

                entity.Property(e => e.ProvinceId).HasColumnName("province_id");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_District.province_id");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grade");


                entity.Property(e => e.GradeId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("grade_id");

                entity.Property(e => e.GradeName)
                    .HasMaxLength(50)
                    .HasColumnName("grade_name");

                entity.Property(e => e.LevelSchoolId).HasColumnName("levelSchool_id");

                entity.HasOne(d => d.LevelSchool)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.LevelSchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grade.levelSchool_id");
            });

            modelBuilder.Entity<IndividualResultDetail>(entity =>
            {
                entity.HasKey(e => e.IndividualResultId)

                    .HasName("PK__Individu__003B1E2ED4FC3639");

                entity.ToTable("Individual Result Detail");

                entity.Property(e => e.IndividualResultId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("individual_result_id");

                entity.Property(e => e.CompleteTime).HasColumnName("complete_time");

                entity.Property(e => e.RoundId).HasColumnName("round_id");

                entity.Property(e => e.RoundScore).HasColumnName("round_score");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Round)
                    .WithMany(p => p.IndividualResultDetails)
                    .HasForeignKey(d => d.RoundId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Individual Result Detail.round_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.IndividualResultDetails)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Individual Result Detail.user_id");
            });

            modelBuilder.Entity<LevelOfSchool>(entity =>
            {
                entity.HasKey(e => e.LevelSchoolId)

                    .HasName("PK__Level of__63E29240346AEA9E");

                entity.ToTable("Level of school");

                entity.Property(e => e.LevelSchoolId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("levelSchool_id");

                entity.Property(e => e.LevelName)
                    .HasMaxLength(50)
                    .HasColumnName("levelName");
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.ToTable("Part");


                entity.Property(e => e.PartId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("part_id");

                entity.Property(e => e.PartCreateDate)
                    .HasColumnType("date")
                    .HasColumnName("part_create_date");

                entity.Property(e => e.PartName)
                    .HasMaxLength(50)
                    .HasColumnName("part_name");

                entity.Property(e => e.PartOrder).HasColumnName("part_order");

                entity.Property(e => e.PartUpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("part_update_date");

                entity.Property(e => e.RoundId).HasColumnName("round_id");

                entity.Property(e => e.TypeOfQuestionId).HasColumnName("type_of_question_id");

                entity.HasOne(d => d.Round)
                    .WithMany(p => p.Parts)
                    .HasForeignKey(d => d.RoundId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Part.round_id");

                entity.HasOne(d => d.TypeOfQuestion)
                    .WithMany(p => p.Parts)
                    .HasForeignKey(d => d.TypeOfQuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Part.type_of_question_id");
            });

            modelBuilder.Entity<PresentPartResultDetail>(entity =>
            {
                entity.HasKey(e => e.PartResultDetailId)

                    .HasName("PK__Present __2008258DDF971C7A");

                entity.ToTable("Present Part Result Detail");

                entity.Property(e => e.PartResultDetailId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("part_result_detail_id ");

                entity.Property(e => e.CompleteTime).HasColumnName("complete_time");

                entity.Property(e => e.IndividualResultId).HasColumnName("individual_result_id");

                entity.Property(e => e.PartId).HasColumnName("part_id ");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.HasOne(d => d.IndividualResult)
                    .WithMany(p => p.PresentPartResultDetails)
                    .HasForeignKey(d => d.IndividualResultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Present Part Result Detail.individual_result_id");

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.PresentPartResultDetails)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Present Part Result Detail.part_id ");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.ToTable("Province");

                entity.Property(e => e.ProvinceId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("province_id");

                entity.Property(e => e.ProvinceName)
                    .HasMaxLength(150)
                    .HasColumnName("province_name");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("question_id ");

                entity.Property(e => e.PartId).HasColumnName("part_id");

                entity.Property(e => e.QuestionMetadata)
                    .HasMaxLength(200)
                    .HasColumnName("question_metadata")
                    .IsFixedLength();

                entity.Property(e => e.QuestionText)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("question_text ")
                    .IsFixedLength();

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question.part_id");
            });

            modelBuilder.Entity<Round>(entity =>
            {
                entity.ToTable("Round");

                entity.Property(e => e.RoundId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("round_id");

                entity.Property(e => e.GradeId).HasColumnName("grade_id");

                entity.Property(e => e.RoundCreateDate)
                    .HasColumnType("date")
                    .HasColumnName("round_create_date");

                entity.Property(e => e.RoundName)
                    .HasMaxLength(50)
                    .HasColumnName("round_name");

                entity.Property(e => e.RoundUpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("round_update_date");

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.Rounds)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Round.grade_id");
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.ToTable("School");
                entity.Property(e => e.SchoolId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("school_id");

                entity.Property(e => e.DistrictId).HasColumnName("district_id");

                entity.Property(e => e.LevelSchoolId).HasColumnName("levelSchool_id");

                entity.Property(e => e.SchoolName)
                    .HasMaxLength(50)
                    .HasColumnName("school_name");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Schools)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_School.district_id");

                entity.HasOne(d => d.LevelSchool)
                    .WithMany(p => p.Schools)
                    .HasForeignKey(d => d.LevelSchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_School.levelSchool_id");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");
                entity.Property(e => e.StudentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("student_id");

                entity.Property(e => e.GradeId).HasColumnName("grade_ID");

                entity.Property(e => e.SchoolId).HasColumnName("school_ID");

                entity.Property(e => e.StudentClass)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("student_class")
                    .IsFixedLength();

                entity.Property(e => e.StudentDob)
                    .HasColumnType("date")
                    .HasColumnName("student_dob");

                entity.Property(e => e.StudentGmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("student_gmail")
                    .IsFixedLength();

                entity.Property(e => e.StudentName)
                    .HasMaxLength(50)
                    .HasColumnName("student_name");

                entity.Property(e => e.StudentPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("student_password")
                    .IsFixedLength();

                entity.Property(e => e.StudentUsername)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("student_username")
                    .IsFixedLength();

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student.grade_ID");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student.school_ID");
            });

            modelBuilder.Entity<TypeOfQuestion>(entity =>
            {
                entity.HasKey(e => e.TypeOfQuestion1)
                    .HasName("PK__Type of __8D14B5FDDF98A526");

                entity.ToTable("Type of question");

                entity.Property(e => e.TypeOfQuestion1)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("type_of_question");

                entity.Property(e => e.TypeOfQuestionName)
                    .HasMaxLength(450)
                    .HasColumnName("type_of_question_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}