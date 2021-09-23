#nullable disable

namespace Market_Express.Domain.Entities
{
    public class InventarioCategoria : BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string AdicionadoPor { get; set; }
        public string ModificadoPor { get; set; }
    }
}
