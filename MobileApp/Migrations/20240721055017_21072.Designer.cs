﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MobileApp.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MobileApp.Migrations
{
    [DbContext(typeof(MobileDbContext))]
    [Migration("20240721055017_21072")]
    partial class _21072
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MobileApp.Entities.UserApp", b =>
                {
                    b.Property<string>("phone")
                        .HasColumnType("text");

                    b.Property<string>("Family")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Fathers")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Sex_Person")
                        .HasColumnType("text");

                    b.Property<int?>("Sex_idPerson")
                        .HasColumnType("integer");

                    b.Property<DateTime>("birthDayPerson")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("snils")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("phone");

                    b.ToTable("UsersApp");
                });
#pragma warning restore 612, 618
        }
    }
}
