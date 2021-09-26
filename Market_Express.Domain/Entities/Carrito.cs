using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Carrito : BaseEntity
    {
        public Carrito()
        {
            CarritoDetalle = new HashSet<CarritoDetalle>();
        }

        public Guid IdCliente { get; set; }
        public DateTime FecApertura { get; set; }
        public string Estado { get; set; }

        public Cliente Cliente { get; set; }
        public ICollection<CarritoDetalle> CarritoDetalle { get; set; }
    }
}
