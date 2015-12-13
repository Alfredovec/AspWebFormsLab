using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankService.Interfaces;
using BankService.Model;

namespace BankService.Repositories
{
    class UserRepository : IUserRepository
    {
        private static readonly List<User> Users = new List<User>
        {
            new User
            {
                AccountNumber = "1111 1111 1111 1111",
                Email = "stas249501@gmail.com",
                Money = 1000,
                Name = "Stas",
                Surname = "Zadorozhnii",
                CardType = CardType.MasterCard,
                Cvv2 = "1234",
                ExpirationDate = "10/2020"
            },
            new User
            {
                AccountNumber = "1111 1111 1111 1111",
                Email = "stas249501@gmail.com",
                Money = 1000,
                Name = "Stas",
                Surname = "Zadorozhnii",
                CardType = CardType.Visa,
                Phone = "1111111111",
                Cvv2 = "1234",
                ExpirationDate = "10/2020"
            },
            new User
            {
                AccountNumber = "2222 2222 2222 2222",
                Email = "user1@email.ru",
                Money = 1000,
                Name = "Sergii",
                Surname = "Rud",
                CardType = CardType.MasterCard,
                Cvv2 = "1234",
                ExpirationDate = "10/2020"
            },
            new User
            {
                AccountNumber = "3333 3333 3333 3333",
                Money = 1000,
                Name = "Test",
                Surname = "Test",
                CardType = CardType.Visa,
                Cvv2 = "1234",
                ExpirationDate = "10/2020"
            },
        };

        public IEnumerable<User> Get()
        {
            return Users;
        }

        public User Get(string name, string accountNumber, CardType type, string cvv, string expirationDate)
        {
            return
                Users.FirstOrDefault(
                    u => u.Name + " " + u.Surname == name
                         && accountNumber == u.AccountNumber
                         && type == u.CardType && cvv == u.Cvv2
                         && expirationDate == u.ExpirationDate);
        }
    }
}