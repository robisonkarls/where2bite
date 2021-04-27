﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WhereToBite.Infrastructure;

namespace WhereToBite.Infrastructure.Migrations
{
    [DbContext(typeof(WhereToBiteContext))]
    [Migration("20210427173846_AddingEstablishmentsConfiguration")]
    partial class AddingEstablishmentsConfiguration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("postgis")
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Establishment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("DineSafeId")
                        .HasColumnType("integer");

                    b.Property<Point>("Location")
                        .HasColumnType("geography (point)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("_establishmentStatusId")
                        .HasColumnType("integer")
                        .HasColumnName("EstablishmentStatusId");

                    b.HasKey("Id");

                    b.HasIndex("DineSafeId")
                        .IsUnique();

                    b.ToTable("Establishments", "w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.EstablishmentStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("EstablishmentStatus", "w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Infraction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<decimal>("AmountFined")
                        .HasColumnType("numeric");

                    b.Property<string>("CourtOutcome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("InspectionId")
                        .HasColumnType("integer");

                    b.Property<int>("_infractionActionId")
                        .HasColumnType("integer")
                        .HasColumnName("InfractionActionId");

                    b.Property<int>("_severityId")
                        .HasColumnType("integer")
                        .HasColumnName("SeverityId");

                    b.HasKey("Id");

                    b.HasIndex("InspectionId");

                    b.ToTable("Infraction", "w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.InfractionAction", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("InfractionAction", "w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Inspection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("EstablishmentId")
                        .HasColumnType("integer");

                    b.Property<int>("_inspectionStatusId")
                        .HasColumnType("integer")
                        .HasColumnName("InspectionStatusId");

                    b.HasKey("Id");

                    b.HasIndex("EstablishmentId");

                    b.ToTable("Inspection", "w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.InspectionStatus", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("InspectionStatus", "w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Severity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Severity", "w2b");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Infraction", b =>
                {
                    b.HasOne("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Inspection", null)
                        .WithMany("Infractions")
                        .HasForeignKey("InspectionId");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Inspection", b =>
                {
                    b.HasOne("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Establishment", null)
                        .WithMany("Inspections")
                        .HasForeignKey("EstablishmentId");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Establishment", b =>
                {
                    b.Navigation("Inspections");
                });

            modelBuilder.Entity("WhereToBite.Domain.AggregatesModel.EstablishmentAggregate.Inspection", b =>
                {
                    b.Navigation("Infractions");
                });
#pragma warning restore 612, 618
        }
    }
}