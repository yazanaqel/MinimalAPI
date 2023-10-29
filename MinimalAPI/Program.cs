using static System.Reflection.Metadata.BlobBuilder;

var builder = WebApplication.CreateBuilder(args);

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

var books = new List<Book> {
new Book {Id=1,Title="C#",Author="Yazan"},
new Book {Id=2,Title="C++",Author="Yazan"},
new Book {Id=3,Title="Python",Author="Yazan"},
new Book {Id=4,Title="Java",Author="Yazan"},
};

app.MapGet("/book", () => { return books; });

app.MapGet("/book{id}", (int id) => { return books.FirstOrDefault(x => x.Id.Equals(id)); });

app.MapPost("/book", (Book book) => { books.Add(book); return books; });

app.MapPut("book/{id}", (Book book, int id) =>
{
    Book requiredBook = books.FirstOrDefault(x => x.Id.Equals(id));

    requiredBook.Title = book.Title;
    requiredBook.Author = book.Author;

    return books;
});

app.MapDelete("/book{id}", (int id) => { var book = books.FirstOrDefault(x => x.Id.Equals(id)); books.Remove(book); return books; });

app.Run();


public class Book
{

    public int Id { get; set; }

    public required string Title { get; set; }

    public required string Author { get; set; }
}