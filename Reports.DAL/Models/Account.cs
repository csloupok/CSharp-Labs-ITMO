using Reports.DAL.Utils;

namespace Reports.DAL.Models;

public class Account
{
    private Guid _id;
    private string _email;
    private string _password;

    public Account(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DatabaseException("Email cannot be null or empty.");
        if (string.IsNullOrWhiteSpace(password))
            throw new DatabaseException("Password cannot be null or empty.");
        _id = Guid.NewGuid();
        _email = email;
        _password = password;
    }

    public Guid Id => _id;
    public string Email => _email;
    public string Password => _password;

    public void ChangeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DatabaseException("Email cannot be null or empty.");
        _email = email;
    }

    public void ChangePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new DatabaseException("Password cannot be null or empty.");
        _password = password;
    }
}