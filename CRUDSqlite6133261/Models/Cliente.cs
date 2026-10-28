using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDSqlite6133261.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string NombreCliente { get; set; }

        [MaxLength(250)]
        public string Correo { get; set; }

        public string Telefono { get; set; }
    }
}
