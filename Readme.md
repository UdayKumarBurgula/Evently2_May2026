


My Project Sample 8.0
-----------------------------------

1. Create Empty Solution
2. Add .EditorConfig, DirectoryBuild
3. Add New folders : src, src/API, src/Modules, src/Modules/Events
4. Add .NET CORE WebAPI project "Events.Api" insider API folder, Add new class-library "Events.Module.Events.Api" in src/Modules/Events folder - add related Events Module code inside Events folder.
5. Add New Classes: Event.cs,EventStatus.cs and properties
6. Add nugget package: Npgsql.EntityFrameworkCore.PostgreSQL
7. Add EventsDbContext.cs, OnModelCreating method and add DbSet<Event> Events { get; set; }
8. Add Microsoft.AspNetCore.App "Events.Module.Events.Api" project
9. Add Event handler : CreateEvent.cs and related code for creating event
10. Add GetEvents.cs and related code for getting events
11. Add MapEndPoint methods for above handlers in Events.Module.Events.Api project
12. Add reference of Events.Module.Events.Api project in Events.Api project
13. Add new class EventsModule.cs in Events.Api project and add code for registering Events Module
14. Add EFCore.NamingConvention package in Events.Api projects and add code for using snake case naming convention in EventsDbContext.cs file
15. Integrate Events module into API project - Add MapEventsModule() method in Program.cs and call it to register Events Module
16. Add connection string in appsettings.json file and add code for using PostgreSQL database in EventsDbContext.cs file
17. Add reference nugget package Microsoft.EntityFramework.Tools in Api project and add code for using EF Core tools in EventsDbContext.cs file
18. Package Manager Console : Add-Migration Create_Database -Context EventsDbContext -o Database/Migrations
	