using System;
using System.Runtime.Serialization;

namespace PA.DTO
{
    [DataContract]
    [Serializable]
    public class BaseDTO
    {
        [DataMember]
        public Int32 Id { get; set; }
    }
}
