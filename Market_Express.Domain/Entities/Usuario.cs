using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public Usuario()
        {
            BitacoraAcceso = new HashSet<BitacoraAcceso>();
            BitacoraMovimiento = new HashSet<BitacoraMovimiento>();
            UsuarioRol = new HashSet<UsuarioRol>();
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

        public Cliente Cliente { get; set; }
        public ICollection<BitacoraAcceso> BitacoraAcceso { get; set; }
        public ICollection<BitacoraMovimiento> BitacoraMovimiento { get; set; }
        public ICollection<UsuarioRol> UsuarioRol { get; set; }
    }
}
