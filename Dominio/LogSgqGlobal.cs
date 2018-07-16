namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogSgqGlobal")]
    public partial class LogSgqGlobal
    {
        public int Id { get; set; }

        public DateTime? addDate { get; set; }

        public string Level { get; set; }

        public string Call_Site { get; set; }

        public string Exception_Type { get; set; }

        public string Exception_Message { get; set; }

        public string Stack_Trace { get; set; }

        public string Additional_Info { get; set; }

        public string Object { get; set; }

        public string email { get; set; }

        public string Second_Log_Path { get; set; }
    }
}
