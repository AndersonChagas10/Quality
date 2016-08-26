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

        public ConsolidationLevel02DTO(List<CollectionLevel02DTO> x)
        {
            ValidaBaseEntity();

            #region Caso for HTP

            if (x.FirstOrDefault().Level01_Id == 1)
            {
            }

            #endregion

            #region Caso for CCA

            if (x.FirstOrDefault().Level01_Id  == 2)
            {
            }

            #endregion

            #region Caso for CFF

            if (x.FirstOrDefault().Level01_Id == 3)
            {
            }

            #endregion

            #region Comum para todos

            ConsolidationDate = DateTime.Now;

            Level02_Id = x.FirstOrDefault().Level02_Id;
            Guard.ForValidFk(Level02_Id, "Level02 Id.");

            //collectionLevel02DTO = x;

            #endregion

        }

        //public List<CollectionLevel02DTO> collectionLevel02DTO { get; set; }
        public Nullable<System.DateTime> ConsolidationDate { get; set; }
        public int Level01Consolidation_Id { get; set; }
        public int Level02_Id { get; set; }

    }
}