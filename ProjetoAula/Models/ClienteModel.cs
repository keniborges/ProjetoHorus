
using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;

namespace ProjetoAula.Models
{
	public class ClienteModel
	{
		public long Id { get; set; }

		[Required]
		[Display(Name = "Nome Fantasia")]
		public string NomeFantasia { get; set; }

		[Required]
		[Display(Name = "Razão Social")]
		public string RazaoSocial { get; set; }

		[Required]
		[Display(Name = "Inscrição Federal")]
		public string InscricaoFederal { get; set; }

		[Display(Name = "Inscrição Estadual")]
		public string InscricaoEstadual { get; set; }

		[Display(Name = "Ativo")]
		public bool Ativo { get; set; }

		[Display(Name = "Tributação")]
		public TributacaoEnum Tributacao { get; set; }

		[Display(Name = "Endereço")]
		public EnderecoModel Endereco { get; set;}
	}
}
