using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly JsonService<Produto> _jsonService;

        public ProdutoController()
        {
            _jsonService = new JsonService<Produto>(Path.Combine(Directory.GetCurrentDirectory(), "db", "produtos.json"));
        }

        // GET: api/produto
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> GetProdutos()
        {
            var produtos = await _jsonService.ReadJsonAsync();
            return Ok(produtos);
        }

        // POST: api/produto
        [HttpPost]
        public async Task<ActionResult> AddProduto([FromBody] Produto novoProduto)
        {
            if (novoProduto == null)
            {
                return BadRequest("Produto não pode ser nulo.");
            }

            // Validação do produto
            if (string.IsNullOrEmpty(novoProduto.Nome) || novoProduto.Preco <= 0 || novoProduto.QuantidadeEstoque < 0)
            {
                return BadRequest("Dados do produto inválidos.");
            }

            var produtos = await _jsonService.ReadJsonAsync();

            // Autoincrementa o ID do produto
            novoProduto.Id = produtos.Count > 0 ? produtos.Max(p => p.Id) + 1 : 1;

            produtos.Add(novoProduto);
            await _jsonService.WriteJsonAsync(produtos);

            return CreatedAtAction(nameof(GetProdutos), new { id = novoProduto.Id }, novoProduto);
        }

        // PUT: api/produto
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduto(int id, [FromBody] Produto produtoAtualizado)
        {
            if (produtoAtualizado == null)
            {
                return BadRequest("Produto não pode ser nulo.");
            }

            var produtos = await _jsonService.ReadJsonAsync();
            var index = produtos.FindIndex(p => p.Id == id);
            if (index == -1) return NotFound(); // Produto não encontrado

            // Validação do produto atualizado
            if (string.IsNullOrEmpty(produtoAtualizado.Nome) || produtoAtualizado.Preco <= 0 || produtoAtualizado.QuantidadeEstoque < 0)
            {
                return BadRequest("Dados do produto inválidos.");
            }

            produtoAtualizado.Id = id; // Garante que o ID permaneça o mesmo
            produtos[index] = produtoAtualizado; // Atualiza o produto
            await _jsonService.WriteJsonAsync(produtos);
            return Ok(produtoAtualizado);
        }

        // DELETE: api/produto/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduto(int id)
        {
            var produtos = await _jsonService.ReadJsonAsync();
            var produto = produtos.Find(p => p.Id == id);
            if (produto == null) return NotFound(); // Produto não encontrado

            produtos.Remove(produto); // Remove o produto
            await _jsonService.WriteJsonAsync(produtos);
            return NoContent();
        }
    }
}
