using System.ComponentModel.DataAnnotations;

public class TipoEndereco
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O tipo de endereço é obrigatório.")]
    [StringLength(50, ErrorMessage = "O tipo de endereço não pode ter mais de 50 caracteres.")]
    public string Tipo { get; set; } // Entrega, Cobrança
}
