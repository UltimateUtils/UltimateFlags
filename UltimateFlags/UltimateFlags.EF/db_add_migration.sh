#!/bin/bash

# Installing Microsoft.EntityFrameworkCore.Design nuget package.
# Run this script from your Host project's directory.

dotnet ef migrations add <MigrationName> -o Db/Migrations
