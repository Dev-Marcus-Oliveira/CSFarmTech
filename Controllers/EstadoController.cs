using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : ControllerBase
    {
        private readonly JsonService<Estado> _jsonService;

        public EstadoController()
        {
            _jsonService = new JsonService<Estado>(Path.Combine(Directory.GetCurrentDirectory(), "db", "estados.json"));
        }

        // GET: api/estado
        [HttpGet]
        public async Task<ActionResult<List<Estado>>> GetEstados()
        {
            var estados = await _jsonService.ReadJsonAsync();
            return Ok(estados);
        }

        // GET: api/estado/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Estado>> GetEstado(int id)
        {
            var estados = await _jsonService.ReadJsonAsync();
            var estado = estados.FirstOrDefault(e => e.Id == id);

            if (estado == null)
            {
                return NotFound();
            }

            return Ok(estado);
        }

        // POST: api/estado
        [HttpPost]
        public async Task<ActionResult> AddEstado([FromBody] Estado novoEstado)
        {
            if (novoEstado == null)
            {
                return BadRequest("Estado não pode ser nulo.");
            }

            // Validação dos dados do estado
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var estados = await _jsonService.ReadJsonAsync();

            // Auto-incremento do ID
            novoEstado.Id = estados.Count > 0 ? estados.Max(e => e.Id) + 1 : 1;

            estados.Add(novoEstado);
            await _jsonService.WriteJsonAsync(estados);

            return CreatedAtAction(nameof(GetEstado), new { id = novoEstado.Id }, novoEstado);
        }

        // PUT: api/estado/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEstado(int id, [FromBody] Estado estadoAtualizado)
        {
            if (estadoAtualizado == null)
            {
                return BadRequest("Estado não pode ser nulo.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var estados = await _jsonService.ReadJsonAsync();
            var index = estados.FindIndex(e => e.Id == id);

            if (index == -1)
            {
                return NotFound("Estado não encontrado.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (estadoAtualizado.Id != id)
            {
                return BadRequest("ID do estado não corresponde ao ID da URL.");
            }

            estados[index] = estadoAtualizado; // Atualiza o estado
            await _jsonService.WriteJsonAsync(estados);

            return Ok(estadoAtualizado);
        }

        // DELETE: api/estado/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEstado(int id)
        {
            var estados = await _jsonService.ReadJsonAsync();
            var estado = estados.FirstOrDefault(e => e.Id == id);

            if (estado == null)
            {
                return NotFound();
            }

            estados.Remove(estado);
            await _jsonService.WriteJsonAsync(estados);

            return NoContent();
        }
    }
}
