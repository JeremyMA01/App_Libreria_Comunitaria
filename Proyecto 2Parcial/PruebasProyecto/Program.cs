
using Proyecto_2Parcial.Contexts;
using Proyecto_2Parcial.Models;
using Microsoft.EntityFrameworkCore;


using (var context = new BookContexts())
{
    
    var genres = new List<Genre>
    {
        new Genre { Name = "Romance", Active = true },
        new Genre { Name = "Ciencia Ficción", Active = true },
        new Genre { Name = "Terror", Active = true },
        new Genre { Name = "Clásica", Active = true },
        new Genre { Name = "Infantil", Active = true },
        new Genre { Name = "Historia", Active = true },
        new Genre { Name = "Fantasía", Active = true },
        new Genre { Name = "Misterio", Active = true },
        new Genre { Name = "Suspenso", Active = true },
        new Genre { Name = "Drama", Active = true },
        new Genre { Name = "Aventura", Active = true },
        new Genre { Name = "Biografía", Active = true },
        new Genre { Name = "Poesía", Active = true }
    };

    context.Genres.AddRange(genres);
    context.SaveChanges(); 

    
    var books = new List<Book>
    {
        new Book
        {
            
            Title = "Cien Años de Soledad",
            Author = "Gabriel García Márquez",
            Year = 1967,
            GenreId = 11, // "Aventura"
            Active = true,
            ReleaseDate = new DateTime(2024, 06, 14),
            Budget = 20,
            Poster = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJvSAV5ld6uZ2nWHf_u6sHkoULM8ap3e-CoKPnSnHjIrLn8moFJYl3MZPVaqQrAZzGirtvnDCw0Tn0SJbwiUOPNMxUTRJF2vz8V5q8gN9_mA&s=10"
        },
        new Book
        {
            
            Title = "1984",
            Author = "George Orwell",
            Year = 1949,
            GenreId = 9, // "Suspenso"
            Active = true,
            ReleaseDate = new DateTime(2024, 06, 14),
            Budget = 18,
            Poster = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRcJIVdrqrYT_SSw2BaQcTlxhscAoNF4wkPrRXMBEJcYmh7PZ_71PBqyzA0ezANjTK3yz9VwDm21zyocVrYLVZy60vSw2ZRh77_Fi4g2uhavQ&s=10"
        },
        new Book
        {
            
            Title = "El Hobbit",
            Author = "J.R.R. Tolkien",
            Year = 1937,
            GenreId = 7, // "Fantasía"
            Active = true,
            ReleaseDate = new DateTime(2024, 06, 14),
            Budget = 25,
            Poster = "https://books.google.com.ec/books/content?id=xi2URRig7jjYC&printsec=frontcover&img=1&zoom=1&edge=curl&imgtk=AFLRE719xPHMrDDvPQwLAF8y_1ULuS0gfQ0dogYtmDvZEoSMewJSNlI0Ek11JLvQ9hMQ5wcW7hVchpu61TAi_7--CEBS2OidPajFZeOeUtW9o6Tpizjf1iD6VSuHBTnktOeMEofCKhcD"
        },
        new Book
        {
            
            Title = "El Código Da Vinci",
            Author = "Dan Brown",
            Year = 2003,
            GenreId = 9, // "Suspenso"
            Active = true,
            ReleaseDate = new DateTime(2024, 06, 14),
            Budget = 30,
            Poster = "https://img1.od-cdn.com/ImageType-400/5822-1/%7B4DF8AADF-FA4B-405D-9BCE-044418F6B841%7DIMG400.JPG"
        },
        new Book
        {
            
            Title = "Orgullo y Prejuicio",
            Author = "Jane Austen",
            Year = 1813,
            GenreId = 10, // "Drama"
            Active = true,
            ReleaseDate = new DateTime(2024, 06, 14),
            Budget = 18,
            Poster = "https://m.media-amazon.com/images/I/81OthjkJBuL._AC_UF1000,1000_QL80_.jpg"
        },
        new Book
        {
            
            Title = "Viaje al Centro de la Tierra",
            Author = "Jules Verne",
            Year = 1864,
            GenreId = 11, // "Aventura"
            Active = true,
            ReleaseDate = new DateTime(2024, 06, 14),
            Budget = 15,
            Poster = "https://images.cdn3.buscalibre.com/fit-in/360x360/75/eb/75eb5612db5984589be2258fc891b0b0.jpg"
        },
        new Book
        {
            
            Title = "Sapiens: De Animales a Dioses",
            Author = "Yuval Noah Harari",
            Year = 2011,
            GenreId = 2, // "Ciencia Ficción"
            Active = true,
            ReleaseDate = new DateTime(2024, 06, 14),
            Budget = 28,
            Poster = "https://m.media-amazon.com/images/I/713jIoMO3UL.jpg"
        },
        new Book
        {
          
            Title = "El Diario de Ana Frank",
            Author = "Ana Frank",
            Year = 1947,
            GenreId = 12, // "Biografía"
            Active = false,
            ReleaseDate = new DateTime(2024, 06, 14),
            Budget = 20,
            Poster = "https://images.cdn1.buscalibre.com/fit-in/360x360/5e/63/5e6382195f84bb7b5c555af930ef6ec2.jpg"
        },
        new Book
        {
           
            Title = "Cien Sonetos de Amor",
            Author = "Pablo Neruda",
            Year = 1956,
            GenreId = 13, // "Poesía"
            Active = true,
            ReleaseDate = new DateTime(2024, 06, 14),
            Budget = 20,
            Poster = "https://anterior.mrbooks.com/mrbooks/portadas/9788418933714.webp"
        }
    };

    context.books.AddRange(books); 
    context.SaveChanges(); 
}
