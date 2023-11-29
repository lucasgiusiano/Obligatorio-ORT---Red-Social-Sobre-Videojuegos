using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Comentario : Publicacion
    {
        public Comentario(string titulo, string contenido, Miembro autor) : base(titulo, contenido, autor)
        {

        }

        public Comentario() : base()
        {

        }

        public override string ToString()
        {
            return base.ToString() + "\n Tipo de Publicación: Comentario";
        }

        public override void Validar()//Valida que el titulo y el contenido del comentario no esten vacíos
        {
            base.Validar();
            if (String.IsNullOrEmpty(Titulo) || Titulo.Length < 3)
            {
                throw new Exception("Error: El titulo debe contener al menos tres caracteres");
            }
            if (String.IsNullOrEmpty(Contenido))
            {
                throw new Exception("Error: El contenido del comentario no puede ser vacío");
            }
        }

        //Recorre la lista de reacciones del comentario contabilizando la cantidad de likes y dislikes para hacer el calculo respectivo al comentario
        public override double ValorDeAceptacion()
        {
            int cantLikes = 0;
            int cantDislikes = 0;


            foreach (Reaccion r in Reacciones)
            {
                if (r.Tipo.ToLower() == "like")
                {
                    cantLikes++;
                }
                if (r.Tipo.ToLower() == "dislike")
                {
                    cantDislikes++;
                }
            }

            return (cantLikes * 5) + (cantDislikes * -2);
        }
    }
}
