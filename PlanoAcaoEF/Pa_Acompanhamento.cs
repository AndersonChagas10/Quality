//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlanoAcaoEF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pa_Acompanhamento
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public Nullable<System.DateTime> AlterDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Acao_Id { get; set; }
        public Nullable<int> Order { get; set; }
        public int Status_Id { get; set; }
        public Nullable<int> MailTo { get; set; }
        public int Author_Id { get; set; }
    }
}
