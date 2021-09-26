using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class BinnacleAccess : BaseEntity
    {
        public Guid IdUsuario { get; set; }
        public DateTime FecInicio { get; set; }
        public DateTime? FecSalida { get; set; }

        public AppUser Usuario { get; set; }
    }
}
