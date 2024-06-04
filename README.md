<h1>Link Shortener Service</h1>

**tech**: C# .NET, ASP.NET Core <br>
**db**: Postgresql, EF Core

<h2>Startup and Configuration</h2>
git clone this repo </br>

cd to `./linkApi` </br> </br>

**for dev** </br>
- rename `appsettings.json` to `appsettings.Development.json`, add credentials for db connection </br>
- run `dotnet watch -lp dev` for auto hot reload as well</br>
</br>

**for prod** </br>
- add credentials for db connection to `appsettings.json` </br>
- run `dotnet run -lp prod`


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

