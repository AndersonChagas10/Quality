using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO.DTO
{
    public class ConsolidationLevel02DTO : EntityBase
    {

        public ConsolidationLevel02DTO() { }

        #region  Properties refletidas da Entidade EDMX Do Entity Framework

        public int Level01ConsolidationId { get; set; }
        public int Level02Id { get; set; }
        public int UnitId { get; set; }
        public Nullable<System.DateTime> ConsolidationDate { get; set; }

        public Level02DTO Level02 { get; set; }
        public List<CollectionLevel02DTO> CollectionLevel02 { get; set; }

        #endregion

        #region Properties e construtores utilizadas na coleta de dados

        public ConsolidationLevel02DTO(List<CollectionLevel02DTO> x)
        {

            ValidaBaseEntity();

            #region Caso for HTP

            if (x.FirstOrDefault().Level01Id == 1)
            {
            }

            #endregion

            #region Caso for CCA

            if (x.FirstOrDefault().Level01Id == 2)
            {
            }

            #endregion

            #region Caso for CFF

            if (x.FirstOrDefault().Level01Id == 3)
            {
            }

            #endregion

            #region Comum para todos

            ConsolidationDate = DateTime.Now;

            Level02Id = x.FirstOrDefault().Level02Id;
            Guard.ForValidFk(Level02Id, "Level02Consolidation Id.");

            //collectionLevel02DTO = x;

            #endregion

        }

        public List<CollectionLevel02DTO> collectionLevel02DTO { get; set; }

        #endregion

    }
}