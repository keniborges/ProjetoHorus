using Microsoft.AspNetCore.Mvc;
using ProjetoAula.Models;
using RestSharp.Authenticators;
using RestSharp;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjetoAula.Controllers
{
	public class ClienteController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Listar()
		{
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
