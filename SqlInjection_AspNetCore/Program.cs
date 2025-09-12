using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// DB-factory
builder.Services.AddScoped<IDbConnection>(_ =>
{
    var conn = new SqliteConnection("Data Source=books.db;Cache=Shared");
    conn.Open();
    return conn;
});

var app = builder.Build();

// Init DB og seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    db.Execute("""
        CREATE TABLE IF NOT EXISTS books (
          id INTEGER PRIMARY KEY,
          title TEXT NOT NULL,
          author TEXT NOT NULL
        );
    """);
    var hasAny = db.ExecuteScalar<int>("SELECT COUNT(1) FROM books");
    if (hasAny == 0)
    {
        db.Execute("INSERT INTO books (title, author) VALUES (@t,@a)",
            new[] {
                new { t = "The Great Gatsby", a = "F. Scott Fitzgerald" },
                new { t = "To Kill a Mockingbird", a = "Harper Lee" },
                new { t = "1984", a = "George Orwell" }
            });
    }
}

app.MapGet("/search", async (string term, IDbConnection db) =>
{
    var sql = $"SELECT title, author FROM books WHERE title LIKE '%{term}%'";
    var rows = await db.QueryAsync(sql); // sårbar for SQLi
    return Results.Ok(rows);
});

app.Run();
