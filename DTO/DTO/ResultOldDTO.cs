using DTO.Entities.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO
{
    public class ColetaDTO : DataCollectionBase
    {
        public int Id_Level3 { get; set; }
        public int Id_Level1 { get; set; }
        public int Id_Level2 { get; set; }
        public int numero1 { get; set; }
        public int numero2 { get; set; }
        public int Period { get; set; }
        public int Reaudit { get; set; }
        public int Auditor { get; set; }

        /// <summary>
        /// Ignoreds pelo Entity Framework.
        /// </summary>
        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }


        /// <summary>
        /// Constructor para nova avaliação.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="id_Level3"></param>
        /// <param name="id_Level1"></param>
        /// <param name="id_Level2"></param>
        /// <param name="evaluate"></param>
        /// <param name="notConform"></param>
        public void ValidaColeta()
        {
            Guard.ForNegative(Period, "The Period must be positive.");
            Guard.forValueZero(Period, "The Period must be diferent of zero.");

            Guard.ForNegative(Auditor, "The Auditor Id must be positive.");
            Guard.forValueZero(Auditor, "The Auditor Id must be diferent of zero.");

            Guard.ForValidFk(Id_Level2, "Cannot insert the data because: The level1 Identity Key is Invalid or Null.");//O Id do Level2 está em formato Inválido ou Nulo
            Guard.ForValidFk(Id_Level1, "Cannot insert the data because: The level2 Identity Key is Invalid or Null.");//O Id da Operação está em formato Inválido ou Nulo
            Guard.ForValidFk(Id_Level3, "Cannot insert the data because: The level3 Identity Key is Invalid or Null.");//O Id da Level3 está em formato Inválido ou Nulo
            Guard.ForNegative(Id, "Cannot insert the data because: The level1 Identity Key is Negative. Please verify a positive value for Save data.");
            Guard.ForNegative(Evaluate, "Evaluate");
            Guard.ForNegative(NotConform, "Not Conform");

            //Id = id;
            //Id_Level3 = id_Level3;
            //Id_Level1 = id_Level1;
            //Id_Level2 = id_Level2;
            //Evaluate = evaluate;
            //NotConform = notConform;
        }
    }
}
