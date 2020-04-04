﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using sketch_mar21a;

namespace sketch_mar21a.Migrations
{
    [DbContext(typeof(weatherContext))]
    [Migration("20200403214044_UserIdsLogicForServerView")]
    partial class UserIdsLogicForServerView
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("sketch_mar21a.Days", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Date");

                    b.Property<double?>("Humidity");

                    b.Property<double?>("Temperature");

                    b.HasKey("Id");

                    b.ToTable("Days");
                });

            modelBuilder.Entity("sketch_mar21a.Devices", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("Status");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("sketch_mar21a.DeviceStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Fan");

                    b.Property<bool>("HeatingFan");

                    b.HasKey("Id");

                    b.ToTable("DeviceStatus");
                });

            modelBuilder.Entity("sketch_mar21a.Hours", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Date");

                    b.Property<double?>("Humidity");

                    b.Property<double?>("Temperature");

                    b.HasKey("Id");

                    b.ToTable("Hours");
                });

            modelBuilder.Entity("sketch_mar21a.Meterings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("MeteringTypeId");

                    b.Property<int>("SensorId");

                    b.Property<int>("UserId");

                    b.Property<double>("Value");

                    b.HasKey("Id");

                    b.HasIndex("MeteringTypeId");

                    b.HasIndex("SensorId");

                    b.HasIndex("UserId");

                    b.ToTable("Meterings");
                });

            modelBuilder.Entity("sketch_mar21a.MeteringTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("MeteringTypes");
                });

            modelBuilder.Entity("sketch_mar21a.Minutes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Date");

                    b.Property<double?>("Humidity");

                    b.Property<double?>("Temperature");

                    b.HasKey("Id");

                    b.ToTable("Minutes");
                });

            modelBuilder.Entity("sketch_mar21a.Mounths", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Date");

                    b.Property<double?>("Humidity");

                    b.Property<double?>("Temperature");

                    b.HasKey("Id");

                    b.ToTable("Mounths");
                });

            modelBuilder.Entity("sketch_mar21a.Seconds", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnName("id");

                    b.Property<DateTime?>("Date")
                        .HasColumnName("date");

                    b.Property<double?>("Humidity")
                        .HasColumnName("humidity");

                    b.Property<double?>("Temperature")
                        .HasColumnName("temperature");

                    b.HasKey("Id");

                    b.ToTable("seconds");
                });

            modelBuilder.Entity("sketch_mar21a.Sensors", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("unit");

                    b.HasKey("Id");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("sketch_mar21a.UserDevices", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("DeviceId");

                    b.HasKey("UserId", "DeviceId");

                    b.HasIndex("DeviceId");

                    b.ToTable("UserDevices");
                });

            modelBuilder.Entity("sketch_mar21a.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("sketch_mar21a.UserSensors", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("SensorId");

                    b.HasKey("UserId", "SensorId");

                    b.HasIndex("SensorId");

                    b.ToTable("UserSensors");
                });

            modelBuilder.Entity("sketch_mar21a.Weeks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("Date");

                    b.Property<double?>("Humidity");

                    b.Property<double?>("Temperature");

                    b.HasKey("Id");

                    b.ToTable("Weeks");
                });

            modelBuilder.Entity("sketch_mar21a.Meterings", b =>
                {
                    b.HasOne("sketch_mar21a.MeteringTypes", "MeteringType")
                        .WithMany()
                        .HasForeignKey("MeteringTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("sketch_mar21a.Sensors", "Sensor")
                        .WithMany()
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("sketch_mar21a.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("sketch_mar21a.UserDevices", b =>
                {
                    b.HasOne("sketch_mar21a.Devices", "Devices")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("sketch_mar21a.Users", "Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("sketch_mar21a.UserSensors", b =>
                {
                    b.HasOne("sketch_mar21a.Sensors", "Sensors")
                        .WithMany()
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("sketch_mar21a.Users", "Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
