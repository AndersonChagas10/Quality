using DTO.BaseEntity;
using DTO.Helpers;

namespace DTO.DTO
{
    public class ExampleDTO : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SelectedElement { get; set; }

        public void IsValid()
        {
            if (Guard.VerifyStringNullValue(Name))
            {
                var nameValidado = "";
                Guard.CheckStringFull(out nameValidado, "Name", Name, "Property Name is invalid", true);
                Name = nameValidado;
            }

            if (Guard.VerifyStringNullValue(Name))
            {
                var desc = "";
                Guard.CheckStringFull(out desc, "Name", Description, "Property Name is invalid", true);
                Name = desc;
            }

            Guard.ForNegative(Guard.ConverteValor<int>(SelectedElement, "Selected Element"), "DropDownTest");

        }
    }
}