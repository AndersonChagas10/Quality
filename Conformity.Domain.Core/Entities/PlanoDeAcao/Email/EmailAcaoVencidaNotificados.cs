using Conformity.Domain.Core.Entities.PlanoDeAcao.Email;
using Conformity.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using static Conformity.Domain.Core.Enums.PlanoDeAcao.Enums;

namespace Conformity.Domain.Core.Entities.PlanoDeAcao
{
    public class EmailAcaoVencidaNotificados : IEmail
    {
        public EmailAcaoVencidaNotificados(Acao acao)
        {
            htmlDaEvidencia = new HtmlDaEvidencia();
            MontarSybject(acao);
            MontarBody(acao);
            MontarTo(acao);
        }

        private string _Body;
        private string _Subject;
        private IEnumerable<string> _To;

        public string Body { get => _Body; set => _Body = value; }
        public string Subject { get => _Subject; set => _Subject = value; }
        public IEnumerable<string> To { get => _To; set => _To = value; }
        public HtmlDaEvidencia htmlDaEvidencia { get; set; }

        public void MontarBody(Acao acao)
        {
            this.Body = $@"   
            Olá!
            <br><br>
	        Seguindo o parâmetro da regra de Plano de Ação cadastrada no sistema SG-SEMST, informamos que a Ação descrita a seguir, em que você foi notificado, venceu em { acao.DataConclusao?.ToString("dd/MM/yyyy") ?? "" }.
            Favor entrar em contato com o responsável { acao.ResponsavelUser.Name } para atualizar o status da Ação no sistema.           
            <br><br>
            Formulário de Ação - ID {acao.Id}
            <br><br>
            Emissor: {acao.EmissorUser.FullName}<br>
            Data de emissão: {acao.DataEmissao?.ToString("dd/MM/yyyy")}<br>
            Hora de emissão: {acao.HoraEmissao}<br>
            Unidade: {acao.ParCompany.Description}<br>
            Centro de Custo: {(acao.ParDepartmentParent?.Name ?? "")}<br>
            Seção / Atividade: {(acao.ParDepartment?.Name ?? "")}<br>
            Item / Tarefa: {(acao.ParCargo?.Name ?? "")}<br><br>
            
            Indicador / Origem: {acao.ParLevel1.Name }<br>
            Monitoramento:  {(acao.ParLevel2?.Name ?? "") }<br>
            Desvio: {(acao.ParLevel3?.Name ?? "")}<br>
             
            Não Conformidade / Ocorrência:  {acao.Acao_Naoconformidade}<br><br>
             
            Ação: {acao.AcaoText}<br><br>
            
            Evidência da Não Conformidade: <br>
            {htmlDaEvidencia.MontarHtmlDaEvidencia(acao.EvidenciaNaoConformidade)}<br><br>

            Evidência da Ação Concluída: <br>
            {htmlDaEvidencia.MontarHtmlDaEvidencia(acao.EvidenciaAcaoConcluida)}<br><br>
            
            Referência: {acao.Referencia}<br>
            Data da conclusão: { (acao.DataConclusao?.ToString("dd/MM/yyyy") ?? "")}<br>
            Hora da conclusão: {acao.HoraConclusao}
            Status da Ação: {Enum.GetName(typeof(EAcaoStatus), acao.Status).Replace('_', ' ')}<br>
            Responsável: { acao.ResponsavelUser.FullName ?? ""}<br>
            Notificar: {string.Join(",", acao.NotificarUsers.Select(x => x.FullName)) }<br><br>
            
            Atenciosamente, <br>
            
            <b>Software SG-SESMT</b>
            ";
        }

        public void MontarSybject(Acao acao)
        {
            this.Subject = $@"Notificação de ação vencida no SG-SESMT – Desvio no indicador {acao.ParLevel1.Name}";
        }

        public void MontarTo(Acao acao)
        {
            this.To = acao.NotificarUsers != null ? acao.NotificarUsers.Select(x => x.Email) : new string[] { };
        }
    }
}
