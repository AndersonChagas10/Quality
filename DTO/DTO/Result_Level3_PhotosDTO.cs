using DTO.BaseEntity;
using System;

namespace DTO.DTO
{
    public class Result_Level3_PhotosDTO
    {
        public int ID { get; set; }
        public Nullable<int> Result_Level3_Id { get; set; }
        public String Photo_Thumbnaills { get; set; }
        public String Photo { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<int> Level1Id { get; set; }
        public Nullable<int> Level2Id { get; set; }
        public Nullable<int> Level3Id { get; set; }
        public Nullable<int> Evaluation { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<int> Period { get; set; }
        public Nullable<int> Shift { get; set; }
        public Nullable<int> Sample { get; set; }
        public string ResultDate { get; set; }

    }
}
