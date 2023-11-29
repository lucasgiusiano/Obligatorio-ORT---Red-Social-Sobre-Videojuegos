using Dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class UsuarioController : Controller
    {

        Sistema Sis = Sistema.GetInstancia();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registro()
        {
			return View();
        }

        [HttpPost]
        public IActionResult Registro(Miembro m)
        {
            try
            {
                Sis.AltaUsuario(m);
				return RedirectToAction("InicioSesion");
            }
            catch (Exception e)
            {
                ViewBag.msg = "No se ha podido completar el registro \n" + e.Message;
            }
            return View();
        }

        public IActionResult InicioSesion()
        {
            return View();
        }

        [HttpPost]
		public IActionResult InicioSesion(string email, string password)
		{
            Usuario usuarioLogueado = Sis.ValidarInicio(email, password);

			if (usuarioLogueado != null)
            {
                HttpContext.Session.SetString("UsuarioLogueado", usuarioLogueado.Email);
				HttpContext.Session.SetInt32("IdUsuarioLogueado", usuarioLogueado.Id);
                HttpContext.Session.SetString("RolUsuarioLogueado", usuarioLogueado.GetType().Name);

				if (usuarioLogueado is Miembro)
                {
                    Miembro miembroLogueado = (Miembro)usuarioLogueado;
                    HttpContext.Session.SetString("NombreUsuarioLogueado", miembroLogueado.Nombre + " " + miembroLogueado.Apellido);
                }

                string estadoBloqueo = "Desbloqueado";
                if (usuarioLogueado.Bloqueado)
                {
					estadoBloqueo = "Bloqueado";
                }
				HttpContext.Session.SetString("EstadoBloqueo", estadoBloqueo);

				return RedirectToAction("Index","Home");
            }
            else
            {
                ViewBag.msg = "Datos incorrectos";
				return View();
			}
		}

		public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
			return RedirectToAction("InicioSesion");
        }   

        public IActionResult ListarAmigos()
        {
            if (HttpContext.Session.GetInt32("IdUsuarioLogueado") == null)
            {
                return RedirectToAction("InicioSesion", "Usuario");
            }
            return View(Sis.BuscarUsuario((int)HttpContext.Session.GetInt32("IdUsuarioLogueado")).VerAmigos());
		}

        public IActionResult ListarSolicitudes()
        {
			if (HttpContext.Session.GetInt32("IdUsuarioLogueado") == null)
			{
				return RedirectToAction("InicioSesion", "Usuario");
			}
			return View(Sis.GetSolicitudesXUsuario((int)HttpContext.Session.GetInt32("IdUsuarioLogueado")));
        }

        public IActionResult ListarMiembros()
        {
			if (HttpContext.Session.GetInt32("IdUsuarioLogueado") == null)
			{
				return RedirectToAction("InicioSesion", "Usuario");
			}
			if (HttpContext.Session.GetString("RolUsuarioLogueado") == "Miembro")
            {
				return View(Sis.BuscarNuevosAmigos((int)HttpContext.Session.GetInt32("IdUsuarioLogueado")));
            }
            else
            {
                return View(Sis.GetMiembros());
            }  
        }

        public IActionResult AgregarAmigo(int idMiembroSolicitado)
        {
			Invitacion nueva = new Invitacion(Sis.BuscarUsuario(HttpContext.Session.GetInt32("IdUsuarioLogueado")), Sis.BuscarUsuario(idMiembroSolicitado));
            Sis.AltaInvitacion(nueva);
            return RedirectToAction("ListarMiembros", new { id = (int)HttpContext.Session.GetInt32("IdUsuarioLogueado")});

		}

		public IActionResult AceptarSolicitud(int IdInvitacion)
		{
            Sis.AceptarInvitacion(IdInvitacion);
            return RedirectToAction("ListarSolicitudes");
		}

		public IActionResult RechazarSolicitud(int IdInvitacion)
        {
			Sis.RechazarInvitacion(IdInvitacion);
			return RedirectToAction("ListarSolicitudes");
		}

        public IActionResult EliminarAmigo(int idAmigoEliminado)
        {
            Sis.EliminarAmigo((int)HttpContext.Session.GetInt32("IdUsuarioLogueado"), idAmigoEliminado);
			return RedirectToAction("ListarAmigos");
		}

        public IActionResult CambiarEstadoBloqueo(int idMiembro)
        {
            Sis.BloquearODesbloquearUsuario(Sis.BuscarUsuario(idMiembro).Email);
            return RedirectToAction("ListarMiembros");
        }

	}
}
