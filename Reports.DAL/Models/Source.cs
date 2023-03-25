using Reports.DAL.Utils;

namespace Reports.DAL.Models;

public class Source
{
    private Guid _id;
    private string _type;
    private Account _account;

    public Source(string type, Account account)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new DatabaseException("Type cannot be null or empty.");
        _id = Guid.NewGuid();
        _account = account ?? throw new DatabaseException("Account cannot be null.");
        _type = type;
    }

    public Guid Id => _id;
    public string Type => _type;
    public Account Account => _account;
}