# Tech2023
# NCEA & Cambridge Revision Helper
![GitHub Workflow Status (with branch)](https://img.shields.io/github/actions/workflow/status/Duo2023/Tech2023/dotnet.yml)
![GitHub last commit](https://img.shields.io/github/last-commit/Duo2023/Tech2023)
![GitHub repo size](https://img.shields.io/github/repo-size/Duo2023/Tech2023)

## Prerequisites
- NET 7 SDK (https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
</br>
This dependency is guaranteed regardless of what you do with the project, as it is required for building.

Other prequisites are recommended but can be swapped out or ignored.
- The web api requires SQL server (https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or a different DB which can be swapped out using ef configurations.
- For native, Avalonia is needed so you can install the VS extension at (https://avaloniaui.net/GettingStarted#installation)
- TypeScript is used on the front end web application, but you could just use the native applications + web api instead.
- Tailwind CSS is also a front end requirement.
- Both TypeScript and Tailwind depend on NodeJS so you would also need to install it (https://nodejs.org/en/download)

## Getting Started
To get started, you will need a valid email account for the web api to use, I recommend outlook during debugging and testing.

Then in Visual Studio access the user secrets by right clicking on the Tech2023.Web.API project

<img src="./assets/img/getting-started-user-secrets.png" width="300">

After that copy in the contents and change the 'FromEmail', 'Username' and 'Password' to your own
```jsonc
{
  "EmailOptions": {
    "FromEmail": "example@outlook.com", /* Your own email */
    "Port": 587, /* Default SMTP outlook setting */
    "SenderName": "Tech2023", /* It doesn't really matter what you use */
    "SmtpServer": "smtp-mail.outlook.com", /* Your own SMTP server if you want to use something else */
    "Username": "[outlook-user]", /* Your smtp username */
    "Password": "[outlook-password]" /* Your password */
  }
}
```

## Technologies Used
<p float="left">
  <img src="./assets/img/dotnet-logo.png" width="100" />
  <img src="./assets/img/ef-core-logo.png" width="100" />
  <img src="./assets/img/sql-server-logo.png" width="100"/>
</p>

<p float="left">
   <img src="./assets/img/node-js-logo.png" width="100"/>
   <img src="./assets/img/typescript-logo.png" width="100"/>
   <img src="./assets/img/tailwind-logo.png" width="100"/>
</p>
