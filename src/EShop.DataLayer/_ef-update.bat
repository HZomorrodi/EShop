 @echo off
dotnet ef migrations add %1 --startup-project ..\EShop.Web
dotnet ef database update --startup-project ..\EShop.Web

