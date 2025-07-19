using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoLEsportsHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "MatchDate", "Region", "Title", "VODUrl", "Winner" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-bbbb-cccc-111111111111"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LCO", "Chiefs vs Pentanet.GG - LCO Finals", null, null },
                    { new Guid("22222222-bbbb-cccc-dddd-222222222222"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CBLOL", "LOUD vs paiN Gaming - CBLOL Finals", null, null },
                    { new Guid("33333333-cccc-dddd-eeee-333333333333"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CBLOL", "KaBuM vs RED Canids - CBLOL Semifinals", null, null },
                    { new Guid("a1d6219f-9f49-4f29-a2ea-d3c302781b01"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EU", "G2 vs Fnatic - Spring Finals", null, null },
                    { new Guid("aa111111-1111-1111-1111-aaaaaaaaaaaa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LPL", "TES vs RNG - LPL Playoffs", null, null },
                    { new Guid("b2e732af-82f2-4ab2-9cd5-47f2a2ffcf32"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "KR", "T1 vs Gen.G - LCK Clash", null, null },
                    { new Guid("bb222222-2222-2222-2222-bbbbbbbbbbbb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "KR", "DRX vs KT Rolster - LCK Quarters", null, null },
                    { new Guid("c3f843cb-14c6-412e-9912-3290a28f31c5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NA", "Cloud9 vs TL - LCS Championship", null, null },
                    { new Guid("cc333333-3333-3333-3333-cccccccccccc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NA", "Evil Geniuses vs 100 Thieves - LCS Semifinals", null, null },
                    { new Guid("d4a9347d-b5b1-4014-9d92-4a2f9dfc1a94"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PCS", "PSG Talon vs Beyond - PCS Showdown", null, null },
                    { new Guid("dd444444-4444-4444-4444-dddddddddddd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EU", "MAD Lions vs Rogue - LEC Semifinals", null, null },
                    { new Guid("e5b0453a-6d9c-48e3-a83d-f5ab2db2b906"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "VCS", "GAM vs Team Whales - VCS Grand Finals", null, null },
                    { new Guid("ee555555-5555-5555-5555-eeeeeeeeeeee"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LJL", "DetonatioN FM vs Crest Gaming - LJL Finals", null, null },
                    { new Guid("f6c156e3-a5b7-4cf9-98c2-6577b8b6fa61"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LPL", "JD Gaming vs Bilibili - LPL Semifinals", null, null },
                    { new Guid("ff666666-6666-6666-6666-ffffffffffff"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LLA", "Isurus vs Estral Esports - LLA Finals", null, null }
                });

            migrationBuilder.InsertData(
                table: "Tournaments",
                columns: new[] { "Id", "IsDeleted", "Name", "OrganizerId", "Region" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-aaaa-aaaa-111111111111"), false, "MSI 2025", null, "Global" },
                    { new Guid("22222222-bbbb-bbbb-bbbb-222222222222"), false, "LEC Summer Split", null, "EU" },
                    { new Guid("33333333-cccc-cccc-cccc-333333333333"), false, "LCK Spring Playoffs", null, "KR" },
                    { new Guid("44444444-dddd-dddd-dddd-444444444444"), false, "VCS Winter Finals", null, "VCS" },
                    { new Guid("55555555-eeee-eeee-eeee-555555555555"), false, "LCS Summer Championship", null, "NA" },
                    { new Guid("66666666-ffff-ffff-ffff-666666666666"), false, "Worlds 2025", null, "Global" },
                    { new Guid("77777777-aaaa-bbbb-cccc-777777777777"), false, "LPL Summer Playoffs", null, "LPL" },
                    { new Guid("88888888-bbbb-cccc-dddd-888888888888"), false, "CBLOL Winter Finals", null, "CBLOL" },
                    { new Guid("99999999-cccc-dddd-eeee-999999999999"), false, "PCS Summer Split", null, "PCS" },
                    { new Guid("aaaaaaaa-dddd-eeee-ffff-aaaaaaaaaaaa"), false, "LLA Apertura Playoffs", null, "LLA" }
                });

            migrationBuilder.InsertData(
                table: "TournamentsMatches",
                columns: new[] { "Id", "MatchId", "ScheduledTime", "TournamentId" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-3333-4444-555555555555"), new Guid("dd444444-4444-4444-4444-dddddddddddd"), "2025-07-15 17:00", new Guid("22222222-bbbb-bbbb-bbbb-222222222222") },
                    { new Guid("22222222-3333-4444-5555-666666666666"), new Guid("bb222222-2222-2222-2222-bbbbbbbbbbbb"), "2025-04-12 16:30", new Guid("33333333-cccc-cccc-cccc-333333333333") },
                    { new Guid("33333333-4444-5555-6666-777777777777"), new Guid("cc333333-3333-3333-3333-cccccccccccc"), "2025-08-08 19:00", new Guid("55555555-eeee-eeee-eeee-555555555555") },
                    { new Guid("44444444-5555-6666-7777-888888888888"), new Guid("aa111111-1111-1111-1111-aaaaaaaaaaaa"), "2025-09-01 14:00", new Guid("77777777-aaaa-bbbb-cccc-777777777777") },
                    { new Guid("55555555-6666-7777-8888-999999999999"), new Guid("22222222-bbbb-cccc-dddd-222222222222"), "2025-06-21 18:00", new Guid("88888888-bbbb-cccc-dddd-888888888888") },
                    { new Guid("66666666-7777-8888-9999-aaaaaaaaaaaa"), new Guid("d4a9347d-b5b1-4014-9d92-4a2f9dfc1a94"), "2025-03-19 12:00", new Guid("99999999-cccc-dddd-eeee-999999999999") },
                    { new Guid("77777777-8888-9999-aaaa-bbbbbbbbbbbb"), new Guid("ff666666-6666-6666-6666-ffffffffffff"), "2025-05-05 21:00", new Guid("aaaaaaaa-dddd-eeee-ffff-aaaaaaaaaaaa") },
                    { new Guid("88888888-9999-aaaa-bbbb-cccccccccccc"), new Guid("e5b0453a-6d9c-48e3-a83d-f5ab2db2b906"), "2025-04-02 15:30", new Guid("44444444-dddd-dddd-dddd-444444444444") },
                    { new Guid("e3a83cb3-df47-492e-b4f0-4a3cfe9d3a6c"), new Guid("b2e732af-82f2-4ab2-9cd5-47f2a2ffcf32"), "2025-10-10 20:00", new Guid("66666666-ffff-ffff-ffff-666666666666") },
                    { new Guid("fa2be4a6-c170-4ec4-b8c1-f4bc6fd8ee7f"), new Guid("a1d6219f-9f49-4f29-a2ea-d3c302781b01"), "2025-05-02 18:00", new Guid("11111111-aaaa-aaaa-aaaa-111111111111") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-bbbb-cccc-111111111111"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("33333333-cccc-dddd-eeee-333333333333"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("c3f843cb-14c6-412e-9912-3290a28f31c5"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("ee555555-5555-5555-5555-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("f6c156e3-a5b7-4cf9-98c2-6577b8b6fa61"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555555"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("22222222-3333-4444-5555-666666666666"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("33333333-4444-5555-6666-777777777777"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("44444444-5555-6666-7777-888888888888"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("55555555-6666-7777-8888-999999999999"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("66666666-7777-8888-9999-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("77777777-8888-9999-aaaa-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("88888888-9999-aaaa-bbbb-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("e3a83cb3-df47-492e-b4f0-4a3cfe9d3a6c"));

            migrationBuilder.DeleteData(
                table: "TournamentsMatches",
                keyColumn: "Id",
                keyValue: new Guid("fa2be4a6-c170-4ec4-b8c1-f4bc6fd8ee7f"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-cccc-dddd-222222222222"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("a1d6219f-9f49-4f29-a2ea-d3c302781b01"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("aa111111-1111-1111-1111-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("b2e732af-82f2-4ab2-9cd5-47f2a2ffcf32"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("bb222222-2222-2222-2222-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("cc333333-3333-3333-3333-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("d4a9347d-b5b1-4014-9d92-4a2f9dfc1a94"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("dd444444-4444-4444-4444-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("e5b0453a-6d9c-48e3-a83d-f5ab2db2b906"));

            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: new Guid("ff666666-6666-6666-6666-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-aaaa-aaaa-111111111111"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-bbbb-bbbb-222222222222"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("33333333-cccc-cccc-cccc-333333333333"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-dddd-dddd-444444444444"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("55555555-eeee-eeee-eeee-555555555555"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("66666666-ffff-ffff-ffff-666666666666"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("77777777-aaaa-bbbb-cccc-777777777777"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("88888888-bbbb-cccc-dddd-888888888888"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("99999999-cccc-dddd-eeee-999999999999"));

            migrationBuilder.DeleteData(
                table: "Tournaments",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-dddd-eeee-ffff-aaaaaaaaaaaa"));
        }
    }
}
