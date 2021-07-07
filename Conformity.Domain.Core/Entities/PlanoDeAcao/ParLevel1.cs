using Conformity.Domain.Core.Entities;
using Conformity.Domain.Core.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{


    public partial class ParLevel1 : BaseModel, IEntity
    {

        public int ParConsolidationType_Id { get; set; }

        public int? ParFrequency_Id { get; set; }

        [NotMapped]
        public string ParFrequencyDescription { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(155)]
        [DisplayName("Indicador")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }

        public bool HasSaveLevel2 { get; set; }

        public bool HasNoApplicableLevel2 { get; set; }

        public bool HasGroupLevel2 { get; set; }

        public bool HasAlert { get; set; }

        public bool? IsSpecific { get; set; }

        public bool IsSpecificHeaderField { get; set; }

        public bool IsSpecificNumberEvaluetion { get; set; }

        public bool IsSpecificNumberSample { get; set; }

        public bool IsSpecificLevel3 { get; set; }

        public bool? IsSpecificGoal { get; set; }

        public bool IsRuleConformity { get; set; }

        public bool IsFixedEvaluetionNumber { get; set; }

        public bool IsLimitedEvaluetionNumber { get; set; }

        public bool IsActive { get; set; }

        public int? Level2Number { get; set; }

        public int? hashKey { get; set; }

        public bool? haveRealTimeConsolidation { get; set; }

        public int? RealTimeConsolitationUpdate { get; set; }

        public bool? IsPartialSave { get; set; }

        public bool? HasCompleteEvaluation { get; set; }

        public int? ParScoreType_Id { get; set; }

        public bool? IsChildren { get; set; }

        public int? ParLevel1Origin_Id { get; set; }

        public bool? PointsDestiny { get; set; }

        public int? ParLevel1Destiny_Id { get; set; }

        public bool EditLevel2 { get; set; }

        public bool HasTakePhoto { get; set; }

        public bool? AllowAddLevel3 { get; set; }

        public bool? AllowEditPatternLevel3Task { get; set; }

        public bool? AllowEditWeightOnLevel3 { get; set; }

        public bool? IsRecravacao { get; set; }

        public int? ParGroupLevel1_Id { get; set; }

        public bool? ShowInTablet { get; set; }

        public bool? ShowScorecard { get; set; }

        public bool GenerateActionOnNotConformity { get; set; }

        public bool OpenPhotoGallery { get; set; }

        public bool HasCollectReport { get; set; }
    }
}
