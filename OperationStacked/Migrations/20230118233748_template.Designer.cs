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
    [Migration("20230118233748_template")]
    partial class template
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("OperationStacked.Entities.Exercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

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

                    b.Property<int>("Template")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("WorkingWeight")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("Exercises");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Exercise");
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

                    b.Property<int>("CurrentDay")
                        .HasColumnType("int");

                    b.Property<int>("CurrentWeek")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

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

                    b.Property<bool>("AuxillaryLift")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Block")
                        .HasColumnType("int");

                    b.Property<decimal>("Intensity")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("RepsPerSet")
                        .HasColumnType("int");

                    b.Property<int>("Sets")
                        .HasColumnType("int");

                    b.Property<decimal>("TrainingMax")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Week")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("A2SHypertrophyExercise");
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

                    b.Property<int>("MaximumReps")
                        .HasColumnType("int");

                    b.Property<int>("MinimumReps")
                        .HasColumnType("int");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("char(36)");

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

                    b.HasDiscriminator().HasValue("LinearProgressionExercise");
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
