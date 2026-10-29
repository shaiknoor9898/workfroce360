using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkForce360.API.Migrations
{
    /// <inheritdoc />
    public partial class RestoreMissingUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'[dbo].[Users]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[Users]
                    (
                        [Id] int NOT NULL IDENTITY(1, 1),
                        [Email] nvarchar(254) NOT NULL,
                        [PasswordHash] nvarchar(512) NOT NULL,
                        [Role] nvarchar(20) NOT NULL,
                        [CreatedAtUtc] datetime2 NOT NULL,
                        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
                    );
                END;

                IF OBJECT_ID(N'[dbo].[Users]', N'U') IS NOT NULL
                   AND NOT EXISTS
                   (
                       SELECT 1
                       FROM sys.indexes
                       WHERE [object_id] = OBJECT_ID(N'[dbo].[Users]', N'U')
                         AND [name] = N'UX_Users_Email'
                   )
                BEGIN
                    CREATE UNIQUE INDEX [UX_Users_Email]
                        ON [dbo].[Users] ([Email]);
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // The original migration owns Users; rolling back this repair must not drop its data.
        }
    }
}
