using System.Linq;
using System.Reflection.Emit;
using TradeLib.Models;

namespace TradeLib
{
    public static class SampleData
    {
        public static void Initialize(Context context)
        {
            if (context.Persons.Any()) return;
            context.Persons.AddRange(
                new Person
                {
                    Name = "Igor",
                    Email = "test1@gmail.ru",
                    Password = "1234",
                    Confirmed = true
                },
                new Person
                {
                    Name = "Alex",
                    Email = "test2@gmail.ru",
                    Password = "1234",
                    Confirmed = true
                },
                new Person
                {
                    Name = "Kostya",
                    Email = "test3@gmail.ru",
                    Password = "1234",
                    Confirmed = true
                }
            );
            context.Products.AddRange(new Product
                {
                    Name = "algosy",
                    Type = "lab",
                    Description = "first lab",
                    ImageUrl = "sadfasdfa",
                    Price = "1200"
                },
                new Product
                {
                    Name = "EVM",
                    Type = "lab",
                    Description = "sixth lab",
                    ImageUrl = "sadfasdfa",
                    Price = "500"
                }
            );
            context.SaveChanges();
        }
    }
}