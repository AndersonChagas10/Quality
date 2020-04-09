using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SgqSystem.ViewModels
{
    public class PargroupQualificationXParLevel3ValueViewModel
    {
        public int Id { get; set; }

        public int PargroupQualification_Id { get; set; }

        public int ParLevel3Value_Id { get; set; }

        public string Value { get; set; }

        public bool IsActive { get; set; }

        public bool IsRequired { get; set; }
    }
}