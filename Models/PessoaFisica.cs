using System;
using System.ComponentModel.DataAnnotations;

public class PessoaFisica
{
    public int Id { get; set; } // FK para Usuario

    [Required(ErrorMessage = "Nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres.")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "CPF é obrigatório.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve ter 11 dígitos.")]
    public string CPF { get; set; } // CPF é o login para PF
    
    [Required(ErrorMessage = "Data de nascimento é obrigatória.")]
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }
}
