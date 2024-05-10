
using kol1.Models.DTOs;
using kol1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace kol1.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookRepository _bookRepository;

    public BookController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    [HttpGet("{id}/genres")]

    public async Task<IActionResult> GetBook(int id)
    {
        if (!await _bookRepository.BookExists(id))
        {
            return NotFound("This book does not exists");
        }

        var book = await _bookRepository.getBook(id);
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(AddBook book)
    {
        if (!await _bookRepository.AddBock(book))
        {
            return NotFound("This genres does not exists");
        }

        return Created("api/Book",null);
    }
}