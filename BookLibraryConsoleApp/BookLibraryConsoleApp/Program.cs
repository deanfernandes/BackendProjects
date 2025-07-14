using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BookLibraryConsoleApp
{
    internal class Program
    {
        static IConfiguration Configuration;

        static void Main()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();

            Configuration = builder.Build();

            while (true)
            {
                Console.WriteLine("\nBook Library");
                Console.WriteLine("1. View all books");
                Console.WriteLine("2. Add a new book");
                Console.WriteLine("3. Update a book");
                Console.WriteLine("4. Remove a book");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ReadAllBooks(); break;
                    case "2": AddBook(); break;
                    case "3": UpdateBook(); break;
                    case "4": RemoveBook(); break;
                    case "5": return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        static void ReadAllBooks()
        {
            using (SqlConnection con = new(Configuration.GetConnectionString("BookLibraryDB")))
            {
                string query = "SELECT Id, Title, Author FROM Books";
                using (SqlCommand cmd = new(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Id: {reader["Id"]}, Title: {reader["Title"]}, Author: {reader["Author"]}");
                        }
                    }
                }

            }
        }

        static void AddBook()
        {
            Console.Write("Enter book title: ");
            string title = Console.ReadLine();

            Console.Write("Enter author name: ");
            string author = Console.ReadLine();

            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("BookLibraryDB")))
            {
                string query = "INSERT INTO Books (Title, Author) VALUES (@Title, @Author)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Author", author);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    Console.WriteLine(rows > 0 ? "Book added successfully!" : "Failed to add book.");
                }
            }
        }

        static void RemoveBook()
        {
            Console.Write("Enter the ID of the book to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("BookLibraryDB")))
            {
                string query = "DELETE FROM Books WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    Console.WriteLine(rows > 0 ? "Book removed successfully!" : "Book not found.");
                }
            }
        }

        static void UpdateBook()
        {
            Console.Write("Enter the ID of the book to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.Write("Enter new title: ");
            string title = Console.ReadLine();

            Console.Write("Enter new author: ");
            string author = Console.ReadLine();

            using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("BookLibraryDB")))
            {
                string query = "UPDATE Books SET Title = @Title, Author = @Author WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Author", author);
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    Console.WriteLine(rows > 0 ? "Book updated successfully!" : "Book not found.");
                }
            }
        }
    }
}
