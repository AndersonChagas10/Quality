using Conformity.Infra.CrossCutting;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.DTOs.Filtros;
using Conformity.Domain.Core.Entities.PlanoDeAcao;
using Conformity.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Conformity.Domain.Core.DTOs.PlanoDeAcao;

namespace Conformity.Infra.Data.Core.Repository.PlanoDeAcao
{
    public class AcaoRepository
    {
        private readonly PlanoDeAcaoEntityContext _dbContext;
        private readonly ADOContext _aDOContext;

        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<Acao> _repositoryAcao;
        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<ParLevel1> _repositoryParLevel1;
        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<ParLevel2> _repositoryParLevel2;
        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<ParLevel3> _repositoryParLevel3;
        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<ParCargo> _repositoryParCargo;
        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<ParCompany> _repositoryParCompany;
        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<ParDepartment> _repositoryParDepartment;
        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<UserSgq> _repositoryUserSgq;
        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<ParCluster> _repositoryParCluster;
        private readonly IPlanoDeAcaoRepositoryNoLazyLoad<ParClusterGroup> _repositoryParClusterGroup;
        private readonly ApplicationConfig _applicationConfig;

        public AcaoRepository(PlanoDeAcaoEntityContext dbContext
                    , ADOContext aDOContext
                    , IPlanoDeAcaoRepositoryNoLazyLoad<Acao> repositoryAcao
                    , IPlanoDeAcaoRepositoryNoLazyLoad<ParLevel1> repositoryParLevel1
                    , IPlanoDeAcaoRepositoryNoLazyLoad<ParLevel2> repositoryParLevel2
                    , IPlanoDeAcaoRepositoryNoLazyLoad<ParLevel3> repositoryParLevel3
                    , IPlanoDeAcaoRepositoryNoLazyLoad<ParCargo> repositoryParCargo
                    , IPlanoDeAcaoRepositoryNoLazyLoad<ParCompany> repositoryParCompany
                    , IPlanoDeAcaoRepositoryNoLazyLoad<ParDepartment> repositoryParDepartment
                    , IPlanoDeAcaoRepositoryNoLazyLoad<UserSgq> repositoryUserSgq
                    , IPlanoDeAcaoRepositoryNoLazyLoad<ParCluster> repositoryParCluster
                    , IPlanoDeAcaoRepositoryNoLazyLoad<ParClusterGroup> repositoryParClusterGroup
                    , ApplicationConfig applicationConfig)
        {
            _dbContext = dbContext;
            _aDOContext = aDOContext;

            _repositoryAcao = repositoryAcao;
            _repositoryParLevel1 = repositoryParLevel1;
            _repositoryParLevel2 = repositoryParLevel2;
            _repositoryParLevel3 = repositoryParLevel3;
            _repositoryParCargo = repositoryParCargo;
            _repositoryParCompany = repositoryParCompany;
            _repositoryParDepartment = repositoryParDepartment;
            _repositoryUserSgq = repositoryUserSgq;
            _repositoryParCluster = repositoryParCluster;
            _repositoryParClusterGroup = repositoryParClusterGroup;
            _applicationConfig = applicationConfig;
        }

        public IEnumerable<AcaoViewModel> ObterAcao(FiltroListagemDeAcaoDoWorkflow form)
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
	     and PCXUS.UserSgq_Id = {_applicationConfig.Authenticated_Id}
         WHERE PAC.DataEmissao BETWEEN @DATAINICIAL AND @DATAFINAL
         {ParCompany}
         {ClusterGroup}
         {ParCluster}
         {ParLevel1}
         {GrupoEmpresa}
         {Regional}";

            var lista = _aDOContext.SearchQuery<AcaoViewModel>(query);
            return lista;
        }
                
        public AcaoFormViewModel ObterAcaoComVinculosPorId(int id)
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
	             and PCXUS.UserSgq_Id = {_applicationConfig.Authenticated_Id}
                 WHERE PAC.Id = {id}";


