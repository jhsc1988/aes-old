using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace aes.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predmet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Klasa = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StanId = table.Column<int>(type: "int", nullable: false),
                    SifraObjekta = table.Column<int>(type: "int", nullable: false),
                    Vrsta = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Adresa = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Kat = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    BrojSTana = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Naselje = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Četvrt = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Površina = table.Column<double>(type: "float", nullable: true),
                    StatusKorištenja = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Korisnik = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Vlasništvo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    DioNekretnine = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Sektor = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dopis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PredmetId = table.Column<int>(type: "int", nullable: false),
                    Urbroj = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dopis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dopis_Predmet_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StanId = table.Column<int>(type: "int", nullable: false),
                    Omm = table.Column<int>(type: "int", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ods_Stan_StanId",
                        column: x => x.StanId,
                        principalTable: "Stan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunHolding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StanId = table.Column<int>(type: "int", nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    RedniBroj = table.Column<int>(type: "int", nullable: false),
                    KlasaPlacanja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DatumPotvrde = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunHolding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunHolding_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunHolding_Stan_StanId",
                        column: x => x.StanId,
                        principalTable: "Stan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElektraKupac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UgovorniRacun = table.Column<long>(type: "bigint", nullable: false),
                    OdsId = table.Column<int>(type: "int", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElektraKupac", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ElektraKupac_Ods_OdsId",
                        column: x => x.OdsId,
                        principalTable: "Ods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OdsKupac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SifraKupca = table.Column<int>(type: "int", nullable: false),
                    OdsId = table.Column<int>(type: "int", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdsKupac", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdsKupac_Ods_OdsId",
                        column: x => x.OdsId,
                        principalTable: "Ods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunElektra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    ElektraKupacId = table.Column<int>(type: "int", nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    RedniBroj = table.Column<int>(type: "int", nullable: false),
                    KlasaPlacanja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DatumPotvrde = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektra_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunElektra_ElektraKupac_ElektraKupacId",
                        column: x => x.ElektraKupacId,
                        principalTable: "ElektraKupac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunElektraIzvrsenjeUsluge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    ElektraKupacId = table.Column<int>(type: "int", nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumIzvrsenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usluga = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    RedniBroj = table.Column<int>(type: "int", nullable: false),
                    KlasaPlacanja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DatumPotvrde = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektraIzvrsenjeUsluge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektraIzvrsenjeUsluge_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunElektraIzvrsenjeUsluge_ElektraKupac_ElektraKupacId",
                        column: x => x.ElektraKupacId,
                        principalTable: "ElektraKupac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunElektraRate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    ElektraKupacId = table.Column<int>(type: "int", nullable: false),
                    Razdoblje = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    RedniBroj = table.Column<int>(type: "int", nullable: false),
                    KlasaPlacanja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DatumPotvrde = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektraRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektraRate_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunElektraRate_ElektraKupac_ElektraKupacId",
                        column: x => x.ElektraKupacId,
                        principalTable: "ElektraKupac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunOdsIzvrsenjaUsluge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRacuna = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    OdsKupacId = table.Column<int>(type: "int", nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumIzvrsenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usluga = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Iznos = table.Column<double>(type: "float", nullable: false),
                    DopisId = table.Column<int>(type: "int", nullable: false),
                    RedniBroj = table.Column<int>(type: "int", nullable: false),
                    KlasaPlacanja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DatumPotvrde = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunOdsIzvrsenjaUsluge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunOdsIzvrsenjaUsluge_Dopis_DopisId",
                        column: x => x.DopisId,
                        principalTable: "Dopis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RacunOdsIzvrsenjaUsluge_OdsKupac_OdsKupacId",
                        column: x => x.OdsKupacId,
                        principalTable: "OdsKupac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RacunElektraObracunPotrosnje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumObracuna = table.Column<DateTime>(type: "datetime2", nullable: false),
                    brojilo = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    RacunElektraId = table.Column<int>(type: "int", nullable: false),
                    RVT = table.Column<int>(type: "int", nullable: false),
                    RNT = table.Column<int>(type: "int", nullable: false),
                    VrijemeUnosa = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RacunElektraObracunPotrosnje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RacunElektraObracunPotrosnje_RacunElektra_RacunElektraId",
                        column: x => x.RacunElektraId,
                        principalTable: "RacunElektra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Dopis_PredmetId",
                table: "Dopis",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_ElektraKupac_OdsId",
                table: "ElektraKupac",
                column: "OdsId");

            migrationBuilder.CreateIndex(
                name: "IX_Ods_Omm",
                table: "Ods",
                column: "Omm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ods_StanId",
                table: "Ods",
                column: "StanId");

            migrationBuilder.CreateIndex(
                name: "IX_OdsKupac_OdsId",
                table: "OdsKupac",
                column: "OdsId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektra_DopisId",
                table: "RacunElektra",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektra_ElektraKupacId",
                table: "RacunElektra",
                column: "ElektraKupacId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraIzvrsenjeUsluge_DopisId",
                table: "RacunElektraIzvrsenjeUsluge",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraIzvrsenjeUsluge_ElektraKupacId",
                table: "RacunElektraIzvrsenjeUsluge",
                column: "ElektraKupacId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraObracunPotrosnje_RacunElektraId",
                table: "RacunElektraObracunPotrosnje",
                column: "RacunElektraId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraRate_DopisId",
                table: "RacunElektraRate",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunElektraRate_ElektraKupacId",
                table: "RacunElektraRate",
                column: "ElektraKupacId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunHolding_DopisId",
                table: "RacunHolding",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunHolding_StanId",
                table: "RacunHolding",
                column: "StanId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunOdsIzvrsenjaUsluge_DopisId",
                table: "RacunOdsIzvrsenjaUsluge",
                column: "DopisId");

            migrationBuilder.CreateIndex(
                name: "IX_RacunOdsIzvrsenjaUsluge_OdsKupacId",
                table: "RacunOdsIzvrsenjaUsluge",
                column: "OdsKupacId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RacunElektraIzvrsenjeUsluge");

            migrationBuilder.DropTable(
                name: "RacunElektraObracunPotrosnje");

            migrationBuilder.DropTable(
                name: "RacunElektraRate");

            migrationBuilder.DropTable(
                name: "RacunHolding");

            migrationBuilder.DropTable(
                name: "RacunOdsIzvrsenjaUsluge");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RacunElektra");

            migrationBuilder.DropTable(
                name: "OdsKupac");

            migrationBuilder.DropTable(
                name: "Dopis");

            migrationBuilder.DropTable(
                name: "ElektraKupac");

            migrationBuilder.DropTable(
                name: "Predmet");

            migrationBuilder.DropTable(
                name: "Ods");

            migrationBuilder.DropTable(
                name: "Stan");
        }
    }
}
