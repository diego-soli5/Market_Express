using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class Carrito : BaseEntity
    {
        public Carrito()
        {
            CarritoDetalles = new HashSet<CarritoDetalle>();
        }

        public Guid IdCliente { get; set; }
        public DateTime FechaApertura { get; set; }
        public string Estado { get; set; }

        public virtual Cliente IdClienteNavigation { get; set; }
        public virtual ICollection<CarritoDetalle> CarritoDetalles { get; set; }
    }
}
