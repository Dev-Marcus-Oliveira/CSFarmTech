using System.ComponentModel.DataAnnotations;

public class Estado
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O nome do estado é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome do estado deve ter no máximo 50 caracteres.")]
    public string NomeEstado { get; set; }

    [Required(ErrorMessage = "A sigla do estado é obrigatória.")]
    [StringLength(2, ErrorMessage = "A sigla do estado deve ter 2 caracteres.")]
    public string Sigla { get; set; }
}
