#nullable disable

using System;

namespace Market_Express.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public DateTime FecCreacion{ get; set; }
        public string AdicionadoPor { get; set; }
        public string ModificadoPor { get; set; }
    }
}
