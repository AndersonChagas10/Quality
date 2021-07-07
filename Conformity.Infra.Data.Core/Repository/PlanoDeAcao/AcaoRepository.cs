using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Conformity.Infra.Data.Core.Repository.PlanoDeAcao
{
    public class AcaoRepository
    {
        private readonly EntityContext _dbContext;

        public AcaoRepository(EntityContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<AcaoViewModel> ObterAcao(DataCarrierFormularioNew form, UserSgq usuarioLogado)
        {
            string ParCompany = "";
            string ClusterGroup = "";
            string ParCluster = "";
            string ParLevel1 = "";
            string Regional = "";
            string GrupoEmpresa = "";

            if (form.ParCompany_Ids.Length > 0) ParCompany = $"AND PC.Id IN({string.Join(",", form.ParCompany_Ids)})";

            if (form.ParClusterGroup_Ids.Length > 0) ClusterGroup = $"AND PAC.ParClusterGroup_Id IN({string.Join(",", form.ParClusterGroup_Ids)})";

            if (form.ParCluster_Ids.Length > 0) ParCluster = $"AND PAC.ParCluster_Id IN({string.Join(",", form.ParCluster_Ids)})";

            if (form.ParLevel1_Ids.Length > 0) ParLevel1 = $"AND PAC.ParLevel1_Id IN({string.Join(",", form.ParLevel1_Ids)})";

            if (form.ParStructure3_Ids.Length > 0) Regional = $"AND PS.Id IN({string.Join(",", form.ParStructure3_Ids)})";

            if (form.ParStructure2_Ids.Length > 0) GrupoEmpresa = $"AND PS.ParStructureParent_Id IN({string.Join(",", form.ParStructure2_Ids)})";

            var query = $@"
         DECLARE @DATAINICIAL DATETIME = '{ form.startDate.ToString("yyyy-MM-dd")} {" 00:00:00"}'
         DECLARE @DATAFINAL   DATETIME =  '{ form.endDate.ToString("yyyy-MM-dd")} {" 23:59:00"}'

         SELECT
         PAC.Id,
         PL1.Id AS ParLevel1_Id,
         PL1.Name AS ParLevel1_Name,
         PL2.Id AS ParLevel2_Id,
         PL2.Name AS ParLevel2_Name,
         PL3.Id AS ParLevel3_Id,
         PL3.Name AS ParLevel3_Name,
         PC.Id AS ParCompany_Id,
         PC.Name AS ParCompany_Name,
         PD.Id AS ParDepartment_Id,
         PD.Name AS ParDepartment_Name,
         PD.Parent_Id AS ParDepartmentParent_Id,
         PDS.Name AS ParDepartmentParent_Name,
         PCG.Id AS ParCargo_Id,
         PCG.Name AS ParCargo_Name,
         PCXUS.UserSgq_Id as UsuarioLogado,
         PAC.Acao_Naoconformidade,
         PAC.AcaoText,
         FORMAT(PAC.DataEmissao, 'dd/MM/yyyy') as DataEmissao,
         PAC.DataConclusao,
         PAC.HoraEmissao,
         PAC.HoraConclusao,
         PAC.Referencia,
         PAC.Responsavel,
         PAC.Emissor,
		US_Emissor.FullName AS EmissorNome,
         PAC.Prioridade,
         PAC.Status,
         PAC.IsActive,
         US.FullName AS Responsavel_Name,
            STUFF((SELECT DISTINCT
			        CONCAT(', ', USGQ.FullName)
		        FROM UserSGQ USGQ
		        INNER JOIN PA.AcaoXNotificarAcao PAXNA
			        ON PAXNA.UserSgq_Id = USGQ.Id
			        AND PAXNA.Acao_Id = PAC.Id
			        AND PAXNA.IsActive = 1
			        AND USGQ.IsActive = 1
		        FOR XML PATH (''))
	        ,
	        1, 2, ''
	        ) AS Notificar
         FROM Pa.Acao PAC  WITH (NOLOCK)
         LEFT JOIN ParLevel1 PL1  WITH (NOLOCK)
         ON PL1.Id = PAC.ParLevel1_Id
         LEFT JOIN ParLevel2 PL2  WITH (NOLOCK)
         ON PL2.Id = PAC.ParLevel2_Id
         LEFT JOIN ParLevel3 PL3  WITH (NOLOCK)
         ON PL3.Id = PAC.ParLevel3_Id
         LEFT JOIN ParCompany PC  WITH (NOLOCK)
         ON PC.Id = PAC.ParCompany_Id
         LEFT JOIN ParCompanyXStructure PCXS WITH (NOLOCK)
		 ON PCXS.ParCompany_Id = PC.Id AND PCXS.Active = 1
		 INNER JOIN ParStructure PS WITH (NOLOCK)
		 ON PS.Id = PCXS.ParStructure_Id 
         LEFT JOIN ParDepartment PD  WITH (NOLOCK)
         ON PD.Id = PAC.ParDepartment_Id
         LEFT JOIN ParDepartment PDS  WITH (NOLOCK)
         ON PDs.Id = PAC.ParDepartmentParent_Id
         LEFT JOIN ParCargo PCG  WITH (NOLOCK)
         ON PCG.Id = PAC.ParCargo_Id
         LEFT JOIN UserSgq US WITH (NOLOCK)
         ON US.Id = PAC.Responsavel
         LEFT JOIN UserSgq US_Emissor WITH (NOLOCK)
         ON US_Emissor.Id = PAC.Emissor
         INNER JOIN ParCompanyXUserSgq PCXUS 
         ON PCXUS.ParCompany_Id = PAC.ParCompany_Id
	     and PCXUS.UserSgq_Id = {usuarioLogado.Id}
         WHERE PAC.DataEmissao BETWEEN @DATAINICIAL AND @DATAFINAL
         {ParCompany}
         {ClusterGroup}
         {ParCluster}
         {ParLevel1}
         {GrupoEmpresa}
         {Regional}";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<AcaoViewModel>(query);
                return lista;
            }
        }

        public IEnumerable<AcaoViewModel> ObterStatusPorId(string status)
        {
            var query = $@"
                    SELECT
                     PAC.Id,
                     PL1.Id AS ParLevel1_Id,
                     PL1.Name AS ParLevel1_Name,
                     PL2.Id AS ParLevel2_Id,
                     PL2.Name AS ParLevel2_Name,
                     PL3.Id AS ParLevel3_Id,
                     PL3.Name AS ParLevel3_Name,
                     PC.Id AS ParCompany_Id,
                     PC.Name AS ParCompany_Name,
                     PD.Id AS ParDepartment_Id,
                     PD.Name AS ParDepartment_Name,
                     PD.Parent_Id AS ParDepartmentParent_Id,
                     PDS.Name AS ParDepartmentParent_Name,
                     PCG.Id AS ParCargo_Id,
                     PCG.Name AS ParCargo_Name,
                     PAC.Acao_Naoconformidade,
                     PAC.AcaoText,
                     FORMAT(PAC.DataEmissao, 'dd/MM/yyyy') as DataEmissao,
                     PAC.DataConclusao,
                     PAC.HoraEmissao,
                     PAC.HoraConclusao,
                     PAC.Referencia,
                     PAC.Responsavel,
                     PAC.Emissor,
                     PAC.Prioridade,
                     PAC.Status,
                     PAC.IsActive,
                     US.FullName AS Responsavel_Name,
                    STUFF((SELECT DISTINCT
			                CONCAT(', ', USGQ.FullName)
		                FROM UserSGQ USGQ
		                INNER JOIN PA.AcaoXNotificarAcao PAXNA
			                ON PAXNA.UserSgq_Id = USGQ.Id
			                AND PAXNA.Acao_Id = PAC.Id
							AND PAXNA.IsActive = 1
							AND USGQ.IsActive = 1
		                FOR XML PATH (''))
	                ,
	                1, 2, ''
	                ) AS Notificar
                     FROM Pa.Acao PAC  WITH (NOLOCK)
                     LEFT JOIN ParLevel1 PL1  WITH (NOLOCK)
                     ON PL1.Id = PAC.ParLevel1_Id
                     LEFT JOIN ParLevel2 PL2  WITH (NOLOCK)
                     ON PL2.Id = PAC.ParLevel2_Id
                     LEFT JOIN ParLevel3 PL3  WITH (NOLOCK)
                     ON PL3.Id = PAC.ParLevel3_Id
                     LEFT JOIN ParCompany PC  WITH (NOLOCK)
                     ON PC.Id = PAC.ParCompany_Id
                     LEFT JOIN ParDepartment PD  WITH (NOLOCK)
                     ON PD.Id = PAC.ParDepartment_Id
                     LEFT JOIN ParDepartment PDS  WITH (NOLOCK)
                     ON PDs.Id = PAC.ParDepartmentParent_Id
                     LEFT JOIN ParCargo PCG  WITH (NOLOCK)
                     ON PCG.Id = PAC.ParCargo_Id
                     LEFT JOIN UserSgq US WITH (NOLOCK)
                     ON US.Id = PAC.Responsavel
                     WHERE PAC.Status = {status}";

            using (ADOFactory.Factory factory = new ADOFactory.Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<AcaoViewModel>(query);
                return lista;
            }
        }

        public AcaoFormViewModel ObterAcaoComVinculosPorId(int id, UserSgq usuarioLogado)
        {
            var query = $@"
                SELECT
                 PAC.Id,
                 PL1.Id AS ParLevel1_Id,
                 PL1.Name AS ParLevel1_Name,
                 PL2.Id AS ParLevel2_Id,
                 PL2.Name AS ParLevel2_Name,
                 PL3.Id AS ParLevel3_Id,
                 PL3.Name AS ParLevel3_Name,
                 PC.Id AS ParCompany_Id,
                 PC.Name AS ParCompany_Name,
                 PD.Id AS ParDepartment_Id,
                 PD.Name AS ParDepartment_Name,
                 PD.Parent_Id AS ParDepartmentParent_Id,
                 PDS.Name AS ParDepartmentParent_Name,
                 PCG.Id AS ParCargo_Id,
                 PCG.Name AS ParCargo_Name,
                 PCXUS.UserSgq_Id as UsuarioLogado,
                 PAC.Acao_Naoconformidade,
                 PAC.AcaoText,
                 PAC.DataEmissao,
                 PAC.DataConclusao,
                 PAC.HoraEmissao,
                 PAC.HoraConclusao,
                 PAC.Referencia,
                 PAC.Responsavel,
                 PAC.Prioridade,
                 PAC.Emissor,
		        US_Emissor.FullName AS EmissorNome,
                 PAC.Status,
                 PAC.IsActive,
                 US.FullName AS Responsavel_Name
                 FROM Pa.Acao PAC  WITH (NOLOCK)
                 LEFT JOIN ParLevel1 PL1  WITH (NOLOCK)
                 ON PL1.Id = PAC.ParLevel1_Id
                 LEFT JOIN ParLevel2 PL2  WITH (NOLOCK)
                 ON PL2.Id = PAC.ParLevel2_Id
                 LEFT JOIN ParLevel3 PL3  WITH (NOLOCK)
                 ON PL3.Id = PAC.ParLevel3_Id
                 LEFT JOIN ParCompany PC  WITH (NOLOCK)
                 ON PC.Id = PAC.ParCompany_Id
                 LEFT JOIN ParDepartment PD  WITH (NOLOCK)
                 ON PD.Id = PAC.ParDepartment_Id
                 LEFT JOIN ParDepartment PDS  WITH (NOLOCK)
                 ON PDs.Id = PAC.ParDepartmentParent_Id
                 LEFT JOIN ParCargo PCG  WITH (NOLOCK)
                 ON PCG.Id = PAC.ParCargo_Id
                 LEFT JOIN UserSgq US WITH (NOLOCK)
                 ON US.Id = PAC.Responsavel
                 LEFT JOIN UserSgq US_Emissor WITH (NOLOCK)
                 ON US_Emissor.Id = PAC.Emissor
                 INNER JOIN ParCompanyXUserSgq PCXUS 
                 ON PCXUS.ParCompany_Id = PAC.ParCompany_Id
	             and PCXUS.UserSgq_Id = {usuarioLogado.Id}
                 WHERE PAC.Id = {id}";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                var acao = factory.SearchQuery<AcaoFormViewModel>(query).FirstOrDefault();
                acao.ListaResponsavel = BuscarListaResponsavel(acao.ParCompany_Id);
                acao.ListaNotificar = BuscarListaNotificar(acao.ParCompany_Id);
                acao.ListaNotificarAcao = BuscarListaNotificarAcao(acao.Id);
                acao.ListaAcompanhamento = BuscarAcompanhamento(acao.Id);

                return acao;
            }
        }

        public Acao ObterAcaoPorId(int id)
        {
            Acao acao = _dbContext.Fin .Find(id);
            return acao;
        }

        private List<NotificarViewModel> BuscarListaNotificarAcao(int acao_id)
        {

            List<NotificarViewModel> listaNotificarAcao;
            string queryUser = $@"
                                SELECT 
	                                USGQ.ID AS ID,
	                                CONCAT(USGQ.FullName, ' (', IIF(LEN(USGQ.NAME) > 3, SUBSTRING(USGQ.NAME,0,4), ''), ')') AS Nome
                                FROM UserSGQ USGQ WITH (NOLOCK)
                                INNER JOIN Pa.AcaoXNotificarAcao AXNA WITH (NOLOCK)
	                                ON AXNA.Acao_Id = {acao_id}
	                                AND AXNA.IsActive = 1
	                                AND AXNA.UserSgq_Id = USGQ.Id
                                WHERE 1=1";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                listaNotificarAcao = factory.SearchQuery<NotificarViewModel>(queryUser).ToList();
            }

            return listaNotificarAcao;
        }
        private List<AcompanhamentoAcaoViewModel> BuscarAcompanhamento(int acao_id)
        {
            List<AcompanhamentoAcaoViewModel> listaAcompanhamentoAcaoViewModel;
            listaAcompanhamentoAcaoViewModel = _db.AcompanhamentoAcao
                .Where(x => x.Acao_Id == acao_id)
                .OrderByDescending(x => x.DataRegistro)
                .Select(x => new AcompanhamentoAcaoViewModel()
                {
                    DataRegistro = x.DataRegistro,
                    Observacao = x.Observacao,
                    Status = x.Status,
                    ListaNotificar = x.ListaNotificar
                        .Select(n => new NotificarViewModel() { Id = n.UserSgq.Id, Nome = n.UserSgq.FullName })
                        .ToList(),
                    Responsavel = x.UserSgq.FullName
                })
                .ToList();

            return listaAcompanhamentoAcaoViewModel;
        }
        private List<NotificarViewModel> BuscarListaResponsavel(int ParCompany_Id)
        {
            var listaAuditor = RetornarUsuariosVinculadosAUnidade(ParCompany_Id);
            List<NotificarViewModel> listaAuditorAcao = new List<NotificarViewModel>();

            foreach (var item in listaAuditor)
            {
                if (item.Role != null && item.Role.ToLower().Contains("Auditor".ToLower()))
                {
                    listaAuditorAcao.Add(listaAuditor.Select(x => new NotificarViewModel()
                    {
                        Id = item.Id,
                        Nome = item.Name
                    }).FirstOrDefault());
                }
            }
            return listaAuditorAcao;
        }
        private List<NotificarViewModel> BuscarListaNotificar(int ParCompany_Id)
        {
            var listaNotificar = RetornarUsuariosVinculadosAUnidade(ParCompany_Id);
            List<NotificarViewModel> listaNotificarAcao = new List<NotificarViewModel>();

            foreach (var item in listaNotificar)
            {
                if (item.Role != null && item.Role.ToLower().Contains("Auditor".ToLower()))
                {
                    listaNotificarAcao.Add(listaNotificar.Select(x => new NotificarViewModel()
                    {
                        Id = item.Id,
                        Nome = item.Name
                    }).FirstOrDefault());
                }
            }
            return listaNotificarAcao;
        }
        private List<UserSgq> RetornarUsuariosVinculadosAUnidade(int ParCompany_Id)
        {
            var query = $@"
                            SELECT 
	                            ID AS ID,
	                            CONCAT(FullName, ' (', IIF(LEN(NAME) > 3, SUBSTRING(NAME,0,4), ''), ')') AS Nome
                            FROM UserSgq WITH (NOLOCK)
                            WHERE 1=1 
                            AND isActive = 1 
                            AND LOWER(Role) like LOWER('%Auditor%')
                            AND id IN(
                             SELECT
                             PCXU.UserSgq_Id
                             FROM
                             ParCompanyXUserSgq PCXU WITH(NOLOCK)                    
                             INNER JOIN UserSgq US ON PCXU.ParCompany_Id = {ParCompany_Id}
                             UNION ALL   

                             SELECT US.Id
                             FROM UserSgq US WITH(NOLOCK)             
                             WHERE US.ParCompany_Id = {ParCompany_Id}
                            )";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                var retorno = factory.SearchQuery<UserSgq>(query).ToList();
                return retorno;
            }
        }

        public void VincularUsuariosASeremNotificadosAAcao(AcaoInputModel objAcao, List<int> listaInserir)
        {
            foreach (var item in listaInserir)
            {
                try
                {
                    string sql = $@"INSERT INTO Pa.AcaoXNotificarAcao(
                                    Acao_Id				
                                    ,UserSgq_Id				
                                    ,AddDate)
                                    VALUES(
                                          @Acao_Id			
                                         ,@UserSgq_Id			
                                         ,@AddDate			
                                        )";


                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                        {
                            cmd.CommandType = CommandType.Text;
                            UtilSqlCommand.AddParameterNullable(cmd, "@Acao_Id", objAcao.Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@UserSgq_Id", item);
                            UtilSqlCommand.AddParameterNullable(cmd, "@AddDate", DateTime.Now);

                            var id = Convert.ToInt32(cmd.ExecuteScalar());

                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        public void InativarUsuariosASeremNotificadosAAcao(AcaoInputModel objAcao, List<int> listaDeletar)
        {
            foreach (var item in listaDeletar)
            {
                try
                {
                    string sql = $@" UPDATE Pa.AcaoXNotificarAcao 
                                        set IsActive = 0 
                                    where Acao_Id = @Acao_Id
                                    and UserSgq_Id = @UserSgq_Id";


                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                        {
                            cmd.CommandType = CommandType.Text;
                            UtilSqlCommand.AddParameterNullable(cmd, "@Acao_Id", objAcao.Id);
                            UtilSqlCommand.AddParameterNullable(cmd, "@UserSgq_Id", item);

                            var id = Convert.ToInt32(cmd.ExecuteScalar());

                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        public List<AcaoXNotificarAcao> RetornarUsuariosASeremNotificadosDaAcao(AcaoInputModel objAcao)
        {
            var query = $@"
                select * from Pa.AcaoXNotificarAcao 
                where Acao_Id = {objAcao.Id}
                and IsActive = 1
            ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<AcaoXNotificarAcao>(query);
                return lista;
            }
        }

        public void AtualizarValoresDaAcao(AcaoInputModel objAcao)
        {

            var queryUpdate = $@"

                update Pa.Acao set 
                     Acao_Naoconformidade	= @Acao_Naoconformidade
                    ,AcaoText				= @AcaoText
                    ,DataConclusao			= @DataConclusao
                    ,HoraConclusao			= @HoraConclusao
                    ,Referencia				= @Referencia
                    ,Responsavel			= @Responsavel		
                    ,Prioridade             = @Prioridade
                    ,Status                 = @Status

                where Id = {objAcao.Id}

            ";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                using (SqlCommand cmd = new SqlCommand(queryUpdate, factory.connection))
                {
                    cmd.CommandType = CommandType.Text;

                    UtilSqlCommand.AddParameterNullable(cmd, "@Acao_Naoconformidade", objAcao.Acao_Naoconformidade);
                    UtilSqlCommand.AddParameterNullable(cmd, "@AcaoText", objAcao.AcaoText);
                    UtilSqlCommand.AddParameterNullable(cmd, "@DataConclusao", objAcao.DataConclusao);
                    UtilSqlCommand.AddParameterNullable(cmd, "@HoraConclusao", objAcao.HoraConclusao);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Referencia", objAcao.Referencia);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Responsavel", objAcao.Responsavel);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Prioridade", objAcao.Prioridade);
                    UtilSqlCommand.AddParameterNullable(cmd, "@Status", objAcao.Status);

                    var id = cmd.ExecuteScalar();

                }
            }
        }
    }
}
