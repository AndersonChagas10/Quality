using DTO.BaseEntity;

namespace DTO.DTO
{
    /// <summary>
    /// A classe DTO contem dados importantes para Regras de Negócio e Banco de dados.
    /// </summary>
    public class ContextExampleDTO : EntityBase
    {
        public ExampleDTO example { get; set; }
    }
}
