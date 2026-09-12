using BookTracker.Data;
using BookTracker.Dtos;
using BookTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTracker.Services;

public class AuthorService
{
    
    private readonly BookTrackerContext _dbContext;

    public AuthorService(BookTrackerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<AuthorDto>> GetAllAuthorsAsync()
    {
        return await _dbContext.Authors
            .Select(author => new AuthorDto(author.Id, author.Name))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Author?> GetAuthorByIdAsync(int id)
    {
        return await _dbContext.Authors.FindAsync(id);
    }

    public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorDto newAuthor)
    {
        Author author = new()
        {
            Name = newAuthor.Name
                    
        };
        _dbContext.Authors.Add(author);
        await _dbContext.SaveChangesAsync();

        AuthorDto authorDto = new AuthorDto(author.Id, author.Name);
        return authorDto;
    }

    public async Task<bool> UpdateAuthorAsync(int id, UpdateAuthorDto updatedAuthorDto)
    {
        var author = await _dbContext.Authors.FindAsync(id);

        if (author is null)
        {
            return false;
        }
            
        author.Name = updatedAuthorDto.Name;
            
        await _dbContext.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteAuthorAsync(int id)
    {
        var hasLinkedBooks = await _dbContext.Books.AnyAsync(b => b.AuthorId == id);

        if (hasLinkedBooks)
        {
            return false;
        }
            
        await _dbContext.Authors
            .Where(author => author.Id == id)
            .ExecuteDeleteAsync();
        
        return true;
    }
    
}