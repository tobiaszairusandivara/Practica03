using Practica02Back.Data.Implementations;
using Practica03.Domain;
using Practica02Back.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();



namespace Practica03
{
    class Program
    {
        static void Main(string[] args)
        {
            ArticulosRepository articulosRepo = new ArticulosRepository();

            Articulo nuevoArticulo = new Articulo
            {
                Nombre = "Articulo de prueba",
                PrecioUnitario = 100
            };

            Console.WriteLine("Creando artículo...");
            bool creado = articulosRepo.Create(nuevoArticulo);
            Console.WriteLine(creado ? "Artículo creado exitosamente." : "Error al crear el artículo.");


            Console.WriteLine("\nObteniendo todos los artículos...");
            List<Articulo> listaArticulos = articulosRepo.GetAll();
            foreach (var articulo in listaArticulos)
            {
                Console.WriteLine($"Nombre: {articulo.Nombre}, Precio Unitario: {articulo.PrecioUnitario}");
            }


            int idActualizar = 1; 
            Articulo articuloActualizado = new Articulo
            {
                Nombre = "Artículo actualizado",
                PrecioUnitario = 150
            };

            bool actualizado = articulosRepo.Update(idActualizar, articuloActualizado);
            Console.WriteLine($"Artículo actualizado (ID {idActualizar}): {actualizado}");


            if (listaArticulos.Count > 0)
            {
                int idEliminar = 1; 
                Console.WriteLine($"\nEliminando artículo con ID {idEliminar}...");
                bool eliminado = articulosRepo.Delete(idEliminar);
                Console.WriteLine(eliminado ? "Artículo eliminado exitosamente." : "Error al eliminar el artículo.");
            }
            else
            {
                Console.WriteLine("\nNo hay artículos para eliminar.");
            }


        }
    }
}