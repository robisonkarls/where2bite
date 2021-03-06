﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WhereToBite.Infrastructure;

namespace WhereToBite.Infrastructure.Migrations
{
    [DbContext(typeof(WhereToBiteContext))]
    [Migration("20201103195354_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Establishment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("DineSafeId")
                        .HasColumnType("integer");

                    b.Property<string>("Latitude")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Longitude")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("_establishmentStatusId")
                        .HasColumnName("EstablishmentStatusId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DineSafeId")
                        .IsUnique();

                    b.HasIndex("_establishmentStatusId");

                    b.ToTable("Establishments","w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.EstablishmentStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("EstablishmentStatus","w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Infraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("AmountFined")
                        .HasColumnType("numeric");

                    b.Property<string>("CourtOutcome")
                        .HasColumnType("text");

                    b.Property<int?>("InspectionId")
                        .HasColumnType("integer");

                    b.Property<int>("_infractionActionId")
                        .HasColumnName("InfractionActionId")
                        .HasColumnType("integer");

                    b.Property<int>("_severityId")
                        .HasColumnName("SeverityId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("InspectionId");

                    b.HasIndex("_infractionActionId");

                    b.HasIndex("_severityId");

                    b.ToTable("Infraction","w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.InfractionAction", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("InfractionAction","w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Inspection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("EstablishmentId")
                        .HasColumnType("integer");

                    b.Property<int>("_inspectionStatusId")
                        .HasColumnName("InspectionStatusId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EstablishmentId");

                    b.HasIndex("_inspectionStatusId");

                    b.ToTable("Inspection","w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.InspectionStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("InspectionStatus","w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Severity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Severity","w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Establishment", b =>
                {
                    b.HasOne("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.EstablishmentStatus", "EstablishmentStatus")
                        .WithMany()
                        .HasForeignKey("_establishmentStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Infraction", b =>
                {
                    b.HasOne("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Inspection", null)
                        .WithMany("Infractions")
                        .HasForeignKey("InspectionId");

                    b.HasOne("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.InfractionAction", "InfractionAction")
                        .WithMany()
                        .HasForeignKey("_infractionActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Severity", "Severity")
                        .WithMany()
                        .HasForeignKey("_severityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Inspection", b =>
                {
                    b.HasOne("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Establishment", null)
                        .WithMany("Inspections")
                        .HasForeignKey("EstablishmentId");

                    b.HasOne("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.InspectionStatus", "InspectionStatus")
                        .WithMany()
                        .HasForeignKey("_inspectionStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
