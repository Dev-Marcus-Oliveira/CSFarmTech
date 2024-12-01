using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecebimentoNFController : ControllerBase
    {
        private readonly JsonService<RecebimentoNF> _jsonService;

        public RecebimentoNFController()
        {
            _jsonService = new JsonService<RecebimentoNF>(Path.Combine(Directory.GetCurrentDirectory(), "db", "recebimentosNF.json"));
        }

        // GET: api/recebimentonf
        [HttpGet]
        public async Task<ActionResult<List<RecebimentoNF>>> GetRecebimentoNFs()
        {
            var recebimentoNFs = await _jsonService.ReadJsonAsync();
            return Ok(recebimentoNFs);
        }

        // GET: api/recebimentonf/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RecebimentoNF>> GetRecebimentoNF(int id)
        {
            var recebimentoNFs = await _jsonService.ReadJsonAsync();
            var recebimentoNF = recebimentoNFs.FirstOrDefault(r => r.Id == id);

            if (recebimentoNF == null)
            {
                return NotFound();
            }

            return Ok(recebimentoNF);
        }

        // POST: api/recebimentonf
        [HttpPost]
        public async Task<ActionResult> AddRecebimentoNF([FromBody] RecebimentoNF novoRecebimentoNF)
        {
            if (novoRecebimentoNF == null)
            {
                return BadRequest("Recebimento de Nota Fiscal não pode ser nulo.");
            }

            // Verificação de validação do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recebimentoNFs = await _jsonService.ReadJsonAsync();

            // Autoincremento do ID
            novoRecebimentoNF.Id = recebimentoNFs.Count > 0 ? recebimentoNFs.Max(r => r.Id) + 1 : 1;

            recebimentoNFs.Add(novoRecebimentoNF);
            await _jsonService.WriteJsonAsync(recebimentoNFs);

            return CreatedAtAction(nameof(GetRecebimentoNF), new { id = novoRecebimentoNF.Id }, novoRecebimentoNF);
        }

        // PUT: api/recebimentonf/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRecebimentoNF(int id, [FromBody] RecebimentoNF recebimentoNFAtualizado)
        {
            if (recebimentoNFAtualizado == null)
            {
                return BadRequest("Recebimento de Nota Fiscal não pode ser nulo.");
            }

            var recebimentoNFs = await _jsonService.ReadJsonAsync();
            var index = recebimentoNFs.FindIndex(r => r.Id == id);

            if (index == -1)
            {
                return NotFound("Recebimento de Nota Fiscal não encontrado.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (recebimentoNFAtualizado.Id != id)
            {
                return BadRequest("ID do recebimento de nota fiscal não corresponde ao ID da URL.");
            }

            recebimentoNFs[index] = recebimentoNFAtualizado; // Atualiza o recebimento
            await _jsonService.WriteJsonAsync(recebimentoNFs);

            return Ok(recebimentoNFAtualizado);
        }

        // DELETE: api/recebimentonf/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRecebimentoNF(int id)
        {
            var recebimentoNFs = await _jsonService.ReadJsonAsync();
            var recebimentoNF = recebimentoNFs.FirstOrDefault(r => r.Id == id);

            if (recebimentoNF == null)
            {
                return NotFound();
            }

            recebimentoNFs.Remove(recebimentoNF);
            await _jsonService.WriteJsonAsync(recebimentoNFs);

            return NoContent();
        }
    }
}
