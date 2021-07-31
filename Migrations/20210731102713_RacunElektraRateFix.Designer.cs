﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using aes.Data;

namespace aes.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210731102713_RacunElektraRateFix")]
    partial class RacunElektraRateFix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("aes.Models.Dopis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Datum")
                        .HasColumnType("datetime2");

                    b.Property<int>("PredmetId")
                        .HasColumnType("int");

                    b.Property<string>("Urbroj")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PredmetId");

                    b.ToTable("Dopis");
                });

            modelBuilder.Entity("aes.Models.ElektraKupac", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Napomena")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("OdsId")
                        .HasColumnType("int");

                    b.Property<long>("UgovorniRacun")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OdsId");

                    b.ToTable("ElektraKupac");
                });

            modelBuilder.Entity("aes.Models.Ods", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Napomena")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Omm")
                        .HasColumnType("int");

                    b.Property<int>("StanId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Omm")
                        .IsUnique();

                    b.HasIndex("StanId");

                    b.ToTable("Ods");
                });

            modelBuilder.Entity("aes.Models.OdsKupac", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Napomena")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("OdsId")
                        .HasColumnType("int");

                    b.Property<int>("SifraKupca")
                        .HasColumnType("int");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OdsId");

                    b.ToTable("OdsKupac");
                });

            modelBuilder.Entity("aes.Models.Predmet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Klasa")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<string>("Naziv")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Predmet");
                });

            modelBuilder.Entity("aes.Models.RacunElektra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrojRacuna")
                        .IsRequired()
                        .HasMaxLength(19)
                        .HasColumnType("nvarchar(19)");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DatumIzdavanja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DatumPotvrde")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DopisId")
                        .HasColumnType("int");

                    b.Property<int>("ElektraKupacId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsItTemp")
                        .HasColumnType("bit");

                    b.Property<double>("Iznos")
                        .HasColumnType("float");

                    b.Property<string>("KlasaPlacanja")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Napomena")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RedniBroj")
                        .HasColumnType("int");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DopisId");

                    b.HasIndex("ElektraKupacId");

                    b.ToTable("RacunElektra");
                });

            modelBuilder.Entity("aes.Models.RacunElektraIzvrsenjeUsluge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrojRacuna")
                        .IsRequired()
                        .HasMaxLength(19)
                        .HasColumnType("nvarchar(19)");

                    b.Property<DateTime>("DatumIzdavanja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatumIzvrsenja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DatumPotvrde")
                        .HasColumnType("datetime2");

                    b.Property<int>("DopisId")
                        .HasColumnType("int");

                    b.Property<int>("ElektraKupacId")
                        .HasColumnType("int");

                    b.Property<double>("Iznos")
                        .HasColumnType("float");

                    b.Property<string>("KlasaPlacanja")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Napomena")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RedniBroj")
                        .HasColumnType("int");

                    b.Property<string>("Usluga")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DopisId");

                    b.HasIndex("ElektraKupacId");

                    b.ToTable("RacunElektraIzvrsenjeUsluge");
                });

            modelBuilder.Entity("aes.Models.RacunElektraObracunPotrosnje", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DatumObracuna")
                        .HasColumnType("datetime2");

                    b.Property<int>("RNT")
                        .HasColumnType("int");

                    b.Property<int>("RVT")
                        .HasColumnType("int");

                    b.Property<int>("RacunElektraId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.Property<string>("brojilo")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.HasKey("Id");

                    b.HasIndex("RacunElektraId");

                    b.ToTable("RacunElektraObracunPotrosnje");
                });

            modelBuilder.Entity("aes.Models.RacunElektraRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrojRacuna")
                        .IsRequired()
                        .HasMaxLength(19)
                        .HasColumnType("nvarchar(19)");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DatumIzdavanja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DatumPotvrde")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DopisId")
                        .HasColumnType("int");

                    b.Property<int>("ElektraKupacId")
                        .HasColumnType("int");

                    b.Property<bool?>("IsItTemp")
                        .HasColumnType("bit");

                    b.Property<double>("Iznos")
                        .HasColumnType("float");

                    b.Property<string>("KlasaPlacanja")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Napomena")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Razdoblje")
                        .HasColumnType("datetime2");

                    b.Property<int>("RedniBroj")
                        .HasColumnType("int");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DopisId");

                    b.HasIndex("ElektraKupacId");

                    b.ToTable("RacunElektraRate");
                });

            modelBuilder.Entity("aes.Models.RacunHolding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrojRacuna")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("DatumIzdavanja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DatumPotvrde")
                        .HasColumnType("datetime2");

                    b.Property<int>("DopisId")
                        .HasColumnType("int");

                    b.Property<double>("Iznos")
                        .HasColumnType("float");

                    b.Property<string>("KlasaPlacanja")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Napomena")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RedniBroj")
                        .HasColumnType("int");

                    b.Property<int>("StanId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DopisId");

                    b.HasIndex("StanId");

                    b.ToTable("RacunHolding");
                });

            modelBuilder.Entity("aes.Models.RacunOdsIzvrsenjaUsluge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrojRacuna")
                        .IsRequired()
                        .HasMaxLength(19)
                        .HasColumnType("nvarchar(19)");

                    b.Property<DateTime>("DatumIzdavanja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatumIzvrsenja")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DatumPotvrde")
                        .HasColumnType("datetime2");

                    b.Property<int>("DopisId")
                        .HasColumnType("int");

                    b.Property<double>("Iznos")
                        .HasColumnType("float");

                    b.Property<string>("KlasaPlacanja")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Napomena")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("OdsKupacId")
                        .HasColumnType("int");

                    b.Property<int>("RedniBroj")
                        .HasColumnType("int");

                    b.Property<string>("Usluga")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DopisId");

                    b.HasIndex("OdsKupacId");

                    b.ToTable("RacunOdsIzvrsenjaUsluge");
                });

            modelBuilder.Entity("aes.Models.Stan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adresa")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("BrojSTana")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("DioNekretnine")
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Kat")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Korisnik")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Naselje")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<double?>("Površina")
                        .HasColumnType("float");

                    b.Property<string>("Sektor")
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<int>("SifraObjekta")
                        .HasColumnType("int");

                    b.Property<int>("StanId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("StatusKorištenja")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Vlasništvo")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.Property<string>("Vrsta")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Četvrt")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Stan");
                });

            modelBuilder.Entity("aes.Models.UgovorOKoristenju", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrojUgovora")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<DateTime>("DatumPotpisaGZ")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatumPotpisaHEP")
                        .HasColumnType("datetime2");

                    b.Property<int>("DopisDostaveId")
                        .HasColumnType("int");

                    b.Property<int>("DopisId")
                        .HasColumnType("int");

                    b.Property<int>("OdsId")
                        .HasColumnType("int");

                    b.Property<int>("RbrDostave")
                        .HasColumnType("int");

                    b.Property<int>("RbrUgovora")
                        .HasColumnType("int");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DopisDostaveId");

                    b.HasIndex("DopisId");

                    b.HasIndex("OdsId");

                    b.ToTable("UgovorOKoristenju");
                });

            modelBuilder.Entity("aes.Models.UgovorOPrijenosu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BrojUgovora")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<DateTime>("DatumPotpisa")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatumPrijenosa")
                        .HasColumnType("datetime2");

                    b.Property<int>("DopisDostaveId")
                        .HasColumnType("int");

                    b.Property<int>("DopisId")
                        .HasColumnType("int");

                    b.Property<string>("Kupac")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<long>("KupacOIB")
                        .HasColumnType("bigint");

                    b.Property<int>("RbrDostave")
                        .HasColumnType("int");

                    b.Property<int>("RbrUgovora")
                        .HasColumnType("int");

                    b.Property<int>("UgovorOKoristenjuId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("VrijemeUnosa")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DopisDostaveId");

                    b.HasIndex("DopisId");

                    b.HasIndex("UgovorOKoristenjuId");

                    b.ToTable("UgovorOPrijenosu");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("aes.Models.Dopis", b =>
                {
                    b.HasOne("aes.Models.Predmet", "Predmet")
                        .WithMany()
                        .HasForeignKey("PredmetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Predmet");
                });

            modelBuilder.Entity("aes.Models.ElektraKupac", b =>
                {
                    b.HasOne("aes.Models.Ods", "Ods")
                        .WithMany()
                        .HasForeignKey("OdsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ods");
                });

            modelBuilder.Entity("aes.Models.Ods", b =>
                {
                    b.HasOne("aes.Models.Stan", "Stan")
                        .WithMany()
                        .HasForeignKey("StanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stan");
                });

            modelBuilder.Entity("aes.Models.OdsKupac", b =>
                {
                    b.HasOne("aes.Models.Ods", "Ods")
                        .WithMany()
                        .HasForeignKey("OdsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ods");
                });

            modelBuilder.Entity("aes.Models.RacunElektra", b =>
                {
                    b.HasOne("aes.Models.Dopis", "Dopis")
                        .WithMany()
                        .HasForeignKey("DopisId");

                    b.HasOne("aes.Models.ElektraKupac", "ElektraKupac")
                        .WithMany()
                        .HasForeignKey("ElektraKupacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dopis");

                    b.Navigation("ElektraKupac");
                });

            modelBuilder.Entity("aes.Models.RacunElektraIzvrsenjeUsluge", b =>
                {
                    b.HasOne("aes.Models.Dopis", "Dopis")
                        .WithMany()
                        .HasForeignKey("DopisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("aes.Models.ElektraKupac", "ElektraKupac")
                        .WithMany()
                        .HasForeignKey("ElektraKupacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dopis");

                    b.Navigation("ElektraKupac");
                });

            modelBuilder.Entity("aes.Models.RacunElektraObracunPotrosnje", b =>
                {
                    b.HasOne("aes.Models.RacunElektra", "RacunElektra")
                        .WithMany()
                        .HasForeignKey("RacunElektraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RacunElektra");
                });

            modelBuilder.Entity("aes.Models.RacunElektraRate", b =>
                {
                    b.HasOne("aes.Models.Dopis", "Dopis")
                        .WithMany()
                        .HasForeignKey("DopisId");

                    b.HasOne("aes.Models.ElektraKupac", "ElektraKupac")
                        .WithMany()
                        .HasForeignKey("ElektraKupacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dopis");

                    b.Navigation("ElektraKupac");
                });

            modelBuilder.Entity("aes.Models.RacunHolding", b =>
                {
                    b.HasOne("aes.Models.Dopis", "Dopis")
                        .WithMany()
                        .HasForeignKey("DopisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("aes.Models.Stan", "Stan")
                        .WithMany()
                        .HasForeignKey("StanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dopis");

                    b.Navigation("Stan");
                });

            modelBuilder.Entity("aes.Models.RacunOdsIzvrsenjaUsluge", b =>
                {
                    b.HasOne("aes.Models.Dopis", "Dopis")
                        .WithMany()
                        .HasForeignKey("DopisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("aes.Models.OdsKupac", "OdsKupac")
                        .WithMany()
                        .HasForeignKey("OdsKupacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dopis");

                    b.Navigation("OdsKupac");
                });

            modelBuilder.Entity("aes.Models.UgovorOKoristenju", b =>
                {
                    b.HasOne("aes.Models.Dopis", "DopisDostave")
                        .WithMany()
                        .HasForeignKey("DopisDostaveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("aes.Models.Dopis", "Dopis")
                        .WithMany()
                        .HasForeignKey("DopisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("aes.Models.Ods", "Ods")
                        .WithMany()
                        .HasForeignKey("OdsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dopis");

                    b.Navigation("DopisDostave");

                    b.Navigation("Ods");
                });

            modelBuilder.Entity("aes.Models.UgovorOPrijenosu", b =>
                {
                    b.HasOne("aes.Models.Dopis", "DopisDostave")
                        .WithMany()
                        .HasForeignKey("DopisDostaveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("aes.Models.Dopis", "Dopis")
                        .WithMany()
                        .HasForeignKey("DopisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("aes.Models.UgovorOKoristenju", "UgovorOKoristenju")
                        .WithMany()
                        .HasForeignKey("UgovorOKoristenjuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dopis");

                    b.Navigation("DopisDostave");

                    b.Navigation("UgovorOKoristenju");
                });
#pragma warning restore 612, 618
        }
    }
}
