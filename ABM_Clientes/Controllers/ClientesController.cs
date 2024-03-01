using ABM_Clientes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ABM_Clientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesContext _context;

        public ClientesController(ClientesContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Clientes>>> GetAll()
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            return await _context.Clientes.ToListAsync();
        }
        [HttpGet("Get/{ID}")]
        public async Task<ActionResult<Clientes>> Get(int ID)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            Clientes cliente = await _context.Clientes.FindAsync(ID);
            if (cliente == null)
            {
                return NotFound();
            }
            return cliente;
        }
        [HttpGet("Search/{nombre}")]
        public async Task<ActionResult<IEnumerable<Clientes>>> Search(string nombre)
        {
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            List<Clientes> clientes = await _context.Clientes.Where(x => x.Nombres.Contains(nombre)).ToListAsync();
            if (clientes == null)
            {
                return NotFound();
            }
            return clientes;
        }
        [HttpPost("Insert")]
        public async Task<ActionResult<Clientes>> Insert(Clientes cliente)
        {
            PropertyInfo[] propiedades = cliente.GetType().GetProperties();

            foreach (PropertyInfo pi in propiedades)
            {
                if (pi.PropertyType == typeof(string))
                {
                    if (pi.GetValue(cliente) == null || pi.GetValue(cliente) == string.Empty)
                    {
                        if (pi.Name != "ID" && pi.Name != "FechaDeNacimiento" && pi.Name != "Domicilio")
                        {
                            return BadRequest($"El campo '{pi.Name}' está vacío. Cliente no registrado. ");
                        }
                    }
                }
            }

            if (!EmailValido(cliente.Email))
            {
                return BadRequest($"El Email '{cliente.Email}' no es válido. Por favor ingresar un Email válido.");
            }

            if (!CuitValido(cliente.CUIT))
            {
                return BadRequest($"El CUIT '{cliente.CUIT}' no es válido. Por favor ingresar un CUIT válido.");
            }
            if (!TelefonoValido(cliente.TelefonoCelular))
            {
                return BadRequest($"El telefono celular '{cliente.TelefonoCelular}' no es válido. Por favor ingresar un teléfono celular válido.");
            }

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new {id = cliente.ID}, cliente);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, Clientes cliente)
        {
            if (id != cliente.ID)
            {
                return BadRequest();
            }

            PropertyInfo[] propiedades = cliente.GetType().GetProperties();

                        foreach (PropertyInfo pi in propiedades)
                        {
                            if (pi.PropertyType == typeof(string))
                            {
                                if (pi.GetValue(cliente) == null || pi.GetValue(cliente) == string.Empty)
                                {
                                    if (pi.Name != "ID" && pi.Name != "FechaDeNacimiento" && pi.Name != "Domicilio")
                                    {
                                        return BadRequest($"El campo '{pi.Name}' está vacío. Cliente no registrado. ");
                                    }
                                }
                            }
                        }

            if (!EmailValido(cliente.Email))
            {
                return BadRequest($"El Email '{cliente.Email}' no es válido. Por favor ingresar un Email válido.");
            }

            if (!CuitValido(cliente.CUIT))
            {
                return BadRequest($"El CUIT '{cliente.CUIT}' no es válido. Por favor ingresar un CUIT válido.");
            }
            if (!TelefonoValido(cliente.TelefonoCelular))
            {
                return BadRequest($"El telefono celular '{cliente.TelefonoCelular}' no es válido. Por favor ingresar un teléfono celular válido.");
            }
            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExistente(id))
                {
                    return NotFound();
                }
                else throw;
            }
            return Ok();
        }

        private bool ClienteExistente(int id)
        {
            return (_context.Clientes?.Any(x => x.ID == id)).GetValueOrDefault();
        }

        private bool EmailValido(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool CuitValido(string cuit)
        {

            bool b = long.TryParse(cuit, out long res);

            if (cuit.Length == 11 && b && res > 0)
            {
                return true;
            }
            return false;
        }
        private bool TelefonoValido(string telefono)
        {

            bool b = long.TryParse(telefono, out long res);

            if (telefono.Length > 8 && telefono.Length < 20 && b && res > 0)
            {
                return true;
            }
            return false;
        }
        
    }

}
