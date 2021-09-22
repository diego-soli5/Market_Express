using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public Usuario()
        {
            BitacoraAccesos = new HashSet<BitacoraAcceso>();
            BitacoraMovimientos = new HashSet<BitacoraMovimiento>();
            UsuarioRols = new HashSet<UsuarioRol>();
        }

        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Clave { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }

        public Cliente Cliente { get; set; }
        public ICollection<BitacoraAcceso> BitacoraAccesos { get; set; }
        public ICollection<BitacoraMovimiento> BitacoraMovimientos { get; set; }
        public ICollection<UsuarioRol> UsuarioRols { get; set; }
    }
}
