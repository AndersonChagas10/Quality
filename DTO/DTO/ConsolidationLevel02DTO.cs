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

        public Nullable<System.DateTime> ConsolidationDate { get; set; }
        public int Level01ConsolidationId { get; set; }
        public int Level02Id { get; set; }

    }
}