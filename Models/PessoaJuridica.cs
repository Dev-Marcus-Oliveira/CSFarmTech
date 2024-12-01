using System;
using System.ComponentModel.DataAnnotations;

public class PessoaJuridica
{
    public int Id { get; set; } // FK para Usuario

    [Required(ErrorMessage = "Nome Fantasia é obrigatório.")]
    [StringLength(100, ErrorMessage = "Nome Fantasia não pode exceder 100 caracteres.")]
    public string NomeFantasia { get; set; }

    [Required(ErrorMessage = "Razão Social é obrigatória.")]
    [StringLength(100, ErrorMessage = "Razão Social não pode exceder 100 caracteres.")]
    public string RazaoSocial { get; set; }

    [Required(ErrorMessage = "CNPJ é obrigatório.")]
    [RegularExpression(@"^\d{14}$", ErrorMessage = "CNPJ deve ter 14 dígitos.")]
    public string CNPJ { get; set; } // CNPJ é o login para PJ

    [Required(ErrorMessage = "Data de criação da empresa é obrigatória.")]
    [DataType(DataType.Date)]
    public DateTime DataCriacaoEmpresa { get; set; }
}
