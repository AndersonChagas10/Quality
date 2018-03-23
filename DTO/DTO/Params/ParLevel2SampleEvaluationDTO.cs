namespace DTO.DTO.Params
{
    public class ParLevel2SampleEvaluationDTO
    {
        public int sampleNumber { get; set; }
        public int? sampleId { get; set; }

        public int evaluationNumber { get; set; }
        public int? evaluationId { get; set; }

        public int? ParLevel1_Id { get; set; }
        public int? ParCluster_Id { get; set; }

        public int level2Id { get; set; }
        public int? companyId { get; set; }
        public int? Id { get; set; }
        public bool IsActive { get; set; }
    }
}
