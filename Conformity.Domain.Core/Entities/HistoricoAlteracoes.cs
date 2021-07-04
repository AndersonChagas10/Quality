namespace Conformity.Domain.Core.Entities
{
    using Conformity.Domain.Core.Interfaces;
    using System;

    //[Table("HistoricoAlteracoes", Schema = "art")]
    public partial class HistoricoAlteracoes : BaseModel, IEntity
    {
        public HistoricoAlteracoes()
        {
        }

        public DateTime DataModificacao { get; set; }
        public int UsuarioModificacaoId { get; set; }
        public int EntidadeId { get; set; }
        public string TabelaAlterada { get; set; }
        public string Propriedade { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorAlterado { get; set; }

    }
}
