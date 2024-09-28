using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica03.Domain;
using Practica02Back.Services;
using System.Diagnostics.Eventing.Reader;

namespace Practica03.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private IFacturaService service;

        public ArticulosController()
        {
            service = new FacturaService();
        }


        [HttpGet]
        public IActionResult GetAllArticulo()
        {
            return Ok(service.GetAllArt());
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteArticulo(int id)
        {
            try
            {
                var del = service.DeleteArt(id);
                if (del == true)
                    return Ok($"Articulo {id} eliminado satisfactoriamente!");
                else
                {
                    return NotFound("Se produjo un error, articulo no encontrado!");
                }
            }
            catch
            {
                return StatusCode(500, "Se produjo un error interno");
            }
        }


        [HttpPost]
        public IActionResult CreateArticulo([FromBody] Articulo oArticulo)
        {
            try
            {
                if (oArticulo == null)
                {
                    return BadRequest("Error. No se brindaron todos los datos solicitados");
                }
                if (service.CreateArt(oArticulo))
                    return Ok("Articulo creado con exito!");
                else
                {
                    return StatusCode(500, "No se pudo crear el articulo");
                }
            }
            catch
            {
                return StatusCode(500, "Se produjo un error interno");
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateArticulo(int id, [FromBody] Articulo updArticulo)
        {
            try
            {
                if (updArticulo == null)
                {
                    return BadRequest("Los datos del artículo no pueden ser nulos.");
                }
                if (id <= 0)
                {
                    return BadRequest("ID inválido.");
                }
                if (service.UpdateArt(id, updArticulo))
                {
                    return Ok($"¡Se actualizó con éxito el artículo con ID {id}!");
                }
                else
                {
                    return NotFound($"No se encontró un artículo con ID {id} para actualizar.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error interno: {ex.Message}");
                return StatusCode(500, "Se produjo un error interno.");
            }
        }

    }
}
