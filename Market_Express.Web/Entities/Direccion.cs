using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class Direccion
    {
        public Guid Id { get; set; }
        public Guid IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
    }
}
