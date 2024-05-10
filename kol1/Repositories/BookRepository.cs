using System.Data.SqlClient;
using kol1.Models;
using kol1.Models.DTOs;
using Microsoft.AspNetCore.Components.Web;

namespace kol1.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IConfiguration _configuration;

    public BookRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    
    /*
    public async Task<bool> BookExists(int bookId)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@bookId", bookId);
               

                command.CommandText =
                    "SELECT COUNT(*) FROM books WHERE PK = @bookId";
                
                Console.WriteLine((int)command.ExecuteScalar());
               
                return (int)command.ExecuteScalar() > 0;
            }
        }
    }
    */
    public async Task<bool> BookExists(int bookId)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        await connection.OpenAsync();

        command.Connection = connection;
                
        command.Parameters.AddWithValue("@bookId", bookId);
               

        command.CommandText =
                    "SELECT COUNT(*) FROM books WHERE PK = @bookId";
        
               
        return (int) await command.ExecuteScalarAsync() > 0;
        
    }
    /*
    public async Task<BookInfo> getBook(int bookID)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@bookID", bookID);
               

                command.CommandText =
                    "SELECT g.name FROM genres g JOIN books_genres bg ON bg.FK_genre = g.PK WHERE bg.FK_book = @bookID";
                
                var reader = command.ExecuteReader();

                var listOfGenres = new List<string>();
                    
                while (reader.Read())
                {
                    listOfGenres.Add(
                        reader["name"].ToString()
                    );
                }
                
                reader.Close();
                
                command.CommandText =
                    "SELECT PK, title FROM books WHERE PK = @bookID";

                reader = command.ExecuteReader();

                BookInfo bookInfo = null;
                
                while (reader.Read())
                {
                    bookInfo = new BookInfo()
                    {
                        id = (int)reader["PK"],
                        title = reader["title"].ToString(),
                        genres = listOfGenres
                    };

                }
                
                
               
                return bookInfo;
            }
        }
    }
*/
    
    public async Task<BookInfo> getBook(int bookID)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        await connection.OpenAsync();
          

           
                command.Connection = connection;
                
                command.Parameters.AddWithValue("@bookID", bookID);
               

                command.CommandText =
                    "SELECT g.name FROM genres g JOIN books_genres bg ON bg.FK_genre = g.PK WHERE bg.FK_book = @bookID";
                
                var reader =  await command.ExecuteReaderAsync();

                var listOfGenres = new List<string>();
                    
                while (await reader.ReadAsync())
                {
                    listOfGenres.Add(
                        reader["name"].ToString()
                    );
                }
                
                reader.CloseAsync();
                
                command.CommandText =
                    "SELECT PK, title FROM books WHERE PK = @bookID";

                reader = await command.ExecuteReaderAsync();

                BookInfo bookInfo = null;
                
                while (await reader.ReadAsync())
                {
                    bookInfo = new BookInfo()
                    {
                        id = (int)reader["PK"],
                        title = reader["title"].ToString(),
                        genres = listOfGenres
                    };

                }
                
                
               
                return bookInfo;
            
        
    }
 /*   public bool AddBock(AddBook book)
    {
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction();

            try
            {



                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.Transaction = transaction;


                    command.Parameters.AddWithValue("@title",  book.title);
                    
                   

                    command.CommandText = "INSERT INTO books VALUES(@title);";

                    command.ExecuteNonQuery();

                    command.CommandText = "SELECT MAX(PK) FROM books";

                    int Id = 0;
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        Id = Convert.ToInt32(result);
                    }







                    var listOfGenres = book.genres;

                    command.Parameters.AddWithValue("@bookId", Id);
                    command.Parameters.AddWithValue("@genreId", null);
                    foreach (var genre in listOfGenres)
                    {
                        command.Parameters["@genreId"].Value = genre;
                        command.CommandText = "INSERT INTO books_genres VALUES(@bookId, @genreId);";
                        command.ExecuteNonQuery();
                    }





                    transaction.Commit();
                    return true;



                }
            } catch (Exception e)
            {
                Console.WriteLine("Transaction failed. Rolling back.");
                transaction.Rollback();
                return false;
            }

        }
    }
*/
    public async Task<bool> AddBock(AddBook book)
 {
     await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
     await using SqlCommand command = new SqlCommand();
     await connection.OpenAsync();

     

         var transaction = await connection.BeginTransactionAsync();

         try
         {



           
                 command.Connection = connection;
                 command.Transaction = transaction as SqlTransaction;


                 command.Parameters.AddWithValue("@title",  book.title);
                    
                   

                 command.CommandText = "INSERT INTO books VALUES(@title);";

                 await command.ExecuteNonQueryAsync();

                 command.CommandText = "SELECT MAX(PK) FROM books";

                 int Id = 0;
                 var result = await command.ExecuteScalarAsync();
                 if (result != null)
                 {
                     Id = Convert.ToInt32(result);
                 }







                 var listOfGenres = book.genres;

                 command.Parameters.AddWithValue("@bookId", Id);
                 command.Parameters.AddWithValue("@genreId", null);
                 foreach (var genre in listOfGenres)
                 {
                     command.Parameters["@genreId"].Value = genre;
                     command.CommandText = "INSERT INTO books_genres VALUES(@bookId, @genreId);";
                     await command.ExecuteNonQueryAsync();
                 }





                 transaction.CommitAsync();
                 return true;



             
         } catch (Exception e)
         {
             Console.WriteLine("Transaction failed. Rolling back.");
             transaction.RollbackAsync();
             return false;
         }

    }
}
