using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class BitacoraMovimiento
    {
        public Guid Id { get; set; }
        public Guid IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public string Detalle { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
