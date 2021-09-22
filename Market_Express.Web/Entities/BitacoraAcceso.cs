using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class BitacoraAcceso
    {
        public Guid Id { get; set; }
        public Guid IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaSalida { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
