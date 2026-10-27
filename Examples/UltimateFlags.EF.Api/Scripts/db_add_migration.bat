@REM Run this script from your Host project's directory
@REM after installing Microsoft.EntityFrameworkCore.Design nuget package

dotnet ef migrations add <MigrationName> -o Db/Migrations
