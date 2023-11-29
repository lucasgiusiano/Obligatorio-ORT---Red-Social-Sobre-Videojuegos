using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class PublicacionController : Controller
    {
        Sistema Sis = Sistema.GetInstancia();
        private IWebHostEnvironment env;

        public PublicacionController(IWebHostEnvironment _env)
        {
            env = _env;
        }

        public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public IActionResult NuevoPost(string Titulo, string Contenido, string Privado, IFormFile Imagen)
		{
			bool Priv = false;
            string Img = String.Empty;

			if (Privado == "on")
			{
				Priv = true;
			}

            if (Imagen != null)
            {
				string rutaGuardado = env.WebRootPath + "//images//";

				if (!Directory.Exists(rutaGuardado))
				{
					Directory.CreateDirectory(rutaGuardado);
				}

                string extension = Path.GetExtension(Imagen.FileName);
                string fileName = Titulo + extension;
				FileStream stream = new FileStream(rutaGuardado + fileName, FileMode.Create);
				Imagen.CopyTo(stream);
				stream.Close();
				Img = fileName;
            }
            else
            {
                Img = "postBasico.png";
			}

			Post Nuevo = new Post(Titulo, Contenido, Sis.BuscarUsuario((int)HttpContext.Session.GetInt32("IdUsuarioLogueado")), Img, Priv);

            try
            {
				Sis.AltaPublicacion(Nuevo);
			}
            catch (Exception e)
            {
                TempData["Mensaje"] = "Error: " + e.Message;
            }
			
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
        public IActionResult NuevoComentarioAPost(int IdPost, string Titulo, string Contenido)
        {

            try
            {
                Miembro miembroAutor = Sis.BuscarUsuario((int)HttpContext.Session.GetInt32("IdUsuarioLogueado"));
                Post p = Sis.BuscarPost(IdPost);
                Comentario c = new Comentario(Titulo, Contenido, miembroAutor);
                Sis.AgregarComentarios(p, c);
            }
            catch (Exception e)
            {
                TempData["error"] = "Error: " + e.Message;
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult PostsFiltrados(string textoPostBuscado, int valorDeAceptacion)
        {
            if (String.IsNullOrEmpty(textoPostBuscado) && valorDeAceptacion == 0)
            {
				return RedirectToAction("Index", "Home");
			}
            else
            {
				return RedirectToAction("Index", "Home", new { textoAFiltrar = textoPostBuscado, valorAFltrar = valorDeAceptacion});
			}
        }

        public IActionResult CensurarPublicacion(int idPubliCensurada)
        {
            if (HttpContext.Session.GetString("RolUsuarioLogueado") == "Administrador")
            {
                Sis.CensurarPublicacion(idPubliCensurada);
            }
            else
            {
                TempData["Mensaje"] = "No cuenta con los permisos necesarios para censurar publicaciones";
            }
            return RedirectToAction("Index", "Home");
        }

		public IActionResult SumarReaccion(int IdPostReaccionado, string TipoReaccion)
        {
            Reaccion reaccion = new Reaccion(TipoReaccion,Sis.BuscarUsuario((int)HttpContext.Session.GetInt32("IdUsuarioLogueado")));
            Publicacion p = Sis.BuscarPublicacion(IdPostReaccionado);
			p.Reaccionar(reaccion);
            return RedirectToAction("Index", "Home");
        }

	}
}
