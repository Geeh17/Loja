﻿using APILoja.Models;
using Microsoft.EntityFrameworkCore;

namespace APILoja.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produto>? Produtos { get; set; }
        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<Usuario>? Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            // Configuração para Categoria
            mb.Entity<Categoria>().HasKey(c => c.CategoriaId);
            mb.Entity<Categoria>().Property(c => c.Nome)
                .HasMaxLength(100)
                .IsRequired();
            mb.Entity<Categoria>().Property(c => c.Descricao)
                .HasMaxLength(150)
                .IsRequired();

            // Configuração para Produto
            mb.Entity<Produto>().HasKey(p => p.ProdutoId);
            mb.Entity<Produto>().Property(p => p.Nome)
                .HasMaxLength(100)
                .IsRequired();
            mb.Entity<Produto>().Property(p => p.Descricao)
                .HasMaxLength(150);
            mb.Entity<Produto>().Property(p => p.Preco).HasPrecision(14, 2);

            mb.Entity<Produto>()
                .HasOne<Categoria>(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId);

            // Configuração para Usuario
            mb.Entity<Usuario>().HasKey(u => u.UsuarioId);
            mb.Entity<Usuario>().Property(u => u.UsuarioId)
                .ValueGeneratedOnAdd(); 
            mb.Entity<Usuario>().Property(u => u.UsuarioNome)
                .HasMaxLength(50)
                .IsRequired();
            mb.Entity<Usuario>().Property(u => u.Senha)
                .HasMaxLength(100)
                .IsRequired();
            mb.Entity<Usuario>().Property(u => u.Role)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
