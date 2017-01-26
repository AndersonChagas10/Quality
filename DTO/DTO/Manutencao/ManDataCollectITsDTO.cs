using System;


namespace DTO.DTO.Manutencao
{
    public class ManDataCollectITsDTO
    {
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime AlterDate { get; set; }
        public DateTime ReferenceDatetime { get; set; }
        public int UserSGQ_Id { get; set; }
        public int ParCompany_Id { get; set; }
        public int DimManutencaoColetaITs_id { get; set; }
        public decimal AmountData { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }

    }
}
