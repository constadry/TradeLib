﻿using Microsoft.EntityFrameworkCore;

namespace TradeLib.Models
{
    public sealed class Context : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}