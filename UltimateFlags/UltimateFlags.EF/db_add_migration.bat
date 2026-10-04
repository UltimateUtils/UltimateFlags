@REM Installing Microsoft.EntityFrameworkCore.Design nuget package.
@REM Run this script from your Host project's directory.

dotnet ef migrations add <MigrationName> -o Db/Migrations
