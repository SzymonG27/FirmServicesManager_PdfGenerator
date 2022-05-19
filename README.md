# FirmServicesManager_PdfGenerator

This is an .NET5.0 application that was created to manage the company's orders. After accepting the order, the project generates a pdf file
with information about the order and barcode. Company can easily find a record by scanning generated barcode. If company change order status, 
customer will get information about it on e-mail address.


## Technologies

- .NET 5.0
- MimeKit
- iText7
- EntityFramework


## Installation

For local installation you must create databases from context.

In command prompt (for local usage) you must go to the directory with project and write:
```
dotnet ef database update --context AppIdentityDbContext
dotnet ef database update --context ApplicationDbContext
```
Now you must do account in PDF_Users database with RCON role and that's it!

For implement this project on web hosting like Azure you can check [This tutorial](https://docs.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?tabs=azure-portal%2Cvisualstudio-deploy%2Cdeploy-instructions-azure-portal%2Cazure-portal-logs%2Cazure-portal-resources)

To configure email sender for your mail you must open the `appsettings.json` file and change settings in `EmailSettings`
```json
"EmailSettings": {
    "From": "",
    "SmtpServer": "",
    "Port": ,
    "Username": "",
    "Password": ""
}
```


## License
[MIT](https://choosealicense.com/licenses/mit/)
