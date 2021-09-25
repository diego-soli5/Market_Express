using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Rol : BaseEntity
    {
        public Rol()
        {
            RolPermisos = new HashSet<RolPermiso>();
        }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string AdicionadoPor { get; set; }
        public string ModificadoPor { get; set; }

        public ICollection<RolPermiso> RolPermisos { get; set; }
    }
}
