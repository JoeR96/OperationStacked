﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OperationStacked.Data;

#nullable disable

namespace OperationStacked.Migrations
{
    [DbContext(typeof(OperationStackedContext))]
    partial class OperationStackedContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("OperationStacked.Entities.EquipmentStack", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("EquipmentStackKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("IncrementCount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("IncrementValue")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("InitialIncrements")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("StartWeight")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("EquipmentStacks");
                });

            modelBuilder.Entity("OperationStacked.Entities.Exercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<int>("EquipmentType")
                        .HasColumnType("int");

                    b.Property<string>("ExerciseName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("OperationStacked.Entities.ExerciseHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CompletedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("CompletedReps")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("CompletedSets")
                        .HasColumnType("int");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("TemplateExerciseId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("TemplateExerciseId");

                    b.ToTable("ExerciseHistory");
                });

            modelBuilder.Entity("OperationStacked.Entities.LinearProgressionExercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("CurrentAttempt")
                        .HasColumnType("int");

                    b.Property<int>("LiftWeek")
                        .HasColumnType("int");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("char(36)");

                    b.Property<int>("WeightIndex")
                        .HasColumnType("int");

                    b.Property<decimal>("WorkingWeight")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("WorkoutExerciseId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("WorkoutExerciseId");

                    b.ToTable("LinearProgressionExercise");
                });

            modelBuilder.Entity("OperationStacked.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("OperationStacked.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<Guid>("CognitoUserId")
                        .HasMaxLength(255)
                        .HasColumnType("char(255)");

                    b.Property<int>("CurrentDay")
                        .HasColumnType("int");

                    b.Property<int>("CurrentWeek")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("WorkoutDaysInWeek")
                        .HasColumnType("int");

                    b.Property<int>("WorkoutWeeks")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OperationStacked.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("OperationStacked.Entities.Workout", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("WorkoutName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Workouts");
                });

            modelBuilder.Entity("OperationStacked.Entities.WorkoutExercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("AttemptsBeforeDeload")
                        .HasColumnType("int");

                    b.Property<bool>("Completed")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("EquipmentStackId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("char(36)");

                    b.Property<int>("LiftDay")
                        .HasColumnType("int");

                    b.Property<int>("LiftOrder")
                        .HasColumnType("int");

                    b.Property<int>("MaximumReps")
                        .HasColumnType("int");

                    b.Property<int>("MinimumReps")
                        .HasColumnType("int");

                    b.Property<int>("RestTimer")
                        .HasColumnType("int");

                    b.Property<int>("Sets")
                        .HasColumnType("int");

                    b.Property<int>("Template")
                        .HasColumnType("int");

                    b.Property<decimal>("WeightProgression")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid>("WorkoutId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("WorkoutExercises");
                });

            modelBuilder.Entity("OperationStacked.Entities.A2SHypertrophyExercise", b =>
                {
                    b.HasBaseType("OperationStacked.Entities.Exercise");

                    b.Property<int>("AmrapRepResult")
                        .HasColumnType("int");

                    b.Property<int>("AmrapRepTarget")
                        .HasColumnType("int");

                    b.Property<int>("Block")
                        .HasColumnType("int");

                    b.Property<decimal>("Intensity")
                        .HasColumnType("decimal(65,30)");

                    b.Property<bool>("PrimaryLift")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("RepsPerSet")
                        .HasColumnType("int");

                    b.Property<decimal>("RoundingValue")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Sets")
                        .HasColumnType("int");

                    b.Property<decimal>("TrainingMax")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Week")
                        .HasColumnType("int");

                    b.ToTable("A2SHypertrophyExercise");
                });

            modelBuilder.Entity("OperationStacked.Entities.ExerciseHistory", b =>
                {
                    b.HasOne("OperationStacked.Entities.Exercise", "Exercise")
                        .WithMany("ExerciseHistories")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OperationStacked.Entities.LinearProgressionExercise", "TemplateExercise")
                        .WithMany()
                        .HasForeignKey("TemplateExerciseId");

                    b.Navigation("Exercise");

                    b.Navigation("TemplateExercise");
                });

            modelBuilder.Entity("OperationStacked.Entities.LinearProgressionExercise", b =>
                {
                    b.HasOne("OperationStacked.Entities.WorkoutExercise", "WorkoutExercise")
                        .WithMany("LinearProgressionExercises")
                        .HasForeignKey("WorkoutExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkoutExercise");
                });

            modelBuilder.Entity("OperationStacked.Entities.UserRole", b =>
                {
                    b.HasOne("OperationStacked.Entities.Role", "Role")
                        .WithMany("UsersRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OperationStacked.Entities.User", "UserAccount")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("UserAccount");
                });

            modelBuilder.Entity("OperationStacked.Entities.WorkoutExercise", b =>
                {
                    b.HasOne("OperationStacked.Entities.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("OperationStacked.Entities.A2SHypertrophyExercise", b =>
                {
                    b.HasOne("OperationStacked.Entities.Exercise", null)
                        .WithOne()
                        .HasForeignKey("OperationStacked.Entities.A2SHypertrophyExercise", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OperationStacked.Entities.Exercise", b =>
                {
                    b.Navigation("ExerciseHistories");
                });

            modelBuilder.Entity("OperationStacked.Entities.Role", b =>
                {
                    b.Navigation("UsersRole");
                });

            modelBuilder.Entity("OperationStacked.Entities.User", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("OperationStacked.Entities.WorkoutExercise", b =>
                {
                    b.Navigation("LinearProgressionExercises");
                });
#pragma warning restore 612, 618
        }
    }
}
