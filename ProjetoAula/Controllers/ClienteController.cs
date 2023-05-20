using Microsoft.AspNetCore.Mvc;
using ProjetoAula.Models;
using RestSharp.Authenticators;
using RestSharp;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoAula.Entidades;
using ProjetoAula.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ProjetoAula.Controllers
{
	public class ClienteController : Controller
	{

		private HorusContext _context;

		public ClienteController(HorusContext context) 
		{
			_context = context;
		}

		public IActionResult Index()
		{
		
			return View();
		}

		public IActionResult Listar()
		{
			var clientes = _context.Cliente.Include(c => c.Endereco).ToList();
			ViewBag.Clientes = clientes;
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Form()
		{
			var model = new ClienteModel();
			var estados = await BuscarEstados();
			var estadosSelect = new List<SelectListItem>() { new SelectListItem() { Value = "", Text = "Selecione o estado" } };
			foreach (var estado in estados.OrderBy(c => c.Nome))
				estadosSelect.Add(new SelectListItem() { Value = estado.Id.ToString(), Text = estado.Nome });
			ViewBag.Estados = estadosSelect;
			return View(model);
		}

		[HttpPost]
		public IActionResult Form(ClienteModel model)
		{

			var cliente = new Cliente()
			{
				Ativo = model.Ativo,
				InscricaoEstadual = model.InscricaoEstadual,
				InscricaoFederal = model.InscricaoFederal,
				NomeFantasia = model.NomeFantasia,
				RazaoSocial = model.RazaoSocial,
				Tributacao = model.Tributacao,
				Endereco = new Endereco()
				{
					Bairro = model.Endereco.Bairro,
					Cep = model.Endereco.Cep,
					Cidade = model.Endereco.Cidade,
					Estado = model.Endereco.Estado,
					Rua = model.Endereco.Rua
				}
			};
			_context.Cliente.Add(cliente);
			_context.SaveChanges();

			return View();
		}

		public async Task<List<EstadoModel>> BuscarEstados()
		{
			var options = new RestClientOptions("https://servicodados.ibge.gov.br/");
			var client = new RestClient(options);
			var request = new RestRequest("api/v1/localidades/estados");
			var response = await client.GetAsync<List<EstadoModel>>(request);
			return response;
		}

		[HttpGet]
		public async Task<JsonResult> PegarCidades(long estadoId)
		{
			var options = new RestClientOptions("https://servicodados.ibge.gov.br/");
			var client = new RestClient(options);
			var request = new RestRequest($"api/v1/localidades/estados/{estadoId}/municipios", Method.Get);
			var cidades = await client.GetAsync<List<CidadeModel>>(request);
			return Json(cidades);

		}


	}
}
