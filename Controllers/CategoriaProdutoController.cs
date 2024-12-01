using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaProdutoController : ControllerBase
    {
        private readonly JsonService<CategoriaProduto> _jsonService;

        public CategoriaProdutoController()
        {
            _jsonService = new JsonService<CategoriaProduto>(Path.Combine(Directory.GetCurrentDirectory(), "db", "categoriasProduto.json"));
        }

        // GET: api/categoriaproduto
        [HttpGet]
        public async Task<ActionResult<List<CategoriaProduto>>> GetCategorias()
        {
            var categorias = await _jsonService.ReadJsonAsync();
            return Ok(categorias);
        }

        // GET: api/categoriaproduto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaProduto>> GetCategoria(int id)
        {
            var categorias = await _jsonService.ReadJsonAsync();
            var categoria = categorias.FirstOrDefault(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        // POST: api/categoriaproduto
        [HttpPost]
        public async Task<ActionResult> AddCategoria([FromBody] CategoriaProduto novaCategoria)
        {
            if (novaCategoria == null)
            {
                return BadRequest("Categoria não pode ser nula.");
            }

            var categorias = await _jsonService.ReadJsonAsync();

            // Gera o próximo ID de forma auto-incrementada
            var novoId = categorias.Any() ? categorias.Max(c => c.Id) + 1 : 1;
            novaCategoria.Id = novoId;

            categorias.Add(novaCategoria);
            await _jsonService.WriteJsonAsync(categorias);

            return CreatedAtAction(nameof(GetCategoria), new { id = novaCategoria.Id }, novaCategoria);
        }

        // PUT: api/categoriaproduto/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategoria(int id, [FromBody] CategoriaProduto categoriaAtualizada)
        {
            if (categoriaAtualizada == null)
            {
                return BadRequest("Categoria não pode ser nula.");
            }

            var categorias = await _jsonService.ReadJsonAsync();
            var index = categorias.FindIndex(c => c.Id == id);

            if (index == -1)
            {
                return NotFound("Categoria não encontrada.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (categoriaAtualizada.Id != id)
            {
                return BadRequest("ID da categoria não corresponde ao ID da URL.");
            }

            categorias[index] = categoriaAtualizada; // Atualiza a categoria
            await _jsonService.WriteJsonAsync(categorias);

            return Ok(categoriaAtualizada);
        }

        // DELETE: api/categoriaproduto/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoria(int id)
        {
            var categorias = await _jsonService.ReadJsonAsync();
            var categoria = categorias.FirstOrDefault(c => c.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categorias.Remove(categoria);
            await _jsonService.WriteJsonAsync(categorias);

            return NoContent();
        }
    }
}
