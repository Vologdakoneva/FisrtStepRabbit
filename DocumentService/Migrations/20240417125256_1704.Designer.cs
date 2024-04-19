﻿// <auto-generated />
using System;
using DocumentService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocumentService.Migrations
{
    [DbContext(typeof(DocumentDbContext))]
    [Migration("20240417125256_1704")]
    partial class _1704
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DocumentService.Entities.DocAnaliz", b =>
                {
                    b.Property<int>("IDALL")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IDALL"));

                    b.Property<string>("AnalizHead")
                        .HasColumnType("text");

                    b.Property<DateTime>("DataChange")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Databiomaterial")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Datadoc")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("DocLink")
                        .HasColumnType("uuid");

                    b.Property<string>("Errors")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fio")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FioDoctor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FioDoctorkey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fiokey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("IdDoctor")
                        .HasColumnType("bigint");

                    b.Property<long?>("IdFio")
                        .HasColumnType("bigint");

                    b.Property<string>("Items")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NomDoc")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Recomedation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UetHead")
                        .HasColumnType("text");

                    b.Property<bool>("successfully")
                        .HasColumnType("boolean");

                    b.HasKey("IDALL");

                    b.HasIndex("DocLink");

                    b.ToTable("DocAnaliz");
                });

            modelBuilder.Entity("DocumentService.Entities.UserTasks", b =>
                {
                    b.Property<int>("IDALL")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IDALL"));

                    b.Property<DateTime?>("DataFinish")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DataTask")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DataTaskPlan")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("DocLink")
                        .HasColumnType("uuid");

                    b.Property<string>("Fio")
                        .HasColumnType("text");

                    b.Property<string>("FioExec")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FioFinish")
                        .HasColumnType("text");

                    b.Property<string>("Fiokey")
                        .HasColumnType("text");

                    b.Property<long?>("IdFio")
                        .HasColumnType("bigint");

                    b.Property<string>("PriorityTask")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TextFinish")
                        .HasColumnType("text");

                    b.Property<string>("TextTask")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<string>("errors")
                        .HasColumnType("text");

                    b.Property<string>("ownertask")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("successfully")
                        .HasColumnType("boolean");

                    b.Property<string>("telegram")
                        .HasColumnType("text");

                    b.Property<bool?>("usemail")
                        .HasColumnType("boolean");

                    b.Property<bool?>("usetelegram")
                        .HasColumnType("boolean");

                    b.HasKey("IDALL");

                    b.HasIndex("DocLink");

                    b.ToTable("UserTask");
                });
#pragma warning restore 612, 618
        }
    }
}
