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
    var hasAnyBooks = db.ExecuteScalar<int>("SELECT COUNT(1) FROM books");
    if (hasAnyBooks == 0)
    {
        db.Execute("INSERT INTO books (title, author) VALUES (@t,@a)",
            new[] {
                new { t = "The Great Gatsby", a = "F. Scott Fitzgerald" },
                new { t = "To Kill a Mockingbird", a = "Harper Lee" },
                new { t = "1984", a = "George Orwell" }
            });
    }
    db.Execute("""
        CREATE TABLE IF NOT EXISTS users (
          id INTEGER PRIMARY KEY,
          username TEXT NOT NULL,
          password_hash TEXT NOT NULL
        );
    """);
    var hasAnyUsers = db.ExecuteScalar<int>("SELECT COUNT(1) FROM users");
    if (hasAnyUsers == 0)
    {
        db.Execute("INSERT INTO users (username, password_hash) VALUES (@t,@a)",
            new[] {
                new { t = "admin", a = "482c811da5d5b4bc6d497ffa98491e38" },
                new { t = "alice", a = "68b08847ad96dcd958117d03828ee75c" },
                new { t = "bob", a = "12b141f35d58b8b3a46eea65e6ac179e" }
            });
    }
}

app.MapGet("/search", async (string term, IDbConnection db) =>
{
    var sql = $"SELECT title, author FROM books WHERE title LIKE '%{term}%'";
    var rows = await db.QueryAsync(sql); 
    return Results.Ok(rows);
});

app.Run();
