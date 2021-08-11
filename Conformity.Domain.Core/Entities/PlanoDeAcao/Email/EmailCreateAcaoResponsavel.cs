using Conformity.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Conformity.Domain.Core.Enums.PlanoDeAcao.Enums;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    public class EmailCreateAcaoResponsavel : IEmail
    {
        private string _Body;
        private string _Subject;
        private IEnumerable<string> _To;

        public EmailCreateAcaoResponsavel(Acao acao)
        {
            MontarSybject(acao);
            MontarBody(acao);
            MontarTo(acao);
        }

        public string Body { get => _Body; set => _Body = value; }

        public string Subject { get => _Subject; set => _Subject = value; }

        public IEnumerable<string> To { get => _To; set => _To = value; }

        public void MontarBody(Acao acao)
        {
            this.Body = $@"        

            Olá, {acao.ResponsavelUser.FullName}!
            <br><br>
            Seguindo o parâmetro da regra de Plano de Ação cadastrada no sistema SG-SESMT, informamos que você é responsável pela Ação descrita abaixo, que deverá ser executada até { (acao.DataConclusao?.ToString("dd/MM/yyyy") ?? "") }.
            <br><br>
            Formulário de Ação - ID {acao.Id}
            <br><br>
            Emissor: {acao.EmissorUser.FullName}<br>
            Data de emissão: {acao.DataEmissao?.ToString("dd/MM/yyyy")}<br>
            Hora de emissão: {acao.HoraEmissao}<br>
            Unidade: {acao.ParCompany.Description}<br>
            Centro de Custo: {(acao.ParDepartmentParent?.Name ?? "") }<br>
            Seção / Atividade: {(acao.ParDepartment?.Name ?? "")}<br>
            Item / Tarefa: {(acao.ParCargo?.Name ?? "") }<br><br>
            
            Indicador / Origem: {acao.ParLevel1.Name}<br>
            Monitoramento:  {(acao.ParLevel2?.Name ?? "")}<br>
            Desvio: {(acao.ParLevel3?.Name ?? "")}<br>
             
            Não Conformidade / Ocorrência:  {acao.Acao_Naoconformidade}<br><br>
             
            Ação: {acao.AcaoText}<br><br>
            
            Evidência da Não Conformidade: {RetornarString(acao.EvidenciaNaoConformidade)}<br>
            Evidência da Ação Concluída: {RetornarString(acao.EvidenciaAcaoConcluida)}<br>

            Prioridade: {(acao.Prioridade != null ? Enum.GetName(typeof(AcaoPrioridade), acao.Prioridade) : "")}<br>
            Referência: {acao.Referencia}<br>
            Data da conclusão: { acao.DataConclusao?.ToString("dd/MM/yyyy") ?? "" }<br>
            Status da Ação: {Enum.GetName(typeof(EAcaoStatus), acao.Status).Replace('_', ' ')}<br>
            Responsável: {acao.ResponsavelUser.FullName}<br>
            Notificar: {string.Join(",", acao.NotificarUsers.Select(x => x.FullName)) }<br><br>
            
            Atenciosamente, <br>
            
            <b>Software SG-SESMT</b>

            ";
        }

        public void MontarSybject(Acao acao)
        {
            this.Subject = $@"Responsável por ação gerada pelo SG-SESMT – Desvio no indicador {acao.ParLevel1.Name }";
        }

        public void MontarTo(Acao acao)
        {
            this.To = new string[] { (acao.ResponsavelUser.Email ) };

        }

        public string RetornarString(IEnumerable<string> lista)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach(var item in lista)
            {
                stringBuilder.Append("<div class='col-sm-4' style='display: inline-block; width: 49%; height: 20%;'>");
                stringBuilder.Append("<img src='data:image/png;base64,");
                stringBuilder.Append(item);
                stringBuilder.Append("'data-img class='img-responsive' style='width:100%; height:100%'/>");
                stringBuilder.Append("</div>");
            }
            return stringBuilder.ToString();
        }
    }
}
