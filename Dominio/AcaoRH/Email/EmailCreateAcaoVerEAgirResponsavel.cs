﻿using Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.AcaoRH.Email
{
    public class EmailCreateAcaoVerEAgirResponsavel : IEmail
    {
        public EmailCreateAcaoVerEAgirResponsavel(Acao acao)
        {
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

        public void MontarBody(Acao acao)
        {
            this.Body = $@"   
            Corpo do e-mail
            Olá!
            
            Seguindo o parâmetro da regra de Plano de Ação cadastrada no sistema SG-SEMST, informamos que você é o responsável pela Ação Corretiva descrita abaixo, que deverá ser executada até {acao.DataConclusao:""dd/MM/yyyy""}.            
            Formulário de Ação - ID {acao.Id}
            
            Emissor: {acao.Emissor}
            Data de emissão: {acao.DataEmissao:""dd/MM/yyyy""}
            Hora de emissão: {acao.HoraEmissao}
            Unidade: {acao.ParCompany}
            Centro de Custo: {acao.ParDepartment}
            Seção / Atividade: {acao.ParDepartment.ParDepartmentFilho}
            Item / Tarefa: {acao.ParCargo}
            
            Indicador / Origem: {acao.ParLevel1.Name}
            Monitoramento:  {acao.ParLevel2.Name}
            Desvio: {acao.ParLevel3.Name}
             
            Não Conformidade / Ocorrência:  {acao.Acao_Naoconformidade}
             
            Ação: {acao.AcaoText}
            
            Evidência da Não Conformidade: 

	        Evidência da Ação Concluída:

	        Hora da conclusão:

            Referência: { acao.Referencia}
            Data da conclusão: { acao.DataConclusao:""dd/MM/yyyy""}
            Status da Ação: { acao.Status}
            Responsável: { acao.Responsavel}
            Notificar: { string.Join(",", acao.Notificar) }
            
            Atenciosamente, 
            
            Software SG-SESMT
            ";
        }

        public void MontarSybject(Acao acao)
        {
            this.Subject = $@"Responsável por Açao gerada pelo SG-SESMT – Desvio no indicador {acao.ParLevel1.Name}";
        }

        public void MontarTo(Acao acao)
        {
            this.To = acao.NotificarUsers.Select(x => x.Email);
        }
    }
}
