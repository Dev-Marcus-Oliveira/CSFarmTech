using System.ComponentModel.DataAnnotations;

public class StatusPedido
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo 'Status' é obrigatório.")]
    [StringLength(50, ErrorMessage = "O 'Status' não pode ter mais que 50 caracteres.")]
    public string Status { get; set; }
}
