using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DTO.DTO
{
    public class ConsolidationLevel01DTO : EntityBase
    {

        public ConsolidationLevel01DTO() { }
        
        #region Properties refletidas da Entidade EDMX Do Entity Framework

        public DateTime ConsolidationDate { get; set; }
        public int UnitId { get; set; }
        public int DepartmentId { get; set; }
        public int Level01Id { get; set; }

        public DepartmentDTO Department { get; set; }
        public Level01DTO Level01 { get; set; }
        public UnitDTO Unit { get; set; }
        public List<ConsolidationLevel02DTO> ConsolidationLevel02 { get; set; }

        #endregion

        #region Properties e construtores utilizadas na coleta de dados

        //public ICollection<ConsolidationLevel02DTO> ConsolidationLevel02 { get; set; }
        public List<ConsolidationLevel02DTO> consolidationLevel02DTO { get; set; }
        public List<CollectionLevel02DTO> collectionLevel02DTO { get; set; }


        public ConsolidationLevel01DTO(RootObject rootObject)
        {

            ValidaBaseEntity();

            #region DateConsolidation
            if (rootObject.nextRoot[0].datetime != null)
            {
                var dataCorrigida = rootObject.nextRoot[0].datetime.Split(':');
                ConsolidationDate = DateTime.ParseExact(rootObject.nextRoot[0].datetime.Split(':')[0] + ":" + rootObject.nextRoot[0].datetime.Split(':')[1], "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
                ConsolidationDate = ConsolidationDate.AddSeconds(Guard.ConverteValor<int>(rootObject.nextRoot[0].datetime.Split(':')[2], "CollectionDate")).AddMilliseconds(Guard.ConverteValor<int>(rootObject.nextRoot[0].datetime.Split(':')[3], "CollectionDate"));
            }
            
            //MOCK
            //rootObject.unidadeid = "1";
            DepartmentId = 1;//int.Parse(rootObject.de);
            Guard.ForValidFk(DepartmentId, "Unit Id must be valid, in ConsolidationLevel01DTO.");

            //MOCK
            rootObject.unidadeid = "1";
            UnitId = Guard.ConverteValor<int>(rootObject.unidadeid, "Level01.unidadeid");//int.Parse(rootObject.unidadeid);
            Guard.ForValidFk(UnitId, "Unit Id must be valid, in ConsolidationLevel01DTO.");

            //Guard.ForValidFk(rootObject.department, "Department Id must be valid.");
            //Department_Id = rootObject.department;

            Level01Id = Guard.ConverteValor<int>(rootObject.level01id, "Level01.level01id");
            Guard.ForValidFk(Level01Id, "Level01 Id must be valid, in ConsolidationLevel01DTO.");

            #endregion

            collectionLevel02DTO = new List<CollectionLevel02DTO>();
            consolidationLevel02DTO = new List<ConsolidationLevel02DTO>();

            #region Coletas necessitam apenas serem salvas.

            if (rootObject.nextRoot == null)
                throw new ExceptionHelper("Lista de collectionLevel02DTO vazia.");

            if (rootObject.nextRoot.Count == 0)
                throw new ExceptionHelper("Lista de collectionLevel02DTO vazia.");

            foreach (var i in rootObject.nextRoot)
                collectionLevel02DTO.Add(new CollectionLevel02DTO(i, UnitId, rootObject.biasedunbiased));

            #endregion

            #region Consolidações tem que ser calculadas baseadas nas coletas.

            var ids = collectionLevel02DTO.Select(r => r.Level02Id).Distinct().ToList();
            consolidationLevel02DTO.Add(new ConsolidationLevel02DTO(collectionLevel02DTO));

            #endregion

        } 

        #endregion
    }
}