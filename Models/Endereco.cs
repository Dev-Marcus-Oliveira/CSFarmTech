using System.ComponentModel.DataAnnotations;

public class Endereco
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo Rua é obrigatório.")]
    [StringLength(100, ErrorMessage = "A Rua não pode ter mais que 100 caracteres.")]
    public string Rua { get; set; }

    [Required(ErrorMessage = "O campo Número é obrigatório.")]
    [StringLength(10, ErrorMessage = "O Número não pode ter mais que 10 caracteres.")]
    public string Numero { get; set; }

    [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
    [StringLength(50, ErrorMessage = "O Bairro não pode ter mais que 50 caracteres.")]
    public string Bairro { get; set; }

    [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
    [StringLength(50, ErrorMessage = "A Cidade não pode ter mais que 50 caracteres.")]
    public string Cidade { get; set; }

    [Required(ErrorMessage = "O campo Estado é obrigatório.")]
    public int EstadoId { get; set; } // FK para Estado

    [Required(ErrorMessage = "O campo CEP é obrigatório.")]
    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O CEP deve estar no formato 99999-999.")]
    public string CEP { get; set; }

    [Required(ErrorMessage = "O campo Tipo de Endereço é obrigatório.")]
    public int TipoEnderecoId { get; set; } // FK para Tipos de Endereço
}
