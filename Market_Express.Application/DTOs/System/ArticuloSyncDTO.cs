using System;

namespace Market_Express.Application.DTOs.System
{
    public class ArticuloSyncDTO
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public string CodigoBarras { get; set; }
        public decimal Precio { get; set; }
    }
}
