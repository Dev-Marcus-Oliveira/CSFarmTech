using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[Controller]
[Route("api/[controller]")]
public class UsuarioController : Controller
{
    private readonly JsonService<Usuario> _jsonService;

    public UsuarioController()
    {
        _jsonService = new JsonService<Usuario>(Path.Combine(Directory.GetCurrentDirectory(), "db", "usuarios.json"));
    }

    [HttpGet]
    public async Task<ActionResult<List<Usuario>>> GetUsuarios()
    {
        var usuarios = await _jsonService.ReadJsonAsync();
        return Ok(usuarios);
    }

    [HttpPost]
    public async Task<ActionResult> AddUsuario([FromBody] Usuario novoUsuario)
    {
        if (!ModelState.IsValid) // Validação do modelo
        {
            return BadRequest(ModelState);
        }

        var usuarios = await _jsonService.ReadJsonAsync();

        // Autoincremento do ID
        novoUsuario.Id = usuarios.Count > 0 ? usuarios.Max(u => u.Id) + 1 : 1;
        novoUsuario.DataCriacao = DateTime.UtcNow; // Define a data de criação como a data atual

        usuarios.Add(novoUsuario);
        await _jsonService.WriteJsonAsync(usuarios);
        return CreatedAtAction(nameof(GetUsuarios), new { id = novoUsuario.Id }, novoUsuario);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] string login, [FromForm] string senha)
    {
        var usuarios = await _jsonService.ReadJsonAsync();

        // Verifica se o usuário existe e se a senha é válida
        var usuarioValido = usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);

        if (usuarioValido == null)
        {
            return BadRequest(new { message = "Login ou senha inválidos" });
        }

        // Se for válido, redireciona para a página inicial ou painel
        return Ok(new { message = "Login realizado com sucesso", usuario = usuarioValido });
    }

    [HttpPost("VerificarEmail")]
    public async Task<IActionResult> VerificarEmail([FromForm] string email)
    {
        var usuarios = await _jsonService.ReadJsonAsync();

        // Verifica se o e-mail existe
        var usuario = usuarios.FirstOrDefault(u => u.Email == email);

        if (usuario == null)
        {
            return BadRequest(new { message = "E-mail não encontrado." });
        }

        // Se o e-mail for encontrado, retorne sucesso e prossiga
        return Ok(new { message = "E-mail válido. Proceda com a recuperação." });
    }

    // Ação para exibir a página de recuperação de senha
    [HttpGet("recuperacao-senha")]
    public IActionResult RecuperacaoSenha()
    {
        return View(); // Retorna a view recuperacao_senha.cshtml
    }

    // Ação para processar a recuperação de senha
    [HttpPost("recuperacao-senha")]
    public async Task<IActionResult> RecuperacaoSenha(string email)
    {
        // Aqui você pode adicionar a lógica para enviar um e-mail de recuperação
        var usuarios = await _jsonService.ReadJsonAsync();
        var usuario = usuarios.FirstOrDefault(u => u.Email == email);

        if (usuario == null)
        {
            return BadRequest(new { message = "E-mail não encontrado." });
        }

        // Lógica de envio de e-mail ou geração de link de recuperação aqui

        return RedirectToAction("RecuperacaoSenhaConfirmacao");
    }

    // Ação para confirmar que o e-mail foi enviado
    public IActionResult RecuperacaoSenhaConfirmacao()
    {
        return View(); // Retorna a view de confirmação
    }
}
