namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Email_ConfiguracaoEmailSgq
    {
        public int Id { get; set; }

        public int Port { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public bool EnableSsl { get; set; }

        [Required]
        public string login { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public string pass { get; set; }

        public bool IsActive { get; set; }
    }
}
