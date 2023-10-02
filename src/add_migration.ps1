param (
    [Parameter(Mandatory=$true)]
    [string]$name
)

if ([string]::IsNullOrWhiteSpace($name)) {
    Write-Error "Cannot create a migration becuase the migration name is null or empty"
} 
else
{
    dotnet-ef migrations add $name --context ApplicationDbContext --startup-project Tech2023.Web --project Tech2023.DAL.Migrations --configuration release
}
