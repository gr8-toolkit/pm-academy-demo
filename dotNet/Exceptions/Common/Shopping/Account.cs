using System;

namespace Common
{
    public class Account
    {
        private readonly string _name;
        private readonly int _age;
        private decimal _balance;

        public string Name { get => _name; }

        public decimal Balance { get => _balance; }

        public int Age { get => _age; }

        public Account(string name, int age, decimal balance)
        {
            _name = name;
            _age = age;
            _balance = balance;
        }

        public decimal Withdraw(decimal amount)
        {
            // DO NOT throw System.Exception or System.SystemException.
            if (amount <= 0)
            {
                throw new ArgumentException($"{nameof(amount)} can't be less ot equal than 0");
            }

            // DO throw an InvalidOperationException if the object is in an inappropriate state.
            if (amount > _balance)
            {
                throw new InvalidOperationException("Insufficient account balance");
            }

            _balance -= amount;

            return _balance;
        }

    }
}
