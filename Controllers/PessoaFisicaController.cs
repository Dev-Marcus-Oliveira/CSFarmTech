using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaFisicaController : ControllerBase
    {
        private readonly JsonService<PessoaFisica> _jsonService;

        public PessoaFisicaController()
        {
            _jsonService = new JsonService<PessoaFisica>(Path.Combine(Directory.GetCurrentDirectory(), "db", "pessoasFisicas.json"));
        }

        // GET: api/pessoafisica
        [HttpGet]
        public async Task<ActionResult<List<PessoaFisica>>> GetPessoasFisicas()
        {
            var pessoasFisicas = await _jsonService.ReadJsonAsync();
            return Ok(pessoasFisicas);
        }

        // GET: api/pessoafisica/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaFisica>> GetPessoaFisica(int id)
        {
            var pessoasFisicas = await _jsonService.ReadJsonAsync();
            var pessoaFisica = pessoasFisicas.FirstOrDefault(p => p.Id == id);

            if (pessoaFisica == null)
            {
                return NotFound();
            }

            return Ok(pessoaFisica);
        }
        // POST: api/pessoafisica
        [HttpPost]
        public async Task<ActionResult> AddPessoaFisica([FromBody] PessoaFisica novaPessoaFisica)
        {
            if (novaPessoaFisica == null)
            {
                return BadRequest("Pessoa Física não pode ser nula.");
            }

            var pessoasFisicas = await _jsonService.ReadJsonAsync();

            // Autoincrementação do ID
            novaPessoaFisica.Id = pessoasFisicas.Any() 
                ? pessoasFisicas.Max(p => p.Id) + 1 
                : 1;

            // Validação de CPF duplicado
            if (pessoasFisicas.Any(p => p.CPF == novaPessoaFisica.CPF))
            {
                return Conflict("Uma pessoa física com o mesmo CPF já existe.");
            }

            pessoasFisicas.Add(novaPessoaFisica);
            await _jsonService.WriteJsonAsync(pessoasFisicas);

            return CreatedAtAction(nameof(GetPessoaFisica), new { id = novaPessoaFisica.Id }, novaPessoaFisica);
        }


        // PUT: api/pessoafisica/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePessoaFisica(int id, [FromBody] PessoaFisica pessoaFisicaAtualizada)
        {
            if (pessoaFisicaAtualizada == null)
            {
                return BadRequest("Pessoa Física não pode ser nula.");
            }

            var pessoasFisicas = await _jsonService.ReadJsonAsync();
            var index = pessoasFisicas.FindIndex(p => p.Id == id);

            if (index == -1)
            {
                return NotFound("Pessoa Física não encontrada.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (pessoaFisicaAtualizada.Id != id)
            {
                return BadRequest("ID da pessoa física não corresponde ao ID da URL.");
            }

            pessoasFisicas[index] = pessoaFisicaAtualizada; // Atualiza a pessoa física
            await _jsonService.WriteJsonAsync(pessoasFisicas);

            return Ok(pessoaFisicaAtualizada);
        }

        // DELETE: api/pessoafisica/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePessoaFisica(int id)
        {
            var pessoasFisicas = await _jsonService.ReadJsonAsync();
            var pessoaFisica = pessoasFisicas.FirstOrDefault(p => p.Id == id);

            if (pessoaFisica == null)
            {
                return NotFound();
            }

            pessoasFisicas.Remove(pessoaFisica);
            await _jsonService.WriteJsonAsync(pessoasFisicas);

            return NoContent();
        }
    }
}
