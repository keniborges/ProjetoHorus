using Microsoft.AspNetCore.Mvc;
using ProjetoAula.Models;
using RestSharp.Authenticators;
using RestSharp;
using System.Threading;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoAula.Entidades;
using ProjetoAula.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders.Testing;

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

		private void BuscarClientes()
		{
			var clientes = _context.Cliente.Include(c => c.Endereco).ToList();
			ViewBag.Clientes = clientes;
		}

		public IActionResult Listar()
		{
			BuscarClientes();
			return View();
		}

		public async Task<IActionResult> Form()
		{
			var model = new ClienteModel();
			var estados = await BuscarEstados();
			var estadosSelect = new List<SelectListItem>() { new SelectListItem() { Text = "Escolha o Estado", Value = "" } };
			foreach (var estado in estados.OrderBy(c => c.Nome))
				estadosSelect.Add(new SelectListItem() { Text = estado.Nome, Value = estado.Id.ToString() });
			ViewBag.Estados = estadosSelect;
			return View(model);
		}

		private List<SelectListItem> EstadosSelectItem(List<EstadoModel> estados)
		{
			var estadosSelect = new List<SelectListItem>() { new SelectListItem() { Text = "Escolha o Estado", Value = "" } };
			foreach (var estado in estados.OrderBy(c => c.Nome))
				estadosSelect.Add(new SelectListItem() { Text = estado.Nome, Value = estado.Id.ToString() });
			return estadosSelect;
		}

		private List<SelectListItem> CidadesSelectItem(List<CidadeModel> cidades)
		{
			var cidadesSelect = new List<SelectListItem>() { new SelectListItem() { Text = "Escolha o Estado", Value = "" } };
			foreach (var cidade in cidades)
				cidadesSelect.Add(new SelectListItem() { Text = cidade.Nome, Value = cidade.Id.ToString() });
			return cidadesSelect;
		}

		[HttpPost]
		public async Task<IActionResult> Form(ClienteModel model)
		{
			if (ModelState.IsValid)
			{
				var cliente = new Cliente()
				{
					Id = model.Id,
					Ativo = model.Ativo,
					InscricaoEstadual = model.InscricaoEstadual,
					InscricaoFederal = model.InscricaoFederal,
					NomeFantasia = model.NomeFantasia,
					RazaoSocial = model.RazaoSocial,
					Tributacao = model.Tributacao,
					Endereco = new Endereco()
					{
						Id = model.Endereco.Id,
						Bairro = model.Endereco.Bairro,
						Cep = model.Endereco.Cep,
						Cidade = model.Endereco.Cidade,
						Estado = model.Endereco.Estado,
						Rua = model.Endereco.Rua
					}
				};
				if (model.Id == 0)
				{
					_context.Cliente.Add(cliente);
				}
				else
				{
					_context.Cliente.Update(cliente);
				}
				_context.SaveChanges();
				return RedirectToAction("Listagem", "Cliente");

			}
			var estados = await BuscarEstados();
			ViewBag.Estados = EstadosSelectItem(estados);
			return View(model);

		}

		public async Task<List<EstadoModel>> BuscarEstados()
		{
			var options = new RestClientOptions("https://servicodados.ibge.gov.br/");
			var client = new RestClient(options);
			var request = new RestRequest("api/v1/localidades/estados");
			return await client.GetAsync<List<EstadoModel>>(request);
		}

		private async Task<List<CidadeModel>> BuscarCidades(long estadoId)
		{
			var options = new RestClientOptions("https://servicodados.ibge.gov.br/");
			var client = new RestClient(options);
			RestRequest request = new RestRequest($"api/v1/localidades/estados/{estadoId}/municipios", Method.Get);
			var cidades = await client.GetAsync<List<CidadeModel>>(request);
			return cidades.OrderBy(c => c.Nome).ToList();
		}

		public async Task<JsonResult> PegarCidades(long estadoId)
		{
			return Json(await BuscarCidades(estadoId));
		}

		public async Task<IActionResult> Editar(long Id)
		{
			var cliente = _context.Cliente.Include(c => c.Endereco).FirstOrDefault(c => c.Id == Id);
			var model = new ClienteModel()
			{
				Id = cliente.Id,
				Ativo = cliente.Ativo,
				InscricaoFederal = cliente.InscricaoFederal,
				InscricaoEstadual = cliente.InscricaoEstadual,
				NomeFantasia = cliente.NomeFantasia,
				RazaoSocial = cliente.RazaoSocial,
				Tributacao = cliente.Tributacao,
				Endereco = new EnderecoModel()
				{
					Id = cliente.Endereco.Id,
					Bairro = cliente.Endereco.Bairro,
					Cep = cliente.Endereco.Cep,
					Estado = cliente.Endereco.Estado,
					Cidade = cliente.Endereco.Cidade,
					Rua = cliente.Endereco.Rua
				}
			};
			var estados = await BuscarEstados();
			var cidades = await BuscarCidades(Convert.ToInt16(cliente.Endereco.Estado));
			ViewBag.Estados = EstadosSelectItem(estados);
			ViewBag.Cidades = CidadesSelectItem(cidades);
			return View("Form", model);
		}

		[HttpGet]
		public IActionResult Excluir(long Id)
		{
			var cliente = _context.Cliente.FirstOrDefault(c => c.Id == Id);
			_context.Cliente.Remove(cliente);
			_context.SaveChanges();
			//_context.Database.ExecuteSqlRaw($"delete from \"Cliente\" where \"Id\" = {Id}");
			BuscarClientes();
			return View("Listagem");
		}


	}
}