            var acao = _aDOContext.SearchQuery<AcaoFormViewModel>(query).FirstOrDefault();
            acao.ListaResponsavel = BuscarListaResponsavel(acao.ParCompany_Id);
            acao.ListaNotificar = BuscarListaNotificar(acao.ParCompany_Id);
            acao.ListaNotificarAcao = BuscarListaNotificarAcao(acao.Id);
            acao.ListaAcompanhamento = BuscarAcompanhamento(acao.Id);

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

            listaNotificarAcao = _aDOContext.SearchQuery<NotificarViewModel>(queryUser).ToList();

            return listaNotificarAcao;
        }
        private List<AcompanhamentoAcaoViewModel> BuscarAcompanhamento(int acao_id)
        {
            List<AcompanhamentoAcaoViewModel> listaAcompanhamentoAcaoViewModel;
            listaAcompanhamentoAcaoViewModel = _dbContext.AcompanhamentoAcao
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
	                            CONCAT(FullName, ' (', IIF(LEN(NAME) > 3, SUBSTRING(NAME,0,4), ''), ')') AS Name,
                                Role AS Role
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

            var retorno = _aDOContext.SearchQuery<UserSgq>(query).ToList();
            return retorno;
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

                    using (SqlCommand cmd = new SqlCommand(sql, _aDOContext.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.AddParameterNullable("@Acao_Id", objAcao.Id);
                        cmd.AddParameterNullable("@UserSgq_Id", item);
                        cmd.AddParameterNullable("@AddDate", DateTime.Now);

                        var id = Convert.ToInt32(cmd.ExecuteScalar());

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

                    using (SqlCommand cmd = new SqlCommand(sql, _aDOContext.connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.AddParameterNullable("@Acao_Id", objAcao.Id);
                        cmd.AddParameterNullable("@UserSgq_Id", item);

                        var id = Convert.ToInt32(cmd.ExecuteScalar());

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

            var lista = _aDOContext.SearchQuery<AcaoXNotificarAcao>(query);
            return lista;
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

            using (SqlCommand cmd = new SqlCommand(queryUpdate, _aDOContext.connection))
            {
                cmd.CommandType = CommandType.Text;

                cmd.AddParameterNullable("@Acao_Naoconformidade", objAcao.Acao_Naoconformidade);
                cmd.AddParameterNullable("@AcaoText", objAcao.AcaoText);
                cmd.AddParameterNullable("@DataConclusao", objAcao.DataConclusao);
                cmd.AddParameterNullable("@HoraConclusao", objAcao.HoraConclusao);
                cmd.AddParameterNullable("@Referencia", objAcao.Referencia);
                cmd.AddParameterNullable("@Responsavel", objAcao.Responsavel);
                cmd.AddParameterNullable("@Prioridade", objAcao.Prioridade);
                cmd.AddParameterNullable("@Status", objAcao.Status);

                var id = cmd.ExecuteScalar();

            }
        }


        public Acao GetById(int id)
        {
            Acao acao = _repositoryAcao.GetById(id);
            acao.ParLevel1 = _repositoryParLevel1.GetById(acao.ParLevel1_Id);
            acao.ParLevel2 = _repositoryParLevel2.GetById(acao.ParLevel2_Id);
            acao.ParLevel3 = _repositoryParLevel3.GetById(acao.ParLevel3_Id);
            acao.ParCargo = _repositoryParCargo.GetById(acao.ParCargo_Id);
            acao.ParCompany = _repositoryParCompany.GetById(acao.ParCompany_Id);
            acao.ParDepartment = _repositoryParDepartment.GetById(acao.ParDepartment_Id); //Sessão
            acao.ParDepartmentParent = _repositoryParDepartment.GetById(acao.ParDepartmentParent_Id); //Centro de custo
            acao.ResponsavelUser = _repositoryUserSgq.GetById(acao.Responsavel.Value);
            acao.ParCluster = _repositoryParCluster.GetById(acao.ParCluster_Id);
            acao.ParClusterGroup = _repositoryParClusterGroup.GetById(acao.ParClusterGroup_Id);
            acao.NotificarUsers = GetNotificarUsersBy(acao.Id);
            acao.EvidenciaAcaoConcluida = new string[] { };
            acao.EvidenciaNaoConformidade = new string[] { };
            acao.EmissorUser = _repositoryUserSgq.GetById(acao.Emissor);

            return acao;
        }

        public IEnumerable<UserSgq> GetNotificarUsersBy(int acao_Id)
        {
            string query = $@"SELECT
                            	U.*
                            FROM UserSgq U
                            INNER JOIN Pa.AcaoXNotificarAcao AXN
                            	ON AXN.UserSgq_Id = u.Id
                            WHERE 1 = 1 
                            AND AXN.IsActive = 1
                            AND AXN.Acao_Id = {acao_Id}";

            IEnumerable<UserSgq> usuarios = _aDOContext.SearchQuery<UserSgq>(query).ToList();
            return usuarios;
        }

        public void AlterarStatusComBaseNoAcompanhamento(int id, AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            Acao acao = _dbContext.Acao.Find(id);

            if (acao != null)
            {
                acao.Status = objAcompanhamentoAcao.Status;
                _dbContext.SaveChanges();
            }
        }

        public int SalvarAcao(Acao item)
        {
            try
            {
                string sql = $@"INSERT INTO Pa.Acao(
                                    ParLevel1_Id				
                                    ,ParLevel2_Id				
                                    ,ParLevel3_Id				
                                    ,ParCompany_Id				
                                    ,ParDepartment_Id			
                                    ,ParDepartmentParent_Id	
                                    ,ParCargo_Id				
                                    ,Acao_Naoconformidade		
                                    ,AcaoText					
                                    ,DataConclusao				
                                    ,HoraConclusao				
                                    ,Referencia				
                                    ,Responsavel								
                                    ,DataEmissao				
                                    ,HoraEmissao				
                                    ,Emissor	
                                    ,Prioridade
                                    ,ParCluster_Id
                                    ,ParClusterGroup_Id
                                    ,Status)
                                    VALUES(
                                          @ParLevel1_Id			
                                         ,@ParLevel2_Id			
                                         ,@ParLevel3_Id			
                                         ,@ParCompany_Id			
                                         ,@ParDepartment_Id		
                                         ,@ParDepartmentParent_Id	
                                         ,@ParCargo_Id			
                                         ,@Acao_Naoconformidade	
                                         ,@AcaoText				
                                         ,@DataConclusao			
                                         ,@HoraConclusao			
                                         ,@Referencia				
                                         ,@Responsavel							
                                         ,@DataEmissao			
                                         ,@HoraEmissao			
                                         ,@Emissor				
                                         ,@Prioridade
                                         ,@ParCluster_Id
                                         ,@ParClusterGroup_Id
                                         ,@Status
                                        );

                SELECT CAST(scope_identity() AS int)";

                var id = 0;

                using (SqlCommand cmd = new SqlCommand(sql, _aDOContext.connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.AddParameterNullable("@ParLevel1_Id", item.ParLevel1_Id);
                    cmd.AddParameterNullable("@ParLevel2_Id", item.ParLevel2_Id);
                    cmd.AddParameterNullable("@ParLevel3_Id", item.ParLevel3_Id);
                    cmd.AddParameterNullable("@ParCompany_Id", item.ParCompany_Id);
                    cmd.AddParameterNullable("@ParDepartment_Id", item.ParDepartment_Id);
                    cmd.AddParameterNullable("@ParDepartmentParent_Id", item.ParDepartmentParent_Id);
                    cmd.AddParameterNullable("@ParCargo_Id", item.ParCargo_Id);
                    cmd.AddParameterNullable("@Acao_Naoconformidade", item.Acao_Naoconformidade);
                    cmd.AddParameterNullable("@AcaoText", item.AcaoText);
                    cmd.AddParameterNullable("@DataConclusao", item.DataConclusao);
                    cmd.AddParameterNullable("@HoraConclusao", item.HoraConclusao);
                    cmd.AddParameterNullable("@Referencia", item.Referencia);
                    cmd.AddParameterNullable("@Responsavel", item.Responsavel);
                    cmd.AddParameterNullable("@DataEmissao", item.DataEmissao);
                    cmd.AddParameterNullable("@HoraEmissao", item.HoraEmissao);
                    cmd.AddParameterNullable("@Emissor", item.Emissor);
                    cmd.AddParameterNullable("@Prioridade", item.Prioridade);
                    cmd.AddParameterNullable("@ParCluster_Id", item.ParCluster_Id);
                    cmd.AddParameterNullable("@ParClusterGroup_Id", item.ParClusterGroup_Id);
                    cmd.AddParameterNullable("@Status", item.Status);

                    id = (int)cmd.ExecuteScalar();

                }
                return id;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public AcaoXAttributesViewModel GerarCodigoDaAcao(Acao objAcao)
        {
            var query =  $@"
                            DECLARE @ANO INT = 2021;
                            DECLARE @PARCOMPANY_ID INT = {objAcao.ParCompany_Id}; 

                            SELECT 
                                CONCAT(A.SIGLA_DA_UNIDADE
                                ,'-'
                                ,RIGHT(CONCAT('000000',A.QUANTIDADE_DE_ACOES+1),5)
                                ,'/'
                                ,A.ANO) AS ProximoCodigo
                            FROM (SELECT
                                    YEAR(PAA.AddDate) AS ANO
                                    , COUNT(1) AS QUANTIDADE_DE_ACOES
                                    , REPLACE(PC.Initials,' ','') AS SIGLA_DA_UNIDADE
                                FROM PA.Acao PAA
                                INNER JOIN PARCOMPANY PC
                                    ON PC.Id = PAA.ParCompany_Id
                                WHERE 1=1
                                    AND YEAR(PAA.AddDate) = @ANO
                                    AND PAA.ParCompany_Id = @PARCOMPANY_ID
                                GROUP BY 
                                    YEAR(PAA.AddDate),
                                    PC.Initials
                                ) A";

            AcaoXAttributesViewModel codigoDaAcao =  _aDOContext.SearchQuery<AcaoXAttributesViewModel>(query).FirstOrDefault();
            return codigoDaAcao;
        }

        public void SalvarCodigoDaAcao(AcaoXAttributes acaoXAttributes)
        {
            string query = $@"INSERT INTO Pa.AcaoXAttributes(
                            Acao_Id, FieldName, Value) 
                            VALUES(@Acao_Id, @FieldName, @Value)";

            using (SqlCommand cmd = new SqlCommand(query, _aDOContext.connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.AddParameterNullable("@Acao_Id", acaoXAttributes.Acao_Id);
                cmd.AddParameterNullable("@FieldName", acaoXAttributes.FieldName);
                cmd.AddParameterNullable("@Value", acaoXAttributes.Value);
                cmd.ExecuteScalar();
            }
        }

        public List<ParCompany> GetUnityByCurrentUser(string search)
        {
            string query = $@"SELECT * FROM dbo.ParCompanyXUserSgq PcXUs
                            INNER JOIN dbo.ParCompany PC
                            ON PcXUs.ParCompany_Id = PC.Id
                            WHERE UserSgq_Id = {_applicationConfig.Authenticated_Id}
                            AND PC.Name LIKE '%{search}%'";

            List<ParCompany> unidades = _aDOContext.SearchQuery<ParCompany>(query).ToList();

            return unidades;
        }

    }
}
