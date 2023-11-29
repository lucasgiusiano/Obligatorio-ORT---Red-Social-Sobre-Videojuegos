using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public abstract class Usuario : IValidacion
    {
        private static int UltimoId { get; set; }
        public int Id { get; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public bool Bloqueado { get; set; }

        protected Usuario(string email, string contrasenia)
        {
            Id = UltimoId++;
            Email = email;
            Contrasenia = contrasenia;
            Bloqueado = false;
        }

        public Usuario()
        {
            Id = UltimoId++;
            Bloqueado = false;
        }

        public override string ToString()
        {
            return $"Email: {Email} || Contraseña: {Contrasenia}\n";
        }

        //Valida que el email y la contraseña estén correctos. En caso de que alguna de las dos no cumpla con los requerimiento, se guarda el mensaje para el usuario en una variable
        // y por ultimo si esa variable no esta vacía, se arroja la excepción expecificando todos los campos que estan mal y porque
        public virtual void Validar()
        {
            string mensajeUsuario = String.Empty;

            if (Email.IndexOf("@") != Email.LastIndexOf("@") || Email.IndexOf("@") == -1)
            {
                mensajeUsuario += "Email inválido: El email no contiene arroba";
            }
            if (Email.IndexOf(".") != Email.LastIndexOf(".") || Email.IndexOf(".") == -1)
            {
                mensajeUsuario += "\nEmail inválido: El email no contiene punto";
            }
            if (String.IsNullOrEmpty(Contrasenia) || Contrasenia.Length <= 5)
            {
                mensajeUsuario += "\nContraseña inválida: La contraseña debe tener más de cinco caracteres";
            }
            if (!String.IsNullOrEmpty(mensajeUsuario))
            {
                throw new Exception(mensajeUsuario);
            }
        }

        public void CambiarBloqueo() // Cambia el estado del atributo bloqueado dependiendo del estado actual del mismo
        {
            if (!Bloqueado)
            {
                Bloqueado = true;
            }
            else
            {
                Bloqueado = false;
            };
        }
    }
}
