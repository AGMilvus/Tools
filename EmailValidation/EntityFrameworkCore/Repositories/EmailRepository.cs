using EmailValidation.Models;

namespace EmailValidation.EntityFrameworkCore.Repositories;

public class EmailRepository : IEmailRepository
{
    private readonly AppDbContext _dbContext;

    public EmailRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(EmailEntity entity)
    {
        _dbContext.Emails.Add(entity);
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}