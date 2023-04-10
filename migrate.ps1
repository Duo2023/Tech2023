$migrationName=$args[0];

dotnet-ef migrations add $migrationName --context ApplicationDbContext --startup-project src/Tech2023.Web.API --project src/Tech2023.DAL.Migrations --configuration release