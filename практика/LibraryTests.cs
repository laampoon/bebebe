using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace практика
{
    using Xunit;
    public class LibraryTests
    {
        //выдача
        [Fact]
        public void BorrowBook_ShouldMakeBookUnavailable()
        {
            Library library = new Library();
            Reader reader = new Reader("Иван");
            Book book = library.Books[0];

            library.BorrowBook(book, reader);

            Assert.False(book.IsAvailable);
            Assert.Single(reader.BorrowedBooks);
        }

        //возврат
        [Fact]
        public void ReturnBook_ShouldMakeBookAvailable()
        {
            Library library = new Library();
            Reader reader = new Reader("Иван");
            Book book = library.Books[0];

            library.BorrowBook(book, reader);
            library.ReturnBook(book, reader);

            Assert.True(book.IsAvailable);
            Assert.Empty(reader.BorrowedBooks);
        }

        //запрет занятой книги
        [Fact]
        public void BorrowBook_WhenBookIsUnavailable_ShouldThrowException()
        {
            Library library = new Library();
            Reader reader1 = new Reader("Иван");
            Reader reader2 = new Reader("Анна");
            Book book = library.Books[0];

            library.BorrowBook(book, reader1);

            Assert.Throws<System.InvalidOperationException>(
                () => library.BorrowBook(book, reader2));
        }

        //ограничение 3 книги
        [Fact]
        public void BorrowBook_WhenLimitExceeded_ShouldThrowException()
        {
            Library library = new Library();
            Reader reader = new Reader("Иван");

            library.BorrowBook(library.Books[0], reader);
            library.BorrowBook(library.Books[1], reader);
            library.BorrowBook(library.Books[2], reader);

            Assert.Throws<System.InvalidOperationException>(
                () => library.BorrowBook(library.Books[3], reader));
        }

        //можно или нельзя взять 3 книги
        [Fact]
        public void BorrowBook_ThreeBooks_ShouldWork()
        {
            Library library = new Library();
            Reader reader = new Reader("Иван");

            library.BorrowBook(library.Books[0], reader);
            library.BorrowBook(library.Books[1], reader);
            library.BorrowBook(library.Books[2], reader);

            Assert.Equal(3, reader.BorrowedBooks.Count);
        }

        //возврат чужой книги
        [Fact]
        public void ReturnBook_WhenReaderDoesNotHaveBook_ShouldThrowException()
        {
            Library library = new Library();
            Reader reader = new Reader("Иван");
            Book book = library.Books[0];

            Assert.Throws<System.InvalidOperationException>(
                () => library.ReturnBook(book, reader));
        }
    }
}
