using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace практика_библиотека
{

    public class Database
    {
        private readonly string connectionString =
            Environment.GetEnvironmentVariable("PRACTICE_Library")
            ?? Environment.GetEnvironmentVariable("PRACTICE_Library", EnvironmentVariableTarget.User)
            ?? @"Server=localhost\SQLEXPRESS02;Database=LibraryUWU;Trusted_Connection=True;TrustServerCertificate=True;";

        // Получение всех книг
        public List<Book> GetBooks()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                string query = @"
                SELECT
                    Books.IDBook AS ID,
                    Books.Title,
                    Books.Author,
                    Books.Year,
                    CASE
                        WHEN EXISTS (
                            SELECT 1
                            FROM Loans
                            WHERE Loans.IDBook = Books.IDBook
                            AND Loans.ReturnDate IS NULL
                        )
                        THEN CAST(0 AS bit)
                        ELSE CAST(1 AS bit)
                    END AS IsAvailable
                FROM Books";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Book book = new Book();
                    book.ID = (int)reader["ID"];
                    book.Title = reader["Title"].ToString();
                    book.Author = reader["Author"].ToString();
                    book.Year = (int)reader["Year"];
                    book.IsAvailable = (bool)reader["IsAvailable"];

                    books.Add(book);
                }
            }

            return books;
        }

        // Получение всех читателей
        public List<Reader> GetUsers()
        {
            List<Reader> users = new List<Reader>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT IDReader AS ID, FullName FROM Readers";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Reader user = new Reader();
                    user.ID = (int)reader["ID"];
                    user.FullName = reader["FullName"].ToString();

                    users.Add(user);
                }
            }

            return users;
        }

        // Добавление книги
        public void AddBook(string title, string author, int year)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                INSERT INTO Books (Title, Author, Year)
                VALUES (@title, @author, @year)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@author", author);
                command.Parameters.AddWithValue("@year", year);

                command.ExecuteNonQuery();
            }
        }

        // Добавление читателя
        public void AddUser(string fullName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                INSERT INTO Readers (FullName)
                VALUES (@name)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", fullName);

                command.ExecuteNonQuery();
            }
        }

        // Выдача книги читателю
        public void BorrowBook(int bookId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Проверяем наличие книги
                string query = "SELECT COUNT(*) FROM Books WHERE IDBook = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", bookId);

                int bookCount = (int)command.ExecuteScalar();

                if (bookCount == 0)
                    throw new Exception("Такой книги нет!");

                // Проверяем наличие читателя
                query = "SELECT COUNT(*) FROM Readers WHERE IDReader = @id";

                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", userId);

                int userCount = (int)command.ExecuteScalar();

                if (userCount == 0)
                    throw new Exception("Такого пользователя нет!");

                // Проверяем, не выдана ли уже книга
                query = @"
                SELECT COUNT(*)
                FROM Loans
                WHERE IDBook = @id
                AND ReturnDate IS NULL";

                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", bookId);

                int loanCount = (int)command.ExecuteScalar();

                if (loanCount > 0)
                    throw new Exception("Эта книга уже занята!");

                // Выдаём книгу
                query = @"
                INSERT INTO Loans (IDBook, IDReader, IssueDate)
                VALUES (@idbook, @iduser, @date)";

                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idbook", bookId);
                command.Parameters.AddWithValue("@iduser", userId);
                command.Parameters.AddWithValue("@date", DateTime.Today);

                command.ExecuteNonQuery();
            }
        }

        // Возврат книги
        public void ReturnBook(int bookId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                UPDATE Loans
                SET ReturnDate = @date
                WHERE IDBook = @id
                AND ReturnDate IS NULL";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@date", DateTime.Today);
                command.Parameters.AddWithValue("@id", bookId);

                int result = command.ExecuteNonQuery();

                if (result == 0)
                    throw new Exception("Эта книга уже свободна!");
            }
        }
    }
}
