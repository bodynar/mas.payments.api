using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MAS.Payments.Migrations
{
    /// <inheritdoc />
    public partial class SwitchToGuidIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =====================================================================
            // Ensure gen_random_uuid() is available (PG < 13 needs pgcrypto)
            // =====================================================================
            migrationBuilder.Sql("""CREATE EXTENSION IF NOT EXISTS "pgcrypto";""");

            // =====================================================================
            // STEP 1: Add temp_new_id (uuid) columns to every table
            // =====================================================================
            migrationBuilder.Sql("""
                ALTER TABLE "PaymentType"          ADD COLUMN "temp_new_id" uuid NOT NULL DEFAULT gen_random_uuid();
                ALTER TABLE "PaymentGroup"         ADD COLUMN "temp_new_id" uuid NOT NULL DEFAULT gen_random_uuid();
                ALTER TABLE "Payment"              ADD COLUMN "temp_new_id" uuid NOT NULL DEFAULT gen_random_uuid();
                ALTER TABLE "MeterMeasurementType" ADD COLUMN "temp_new_id" uuid NOT NULL DEFAULT gen_random_uuid();
                ALTER TABLE "MeterMeasurement"     ADD COLUMN "temp_new_id" uuid NOT NULL DEFAULT gen_random_uuid();
                ALTER TABLE "PaymentFile"          ADD COLUMN "temp_new_id" uuid NOT NULL DEFAULT gen_random_uuid();
                ALTER TABLE "UserSetting"          ADD COLUMN "temp_new_id" uuid NOT NULL DEFAULT gen_random_uuid();
                ALTER TABLE "UserNotification"     ADD COLUMN "temp_new_id" uuid NOT NULL DEFAULT gen_random_uuid();
            """);

            // =====================================================================
            // STEP 2: Add temp FK columns and populate via existing relationships
            // =====================================================================

            // -- Payment.PaymentTypeId → PaymentType.Id (NOT NULL)
            migrationBuilder.Sql("""
                ALTER TABLE "Payment" ADD COLUMN "temp_new_payment_type_id" uuid;

                UPDATE "Payment" p
                   SET "temp_new_payment_type_id" = pt."temp_new_id"
                  FROM "PaymentType" pt
                 WHERE pt."Id" = p."PaymentTypeId";

                ALTER TABLE "Payment" ALTER COLUMN "temp_new_payment_type_id" SET NOT NULL;
            """);

            // -- Payment.PaymentGroupId → PaymentGroup.Id (NULLABLE)
            migrationBuilder.Sql("""
                ALTER TABLE "Payment" ADD COLUMN "temp_new_payment_group_id" uuid;

                UPDATE "Payment" p
                   SET "temp_new_payment_group_id" = pg."temp_new_id"
                  FROM "PaymentGroup" pg
                 WHERE pg."Id" = p."PaymentGroupId";
            """);

            // -- MeterMeasurementType.PaymentTypeId → PaymentType.Id (NOT NULL)
            migrationBuilder.Sql("""
                ALTER TABLE "MeterMeasurementType" ADD COLUMN "temp_new_payment_type_id" uuid;

                UPDATE "MeterMeasurementType" mmt
                   SET "temp_new_payment_type_id" = pt."temp_new_id"
                  FROM "PaymentType" pt
                 WHERE pt."Id" = mmt."PaymentTypeId";

                ALTER TABLE "MeterMeasurementType" ALTER COLUMN "temp_new_payment_type_id" SET NOT NULL;
            """);

            // -- MeterMeasurement.MeterMeasurementTypeId → MeterMeasurementType.Id (NOT NULL)
            migrationBuilder.Sql("""
                ALTER TABLE "MeterMeasurement" ADD COLUMN "temp_new_meter_measurement_type_id" uuid;

                UPDATE "MeterMeasurement" mm
                   SET "temp_new_meter_measurement_type_id" = mmt."temp_new_id"
                  FROM "MeterMeasurementType" mmt
                 WHERE mmt."Id" = mm."MeterMeasurementTypeId";

                ALTER TABLE "MeterMeasurement" ALTER COLUMN "temp_new_meter_measurement_type_id" SET NOT NULL;
            """);

            // -- PaymentFile.PaymentId → Payment.Id (NULLABLE)
            migrationBuilder.Sql("""
                ALTER TABLE "PaymentFile" ADD COLUMN "temp_new_payment_id" uuid;

                UPDATE "PaymentFile" pf
                   SET "temp_new_payment_id" = p."temp_new_id"
                  FROM "Payment" p
                 WHERE p."Id" = pf."PaymentId";
            """);

            // -- PaymentFile.PaymentGroupId → PaymentGroup.Id (NULLABLE)
            migrationBuilder.Sql("""
                ALTER TABLE "PaymentFile" ADD COLUMN "temp_new_payment_group_id" uuid;

                UPDATE "PaymentFile" pf
                   SET "temp_new_payment_group_id" = pg."temp_new_id"
                  FROM "PaymentGroup" pg
                 WHERE pg."Id" = pf."PaymentGroupId";
            """);

            // =====================================================================
            // STEP 3: Drop all constraints (FK → indexes → check → PK)
            // =====================================================================

            // -- FK constraints
            migrationBuilder.DropForeignKey(name: "FK_PaymentFile_Payment_PaymentId", table: "PaymentFile");
            migrationBuilder.DropForeignKey(name: "FK_PaymentFile_PaymentGroup_PaymentGroupId", table: "PaymentFile");
            migrationBuilder.DropForeignKey(name: "FK_Payment_PaymentType_PaymentTypeId", table: "Payment");
            migrationBuilder.DropForeignKey(name: "FK_Payment_PaymentGroup_PaymentGroupId", table: "Payment");
            migrationBuilder.DropForeignKey(name: "FK_MeterMeasurement_MeterMeasurementType_MeterMeasurementTypeId", table: "MeterMeasurement");
            migrationBuilder.DropForeignKey(name: "FK_MeterMeasurementType_PaymentType_PaymentTypeId", table: "MeterMeasurementType");

            // -- Indexes on FK columns
            migrationBuilder.DropIndex(name: "IX_PaymentFile_PaymentId", table: "PaymentFile");
            migrationBuilder.DropIndex(name: "IX_PaymentFile_PaymentGroupId", table: "PaymentFile");
            migrationBuilder.DropIndex(name: "IX_Payment_PaymentTypeId", table: "Payment");
            migrationBuilder.DropIndex(name: "IX_Payment_PaymentGroupId", table: "Payment");
            migrationBuilder.DropIndex(name: "IX_MeterMeasurement_MeterMeasurementTypeId", table: "MeterMeasurement");
            migrationBuilder.DropIndex(name: "IX_MeterMeasurementType_PaymentTypeId", table: "MeterMeasurementType");

            // -- Check constraint
            migrationBuilder.DropCheckConstraint(name: "CK_PaymentFile_SingleLink", table: "PaymentFile");

            // -- PK constraints
            migrationBuilder.DropPrimaryKey(name: "PK_PaymentFile", table: "PaymentFile");
            migrationBuilder.DropPrimaryKey(name: "PK_MeterMeasurement", table: "MeterMeasurement");
            migrationBuilder.DropPrimaryKey(name: "PK_MeterMeasurementType", table: "MeterMeasurementType");
            migrationBuilder.DropPrimaryKey(name: "PK_Payment", table: "Payment");
            migrationBuilder.DropPrimaryKey(name: "PK_PaymentGroup", table: "PaymentGroup");
            migrationBuilder.DropPrimaryKey(name: "PK_PaymentType", table: "PaymentType");
            migrationBuilder.DropPrimaryKey(name: "PK_UserNotification", table: "UserNotification");
            migrationBuilder.DropPrimaryKey(name: "PK_UserSetting", table: "UserSetting");

            // =====================================================================
            // STEP 4: Drop old Id and FK columns
            // =====================================================================
            migrationBuilder.Sql("""
                -- FK columns
                ALTER TABLE "Payment"              DROP COLUMN "PaymentTypeId";
                ALTER TABLE "Payment"              DROP COLUMN "PaymentGroupId";
                ALTER TABLE "MeterMeasurementType" DROP COLUMN "PaymentTypeId";
                ALTER TABLE "MeterMeasurement"     DROP COLUMN "MeterMeasurementTypeId";
                ALTER TABLE "PaymentFile"          DROP COLUMN "PaymentId";
                ALTER TABLE "PaymentFile"          DROP COLUMN "PaymentGroupId";

                -- PK columns
                ALTER TABLE "PaymentType"          DROP COLUMN "Id";
                ALTER TABLE "PaymentGroup"         DROP COLUMN "Id";
                ALTER TABLE "Payment"              DROP COLUMN "Id";
                ALTER TABLE "MeterMeasurementType" DROP COLUMN "Id";
                ALTER TABLE "MeterMeasurement"     DROP COLUMN "Id";
                ALTER TABLE "PaymentFile"          DROP COLUMN "Id";
                ALTER TABLE "UserSetting"          DROP COLUMN "Id";
                ALTER TABLE "UserNotification"     DROP COLUMN "Id";
            """);

            // =====================================================================
            // STEP 5: Rename temp columns to final names & drop defaults
            // =====================================================================
            migrationBuilder.Sql("""
                -- Rename Id columns
                ALTER TABLE "PaymentType"          RENAME COLUMN "temp_new_id" TO "Id";
                ALTER TABLE "PaymentGroup"         RENAME COLUMN "temp_new_id" TO "Id";
                ALTER TABLE "Payment"              RENAME COLUMN "temp_new_id" TO "Id";
                ALTER TABLE "MeterMeasurementType" RENAME COLUMN "temp_new_id" TO "Id";
                ALTER TABLE "MeterMeasurement"     RENAME COLUMN "temp_new_id" TO "Id";
                ALTER TABLE "PaymentFile"          RENAME COLUMN "temp_new_id" TO "Id";
                ALTER TABLE "UserSetting"          RENAME COLUMN "temp_new_id" TO "Id";
                ALTER TABLE "UserNotification"     RENAME COLUMN "temp_new_id" TO "Id";

                -- Rename FK columns
                ALTER TABLE "Payment"              RENAME COLUMN "temp_new_payment_type_id"              TO "PaymentTypeId";
                ALTER TABLE "Payment"              RENAME COLUMN "temp_new_payment_group_id"             TO "PaymentGroupId";
                ALTER TABLE "MeterMeasurementType" RENAME COLUMN "temp_new_payment_type_id"              TO "PaymentTypeId";
                ALTER TABLE "MeterMeasurement"     RENAME COLUMN "temp_new_meter_measurement_type_id"    TO "MeterMeasurementTypeId";
                ALTER TABLE "PaymentFile"          RENAME COLUMN "temp_new_payment_id"                   TO "PaymentId";
                ALTER TABLE "PaymentFile"          RENAME COLUMN "temp_new_payment_group_id"             TO "PaymentGroupId";

                -- Drop gen_random_uuid() defaults (app generates Guids, not DB)
                ALTER TABLE "PaymentType"          ALTER COLUMN "Id" DROP DEFAULT;
                ALTER TABLE "PaymentGroup"         ALTER COLUMN "Id" DROP DEFAULT;
                ALTER TABLE "Payment"              ALTER COLUMN "Id" DROP DEFAULT;
                ALTER TABLE "MeterMeasurementType" ALTER COLUMN "Id" DROP DEFAULT;
                ALTER TABLE "MeterMeasurement"     ALTER COLUMN "Id" DROP DEFAULT;
                ALTER TABLE "PaymentFile"          ALTER COLUMN "Id" DROP DEFAULT;
                ALTER TABLE "UserSetting"          ALTER COLUMN "Id" DROP DEFAULT;
                ALTER TABLE "UserNotification"     ALTER COLUMN "Id" DROP DEFAULT;
            """);

            // =====================================================================
            // STEP 6: Re-create PK, indexes, FK, check constraints
            // =====================================================================

            // -- PKs
            migrationBuilder.AddPrimaryKey(name: "PK_UserSetting", table: "UserSetting", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_UserNotification", table: "UserNotification", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_PaymentType", table: "PaymentType", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_PaymentGroup", table: "PaymentGroup", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_Payment", table: "Payment", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_MeterMeasurementType", table: "MeterMeasurementType", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_MeterMeasurement", table: "MeterMeasurement", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_PaymentFile", table: "PaymentFile", column: "Id");

            // -- Indexes on FK columns
            migrationBuilder.CreateIndex(name: "IX_Payment_PaymentTypeId", table: "Payment", column: "PaymentTypeId");
            migrationBuilder.CreateIndex(name: "IX_Payment_PaymentGroupId", table: "Payment", column: "PaymentGroupId");
            migrationBuilder.CreateIndex(name: "IX_MeterMeasurementType_PaymentTypeId", table: "MeterMeasurementType", column: "PaymentTypeId");
            migrationBuilder.CreateIndex(name: "IX_MeterMeasurement_MeterMeasurementTypeId", table: "MeterMeasurement", column: "MeterMeasurementTypeId");
            migrationBuilder.CreateIndex(name: "IX_PaymentFile_PaymentId", table: "PaymentFile", column: "PaymentId", unique: true, filter: "\"PaymentId\" IS NOT NULL");
            migrationBuilder.CreateIndex(name: "IX_PaymentFile_PaymentGroupId", table: "PaymentFile", column: "PaymentGroupId", unique: true, filter: "\"PaymentGroupId\" IS NOT NULL");

            // -- FK constraints
            migrationBuilder.AddForeignKey(name: "FK_Payment_PaymentType_PaymentTypeId", table: "Payment", column: "PaymentTypeId", principalTable: "PaymentType", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Payment_PaymentGroup_PaymentGroupId", table: "Payment", column: "PaymentGroupId", principalTable: "PaymentGroup", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_MeterMeasurementType_PaymentType_PaymentTypeId", table: "MeterMeasurementType", column: "PaymentTypeId", principalTable: "PaymentType", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_MeterMeasurement_MeterMeasurementType_MeterMeasurementTypeId", table: "MeterMeasurement", column: "MeterMeasurementTypeId", principalTable: "MeterMeasurementType", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_PaymentFile_Payment_PaymentId", table: "PaymentFile", column: "PaymentId", principalTable: "Payment", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_PaymentFile_PaymentGroup_PaymentGroupId", table: "PaymentFile", column: "PaymentGroupId", principalTable: "PaymentGroup", principalColumn: "Id", onDelete: ReferentialAction.Cascade);

            // -- Check constraint
            migrationBuilder.AddCheckConstraint(name: "CK_PaymentFile_SingleLink", table: "PaymentFile", sql: "(\"PaymentId\" IS NOT NULL AND \"PaymentGroupId\" IS NULL) OR (\"PaymentId\" IS NULL AND \"PaymentGroupId\" IS NOT NULL)");

            // =====================================================================
            // STEP 7: Fix seed data — update existing row to expected Guid
            // =====================================================================
            migrationBuilder.Sql("""
                UPDATE "UserSetting"
                   SET "Id" = 'd3f5a7b2-1e4c-4f8a-9b6d-2c7e8f0a1b3d'
                 WHERE "Name" = 'DisplayMeasurementsNotification';
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse migration: uuid → bigint (data-preserving reverse is impractical,
            // so we truncate and recreate with identity columns)

            // ── 1. Drop all constraints ──
            migrationBuilder.DropForeignKey(name: "FK_PaymentFile_Payment_PaymentId", table: "PaymentFile");
            migrationBuilder.DropForeignKey(name: "FK_PaymentFile_PaymentGroup_PaymentGroupId", table: "PaymentFile");
            migrationBuilder.DropForeignKey(name: "FK_Payment_PaymentType_PaymentTypeId", table: "Payment");
            migrationBuilder.DropForeignKey(name: "FK_Payment_PaymentGroup_PaymentGroupId", table: "Payment");
            migrationBuilder.DropForeignKey(name: "FK_MeterMeasurement_MeterMeasurementType_MeterMeasurementTypeId", table: "MeterMeasurement");
            migrationBuilder.DropForeignKey(name: "FK_MeterMeasurementType_PaymentType_PaymentTypeId", table: "MeterMeasurementType");

            migrationBuilder.DropIndex(name: "IX_PaymentFile_PaymentId", table: "PaymentFile");
            migrationBuilder.DropIndex(name: "IX_PaymentFile_PaymentGroupId", table: "PaymentFile");
            migrationBuilder.DropIndex(name: "IX_Payment_PaymentTypeId", table: "Payment");
            migrationBuilder.DropIndex(name: "IX_Payment_PaymentGroupId", table: "Payment");
            migrationBuilder.DropIndex(name: "IX_MeterMeasurement_MeterMeasurementTypeId", table: "MeterMeasurement");
            migrationBuilder.DropIndex(name: "IX_MeterMeasurementType_PaymentTypeId", table: "MeterMeasurementType");

            migrationBuilder.DropCheckConstraint(name: "CK_PaymentFile_SingleLink", table: "PaymentFile");

            migrationBuilder.DropPrimaryKey(name: "PK_PaymentFile", table: "PaymentFile");
            migrationBuilder.DropPrimaryKey(name: "PK_MeterMeasurement", table: "MeterMeasurement");
            migrationBuilder.DropPrimaryKey(name: "PK_MeterMeasurementType", table: "MeterMeasurementType");
            migrationBuilder.DropPrimaryKey(name: "PK_Payment", table: "Payment");
            migrationBuilder.DropPrimaryKey(name: "PK_PaymentGroup", table: "PaymentGroup");
            migrationBuilder.DropPrimaryKey(name: "PK_PaymentType", table: "PaymentType");
            migrationBuilder.DropPrimaryKey(name: "PK_UserNotification", table: "UserNotification");
            migrationBuilder.DropPrimaryKey(name: "PK_UserSetting", table: "UserSetting");

            // ── 2. Truncate & convert uuid → bigint ──
            migrationBuilder.Sql("""
                TRUNCATE TABLE "PaymentFile";
                TRUNCATE TABLE "MeterMeasurement";
                TRUNCATE TABLE "Payment";
                TRUNCATE TABLE "MeterMeasurementType";
                TRUNCATE TABLE "PaymentGroup";
                TRUNCATE TABLE "PaymentType";
                TRUNCATE TABLE "UserNotification";
                TRUNCATE TABLE "UserSetting";
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "Payment"              DROP COLUMN "PaymentTypeId";
                ALTER TABLE "Payment"              DROP COLUMN "PaymentGroupId";
                ALTER TABLE "MeterMeasurementType" DROP COLUMN "PaymentTypeId";
                ALTER TABLE "MeterMeasurement"     DROP COLUMN "MeterMeasurementTypeId";
                ALTER TABLE "PaymentFile"          DROP COLUMN "PaymentId";
                ALTER TABLE "PaymentFile"          DROP COLUMN "PaymentGroupId";

                ALTER TABLE "PaymentType"          DROP COLUMN "Id";
                ALTER TABLE "PaymentGroup"         DROP COLUMN "Id";
                ALTER TABLE "Payment"              DROP COLUMN "Id";
                ALTER TABLE "MeterMeasurementType" DROP COLUMN "Id";
                ALTER TABLE "MeterMeasurement"     DROP COLUMN "Id";
                ALTER TABLE "PaymentFile"          DROP COLUMN "Id";
                ALTER TABLE "UserSetting"          DROP COLUMN "Id";
                ALTER TABLE "UserNotification"     DROP COLUMN "Id";

                ALTER TABLE "PaymentType"          ADD COLUMN "Id" bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL;
                ALTER TABLE "PaymentGroup"         ADD COLUMN "Id" bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL;
                ALTER TABLE "Payment"              ADD COLUMN "Id" bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL;
                ALTER TABLE "MeterMeasurementType" ADD COLUMN "Id" bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL;
                ALTER TABLE "MeterMeasurement"     ADD COLUMN "Id" bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL;
                ALTER TABLE "PaymentFile"          ADD COLUMN "Id" bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL;
                ALTER TABLE "UserSetting"          ADD COLUMN "Id" bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL;
                ALTER TABLE "UserNotification"     ADD COLUMN "Id" bigint GENERATED BY DEFAULT AS IDENTITY NOT NULL;

                ALTER TABLE "Payment"              ADD COLUMN "PaymentTypeId" bigint NOT NULL DEFAULT 0;
                ALTER TABLE "Payment"              ADD COLUMN "PaymentGroupId" bigint;
                ALTER TABLE "MeterMeasurementType" ADD COLUMN "PaymentTypeId" bigint NOT NULL DEFAULT 0;
                ALTER TABLE "MeterMeasurement"     ADD COLUMN "MeterMeasurementTypeId" bigint NOT NULL DEFAULT 0;
                ALTER TABLE "PaymentFile"          ADD COLUMN "PaymentId" bigint;
                ALTER TABLE "PaymentFile"          ADD COLUMN "PaymentGroupId" bigint;

                ALTER TABLE "Payment"              ALTER COLUMN "PaymentTypeId" DROP DEFAULT;
                ALTER TABLE "MeterMeasurementType" ALTER COLUMN "PaymentTypeId" DROP DEFAULT;
                ALTER TABLE "MeterMeasurement"     ALTER COLUMN "MeterMeasurementTypeId" DROP DEFAULT;
            """);

            // ── 3. Re-create constraints ──
            migrationBuilder.AddPrimaryKey(name: "PK_UserSetting", table: "UserSetting", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_UserNotification", table: "UserNotification", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_PaymentType", table: "PaymentType", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_PaymentGroup", table: "PaymentGroup", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_Payment", table: "Payment", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_MeterMeasurementType", table: "MeterMeasurementType", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_MeterMeasurement", table: "MeterMeasurement", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_PaymentFile", table: "PaymentFile", column: "Id");

            migrationBuilder.CreateIndex(name: "IX_Payment_PaymentTypeId", table: "Payment", column: "PaymentTypeId");
            migrationBuilder.CreateIndex(name: "IX_Payment_PaymentGroupId", table: "Payment", column: "PaymentGroupId");
            migrationBuilder.CreateIndex(name: "IX_MeterMeasurementType_PaymentTypeId", table: "MeterMeasurementType", column: "PaymentTypeId");
            migrationBuilder.CreateIndex(name: "IX_MeterMeasurement_MeterMeasurementTypeId", table: "MeterMeasurement", column: "MeterMeasurementTypeId");
            migrationBuilder.CreateIndex(name: "IX_PaymentFile_PaymentId", table: "PaymentFile", column: "PaymentId", unique: true, filter: "\"PaymentId\" IS NOT NULL");
            migrationBuilder.CreateIndex(name: "IX_PaymentFile_PaymentGroupId", table: "PaymentFile", column: "PaymentGroupId", unique: true, filter: "\"PaymentGroupId\" IS NOT NULL");

            migrationBuilder.AddForeignKey(name: "FK_Payment_PaymentType_PaymentTypeId", table: "Payment", column: "PaymentTypeId", principalTable: "PaymentType", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_Payment_PaymentGroup_PaymentGroupId", table: "Payment", column: "PaymentGroupId", principalTable: "PaymentGroup", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_MeterMeasurementType_PaymentType_PaymentTypeId", table: "MeterMeasurementType", column: "PaymentTypeId", principalTable: "PaymentType", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_MeterMeasurement_MeterMeasurementType_MeterMeasurementTypeId", table: "MeterMeasurement", column: "MeterMeasurementTypeId", principalTable: "MeterMeasurementType", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_PaymentFile_Payment_PaymentId", table: "PaymentFile", column: "PaymentId", principalTable: "Payment", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(name: "FK_PaymentFile_PaymentGroup_PaymentGroupId", table: "PaymentFile", column: "PaymentGroupId", principalTable: "PaymentGroup", principalColumn: "Id", onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddCheckConstraint(name: "CK_PaymentFile_SingleLink", table: "PaymentFile", sql: "(\"PaymentId\" IS NOT NULL AND \"PaymentGroupId\" IS NULL) OR (\"PaymentId\" IS NULL AND \"PaymentGroupId\" IS NOT NULL)");

            // ── 4. Re-insert seed ──
            migrationBuilder.InsertData(
                table: "UserSetting",
                columns: new[] { "Id", "CreatedOn", "DisplayName", "Name", "RawValue", "TypeName" },
                values: new object[] { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Отображать уведомления по показаниям", "DisplayMeasurementsNotification", "true", "Boolean" });
        }
    }
}
