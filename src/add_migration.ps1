$migrationName=$args[0];

if ([string]::IsNullOrWhiteSpace($migrationName)) {
    Write-Error "Cannot create a migration becuase the migration name is null or empty"
} 
else
{
    dotnet-ef migrations add $migrationName --context ApplicationDbContext --startup-project Tech2023.Web --project Tech2023.DAL.Migrations --configuration release
}
