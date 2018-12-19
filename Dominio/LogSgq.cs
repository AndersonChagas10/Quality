namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogSgq")]
    public partial class LogSgq
    {
        public int Id { get; set; }

        public DateTime addDate { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Level { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Call_Site { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Exception_Type { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Exception_Message { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Stack_Trace { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Additional_Info { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Second_Log_Path { get; set; }
    }
}
