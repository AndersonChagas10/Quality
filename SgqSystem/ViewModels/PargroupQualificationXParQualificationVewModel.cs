using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.ViewModels
{
    public class PargroupQualificationXParQualificationVewModel
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public int? PargroupQualification_Id { get; set; }

        public int? ParQualification_Id { get; set; }
    }
}