﻿using APILoja.Context;
using APILoja.Models;
using Microsoft.EntityFrameworkCore;

namespace APILoja.ApiEndpoints
{
    public static class CategoriasEndpoints
    {
        public static void MapCategoriasEndpoints(this WebApplication app)
        {
            // Endpoint para criar uma categoria - restrito a Admin
            app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) => {
                db.Categorias.Add(categoria);
                await db.SaveChangesAsync();
                return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
            }).RequireAuthorization("AdminOnly");

            // Endpoint para listar todas as categorias com paginação
            app.MapGet("/categorias", async (int pageNumber, int pageSize, AppDbContext db) => {
                pageNumber = pageNumber < 1 ? 1 : pageNumber;
                pageSize = pageSize < 1 ? 10 : pageSize;

                var categorias = await db.Categorias
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Results.Ok(categorias);
            }).RequireAuthorization();

            // Endpoint para obter uma categoria específica por ID
            app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) => {
                var categoria = await db.Categorias.FindAsync(id);
                return categoria != null
                    ? Results.Ok(categoria)
                    : Results.NotFound("Categoria não encontrada.");
            }).RequireAuthorization();

            // Endpoint para atualizar uma categoria por ID
            app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) => {
                if (categoria.CategoriaId != id)
                {
                    return Results.BadRequest("Erro: O ID da categoria na URL não corresponde ao ID do corpo da requisição.");
                }

                var categoriaDB = await db.Categorias.FindAsync(id);
                if (categoriaDB is null)
                {
                    return Results.NotFound("Erro: Categoria não encontrada para atualização.");
                }

                categoriaDB.Nome = categoria.Nome;
                categoriaDB.Descricao = categoria.Descricao;
                await db.SaveChangesAsync();
                return Results.Ok(categoriaDB);
            }).RequireAuthorization();

            // Endpoint para excluir uma categoria por ID - restrito a Admin
            app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) => {
                var categoria = await db.Categorias.FindAsync(id);
                if (categoria is null)
                {
                    return Results.NotFound("Categoria não encontrada.");
                }

                db.Categorias.Remove(categoria);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }).RequireAuthorization("AdminOnly");
        }
    }
}
