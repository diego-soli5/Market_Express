using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class BitacoraMovimiento : BaseEntity
    {
        public Guid IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public string Detalle { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
