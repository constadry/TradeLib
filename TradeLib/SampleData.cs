using System.Linq;
using TradeLib.Models;

namespace TradeLib
{
    public class SampleData
    {
        public static void Initialize(Context context)
        {
            if (context.Persons.Any()) return;
            
            context.Persons.AddRange(
                new Person
                {
                    Name = "Igor",
                    Email = "test1@gmail.ru",
                    Password = "1234"
                },
                new Person
                {
                    Name = "Alex",
                    Email = "test2@gmail.ru",
                    Password = "1234"
                },
                new Person
                {
                    Name = "Kostya",
                    Email = "test3@gmail.ru",
                    Password = "1234"
                }
            );
            context.SaveChanges();
        }
    }
}