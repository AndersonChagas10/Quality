using DTO.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO
{
    public class ColetaDTO : DataCollectionBase
    {
        //public int numero1 { get; set; }
        //public int numero2 { get; set; }
        public int Id_Level1 { get; set; }
        public int Id_Level2 { get; set; }
        public int Id_Level3 { get; set; }
        public bool Reaudit { get; set; }
        public int UserIdInsercao { get; set; }
        public int Period { get; set; }
        public decimal Cattle_Type { get; set; }
        public decimal Chain_Speed { get; set; }
        public decimal Lot { get; set; }
        public decimal Mud_Score { get; set; }
        public decimal TotalDefectsLevel1 { get; set; }
        public decimal TotalDefectsLevel2 { get; set; }
        public decimal TotalDefectsLevel3 { get; set; }

        /// <summary>
        /// Ignoreds pelo Entity Framework.
        /// </summary>
        public string Level1Name { get; set; }
        public string Level2Name { get; set; }
        public string Level3Name { get; set; }

       

        /// <summary>
        /// Constructor para nova avaliação.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id_Level3"></param>
        /// <param name="id_Level1"></param>
        /// <param name="id_Level2"></param>
        /// <param name="evaluate"></param>
        /// <param name="notConform"></param>
        public void ValidaColeta(bool isAlter = false)
        {
            ValidaBaseEntity(isAlter);
            ValidaDataCollectionBase();

            Guard.ForNegative(UserIdInsercao, "The Auditor Id must be positive.");
            Guard.forValueZero(UserIdInsercao, "The Auditor Id must be diferent of zero.");
            Guard.ForValidFk(Id_Level2, "Cannot insert the data because: The level1 Identity Key is Invalid or Null.");//O Id do Level2 está em formato Inválido ou Nulo
            Guard.ForValidFk(Id_Level1, "Cannot insert the data because: The level2 Identity Key is Invalid or Null.");//O Id da Operação está em formato Inválido ou Nulo
            Guard.ForValidFk(Id_Level3, "Cannot insert the data because: The level3 Identity Key is Invalid or Null.");//O Id da Level3 está em formato Inválido ou Nulo
        }
    }
}
