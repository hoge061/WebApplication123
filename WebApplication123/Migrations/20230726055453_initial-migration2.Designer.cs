﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wing1.Models;

#nullable disable

namespace Wing1.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20230726055453_initial-migration2")]
    partial class initialmigration2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Wing1.Models.Details", b =>
                {
                    b.Property<string>("Userid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Content1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content10")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content6")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content7")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content8")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content9")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Endtime1")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime10")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime2")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime3")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime4")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime5")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime6")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime7")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime8")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime9")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime1")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime10")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime2")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime3")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime4")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime5")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime6")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime7")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime8")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime9")
                        .HasColumnType("datetime2");

                    b.HasKey("Userid", "Date");

                    b.ToTable("Details");
                });

            modelBuilder.Entity("Wing1.Models.Kintai", b =>
                {
                    b.Property<string>("Userid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Break1end")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Break1start")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Break2end")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Break2start")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Break3end")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Break3start")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Break4end")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Break4start")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Endtime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Starttime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Workstyle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("biko")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Userid", "Date");

                    b.ToTable("Kintai");
                });

            modelBuilder.Entity("Wing1.Models.Users", b =>
                {
                    b.Property<string>("Userid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<TimeSpan?>("Jitudo")
                        .HasColumnType("time");

                    b.Property<DateTime?>("Kendtime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Kstarttime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Pass")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("adminflag")
                        .HasColumnType("int");

                    b.HasKey("Userid");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}