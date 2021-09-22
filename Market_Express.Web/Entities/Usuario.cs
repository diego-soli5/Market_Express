using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class Usuario
    {
        public Usuario()
        {
            BitacoraAccesos = new HashSet<BitacoraAcceso>();
            BitacoraMovimientos = new HashSet<BitacoraMovimiento>();
            UsuarioRols = new HashSet<UsuarioRol>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Clave { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<BitacoraAcceso> BitacoraAccesos { get; set; }
        public virtual ICollection<BitacoraMovimiento> BitacoraMovimientos { get; set; }
        public virtual ICollection<UsuarioRol> UsuarioRols { get; set; }
    }
}
