﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZoomOpera.Server.Data;

#nullable disable

namespace ZoomOpera.Shared.Migrations
{
    [DbContext(typeof(ZoomOperaContext))]
    [Migration("20220805121136_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Building", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BuildingCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.ImageMap", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DetailedDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ImageMapShape")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OperaImageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OperaImageId");

                    b.ToTable("ImageMaps");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.ImageMapCoordinate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ImageMapId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImageMapId");

                    b.ToTable("ImageMapCoordinates");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Level", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BuildingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LevelNumber")
                        .HasColumnType("int");

                    b.Property<string>("Planimetry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LevelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LocationCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LevelId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.MonitorPlatform", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LevelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MonitorCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LevelId");

                    b.ToTable("MonitorPlatforms");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Opera", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuthorFirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AuthorLastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItalianDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LocationId")
                        .IsUnique();

                    b.ToTable("Operas");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.OperaImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OperaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OperaId")
                        .IsUnique();

                    b.ToTable("OperaImages");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.ImageMap", b =>
                {
                    b.HasOne("ZoomOpera.Shared.Entities.OperaImage", "OperaImage")
                        .WithMany("ImageMaps")
                        .HasForeignKey("OperaImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OperaImage");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.ImageMapCoordinate", b =>
                {
                    b.HasOne("ZoomOpera.Shared.Entities.ImageMap", "ImageMap")
                        .WithMany("ImageMapCoordinates")
                        .HasForeignKey("ImageMapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ImageMap");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Level", b =>
                {
                    b.HasOne("ZoomOpera.Shared.Entities.Building", "Building")
                        .WithMany("Levels")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Building");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Location", b =>
                {
                    b.HasOne("ZoomOpera.Shared.Entities.Level", "Level")
                        .WithMany("Locations")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Level");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.MonitorPlatform", b =>
                {
                    b.HasOne("ZoomOpera.Shared.Entities.Level", "Level")
                        .WithMany("MonitorPlatforms")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Level");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Opera", b =>
                {
                    b.HasOne("ZoomOpera.Shared.Entities.Location", "Location")
                        .WithOne("Opera")
                        .HasForeignKey("ZoomOpera.Shared.Entities.Opera", "LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.OperaImage", b =>
                {
                    b.HasOne("ZoomOpera.Shared.Entities.Opera", "Opera")
                        .WithOne("Image")
                        .HasForeignKey("ZoomOpera.Shared.Entities.OperaImage", "OperaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Opera");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Building", b =>
                {
                    b.Navigation("Levels");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.ImageMap", b =>
                {
                    b.Navigation("ImageMapCoordinates");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Level", b =>
                {
                    b.Navigation("Locations");

                    b.Navigation("MonitorPlatforms");
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Location", b =>
                {
                    b.Navigation("Opera")
                        .IsRequired();
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.Opera", b =>
                {
                    b.Navigation("Image")
                        .IsRequired();
                });

            modelBuilder.Entity("ZoomOpera.Shared.Entities.OperaImage", b =>
                {
                    b.Navigation("ImageMaps");
                });
#pragma warning restore 612, 618
        }
    }
}
