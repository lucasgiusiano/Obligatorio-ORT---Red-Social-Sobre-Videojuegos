using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Invitacion
    {
        private static int UltimoId { get; set; }
        public int Id { get; }
        public Miembro Solicitante { get; set; }
        public Miembro Solicitado { get; set; }
        public EstadoInvitacion Estado { get; set; }
        public DateTime Fecha { get; set; }

        public Invitacion(Miembro solicitante, Miembro solicitado)
        {
            Id = UltimoId++;
            Solicitante = solicitante;
            Solicitado = solicitado;
            Estado = EstadoInvitacion.Pendiente; //La invitación se crea por defecto con el estado PENDIENTE 
            Fecha = DateTime.Now; //La fecha de la invitacion se fija en el momento de su creación
        }

        public Invitacion()
        {
            Id = UltimoId++;
            Estado = EstadoInvitacion.Pendiente;
            Fecha = DateTime.Now;
        }

        public void AceptarInvitacion()//Cambia el estado de la invitaciona a APROBADA y agrega a cada miembro a la lista de amigos del otro
        {
            Estado = EstadoInvitacion.Aprobada;

            Solicitante.AgregarAmigo(Solicitado);
            Solicitado.AgregarAmigo(Solicitante);
        }

        public void RechazarInvitacion() //Cambia el estado de la invitacion a RECHAZADA
        {
            Estado = EstadoInvitacion.Rechazada;
        }

        public override string ToString()
        {
            return $"Solicitante: {Solicitante.Nombre} {Solicitante.Apellido} || Solicitado: {Solicitado.Nombre} {Solicitado.Apellido} || Estado de la initación: {Estado} || Fecha: {Fecha.ToString("dd/MM/yyyy")}";
        }
    }
}
