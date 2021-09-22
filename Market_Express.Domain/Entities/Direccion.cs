using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class Direccion : BaseEntity
    {
        public Guid IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
    }
}
