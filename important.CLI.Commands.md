*to build entitties from an existing table (DB first)*
 `dotnet ef dbcontext scaffold <connection_string> Npgsql.EntityFrameworkCore.PostgreSQL -o Models`

DEV:

*psql*
 `psql -U postgres` : connect to PG via terminal as a postgres user
 `\c link_shortener` : connect to link_shortner db
 `\d` : list schemas
 `\d table_name` : describe table