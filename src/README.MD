This folder contains all the executable source code and libraries built with it

# Scripts in /src
## Before running any other script
run ./setup.ps1
<br/>
This will install or update your EF tools for the scripts to work.

## Add and Remove Migrations
To add a migration run the command ./add_migration.ps1 [name].<br/>
To revert a migration run ./remove_migration.ps1
