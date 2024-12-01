using System.ComponentModel.DataAnnotations;

public class CategoriaProduto
{
    // O ID será gerenciado pelo código para ser auto-incrementado
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome da categoria deve ter no máximo 100 caracteres.")]
    public string NomeCategoria { get; set; }
}
