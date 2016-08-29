using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO
{
    public class CollectionLevel03DTO : EntityBase
    {
        public CollectionLevel03DTO() { }

        public CollectionLevel03DTO(NextnextRoot nextnextRoot, int level02Id, int level01Id)
        {
            try
            {
                ValidaBaseEntity();

                #region Caso for HTP

                if (level01Id == 1)
                {
                }

                #endregion

                #region Caso for CCA

                if (level01Id == 2)
                {
                }

                #endregion

                #region Caso for CFF

                if (level01Id == 3)
                {
                }

                #endregion

                #region Valores Comum para todos

                Level03Id = int.Parse(nextnextRoot.level03id);
                Guard.ForValidFk(Level03Id.Value, "Level03 Id must be valid.");

                //TotalError = decimal.Parse(nextnextRoot.totalerror);
                //Guard.ForNegative(TotalError, "Total Error level03");

                ConformedIs = Convert.ToBoolean(nextnextRoot.conform);
                Guard.VerifyIfIsBool(ConformedIs, "ConformedIs");

                if (nextnextRoot.value != null)
                    Value = decimal.Parse(nextnextRoot.value);
                Guard.ForNegative(Value, "Value level03");

                ValueText = "";

                #endregion
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar CollectionLevel03DTO", e);
            }

        }

        public int CollectionLevel02Id { get; set; }
        public Nullable<int> Level03Id { get; set; }
        public bool ConformedIs { get; set; }
        public decimal Value { get; set; }
        public string ValueText { get; set; }


    }
}
