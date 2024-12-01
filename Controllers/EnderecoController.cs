using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly JsonService<Endereco> _jsonService;

        public EnderecoController()
        {
            _jsonService = new JsonService<Endereco>(Path.Combine(Directory.GetCurrentDirectory(), "db", "enderecos.json"));
        }

        // GET: api/endereco
        [HttpGet]
        public async Task<ActionResult<List<Endereco>>> GetEnderecos()
        {
            var enderecos = await _jsonService.ReadJsonAsync();
            return Ok(enderecos);
        }

        // GET: api/endereco/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Endereco>> GetEndereco(int id)
        {
            var enderecos = await _jsonService.ReadJsonAsync();
            var endereco = enderecos.FirstOrDefault(e => e.Id == id);

            if (endereco == null)
            {
                return NotFound();
            }

            return Ok(endereco);
        }

        // POST: api/endereco
        [HttpPost]
        public async Task<ActionResult> AddEndereco([FromBody] Endereco novoEndereco)
        {
            if (novoEndereco == null)
            {
                return BadRequest("Endereço não pode ser nulo.");
            }

            var enderecos = await _jsonService.ReadJsonAsync();

            // Autoincremento do ID
            if (enderecos.Any())
            {
                novoEndereco.Id = enderecos.Max(e => e.Id) + 1;
            }
            else
            {
                novoEndereco.Id = 1;
            }

            enderecos.Add(novoEndereco);
            await _jsonService.WriteJsonAsync(enderecos);

            return CreatedAtAction(nameof(GetEndereco), new { id = novoEndereco.Id }, novoEndereco);
        }

        // PUT: api/endereco/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEndereco(int id, [FromBody] Endereco enderecoAtualizado)
        {
            if (enderecoAtualizado == null)
            {
                return BadRequest("Endereço não pode ser nulo.");
            }

            var enderecos = await _jsonService.ReadJsonAsync();
            var index = enderecos.FindIndex(e => e.Id == id);

            if (index == -1)
            {
                return NotFound("Endereço não encontrado.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (enderecoAtualizado.Id != id)
            {
                return BadRequest("ID do endereço não corresponde ao ID da URL.");
            }

            enderecos[index] = enderecoAtualizado; // Atualiza o endereço
            await _jsonService.WriteJsonAsync(enderecos);

            return Ok(enderecoAtualizado);
        }

        // DELETE: api/endereco/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEndereco(int id)
        {
            var enderecos = await _jsonService.ReadJsonAsync();
            var endereco = enderecos.FirstOrDefault(e => e.Id == id);

            if (endereco == null)
            {
                return NotFound();
            }

            enderecos.Remove(endereco);
            await _jsonService.WriteJsonAsync(enderecos);

            return NoContent();
        }
    }
}
