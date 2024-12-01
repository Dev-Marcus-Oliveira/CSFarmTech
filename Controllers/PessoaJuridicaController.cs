using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaJuridicaController : ControllerBase
    {
        private readonly JsonService<PessoaJuridica> _jsonService;

        public PessoaJuridicaController()
        {
            _jsonService = new JsonService<PessoaJuridica>(Path.Combine(Directory.GetCurrentDirectory(), "db", "pessoasJuridicas.json"));
        }

        // GET: api/pessoajuridica
        [HttpGet]
        public async Task<ActionResult<List<PessoaJuridica>>> GetPessoasJuridicas()
        {
            var pessoasJuridicas = await _jsonService.ReadJsonAsync();
            return Ok(pessoasJuridicas);
        }

        // GET: api/pessoajuridica/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaJuridica>> GetPessoaJuridica(int id)
        {
            var pessoasJuridicas = await _jsonService.ReadJsonAsync();
            var pessoaJuridica = pessoasJuridicas.FirstOrDefault(p => p.Id == id);

            if (pessoaJuridica == null)
            {
                return NotFound();
            }

            return Ok(pessoaJuridica);
        }

        // POST: api/pessoajuridica
        [HttpPost]
        public async Task<ActionResult> AddPessoaJuridica([FromBody] PessoaJuridica novaPessoaJuridica)
        {
            if (novaPessoaJuridica == null)
            {
                return BadRequest("Pessoa Jurídica não pode ser nula.");
            }

            var pessoasJuridicas = await _jsonService.ReadJsonAsync();

            // Autoincrementação do ID
            novaPessoaJuridica.Id = pessoasJuridicas.Any() 
                ? pessoasJuridicas.Max(p => p.Id) + 1 
                : 1;

            // Validação de CNPJ duplicado
            if (pessoasJuridicas.Any(p => p.CNPJ == novaPessoaJuridica.CNPJ))
            {
                return Conflict("Uma pessoa jurídica com o mesmo CNPJ já existe.");
            }

            pessoasJuridicas.Add(novaPessoaJuridica);
            await _jsonService.WriteJsonAsync(pessoasJuridicas);

            return CreatedAtAction(nameof(GetPessoaJuridica), new { id = novaPessoaJuridica.Id }, novaPessoaJuridica);
        }

        // PUT: api/pessoajuridica/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePessoaJuridica(int id, [FromBody] PessoaJuridica pessoaJuridicaAtualizada)
        {
            if (pessoaJuridicaAtualizada == null)
            {
                return BadRequest("Pessoa Jurídica não pode ser nula.");
            }

            var pessoasJuridicas = await _jsonService.ReadJsonAsync();
            var index = pessoasJuridicas.FindIndex(p => p.Id == id);

            if (index == -1)
            {
                return NotFound("Pessoa Jurídica não encontrada.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (pessoaJuridicaAtualizada.Id != id)
            {
                return BadRequest("ID da pessoa jurídica não corresponde ao ID da URL.");
            }

            pessoasJuridicas[index] = pessoaJuridicaAtualizada; // Atualiza a pessoa jurídica
            await _jsonService.WriteJsonAsync(pessoasJuridicas);

            return Ok(pessoaJuridicaAtualizada);
        }

        // DELETE: api/pessoajuridica/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePessoaJuridica(int id)
        {
            var pessoasJuridicas = await _jsonService.ReadJsonAsync();
            var pessoaJuridica = pessoasJuridicas.FirstOrDefault(p => p.Id == id);

            if (pessoaJuridica == null)
            {
                return NotFound();
            }

            pessoasJuridicas.Remove(pessoaJuridica);
            await _jsonService.WriteJsonAsync(pessoasJuridicas);

            return NoContent();
        }
    }
}
