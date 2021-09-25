﻿using System;
#nullable disable

namespace Market_Express.Domain.Entities
{
    public class BitacoraMovimiento : BaseEntity
    {
        public Guid IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public string Detalle { get; set; }

        public Usuario IdUsuarioNavigation { get; set; }
    }
}