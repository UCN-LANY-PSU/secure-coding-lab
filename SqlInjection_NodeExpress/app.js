import express from "express";
import helmet from "helmet";
import Database from "better-sqlite3";

const app = express();
app.use(helmet());
app.use(express.json());

const db = new Database("books.db");
db.exec(`
  CREATE TABLE IF NOT EXISTS books (
    id INTEGER PRIMARY KEY,
    title TEXT NOT NULL,
    author TEXT NOT NULL
  );

  CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY,
    username TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL
  );
`);
const seed_books = db.prepare("INSERT INTO books (title, author) VALUES (?, ?)");
if (!db.prepare("SELECT 1 FROM books LIMIT 1").get()) {
  seed_books.run("The Great Gatsby", "F. Scott Fitzgerald");
  seed_books.run("To Kill a Mockingbird", "Harper Lee");
  seed_books.run("1984", "George Orwell");
}
const seed_users = db.prepare("INSERT INTO users (username, password_hash) VALUES (?, ?)");
if (!db.prepare("SELECT 1 FROM users LIMIT 1").get()) {
  seed_users.run("admin", "hashed_password_123");
  seed_users.run("alice", "hashed_super_strong_password");
  seed_users.run("bob", "hashed_bob_password");
}

app.get("/search", (req, res) => {
  const term = (req.query.term ?? "").toString();
  const sql = "SELECT title, author FROM books WHERE title LIKE '%" + term + "%'";
  try {
    const rows = db.prepare(sql).all(); 
    res.json(rows);
  } catch (e) {
    res.status(500).json({ error: `DB-fejl: ${e.message}` });
  }
});

app.listen(3000, () => {
  console.log("Lytter på http://localhost:3000");
});
