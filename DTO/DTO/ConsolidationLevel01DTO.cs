using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO.DTO
{
    public class ConsolidationLevel01DTO : EntityBase
    {
        public Nullable<System.DateTime> ConsolidationDate { get; set; }
        public int UnitId { get; set; }
        public int DepartmentId { get; set; }
        public int Level01Id { get; set; }

        public DepartmentDTO Department { get; set; }
        public Level01DTO Level01 { get; set; }
        public UnitDTO Unit { get; set; }

        //public ICollection<ConsolidationLevel02DTO> ConsolidationLevel02 { get; set; }
        public List<ConsolidationLevel02DTO> consolidationLevel02DTO { get; set; }
        public List<CollectionLevel02DTO> collectionLevel02DTO { get; set; }

        public ConsolidationLevel01DTO() { }

        public ConsolidationLevel01DTO(RootObject rootObject)
        {

            try
            {
                ValidaBaseEntity();

                #region DateConsolidation

                ConsolidationDate = DateTime.Now;
                //MOCK
                //rootObject.unidadeid = "1";
                DepartmentId = 1;//int.Parse(rootObject.de);
                Guard.ForValidFk(DepartmentId, "Unit Id must be valid, in ConsolidationLevel01DTO.");


                //MOCK
                rootObject.unidadeid = "1";
                UnitId = int.Parse(rootObject.unidadeid);
                Guard.ForValidFk(UnitId, "Unit Id must be valid, in ConsolidationLevel01DTO.");

                //Guard.ForValidFk(rootObject.department, "Department Id must be valid.");
                //Department_Id = rootObject.department;

                Level01Id = int.Parse(rootObject.level01id);
                Guard.ForValidFk(Level01Id, "Level01 Id must be valid, in ConsolidationLevel01DTO.");

                #endregion


                collectionLevel02DTO = new List<CollectionLevel02DTO>();
                consolidationLevel02DTO = new List<ConsolidationLevel02DTO>();

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar ConsolidationLevel01DTO", e);
            }
            #region Coletas necessitam apenas serem salvas.

            if (rootObject.nextRoot == null)
                throw new Exception("Lista de collectionLevel02DTO vazia.");

            if (rootObject.nextRoot.Count == 0)
                throw new Exception("Lista de collectionLevel02DTO vazia.");

            foreach (var i in rootObject.nextRoot)
                collectionLevel02DTO.Add(new CollectionLevel02DTO(i));

            #endregion

            #region Consolidações tem que ser calculadas baseadas nas coletas.

            var ids = collectionLevel02DTO.Select(r => r.Level02Id).Distinct().ToList();
            consolidationLevel02DTO.Add(new ConsolidationLevel02DTO(collectionLevel02DTO));

            #endregion

        }
    }
}