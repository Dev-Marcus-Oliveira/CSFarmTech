using System.ComponentModel.DataAnnotations;

public class Pedido
{
    public int IdPedido { get; set; }
    
    [Required(ErrorMessage = "ID do usuário é obrigatório.")]
    public int UsuarioId { get; set; } // FK para Usuario
    
    [Required(ErrorMessage = "Data do pedido é obrigatória.")]
    public DateTime DataPedido { get; set; }
    
    [Required(ErrorMessage = "Status do pedido é obrigatório.")]
    public int Status { get; set; } // FK para statusPedido
    
    [Required(ErrorMessage = "Total do pedido é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O total deve ser maior que zero.")]
    public decimal Total { get; set; }
}
