using System.ComponentModel.DataAnnotations;

public class FormaPagamento
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O tipo de pagamento é obrigatório.")]
    [StringLength(20, ErrorMessage = "O tipo de pagamento deve ter no máximo 20 caracteres.")]
    public string TipoPagamento { get; set; } // Crédito, Débito, Boleto, PIX
}
