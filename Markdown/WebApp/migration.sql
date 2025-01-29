CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250128225813_initial') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL,
        "Login" character varying(50) NOT NULL,
        "PasswordHash" character varying(70) NOT NULL,
        "Role" character varying(20) NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250128225813_initial') THEN
    CREATE TABLE "Documents" (
        "Id" uuid NOT NULL,
        "Name" character varying(255) NOT NULL,
        "AuthorId" uuid NOT NULL,
        "AuthorName" character varying(255) NOT NULL,
        "CreationDateTime" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Documents" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Documents_Users_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250128225813_initial') THEN
    CREATE TABLE "DocumentAccesses" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "DocumentId" uuid NOT NULL,
        CONSTRAINT "PK_DocumentAccesses" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_DocumentAccesses_Documents_DocumentId" FOREIGN KEY ("DocumentId") REFERENCES "Documents" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_DocumentAccesses_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250128225813_initial') THEN
    CREATE INDEX "IX_DocumentAccesses_DocumentId" ON "DocumentAccesses" ("DocumentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250128225813_initial') THEN
    CREATE UNIQUE INDEX "IX_DocumentAccesses_UserId_DocumentId" ON "DocumentAccesses" ("UserId", "DocumentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250128225813_initial') THEN
    CREATE INDEX "IX_Documents_AuthorId" ON "Documents" ("AuthorId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250128225813_initial') THEN
    CREATE UNIQUE INDEX "IX_Users_Login" ON "Users" ("Login");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250128225813_initial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20250128225813_initial', '9.0.1');
    END IF;
END $EF$;
COMMIT;

