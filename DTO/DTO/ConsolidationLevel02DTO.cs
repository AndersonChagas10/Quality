using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO.DTO
{
    public class ConsolidationLevel02DTO : EntityBase
    {
        public ConsolidationLevel02DTO(List<CollectionLevel02DTO> x)
        {
            ValidaBaseEntity();
            
            //Level01Consolidation_Id DOMAIN
            try
            {

                ConsolidationDate = DateTime.Now;

                Level02_Id = x.FirstOrDefault().Level02_Id;
                Guard.ForValidFk(Level02_Id, "Level02 Id.");

                collectionLevel02DTO = x;

            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public List<CollectionLevel02DTO> collectionLevel02DTO { get; set; }
        public Nullable<System.DateTime> ConsolidationDate { get; set; }
        public int Level01Consolidation_Id { get; set; }
        public int Level02_Id { get; set; }

    }
}