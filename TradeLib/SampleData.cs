using System.Linq;
using TradeLib.Models;

namespace TradeLib
{
    public static class SampleData
    {
        public static void Initialize(Context context)
        {
            if (context.Persons.Any()) return;
            context.Persons.Add(
                new Person
                {
                    Name = "Igor",
                    Email = "test1@gmail.ru",
                    Password = "1234",
                    Confirmed = true
                }
            );
            context.SaveChanges();
        }
    }
}