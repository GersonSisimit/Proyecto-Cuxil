using System;
using System.Collections.Generic;

#nullable disable

namespace AgroservicioCuxil.Models
{
    public partial class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public int IdTipoCliente { get; set; }
    }
}
