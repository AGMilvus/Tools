using EmailValidation.Models;

namespace EmailValidation.EntityFrameworkCore.Repositories;

public interface IEmailRepository
{
    void Add(EmailEntity entity);

    void Save();
}