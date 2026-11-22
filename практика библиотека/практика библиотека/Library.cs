using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace практика_библиотека
{
    public class Library
    {
        Database database = new Database();

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n========== БИБЛИОТЕКА ==========");
                Console.WriteLine("1. Показать все книги");
                Console.WriteLine("2. Показать свободные книги");
                Console.WriteLine("3. Показать занятые книги");
                Console.WriteLine("4. Выдать книгу");
                Console.WriteLine("5. Вернуть книгу");
                Console.WriteLine("6. Добавить книгу");
                Console.WriteLine("7. Фильтрация книг");
                Console.WriteLine("8. Сортировка книг");
                Console.WriteLine("9. Добавить пользователя");
                Console.WriteLine("0. Выход");

                Console.Write("\nВыберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ShowBooks(database.GetBooks());
                            break;

                        case "2":
                            ShowBooks(database.GetBooks()
                                .Where(book => book.IsAvailable).ToList());
                            break;

                        case "3":
                            ShowBooks(database.GetBooks()
                                .Where(book => !book.IsAvailable).ToList());
                            break;

                        case "4":
                            BorrowBook();
                            break;

                        case "5":
                            ReturnBook();
                            break;

                        case "6":
                            AddBook();
                            break;

                        case "7":
                            FilterBooks();
                            break;

                        case "8":
                            SortBooks();
                            break;

                        case "9":
                            AddUser();
                            break;

                        case "0":
                            Console.WriteLine("Выход из программы.");
                            return;

                        default:
                            Console.WriteLine("Такого пункта меню нет!");
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Ошибка SQL: " + ex.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

        public void ShowBooks(List<Book> books)
        {
            Console.WriteLine("\n========== СПИСОК КНИГ ==========");

            if (books.Count == 0)
            {
                Console.WriteLine("Книг нет.");
                return;
            }

            foreach (Book book in books)
            {
                string status;

                if (book.IsAvailable)
                {
                    status = "Свободна";
                }
                else
                {
                    status = "Занята";
                }

                Console.WriteLine(
                    book.ID + ". " +
                    book.Title + " — " +
                    book.Author + " (" +
                    book.Year + ") [" +
                    status + "]");
            }
        }

        public void BorrowBook()
        {
            ShowBooks(database.GetBooks());

            Console.Write("\nВведите ID книги: ");

            int bookId;

            if (!int.TryParse(Console.ReadLine(), out bookId))
            {
                Console.WriteLine("Нужно ввести число!");
                return;
            }

            Console.WriteLine("\n========== ПОЛЬЗОВАТЕЛИ ==========");

            List<Reader> users = database.GetUsers();

            foreach (Reader user in users)
            {
                Console.WriteLine(user.ID + ". " + user.FullName);
            }

            Console.Write("\nВведите ID пользователя: ");

            int userId;

            if (!int.TryParse(Console.ReadLine(), out userId))
            {
                Console.WriteLine("Нужно ввести число!");
                return;
            }

            database.BorrowBook(bookId, userId);

            Console.WriteLine("Книга успешно выдана!");
        }

        public void ReturnBook()
        {
            List<Book> books = database.GetBooks();

            ShowBooks(books.Where(book => !book.IsAvailable).ToList());

            Console.Write("\nВведите ID книги: ");

            int bookId;

            if (!int.TryParse(Console.ReadLine(), out bookId))
            {
                Console.WriteLine("Нужно ввести число!");
                return;
            }

            database.ReturnBook(bookId);

            Console.WriteLine("Книга успешно возвращена!");
        }

        public void AddBook()
        {
            Console.Write("\nВведите название книги: ");
            string title = Console.ReadLine();

            Console.Write("Введите автора: ");
            string author = Console.ReadLine();

            Console.Write("Введите год издания: ");

            int year;

            if (!int.TryParse(Console.ReadLine(), out year))
            {
                Console.WriteLine("Неверный год!");
                return;
            }

            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(author) ||
                year <= 0)
            {
                Console.WriteLine("Заполните все поля правильно!");
                return;
            }

            database.AddBook(title, author, year);

            Console.WriteLine("Книга успешно добавлена!");
        }

        public void FilterBooks()
        {
            Console.Write("\nВведите год издания: ");

            int year;

            if (!int.TryParse(Console.ReadLine(), out year))
            {
                Console.WriteLine("Нужно ввести число!");
                return;
            }

            List<Book> books = database.GetBooks();

            List<Book> result = books
                .Where(book => book.Year == year)
                .ToList();

            ShowBooks(result);
        }

        public void SortBooks()
        {
            Console.WriteLine("\n1. По названию");
            Console.WriteLine("2. По автору");

            Console.Write("Выберите сортировку: ");

            string choice = Console.ReadLine();

            List<Book> books = database.GetBooks();

            if (choice == "1")
            {
                books = books.OrderBy(book => book.Title).ToList();
            }
            else if (choice == "2")
            {
                books = books.OrderBy(book => book.Author).ToList();
            }
            else
            {
                Console.WriteLine("Неверный пункт!");
                return;
            }

            ShowBooks(books);
        }

        public void AddUser()
        {
            Console.Write("\nВведите ФИО пользователя: ");

            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя не может быть пустым!");
                return;
            }

            database.AddUser(name);

            Console.WriteLine("Пользователь успешно добавлен!");
        }
    }
}

