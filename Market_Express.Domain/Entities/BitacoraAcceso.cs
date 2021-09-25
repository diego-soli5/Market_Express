using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class BitacoraAcceso : BaseEntity
    {
        public Guid IdUsuario { get; set; }
        public DateTime FecInicio { get; set; }
        public DateTime? FecSalida { get; set; }

        public Usuario Usuario { get; set; }
    }
}
