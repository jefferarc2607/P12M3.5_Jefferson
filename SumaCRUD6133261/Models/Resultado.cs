using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SumaCRUD6133261.Models
{
    [Table("Resultados")]
    public class Resultado
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Numero1 { get; set; }
        public string Numero2 { get; set; }
        public string Suma { get; set; }
    }
}
