using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public AppUser()
        {
            BitacoraAcceso = new HashSet<BinnacleAccess>();
            BitacoraMovimiento = new HashSet<BinnacleMovement>();
            UsuarioRol = new HashSet<AppUserRole>();
        }

        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Clave { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public DateTime FecCreacion { get; set; }
        public string AdicionadoPor { get; set; }
        public string ModificadoPor { get; set; }

        #region HELPER PROP
        public string GetCedulaSinGuiones
        {
            get
            {
                return Cedula.Trim().Replace('-', Convert.ToChar(string.Empty));
            }
        }
        #endregion

        public Client Cliente { get; set; }
        public ICollection<BinnacleAccess> BitacoraAcceso { get; set; }
        public ICollection<BinnacleMovement> BitacoraMovimiento { get; set; }
        public ICollection<AppUserRole> UsuarioRol { get; set; }
    }
}
