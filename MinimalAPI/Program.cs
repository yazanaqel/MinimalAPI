global using Microsoft.EntityFrameworkCore;
using MinimalAPI;
using static System.Reflection.Metadata.BlobBuilder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<DataContext>
    (op => op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/book", async (DataContext dataContext) =>
await dataContext.Books.ToListAsync());

app.MapGet("/book{id}", async (DataContext dataContext, int id) =>
await dataContext.Books.FindAsync(id) is Book book ? Results.Ok(book) : Results.NotFound("Not Found!"));

app.MapPost("/book", async (DataContext dataContext, Book book) =>
{
    dataContext.Books.Add(book);
    await dataContext.SaveChangesAsync();
    return Results.Ok(await dataContext.Books.ToListAsync());
});



app.MapPut("book/{id}",async (DataContext dataContext, Book book, int id) =>
{
    Book? requiredBook = await dataContext.Books.FindAsync(id);

    if (requiredBook is null)
        return Results.NotFound("Not Found!");

    requiredBook.Title = book.Title;
    requiredBook.Author = book.Author;

    await dataContext.SaveChangesAsync();

    return Results.Ok(await dataContext.Books.ToListAsync());
});

app.MapDelete("/book{id}",async (DataContext dataContext, int id) => 
{ 
    var book = await dataContext.Books.FindAsync(id);

    if (book is null)
        return Results.NotFound("Not Found!");

    dataContext.Books.Remove(book);
    await dataContext.SaveChangesAsync();

    return Results.Ok(await dataContext.Books.ToListAsync());
});

app.Run();


public class Book
{

    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Author { get; set; }
}