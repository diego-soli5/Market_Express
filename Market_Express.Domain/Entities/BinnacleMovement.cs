using System;
#nullable disable

namespace Market_Express.Domain.Entities
{
    public class BinnacleMovement : BaseEntity
    {
        public Guid IdUsuario { get; set; }
        public DateTime FecRealiza { get; set; }
        public string Tipo { get; set; }
        public string Detalle { get; set; }

        public AppUser Usuario { get; set; }
    }
}
