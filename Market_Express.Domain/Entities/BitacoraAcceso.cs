using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class BitacoraAcceso : BaseEntity
    {
        public Guid IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaSalida { get; set; }

        public Usuario IdUsuarioNavigation { get; set; }
    }
}
