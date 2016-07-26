using DTO.BaseEntity;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DTO.DTO
{
    [DataContract]
    [Serializable]
    public class Level02DTO : EntityBase
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Alias { get; set; }
        [DataMember]
        public string ClientSpecification { get; set; }
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public int PeopleCreateId { get; set; }
        [DataMember]
        public int Ordering { get; set; }

        [DataMember]
        public List<Level02ConsolidationDTO> Level02Consolidation { get; set; }

        public void ValidaLeve02DTO()
        {
            ValidaBaseEntity();

            #region Name
            string NameValue;
            Guard.NullOrEmptyValuesCheck(retorno: out NameValue, propName: "Name", value: Name, requerido: true);
            Name = NameValue;
            #endregion

            #region Alias

            #endregion

            #region ClientSpecification

            #endregion

            #region Active

            #endregion

            #region PeopleCreateId:  Não pode ser Zero, não pode ser negativo.
            Guard.ForNegative(PeopleCreateId, "PeopleCreateId");
            Guard.forValueZero(PeopleCreateId, "PeopleCreateId");
            #endregion

            #region Ordering:

            #endregion

        }

    }
}