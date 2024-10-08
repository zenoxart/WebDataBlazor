﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebData.Backend;

#nullable disable

namespace WebData.Backend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240814071510_UserRolesAndTasks")]
    partial class UserRolesAndTasks
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.6.24327.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebData.Objects.PageContext.Objs.UserObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordIV")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebData.Objects.PageContext.Objs.UserTasks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserTasks");
                });

            modelBuilder.Entity("WebData.Objects.PageContext.Objs.ZugewieseneAufgabe", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("UserTaskId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "UserTaskId");

                    b.HasIndex("UserTaskId");

                    b.ToTable("ZugewieseneAufgaben");
                });

            modelBuilder.Entity("WebData.Objects.PageContext.Objs.ZugewieseneAufgabe", b =>
                {
                    b.HasOne("WebData.Objects.PageContext.Objs.UserObject", "User")
                        .WithMany("UserAssignedTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebData.Objects.PageContext.Objs.UserTasks", "UserTask")
                        .WithMany("UserAssignedTasks")
                        .HasForeignKey("UserTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("UserTask");
                });

            modelBuilder.Entity("WebData.Objects.PageContext.Objs.UserObject", b =>
                {
                    b.Navigation("UserAssignedTasks");
                });

            modelBuilder.Entity("WebData.Objects.PageContext.Objs.UserTasks", b =>
                {
                    b.Navigation("UserAssignedTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
