using ProjetoAula.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjetoAula.Entidades
{
	public class Cliente : Entidade
	{

		[Required, MaxLength(120)]
		public string NomeFantasia { get; set; }

		[Required, MaxLength(120)]
		public string RazaoSocial { get; set; }

		[Required, MaxLength(18)]
		public string InscricaoFederal { get; set; }

		public string? InscricaoEstadual { get; set; }

		public bool Ativo { get; set; } = true;

		public TributacaoEnum Tributacao { get; set; }

		public Endereco Endereco { get; set; }
	}
}
