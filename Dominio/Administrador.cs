using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Administrador : Usuario
    {
        public Administrador(string email, string contrasenia) : base(email, contrasenia)
        {

        }

        public Administrador() : base()
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Validar()
        {
            base.Validar();
        }
    }
}
