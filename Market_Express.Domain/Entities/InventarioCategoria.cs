using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class InventarioCategoria : BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}
