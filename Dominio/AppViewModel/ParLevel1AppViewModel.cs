using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.AppViewModel
{
    public class ParLevel1AppViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool HasTakePhoto { get; set; }

        public bool GenerateActionOnNotConformity { get; set; }

        public bool OpenPhotoGallery { get; set; }
    }
}
