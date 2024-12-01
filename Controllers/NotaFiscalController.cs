using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly JsonService<NotaFiscal> _jsonService;

        public NotaFiscalController()
        {
            _jsonService = new JsonService<NotaFiscal>(Path.Combine(Directory.GetCurrentDirectory(), "db", "notasFiscais.json"));
        }

        // GET: api/notafiscal
        [HttpGet]
        public async Task<ActionResult<List<NotaFiscal>>> GetNotasFiscais()
        {
            var notasFiscais = await _jsonService.ReadJsonAsync();
            return Ok(notasFiscais);
        }

        // GET: api/notafiscal/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<NotaFiscal>> GetNotaFiscal(int id)
        {
            var notasFiscais = await _jsonService.ReadJsonAsync();
            var notaFiscal = notasFiscais.FirstOrDefault(n => n.IdNotaFiscal == id);

            if (notaFiscal == null)
            {
                return NotFound();
            }

            return Ok(notaFiscal);
        }

        // POST: api/notafiscal
        [HttpPost]
        public async Task<ActionResult> AddNotaFiscal([FromBody] NotaFiscal novaNotaFiscal)
        {
            if (novaNotaFiscal == null)
            {
                return BadRequest("Nota fiscal não pode ser nula.");
            }

            var notasFiscais = await _jsonService.ReadJsonAsync();

            // Autoincrementação do ID
            novaNotaFiscal.IdNotaFiscal = notasFiscais.Any() 
                ? notasFiscais.Max(n => n.IdNotaFiscal) + 1 
                : 1;

            notasFiscais.Add(novaNotaFiscal);
            await _jsonService.WriteJsonAsync(notasFiscais);

            return CreatedAtAction(nameof(GetNotaFiscal), new { id = novaNotaFiscal.IdNotaFiscal }, novaNotaFiscal);
        }


        // PUT: api/notafiscal/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNotaFiscal(int id, [FromBody] NotaFiscal notaFiscalAtualizada)
        {
            if (notaFiscalAtualizada == null)
            {
                return BadRequest("Nota fiscal não pode ser nula.");
            }

            var notasFiscais = await _jsonService.ReadJsonAsync();
            var index = notasFiscais.FindIndex(n => n.IdNotaFiscal == id);

            if (index == -1)
            {
                return NotFound("Nota fiscal não encontrada.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (notaFiscalAtualizada.IdNotaFiscal != id)
            {
                return BadRequest("ID da nota fiscal não corresponde ao ID da URL.");
            }

            notasFiscais[index] = notaFiscalAtualizada; // Atualiza a nota fiscal
            await _jsonService.WriteJsonAsync(notasFiscais);

            return Ok(notaFiscalAtualizada);
        }

        // DELETE: api/notafiscal/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotaFiscal(int id)
        {
            var notasFiscais = await _jsonService.ReadJsonAsync();
            var notaFiscal = notasFiscais.FirstOrDefault(n => n.IdNotaFiscal == id);

            if (notaFiscal == null)
            {
                return NotFound();
            }

            notasFiscais.Remove(notaFiscal);
            await _jsonService.WriteJsonAsync(notasFiscais);

            return NoContent();
        }
    }
}
