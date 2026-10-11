using mi_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace mi_api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ClientesController : Controller
    {
        private readonly DbEmpresaContext _context;
        public ClientesController(DbEmpresaContext ctx)
        {
            _context = ctx;
        }

        //Select: listar a los clientes
        [HttpGet("ListarUsuarios")]
        public async Task<ActionResult<List<Cliente>>> ListarClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        //Buscar por ID

        [HttpGet("obtenerUsuario/{id}")]
        public async Task<ActionResult<Cliente>> btenerUsuario(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(e => e.IdClientes == id);
            if (cliente == null)
            {
                return NotFound("No se encontro ningun cliente");
            }
            return Ok(cliente);
        }

        //Crear a un cliente
        [HttpPost("CrearCliente")]
        public async Task<ActionResult<Cliente>> CrearCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(btenerUsuario),
                new
                {
                    id = cliente.IdClientes
                },
                cliente);
        }

        //Actualizar a un cliente
        [HttpPut("ActualizarCliente/{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, Cliente cliente)
        {
            if (id != cliente.IdClientes)
            {
                return BadRequest(" El ID no coincde con el del cliente");
            }

            var clienteExistente = await _context.Clientes.AnyAsync(c => c.IdClientes == id);

            if (!clienteExistente)
            {
                return NotFound("No se encontro el cliente");
            }

            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Eliminar a un cliente
        [HttpDelete("EliminarCliente/{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdClientes == id);
            if (cliente == null)
            {
                return NotFound("No se encontro el cliente");
            }
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
