using System;

namespace DTO.ResultSet
{
    public class VisaoGeralDaAreaResultSet
    {
        public DateTime date { get; set; }
        public string _date { get { return date.ToString("dd/MM/yyyy"); } }

        public decimal scorecard { get; set; }
        public int regId { get; set; }
        public string regName { get; set; }

        public string level1Name { get; set; }
        public string level2Name { get; set; }
        public string level3Name { get; set; }
        public int level1Id { get; set; }
        public int level2Id { get; set; }
        public int level3Id { get; set; }

        public decimal scorecardJbs { get; set; }
        public decimal scorecardJbsReg { get; set; }
        public string companySigla { get; set; }
        public decimal companyScorecard { get; set; }
        public decimal procentagemNc { get; set; }
        public decimal nc { get; set; }
        public decimal av { get; set; }
    }
}
