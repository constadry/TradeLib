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
                }
            );
            context.SaveChanges();
        }
    }
}