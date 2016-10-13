using DTO.BaseEntity;

namespace DTO.DTO
{
    /// <summary>
    /// A classe DTO contem dados importantes para Regras de Negócio e Banco de dados.
    /// </summary>
    public class ParamsDTO : EntityBase
    {
        public string Name { get; set; }
        public ExampleDTO exemplo { get; set; }
        public int ShiftSelectListExampleValueSelected { get; set; }
    }
}
