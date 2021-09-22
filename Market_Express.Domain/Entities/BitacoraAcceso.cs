using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class BitacoraAcceso : BaseEntity
    {
        public Guid IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaSalida { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
