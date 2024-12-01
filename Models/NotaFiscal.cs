using System.ComponentModel.DataAnnotations;
public class NotaFiscal
{
    public int IdNotaFiscal { get; set; }
    public int PedidoId { get; set; } // FK para Pedido
    
    [Required(ErrorMessage = "Número da nota é obrigatório.")]
    [StringLength(50, ErrorMessage = "Número da nota não pode exceder 50 caracteres.")]
    public string NumeroNota { get; set; }
    
    [Required(ErrorMessage = "Data de emissão é obrigatória.")]
    public DateTime DataEmissao { get; set; }

    [Required(ErrorMessage = "Forma de recebimento é obrigatória.")]
    [RegularExpression("Email|SMS", ErrorMessage = "Forma de recebimento deve ser 'Email' ou 'SMS'.")]
    public string FormaRecebimento { get; set; } // Email ou SMS
}
