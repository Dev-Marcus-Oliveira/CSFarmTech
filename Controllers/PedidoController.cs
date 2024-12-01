using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PIM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly JsonService<Pedido> _jsonService;

        public PedidoController()
        {
            _jsonService = new JsonService<Pedido>(Path.Combine(Directory.GetCurrentDirectory(), "db", "pedidos.json"));
        }

        // GET: api/pedido
        [HttpGet]
        public async Task<ActionResult<List<Pedido>>> GetPedidos()
        {
            var pedidos = await _jsonService.ReadJsonAsync();
            return Ok(pedidos);
        }

        // GET: api/pedido/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedidos = await _jsonService.ReadJsonAsync();
            var pedido = pedidos.FirstOrDefault(p => p.IdPedido == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return Ok(pedido);
        }

        // POST: api/pedido
        [HttpPost]
        public async Task<ActionResult> AddPedido([FromBody] Pedido novoPedido)
        {
            if (novoPedido == null)
            {
                return BadRequest("Pedido não pode ser nulo.");
            }

            var pedidos = await _jsonService.ReadJsonAsync();

            // Autoincrementação do ID
            novoPedido.IdPedido = pedidos.Any() 
                ? pedidos.Max(p => p.IdPedido) + 1 
                : 1;

            pedidos.Add(novoPedido);
            await _jsonService.WriteJsonAsync(pedidos);

            return CreatedAtAction(nameof(GetPedido), new { id = novoPedido.IdPedido }, novoPedido);
        }
                    

        // PUT: api/pedido/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePedido(int id, [FromBody] Pedido pedidoAtualizado)
        {
            if (pedidoAtualizado == null)
            {
                return BadRequest("Pedido não pode ser nulo.");
            }

            var pedidos = await _jsonService.ReadJsonAsync();
            var index = pedidos.FindIndex(p => p.IdPedido == id);

            if (index == -1)
            {
                return NotFound("Pedido não encontrado.");
            }

            // Verifica se o id do corpo da requisição corresponde ao id da URL
            if (pedidoAtualizado.IdPedido != id)
            {
                return BadRequest("ID do pedido não corresponde ao ID da URL.");
            }

            pedidos[index] = pedidoAtualizado; // Atualiza o pedido
            await _jsonService.WriteJsonAsync(pedidos);

            return Ok(pedidoAtualizado);
        }

        // DELETE: api/pedido/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePedido(int id)
        {
            var pedidos = await _jsonService.ReadJsonAsync();
            var pedido = pedidos.FirstOrDefault(p => p.IdPedido == id);

            if (pedido == null)
            {
                return NotFound();
            }

            pedidos.Remove(pedido);
            await _jsonService.WriteJsonAsync(pedidos);

            return NoContent();
        }
    }
}
