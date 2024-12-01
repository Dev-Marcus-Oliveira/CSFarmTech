using System.ComponentModel.DataAnnotations;

public class RecebimentoNF
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O tipo de recebimento é obrigatório.")]
    [StringLength(10, ErrorMessage = "O tipo de recebimento deve ter no máximo 10 caracteres.")]
    public string Tipo { get; set; } // Email, SMS
}
