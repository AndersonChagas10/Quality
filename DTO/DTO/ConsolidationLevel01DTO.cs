using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    public class ConsolidationLevel01DTO : EntityBase
    {
        public Nullable<System.DateTime> ConsolidationDate { get; set; }
        public int Unit_Id { get; set; }
        public int Department_Id { get; set; }
        public int Level01_Id { get; set; }

        public List<ConsolidationLevel02DTO> consolidationLevel02DTO { get; set; }
        public List<CollectionLevel02DTO> collectionLevel02DTO { get; set; }

        public ConsolidationLevel01DTO() { }

        public ConsolidationLevel01DTO(RootObject rootObject)
        {

            ValidaBaseEntity();

            #region DateConsolidation

            ConsolidationDate = DateTime.Now;

            Unit_Id = int.Parse(rootObject.unidadeid);
            Guard.ForValidFk(Unit_Id, "Unit Id must be valid, in ConsolidationLevel01DTO.");
            
            //Guard.ForValidFk(rootObject.department, "Department Id must be valid.");
            //Department_Id = rootObject.department;

            Level01_Id = int.Parse(rootObject.level01id);
            Guard.ForValidFk(Level01_Id, "Level01 Id must be valid, in ConsolidationLevel01DTO.");

            #endregion

            
            collectionLevel02DTO = new List<CollectionLevel02DTO>();
            consolidationLevel02DTO = new List<ConsolidationLevel02DTO>();

            #region Coletas necessitam apenas serem salvas.


            foreach (var i in rootObject.nextRoot)
                collectionLevel02DTO.Add(new CollectionLevel02DTO(i));

            #endregion

            #region Consolidações tem que ser calculadas baseadas nas coletas.

            var ids = collectionLevel02DTO.Select(r => r.Level02_Id).Distinct().ToList();
            consolidationLevel02DTO.Add(new ConsolidationLevel02DTO(collectionLevel02DTO));


            #endregion

        }
    }
}