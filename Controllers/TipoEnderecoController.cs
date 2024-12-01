using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoEnderecoController : ControllerBase
    {
        private readonly JsonService<TipoEndereco> _jsonService;

        public TipoEnderecoController()
        {
            _jsonService = new JsonService<TipoEndereco>(Path.Combine(Directory.GetCurrentDirectory(), "db", "tiposEndereco.json"));
        }

        // GET: api/tipoendereco
        [HttpGet]
        public async Task<ActionResult<List<TipoEndereco>>> GetTiposEndereco()
        {
            var tiposEndereco = await _jsonService.ReadJsonAsync();
            return Ok(tiposEndereco);
        }

        // GET: api/tipoendereco/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoEndereco>> GetTipoEndereco(int id)
        {
            var tiposEndereco = await _jsonService.ReadJsonAsync();
            var tipoEndereco = tiposEndereco.FirstOrDefault(t => t.Id == id);

            if (tipoEndereco == null)
            {
                return NotFound();
            }

            return Ok(tipoEndereco);
        }

        // POST: api/tipoendereco
        [HttpPost]
        public async Task<ActionResult> AddTipoEndereco([FromBody] TipoEndereco novoTipoEndereco)
        {
            if (!ModelState.IsValid) // Validação do modelo
            {
                return BadRequest(ModelState);
            }

            var tiposEndereco = await _jsonService.ReadJsonAsync();

            // Validação de ID duplicado
            if (tiposEndereco.Any(t => t.Tipo.Equals(novoTipoEndereco.Tipo, StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict("Um tipo de endereço com o mesmo nome já existe.");
            }

            // Autoincremento do ID
            novoTipoEndereco.Id = tiposEndereco.Count > 0 ? tiposEndereco.Max(t => t.Id) + 1 : 1;

            tiposEndereco.Add(novoTipoEndereco);
            await _jsonService.WriteJsonAsync(tiposEndereco);

            return CreatedAtAction(nameof(GetTipoEndereco), new { id = novoTipoEndereco.Id }, novoTipoEndereco);
        }

        // PUT: api/tipoendereco/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTipoEndereco(int id, [FromBody] TipoEndereco tipoEnderecoAtualizado)
        {
            if (!ModelState.IsValid) // Validação do modelo
            {
                return BadRequest(ModelState);
            }

            var tiposEndereco = await _jsonService.ReadJsonAsync();
            var index = tiposEndereco.FindIndex(t => t.Id == id);

            if (index == -1)
            {
                return NotFound("Tipo de Endereço não encontrado.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (tipoEnderecoAtualizado.Id != id)
            {
                return BadRequest("ID do tipo de endereço não corresponde ao ID da URL.");
            }

            tiposEndereco[index] = tipoEnderecoAtualizado; // Atualiza o tipo de endereço
            await _jsonService.WriteJsonAsync(tiposEndereco);

            return Ok(tipoEnderecoAtualizado);
        }


        // DELETE: api/tipoendereco/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTipoEndereco(int id)
        {
            var tiposEndereco = await _jsonService.ReadJsonAsync();
            var tipoEndereco = tiposEndereco.FirstOrDefault(t => t.Id == id);

            if (tipoEndereco == null)
            {
                return NotFound();
            }

            tiposEndereco.Remove(tipoEndereco);
            await _jsonService.WriteJsonAsync(tiposEndereco);

            return NoContent();
        }
    }
}
