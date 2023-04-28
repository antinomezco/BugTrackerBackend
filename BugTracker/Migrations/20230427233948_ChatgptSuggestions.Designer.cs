﻿// <auto-generated />
using System;
using BugTracker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BugTracker.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230427233948_ChatgptSuggestions")]
    partial class ChatgptSuggestions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BugTracker.Entity.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Personnel");
                });

            modelBuilder.Entity("BugTracker.Entity.PersonProject", b =>
                {
                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("PersonId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("PersonnelProjects");
                });

            modelBuilder.Entity("BugTracker.Entity.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("BugTracker.Entity.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("TicketPriority")
                        .HasColumnType("int");

                    b.Property<int>("TicketStatus")
                        .HasColumnType("int");

                    b.Property<int>("TicketType")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("BugTracker.Entity.TicketPerson", b =>
                {
                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.Property<int?>("PersonId1")
                        .HasColumnType("int");

                    b.Property<int?>("TicketId1")
                        .HasColumnType("int");

                    b.Property<bool>("isSubmitter")
                        .HasColumnType("bit");

                    b.HasKey("PersonId", "TicketId");

                    b.HasIndex("PersonId1");

                    b.HasIndex("TicketId");

                    b.HasIndex("TicketId1")
                        .IsUnique()
                        .HasFilter("[TicketId1] IS NOT NULL");

                    b.ToTable("TicketsPersonnel");
                });

            modelBuilder.Entity("BugTracker.Entity.PersonProject", b =>
                {
                    b.HasOne("BugTracker.Entity.Person", "Person")
                        .WithMany("PersonnelProjects")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BugTracker.Entity.Project", "Project")
                        .WithMany("PersonnelProjects")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BugTracker.Entity.Ticket", b =>
                {
                    b.HasOne("BugTracker.Entity.Project", "Project")
                        .WithMany("Tickets")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BugTracker.Entity.TicketPerson", b =>
                {
                    b.HasOne("BugTracker.Entity.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BugTracker.Entity.Person", null)
                        .WithMany("TicketsPeople")
                        .HasForeignKey("PersonId1");

                    b.HasOne("BugTracker.Entity.Ticket", "Ticket")
                        .WithMany("AssignedPeople")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BugTracker.Entity.Ticket", null)
                        .WithOne("SubmitterPerson")
                        .HasForeignKey("BugTracker.Entity.TicketPerson", "TicketId1");

                    b.Navigation("Person");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("BugTracker.Entity.Person", b =>
                {
                    b.Navigation("PersonnelProjects");

                    b.Navigation("TicketsPeople");
                });

            modelBuilder.Entity("BugTracker.Entity.Project", b =>
                {
                    b.Navigation("PersonnelProjects");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("BugTracker.Entity.Ticket", b =>
                {
                    b.Navigation("AssignedPeople");

                    b.Navigation("SubmitterPerson");
                });
#pragma warning restore 612, 618
        }
    }
}
