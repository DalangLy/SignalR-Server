# SignalR-Server
The is sample for signal R Server on ASP.NET Framework Web Application

# Note
- .Net Framework 4.5.2
- ASP.NET Web Application Web API MVC with Authentication Individual User Accounts

# Introduction
- Restore database
- edit sql server connection string in Web.config file
- install "Microsoft.AspNet.SignalR" Nuget
- add "app.MapSignalR();" to QWIN Startup.cs
- create folder name "Hubs" to store all signalR Hub
- call singalR hub anywhere u want, but for me i call from controller
