using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class Carrito
    {
        public Carrito()
        {
            CarritoDetalles = new HashSet<CarritoDetalle>();
        }

        public Guid Id { get; set; }
        public Guid IdCliente { get; set; }
        public DateTime FechaApertura { get; set; }
        public string Estado { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual ICollection<CarritoDetalle> CarritoDetalles { get; set; }
    }
}
