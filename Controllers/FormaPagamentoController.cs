using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormaPagamentoController : ControllerBase
    {
        private readonly JsonService<FormaPagamento> _jsonService;

        public FormaPagamentoController()
        {
            _jsonService = new JsonService<FormaPagamento>(Path.Combine(Directory.GetCurrentDirectory(), "db", "formasPagamento.json"));
        }

        // GET: api/formapagamento
        [HttpGet]
        public async Task<ActionResult<List<FormaPagamento>>> GetFormasPagamento()
        {
            var formasPagamento = await _jsonService.ReadJsonAsync();
            return Ok(formasPagamento);
        }

        // GET: api/formapagamento/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FormaPagamento>> GetFormaPagamento(int id)
        {
            var formasPagamento = await _jsonService.ReadJsonAsync();
            var formaPagamento = formasPagamento.FirstOrDefault(f => f.Id == id);

            if (formaPagamento == null)
            {
                return NotFound();
            }

            return Ok(formaPagamento);
        }

        // POST: api/formapagamento
        [HttpPost]
        public async Task<ActionResult> AddFormaPagamento([FromBody] FormaPagamento novaFormaPagamento)
        {
            if (novaFormaPagamento == null)
            {
                return BadRequest("Forma de pagamento não pode ser nula.");
            }

            // Validações do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var formasPagamento = await _jsonService.ReadJsonAsync();

            // Auto-incremento do ID
            novaFormaPagamento.Id = formasPagamento.Count > 0 ? formasPagamento.Max(f => f.Id) + 1 : 1;

            formasPagamento.Add(novaFormaPagamento);
            await _jsonService.WriteJsonAsync(formasPagamento);

            return CreatedAtAction(nameof(GetFormaPagamento), new { id = novaFormaPagamento.Id }, novaFormaPagamento);
        }

        // PUT: api/formapagamento/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFormaPagamento(int id, [FromBody] FormaPagamento formaPagamentoAtualizada)
        {
            if (formaPagamentoAtualizada == null)
            {
                return BadRequest("Forma de pagamento não pode ser nula.");
            }

            // Validações do modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var formasPagamento = await _jsonService.ReadJsonAsync();
            var index = formasPagamento.FindIndex(f => f.Id == id);

            if (index == -1)
            {
                return NotFound("Forma de pagamento não encontrada.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (formaPagamentoAtualizada.Id != id)
            {
                return BadRequest("ID da forma de pagamento não corresponde ao ID da URL.");
            }

            formasPagamento[index] = formaPagamentoAtualizada; // Atualiza a forma de pagamento
            await _jsonService.WriteJsonAsync(formasPagamento);

            return Ok(formaPagamentoAtualizada);
        }

        // DELETE: api/formapagamento/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFormaPagamento(int id)
        {
            var formasPagamento = await _jsonService.ReadJsonAsync();
            var formaPagamento = formasPagamento.FirstOrDefault(f => f.Id == id);

            if (formaPagamento == null)
            {
                return NotFound();
            }

            formasPagamento.Remove(formaPagamento);
            await _jsonService.WriteJsonAsync(formasPagamento);

            return NoContent();
        }
    }
}
