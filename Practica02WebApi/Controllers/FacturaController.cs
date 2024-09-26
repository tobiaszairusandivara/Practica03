using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practica01.Domain;
using Practica02.Domain;
using Practica02Back.Services;

namespace Practica02WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : Controller
    {
        private IFacturaService service;
       
        public FacturaController()
        {
            service = new FacturaService();
        }


        [HttpGet]
        public IActionResult GetAllFactura()
        {
            return Ok(service.GetAllFact());
        }


        [HttpPost]
        public IActionResult CreateFactura([FromBody] Factura oFactura)
        {
            try
            {
                if (oFactura == null)
                {
                    return BadRequest("Error. No se brindaron todos los datos solicitados");
                }
                if (service.CreateFact(oFactura))
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

        /// NO FUNCA PORQUE SOLO TENGO UPDATE DE FACTURA + DETALLE
        [HttpPut("{id}")]
        public IActionResult UpdateFactura(int id, [FromBody] Factura oFactura)
        {
            try
            {
                if (oFactura == null)
                {
                    return BadRequest("Los datos de la factura no pueden ser nulos.");
                }
                if (id <= 0)
                {
                    return BadRequest("ID inválido.");
                }
                if (service.UpdateFact(id, oFactura))
                {
                    return Ok($"¡Se actualizó con éxito la factura con ID {id}!");
                }
                else
                {
                    return NotFound($"No se encontró una factura con ID {id} para actualizar.");
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
