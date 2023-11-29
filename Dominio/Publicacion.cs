using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public abstract class Publicacion : IValidacion, IComparable<Publicacion>
    {
        private static int UltimoId { get; set; }
        public int Id { get; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public bool Censurado { get; set; }
        public DateTime Fecha { get; set; }
        public Miembro Autor { get; set; }
        protected List<Reaccion> Reacciones = new List<Reaccion>();

        public Publicacion(string titulo, string contenido, Miembro autor)
        {
            Id = UltimoId++;
            Titulo = titulo;
            Contenido = contenido;
            Censurado = false;
            Fecha = DateTime.Now;
            Autor = autor;
        }

        public Publicacion()
        {
            Id = UltimoId++;
            Censurado = false;
            Fecha = DateTime.Now;
        }

        public override string ToString()
        {
            string txtCensurado = "No";
            if (Censurado)
            {
                txtCensurado = "Si";
            }
            return $"Autor: {Autor.Nombre} {Autor.Apellido} || Titulo: {Titulo} || Fecha: {Fecha.ToString("dd/MM/yyyy")} || Censurado: {txtCensurado} \nContenido: {Contenido}";
        }

        public int GetCantTipoReaccion(string tipo) //Devuelve la cantidad de reacciones de un tipo que tiene una publicacion
        {
            int ret = 0;

            foreach (var reaccion in Reacciones)
            {
                if (reaccion.Tipo == tipo)
                {
                    ret++;
                }
            }
            return ret;
        }

        public virtual void Validar()//Valida que el titulo y el contenido de la publicación no esten vacios
        {
            if (String.IsNullOrEmpty(Titulo) || Titulo.Length < 5)
            {
                throw new Exception("El titulo debe tener al menos cinco caracteres");
            }
            if (String.IsNullOrEmpty(Contenido))
            {
                throw new Exception("El contenido no puede estar vacío");
            }
        }

        //Verifica si el usuario que está reaccionando a la publicación ya lo hizo anteriormente y en caso de que asi sea pero la reacción actual sea otra, la cambia, de lo contrario arroja una excepción
        //Si el usuario que reacciona es uno nuevo, agrega la reaccion a la lista de reacciones de la publicación
        public void Reaccionar(Reaccion r)
        {
            bool agregar = true;

            foreach (Reaccion reaccionDePubli in Reacciones)
            {
                if (reaccionDePubli.Autor.Id == r.Autor.Id && reaccionDePubli.Tipo == r.Tipo)
                {
					agregar = false;
					Reacciones.Remove(reaccionDePubli);
                    break;
                }
                else if (reaccionDePubli.Autor.Id == r.Autor.Id && reaccionDePubli.Tipo != r.Tipo)
                {
                    agregar = false;
                    reaccionDePubli.Tipo = r.Tipo;
					break;
                }
            }

            if (agregar)
            {
                Reacciones.Add(r);
            }

        }

        public Reaccion UsuarioReacciono(int idUsuario) //Busca si el usuario logueado ya reaccionó a esa publicacion
        {
            foreach (var reaccion in Reacciones)
            {
                if (reaccion.Autor.Id == Id)
                {
                    return reaccion;
                }
            }
            return null;
        }

        public List<Reaccion> GetReacciones()//Devuelve la lista de reacciones que tiene la publicación
        {
            return Reacciones;
        }

        public abstract double ValorDeAceptacion();//Funcion abastracta para acceder a las funciones de calculo del valor de aceptación de posts y comentarios.

        public void CambiarCensura()//Cambia el estado de censura dependiendo el estado actual del mismo
        {
            if (Censurado)
            {
                Censurado = false;
            }
            else
            {
                Censurado = true;
            }
        }

        public int CompareTo(Publicacion? other)
        {
            if (Titulo.CompareTo(other.Titulo) > 0)
            {
                return 1;
            }
            else if (Titulo.CompareTo(other.Titulo) < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
