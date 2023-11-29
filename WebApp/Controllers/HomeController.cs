using Dominio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        Sistema Sis = Sistema.GetInstancia();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string textoAFiltrar, int valorAFltrar)
        {
            ViewBag.msg = TempData["error"];

			if (String.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogueado")))
            {
				return RedirectToAction("InicioSesion", "Usuario");
            }
            else
            {
                if (String.IsNullOrEmpty(textoAFiltrar) && valorAFltrar == 0)
                {

					if (HttpContext.Session.GetString("RolUsuarioLogueado") == "Miembro")
                    {
                        var modelo = new Tuple<List<Post>, List<Comentario>>(Sis.GetPostParaUsuario((int)HttpContext.Session.GetInt32("IdUsuarioLogueado")), new List<Comentario>());
						return View(modelo);
                    }
                    else
                    {
						var modelo = new Tuple<List<Post>, List<Comentario>>(Sis.GetPosts(), new List<Comentario>());
						return View(modelo);
					}	
                }
                else
                {
					var modelo = new Tuple<List<Post>, List<Comentario>>(Sis.GetPostFiltradoParaUsuario((int)HttpContext.Session.GetInt32("IdUsuarioLogueado"), textoAFiltrar, valorAFltrar), Sis.GetComentarioFiltrados((int)HttpContext.Session.GetInt32("IdUsuarioLogueado"), textoAFiltrar, valorAFltrar));
                    return View(modelo);
				}
			}
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}