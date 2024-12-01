using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class StatusPedidoController : ControllerBase
{
    private readonly JsonService<StatusPedido> _jsonService;

    public StatusPedidoController()
    {
        _jsonService = new JsonService<StatusPedido>(Path.Combine(Directory.GetCurrentDirectory(), "db", "statusPedido.json"));
    }

    // GET: api/statuspedido
    [HttpGet]
    public async Task<ActionResult<List<StatusPedido>>> GetStatusPedidos()
    {
        var statusPedidos = await _jsonService.ReadJsonAsync();
        return Ok(statusPedidos);
    }

    // GET: api/statuspedido/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<StatusPedido>> GetStatusPedido(int id)
    {
        var statusPedidos = await _jsonService.ReadJsonAsync();
        var statusPedido = statusPedidos.FirstOrDefault(t => t.Id == id);

        if (statusPedido == null)
        {
            return NotFound("Status do pedido não encontrado.");
        }

        return Ok(statusPedido);
    }

    // POST: api/statuspedido
    [HttpPost]
    public async Task<ActionResult> AddStatusPedido([FromBody] StatusPedido novoStatusPedido)
    {
        if (novoStatusPedido == null)
        {
            return BadRequest("O status do pedido não pode ser nulo.");
        }

        var statusPedidos = await _jsonService.ReadJsonAsync();

        // Autoincrementação do ID
        int novoId = statusPedidos.Any() ? statusPedidos.Max(t => t.Id) + 1 : 1;
        novoStatusPedido.Id = novoId;

        statusPedidos.Add(novoStatusPedido);
        await _jsonService.WriteJsonAsync(statusPedidos);

        return CreatedAtAction(nameof(GetStatusPedido), new { id = novoStatusPedido.Id }, novoStatusPedido);
    }

    // PUT: api/statuspedido/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStatusPedido(int id, [FromBody] StatusPedido statusPedidoAtualizado)
    {
        if (statusPedidoAtualizado == null)
        {
            return BadRequest("O status do pedido não pode ser nulo.");
        }

        var statusPedidos = await _jsonService.ReadJsonAsync();
        var index = statusPedidos.FindIndex(t => t.Id == id);

        if (index == -1)
        {
            return NotFound("Status do pedido não encontrado.");
        }

        // Verifica se o ID do corpo da requisição corresponde ao ID da URL
        if (statusPedidoAtualizado.Id != id)
        {
            return BadRequest("O ID do status do pedido não corresponde ao ID da URL.");
        }

        statusPedidos[index] = statusPedidoAtualizado; // Atualiza o status do pedido
        await _jsonService.WriteJsonAsync(statusPedidos);

        return Ok(statusPedidoAtualizado);
    }

    // DELETE: api/statuspedido/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStatusPedido(int id)
    {
        var statusPedidos = await _jsonService.ReadJsonAsync();
        var statusPedido = statusPedidos.FirstOrDefault(t => t.Id == id);

        if (statusPedido == null)
        {
            return NotFound("Status do pedido não encontrado.");
        }

        statusPedidos.Remove(statusPedido);
        await _jsonService.WriteJsonAsync(statusPedidos);

        return NoContent();
    }
}
