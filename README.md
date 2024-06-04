<h1>Link Shortener Service</h1>

**tech**: C# .NET, ASP.NET Core <br>
**db**: Postgresql, EF Core

**!!!** The project is commited without appsettings.json file, containing DB connection credentials. <br>
DB connection string needs to be configured first, before runnig the app.

</br>
<h3>IMPORTANT CLI COMMANDS</h3>

**ef-core** </br>
build entitties from an existing table (DB first approach) </br>
 `dotnet ef dbcontext scaffold <connection_string> Npgsql.EntityFrameworkCore.PostgreSQL -o Models`

**psql** </br>
 `psql -U postgres` : connect to PG via terminal as a postgres user </br>
 `\c link_shortener` : connect to link_shortner db </br>
 `\d` : list schemas </br>
 `\d table_name` : describe table </br>

**dotnet**</br>
`dotnet watch -lp https` : start the app with auto hot reload, use https as a launch profile

