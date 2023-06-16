namespace ProjetoAula.Models
{
	public class EnderecoModel
	{

		public long Id { get; set; }
		public string Rua { get; set; }

		public string Bairro { get; set; }	

		public string Estado { get; set; }

		public string Cidade { get; set; }

		public string Cep { get; set; }
	}
}
