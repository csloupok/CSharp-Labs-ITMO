using Banks.Interfaces;
using Banks.Tools.Exceptions;

namespace Banks.Models.Clients;

public class Client : IObserver
{
    private string _firstName;
    private string _lastName;
    private string? _address;
    private string? _passport;
    private Guid _id;
    private bool _subscription;

    private Client(string? firstName, string? lastName, string? passport, string? address)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new BanksException("First name is null.");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new BanksException("Last name is null.");
        _firstName = firstName;
        _lastName = lastName;
        _passport = passport;
        _address = address;
        _id = Guid.NewGuid();
        _subscription = false;
    }

    public string? FirstName => _firstName;
    public string? LastName => _lastName;
    public string? Address => _address;
    public string? Passport => _passport;
    public Guid Id => _id;
    public bool IsSubscribed => _subscription;

    public Client SetPassport(string passport)
    {
        if (string.IsNullOrWhiteSpace(passport))
            throw new BanksException("Passport is null.");
        if (_passport is not null)
            throw new BanksException("Passport is already set.");
        _passport = passport;
        return this;
    }

    public Client SetAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new BanksException("Address is null.");
        if (_address is not null)
            throw new BanksException("Address is already set.");
        _address = address;
        return this;
    }

    public void Subscribe()
    {
        if (_subscription)
            throw new BanksException("Already subscribed.");
        _subscription = true;
    }

    public void Unsubscribe()
    {
        if (!_subscription)
            throw new BanksException("Already unsubscribed.");
        _subscription = false;
    }

    public void Update(ISubject subject)
    {
        Console.WriteLine("I reacted.");
    }

    public bool IsConfirmed()
    {
        return _address is not null && _passport is not null;
    }

    public class Builder
    {
        private string? _firstName;
        private string? _lastName;
        private string? _passport;
        private string? _address;

        public Builder FirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new BanksException("First name is null.");
            _firstName = firstName;
            return this;
        }

        public Builder LastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new BanksException("Last name is null.");
            _lastName = lastName;
            return this;
        }

        public Builder Passport(string passport)
        {
            if (string.IsNullOrWhiteSpace(passport))
                throw new BanksException("Passport is null.");
            _passport = passport;
            return this;
        }

        public Builder Address(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new BanksException("Address is null.");
            _address = address;
            return this;
        }

        public Client Create() => new Client(_firstName, _lastName, _passport, _address);
    }
}