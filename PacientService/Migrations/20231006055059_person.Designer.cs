﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PacientService.Data;

#nullable disable

namespace PacientService.Migrations
{
    [DbContext(typeof(PacientDbContext))]
    [Migration("20231006055059_person")]
    partial class person
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PacientService.Entities.Person", b =>
                {
                    b.Property<int>("IDALL")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IDALL"));

                    b.Property<DateTime>("DataCreatePerson")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateChangePerson")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FamilyPerson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FathersPerson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("IdPromedPerson")
                        .HasColumnType("integer");

                    b.Property<string>("Inn_Person")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NamePerson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("PersonLink")
                        .HasColumnType("uuid");

                    b.Property<string>("PhonePerson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Sex_Person")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Sex_idPerson")
                        .HasColumnType("integer");

                    b.Property<string>("SnilsPerson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SocStatus_Person")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SocStatus_id_Person")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("birthDayPerson")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("IDALL");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("PacientService.Entities.Setups", b =>
                {
                    b.Property<int>("IDALL")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IDALL"));

                    b.Property<string>("NamenRus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Namenastr")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ValueString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IDALL");

                    b.ToTable("Setups");

                    b.HasData(
                        new
                        {
                            IDALL = 1,
                            NamenRus = "Url для сервиса пациенты",
                            Namenastr = "URL_PACIENT",
                            ValueString = "http://localhost:39289/api/Pacient"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
