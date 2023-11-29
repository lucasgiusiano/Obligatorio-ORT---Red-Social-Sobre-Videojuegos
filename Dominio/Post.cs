using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dominio
{
    public class Post : Publicacion
    {
        public string Imagen { get; set; }
        public bool Privado { get; set; }
        private List<Comentario> Comentarios = new List<Comentario>();

        public Post(string titulo, string contenido, Miembro autor, string imagen, bool privado) : base(titulo, contenido, autor)
        {
            Imagen = imagen;
            Privado = privado;
        }

        public Post() : base()
        {

        }

        public override string ToString()
        {
            string txtPrivacidad = "Público";
            if (Privado)
            {
                txtPrivacidad = "Privado";
            }
            return base.ToString() + $"\nImagen: {Imagen} || Privacidad: {txtPrivacidad} || Tipo de Publicación: Post";
        }

        public override void Validar() //Valida que el texto de la imagen no este vacia y tenga las terminaciones .png o .jpg
        {
            base.Validar();

            bool imagenTieneFormato = false;

            if (String.IsNullOrEmpty(Imagen))
            {
                throw new Exception("Imagen inválida: Debe ingresar el nombre de una imagen");
            }
            if (Imagen.EndsWith(".jpg"))
            {
                imagenTieneFormato = true;
            }
            if (Imagen.EndsWith(".png") && !imagenTieneFormato)
            {
                imagenTieneFormato = true;
            }
            if (!imagenTieneFormato)
            {
                throw new Exception("Imagen inválida: La imagen debe tener una extensión de formato '.png' o '.jpg'");
            }
        }

        public void Comentar(Comentario c)//Añade un comentario a la lista de comentarios
        {
            c.Validar();
            Comentarios.Add(c);
        }

        public List<Comentario> GetComentarios(string RolMiembro)//Devuelve la lista de comentarios que tiene el post dependiendo el rol del usuario que los solicite
        {
            if (RolMiembro == "Miembro")
            {
                List<Comentario> ret = new List<Comentario>();
                foreach (Comentario c in Comentarios) 
                {
                    if (!c.Censurado)
                    {
						ret.Add(c);
					}
                }
                return ret;
			}
			return Comentarios;
		}

        //Recorre la lista de reacciones de la publicación contabilizando la cantidad de likes y dislikes, realiza el calculo acorde a los post
        //en caso de ser un post publico, suma 10 puntos al resultado
        public override double ValorDeAceptacion()
        {
            int ret;
            int cantLikes = 0;
            int cantDislikes = 0;

            foreach (Reaccion r in Reacciones)
            {
                if (r.Tipo == "like")
                {
                    cantLikes++;
                }
                if (r.Tipo == "dislike")
                {
                    cantDislikes++;
                }
            }

            ret = (cantLikes * 5) + (cantDislikes * -2);

            if (!Privado)
            {
                ret += 10;
            }

            return ret;
        }

        public void CambiarPrivacidad()//Cambia el estado de la privacidad dependiendo del estado actual de la misma
        {
            if (Privado)
            {
                Privado = false;
            }
            else
            {
                Privado = true;
            }
        }
    }
}
