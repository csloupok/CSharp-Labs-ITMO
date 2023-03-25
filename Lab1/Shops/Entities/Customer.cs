using Shops.Tools.Exceptions;

namespace Shops.Entities;

public class Customer
{
   private const int MinimalAmountOfCredits = 0;
   private string _name;
   private decimal _credits;

   public Customer(string name, decimal credits)
   {
      if (string.IsNullOrWhiteSpace(name))
         throw new ProductException("Customer name is empty");
      if (credits < MinimalAmountOfCredits)
         throw new CreditsException("Credits can't be negative");
      _name = name;
      _credits = credits;
   }

   public string Name => _name;
   public decimal Credits => _credits;

   public decimal WithdrawCredits(decimal value)
   {
      if (_credits < value)
      {
         throw new CreditsException(
            $"{_name} doesn't have sufficient credits. \n Total is {value}. Customer's credits are {_credits}");
      }

      _credits -= value;
      return value;
   }
}