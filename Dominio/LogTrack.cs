namespace Dominio
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LogTrack")]
    public partial class LogTrack
    {
        public int Id { get; set; }
        public int UserSgq_Id { get; set; }
        public string Tabela { get; set; }
        public int Json_Id { get; set; }
        public string Json { get; set; }
        public DateTime AddDate { get; set; }

        public int ParReason_Id { get; set; }
        public string Motivo { get; set; }

        [ForeignKey("UserSgq_Id")]
        public UserSgq UserSgq { get; set; }

        [ForeignKey("ParReason_Id")]
        public ParReason ParReason { get; set; }
    }
}
