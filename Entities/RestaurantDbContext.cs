﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication44Udemy.Entities
{
    public class RestaurantDbContext : DbContext
    {
        
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options):base(options)
        {

        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().Property(b => b.Email)
    .IsRequired();
            modelBuilder.Entity<Role>().Property(b => b.Name);

            modelBuilder.Entity<Restaurant>().Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Dish>().Property(d => d.Name)
                .IsRequired();
            modelBuilder.Entity<Adress>().Property(a => a.City).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Adress>().Property(a => a.Street).IsRequired().HasMaxLength(50);

        }
       

    }
}
