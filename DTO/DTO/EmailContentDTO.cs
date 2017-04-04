using DTO.BaseEntity;
using System;

namespace DTO.DTO
{
    public class EmailContentDTO : EntityBase
    {
        public int Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime AlterDate { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public string SendStatus { get; set; }
        public DateTime SendDate { get; set; }
        public string Project { get; set; }
        public bool IsBodyHtml { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
    }
}
