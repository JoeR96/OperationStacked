﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OperationStacked.Data;

#nullable disable

namespace OperationStacked.Migrations
{
    [DbContext(typeof(OperationStackedContext))]
    [Migration("20230529201919_new884324")]
    partial class new884324
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Completed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("EquipmentType")
                        .HasColumnType("int");

                    b.Property<string>("ExerciseName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("LiftDay")
                        .HasColumnType("int");

                    b.Property<int>("LiftOrder")
                        .HasColumnType("int");

                    b.Property<int>("LiftWeek")
                        .HasColumnType("int");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Template")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("WorkingWeight")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("Exercises");
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

            modelBuilder.Entity("OperationStacked.Entities.ToDo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Completed")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("CompletedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("ToDos");
                });

            modelBuilder.Entity("OperationStacked.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CognitoUserId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

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

            modelBuilder.Entity("OperationStacked.Entities.LinearProgressionExercise", b =>
                {
                    b.HasBaseType("OperationStacked.Entities.Exercise");

                    b.Property<int>("AttemptsBeforeDeload")
                        .HasColumnType("int");

                    b.Property<int>("CurrentAttempt")
                        .HasColumnType("int");

                    b.Property<int>("CurrentSets")
                        .HasColumnType("int");

                    b.Property<Guid>("EquipmentStackId")
                        .HasColumnType("char(36)");

                    b.Property<int>("EquipmentStackKey")
                        .HasColumnType("int");

                    b.Property<int>("MaximumReps")
                        .HasColumnType("int");

                    b.Property<int>("MinimumReps")
                        .HasColumnType("int");

                    b.Property<bool>("PrimaryExercise")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("StartingSets")
                        .HasColumnType("int");

                    b.Property<decimal>("StartingWeight")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("TargetSets")
                        .HasColumnType("int");

                    b.Property<int>("WeightIndex")
                        .HasColumnType("int");

                    b.Property<decimal>("WeightProgression")
                        .HasColumnType("decimal(65,30)");

                    b.HasIndex("EquipmentStackId");

                    b.ToTable("LinearProgressionExercise");
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

            modelBuilder.Entity("OperationStacked.Entities.A2SHypertrophyExercise", b =>
                {
                    b.HasOne("OperationStacked.Entities.Exercise", null)
                        .WithOne()
                        .HasForeignKey("OperationStacked.Entities.A2SHypertrophyExercise", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OperationStacked.Entities.LinearProgressionExercise", b =>
                {
                    b.HasOne("OperationStacked.Entities.EquipmentStack", "EquipmentStack")
                        .WithMany()
                        .HasForeignKey("EquipmentStackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OperationStacked.Entities.Exercise", null)
                        .WithOne()
                        .HasForeignKey("OperationStacked.Entities.LinearProgressionExercise", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EquipmentStack");
                });

            modelBuilder.Entity("OperationStacked.Entities.Role", b =>
                {
                    b.Navigation("UsersRole");
                });

            modelBuilder.Entity("OperationStacked.Entities.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
