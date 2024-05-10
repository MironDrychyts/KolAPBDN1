using kol1.Models;
using kol1.Models.DTOs;

namespace kol1.Repositories;

public interface IBookRepository
{

    public Task<bool> BookExists(int bookId);
    public Task<BookInfo> getBook(int bookID);

    public Task<bool> AddBock(AddBook book);
}