using System;
using System.Collections.Generic;
using System.Linq;

public class Book
{
    public string Name { get; private set; }
    public string Author { get; private set; }
    public string ISBN { get; private set; }
    public bool Available { get; private set; }

    public Book(string name, string author, string isbn)
    {
        Name = name;
        Author = author;
        ISBN = isbn;
        Available = true;
    }

    public void ChangeAvailability(bool status)
    {
        Available = status;
    }
}

public class LibraryMember
{
    public string FullName { get; private set; }
    private List<Book> BorrowedBooks { get; set; }

    public LibraryMember(string fullName)
    {
        FullName = fullName;
        BorrowedBooks = new List<Book>();
    }

    public void BorrowBook(Book book)
    {
        if (BorrowedBooks.Count >= 5)
        {
            Console.WriteLine($"Пользователь {FullName} не может взять больше 5 книг.");
            return;
        }

        if (book.Available)
        {
            BorrowedBooks.Add(book);
            book.ChangeAvailability(false);
            Console.WriteLine($"Книга '{book.Name}' взята {FullName}.");
        }
        else
        {
            Console.WriteLine($"Книга '{book.Name}' уже занята.");
        }
    }

    public void ReturnBook(Book book)
    {
        if (BorrowedBooks.Contains(book))
        {
            BorrowedBooks.Remove(book);
            book.ChangeAvailability(true);
            Console.WriteLine($"Книга '{book.Name}' возвращена {FullName}.");
        }
        else
        {
            Console.WriteLine($"Пользователь {FullName} не арендовал книгу '{book.Name}'.");
        }
    }
}

public class Library
{
    private List<Book> Inventory { get; set; }

    public Library()
    {
        Inventory = new List<Book>();
    }

    public void AddBook(Book book)
    {
        Inventory.Add(book);
        Console.WriteLine($"Книга '{book.Name}' добавлена в библиотеку.");
    }

    public void ShowBooks()
    {
        Console.WriteLine("Книги в библиотеке:");
        foreach (var book in Inventory)
        {
            var status = book.Available ? "Доступна" : "Занята";
            Console.WriteLine($"{book.Name} — {book.Author} ({status})");
        }
    }

    public Book FindBook(string name)
    {
        return Inventory.FirstOrDefault(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}

class Program
{
    static void Main(string[] args)
    {
        var library = new Library();
        library.AddBook(new Book("Слова назидания", "Абай К.", "44"));
        library.AddBook(new Book("Путь Абая", "Мухтар А.", "002"));

        var member = new LibraryMember("Bruh");

        library.ShowBooks();
        var book = library.FindBook("1961");
        member.BorrowBook(book);

        library.ShowBooks();
        member.ReturnBook(book);

        library.ShowBooks();
    }
}
