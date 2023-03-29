﻿using Microsoft.EntityFrameworkCore;
using WebAPI_My_Home_Library.Models;

namespace WebAPI_My_Home_Library.Context
{
    public class MyHomeLibraryContext : DbContext
    {
        public MyHomeLibraryContext(DbContextOptions<MyHomeLibraryContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Perfil> Perfil { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
