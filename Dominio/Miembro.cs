using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Miembro : Usuario, IComparable<Miembro>
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNac { get; set; }
        private List<Miembro> Amigos = new List<Miembro>();

        public Miembro(string email, string contrasenia, string nombre, string apellido, DateTime fechaNac) : base(email, contrasenia)
        {
            Nombre = nombre;
            Apellido = apellido;
            FechaNac = fechaNac;
        }

        public Miembro() : base()
        {

        }

        public List<Miembro> VerAmigos()//Devuelve la lista de amigos del ususario
        {
            return Amigos;
        }

        public bool BuscarAmigo(int idAmigoBuscado)
        {
            foreach (var amigo in Amigos)
            {
                if (amigo.Id == idAmigoBuscado)
                {
                    return true;
                }
            }
            return false;
        }

        public void AgregarAmigo(Miembro nuevoAmigo)//Agrega un nuevo miembro a la lista de amigos
        {
            Amigos.Add(nuevoAmigo);
        }

        public override string ToString()
        {
            string estaBloqueado = "No";
            if (Bloqueado)
            {
                estaBloqueado = "Si";
            }

            return base.ToString() + $"Nombre y Apellido: {Nombre} {Apellido} || Fecha de nacimiento: {FechaNac} || Está bloqueado: {estaBloqueado}";
        }

        //Valida los atributos del nuevo miembro y en caso de que no cumpla con alguno, guarda el mensaje de error en una variable y al final se la variable de mensajes no se encuentra vacía
        //Arroja la excepción con todos los mensajes de los campos que hayan estado incorrectos
        public override void Validar()
        {
            base.Validar();
            string mensajeMiembro = string.Empty;

            if (String.IsNullOrEmpty(Nombre) || Nombre.Length < 3)
            {
                mensajeMiembro += "\nNombre inválido: El nombre debe tener mas de tres caracteres";
            }
            if (String.IsNullOrEmpty(Apellido) || Apellido.Length < 3)
            {
                mensajeMiembro += "\nApellido inválido: El apellido debe tener mas de tres caracteres";
            }
            if (FechaNac == DateTime.MinValue)
            {
                mensajeMiembro += "\nFecha inválida: No ha ingresado una fecha";
            }
            if (!String.IsNullOrEmpty(mensajeMiembro))
            {
                throw new Exception(mensajeMiembro);
            }
        }

		public int CompareTo(Miembro? other)
		{
			if (Nombre.CompareTo(other.Nombre) > 0)
			{
				return 1;
			}
			else if (Nombre.CompareTo(other.Nombre) < 0)
			{
				return -1;
			}
			else
			{
				if (Apellido.CompareTo(other.Apellido) > 0)
				{
					return 1;
				}
				else if (Apellido.CompareTo(other.Apellido) < 0)
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
}
