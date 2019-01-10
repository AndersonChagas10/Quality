namespace Dominio
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VolumeCepRecortes : BaseModel
    {
        public int Id { get; set; }

        public int? Indicador { get; set; }

        public int? Unidade { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Data { get; set; }

        [StringLength(255)]
        public string Departamento { get; set; }

        public int? HorasTrabalhadasPorDia { get; set; }

        public int? QtdadeMediaKgRecProdDia { get; set; }

        public int? QtdadeMediaKgRecProdHora { get; set; }

        [NotMapped()]
        public int? QtdadeFamiliaProduto { get; set; }

        public int? NBR { get; set; }

        public int? TotalKgAvaliaHoraProd { get; set; }

        public int? QtadeTrabEsteiraRecortes { get; set; }

        public int? TotalAvaliaColaborEsteirHoraProd { get; set; }

        public int? TamanhoAmostra { get; set; }

        public int? TotalAmostraAvaliaColabEsteiraHoraProd { get; set; }

        public int? Avaliacoes { get; set; }

        public int? Amostras { get; set; }

        public int? ParCompany_id { get; set; }

        public int? ParLevel1_id { get; set; }

        public int? Shift_Id { get; set; }

        public string Agendamento { get; set; }

        [NotMapped]
        public string Frequencia { get; set; }

        [ForeignKey("Shift_Id")]
        public virtual Shift Shift { get; set; }

        public virtual ParCompany ParCompany { get; set; }

        public virtual ParLevel1 ParLevel1 { get; set; }
    }
}
