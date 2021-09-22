using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Carrito : BaseEntity
    {
        public Carrito()
        {
            CarritoDetalles = new HashSet<CarritoDetalle>();
        }

        public Guid IdCliente { get; set; }
        public DateTime FechaApertura { get; set; }
        public string Estado { get; set; }

        public Cliente IdClienteNavigation { get; set; }
        public ICollection<CarritoDetalle> CarritoDetalles { get; set; }
    }
}
