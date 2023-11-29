using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Dominio
{
    public class Reaccion : IValidacion
    {
        public string Tipo { get; set; }
        public Miembro Autor { get; set; }

        public Reaccion(string tipo, Miembro autor)
        {
            Tipo = tipo.ToLower(); //El tipo se transforma a minusculas para facilitar el trabajo de validacion del tipo
            Autor = autor;
        }

        public Reaccion()
        {

        }

        public void Validar() //Valida el tipo de la reaccion para que solo sea un tipo acorde a los utilizados y permitidos para la aplicación
        {
            if (Tipo != "like" || Tipo != "dislike")
            {
                throw new Exception("Error: La reacción solo puede ser del tipo 'Like' o 'Dislike'");
            }
        }

        public override string ToString()
        {
            return $"Tipo: {Tipo} || Autor: {Autor.Email}";
        }
    }
}
