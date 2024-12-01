using System;
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "O tipo de usuário deve ter exatamente 2 caracteres (PF ou PJ).")]
    public string TipoUsuario { get; set; } // PF ou PJ

    [Required(ErrorMessage = "O login é obrigatório.")]
    [StringLength(50, ErrorMessage = "O login não pode ter mais de 50 caracteres.")]
    public string Login { get; set; } 

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
    public string Senha { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "O formato do telefone é inválido.")]
    public string Telefone { get; set; }

    public string ImagemPerfil { get; set; } // Imagem não obrigatória

    public DateTime DataCriacao { get; set; }

    [Required(ErrorMessage = "O ID do endereço é obrigatório.")]
    public int EnderecoId { get; set; }
}
