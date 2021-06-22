using ADOFactory;
using Dominio;
using Dominio.AcaoRH;
using Dominio.AcaoRH.Email;
using DTO;
using Helper;
using SgqServiceBusiness.Controllers.RH;
using SgqSystem.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.PlanoDeAcao
{
    [RoutePrefix("api/AcaoApi")]
    public class AcaoApiController : BaseApiController
    {
        [Route("GetAcaoByFilter")]
        [HttpPost]
        public IEnumerable<AcaoViewModel> GetAcaoByFilter([FromBody] DataCarrierFormularioNew form)
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
        WHERE PAC.DataEmissao BETWEEN @DATAINICIAL AND @DATAFINAL
        {ParCompany}
        {ClusterGroup}
        {ParCluster}
        {ParLevel1}
        {GrupoEmpresa}
        {Regional}";

            using (ADOFactory.Factory factory = new ADOFactory.Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<AcaoViewModel>(query);
                return lista;
            }
        }

        [Route("GetByIdStatus/{status}")]
        [HttpGet]
        public IEnumerable<AcaoViewModel> GetByIdStatus(string status)
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

        [Route("Post")]
        [HttpPost]
        public AcaoViewModel Post([System.Web.Http.FromBody] AcaoViewModel objAcao)
        {
            try
            {

                //salva os campos comuns da ação
                UpdateAction(objAcao);

                //salva/deleta a listagem de usuario no campo notificar
                GetNotificarList(objAcao);

                //salva/deleta a listagem de imagens de evidencias
                GetEvidenciaList(objAcao);

                GetEvidenciaConcluidaList(objAcao);

                if (objAcao.Responsavel != null)
                {
                    SendMail(objAcao);
                }

            }
            catch (Exception e)
            {
                return null;
            }


            return null;
        }


        public void SendMail(AcaoViewModel acao)
        {
            //1 Pendente - nao envia email
            //2 Em andamento - cenario 1 e 2
            //3 Concluída - cenario 3 e 4
            //4 Atrasada  - cenario 5 e 6
            //5 Cancelada - cenario 7 e 8

            if (int.Parse(acao.Status) == 2)
            {
                var acaoCompleta = new AcaoBusiness().GetBy(acao.Id);

                var emailResponsavel = new MontaEmail(new EmailCreateAcaoResponsavel(acaoCompleta));
                EmailAcaoService.Send(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoNotificados(acaoCompleta));
                EmailAcaoService.Send(emailNotificados);

            }

            if (int.Parse(acao.Status) == 3)
            {
                var acaoCompleta = new AcaoBusiness().GetBy(acao.Id);

                var emailResponsavel = new MontaEmail(new EmailCreateAcaoVerEAgirResponsavel(acaoCompleta));
                EmailAcaoService.Send(emailResponsavel);

                var emailNotificados = new MontaEmail(new EmailCreateAcaoVerEAgirNotificados(acaoCompleta));
                EmailAcaoService.Send(emailNotificados);

            }
        }

        public void GetEvidenciaList(AcaoViewModel objAcao)
        {
            var listaEvidenciasDB = getEvidenciasDB(objAcao.Id);
            var listaEvidenciasPathsEditadas = new List<Evidencia>();

            if (objAcao.ListaEvidencia != null)
            {
                listaEvidenciasPathsEditadas = objAcao.ListaEvidencia.ToList();
            }

            var listaInserir = listaEvidenciasPathsEditadas.Where(x => !listaEvidenciasDB.Select(y => y.Path).ToList().Contains(x.Path)).ToList();

            var listaDeletar = listaEvidenciasDB.Where(x => !listaEvidenciasPathsEditadas.Select(y => y.Path).Contains(x.Path)).ToList();

            if (listaInserir.Count > 0)
                InserirEvidenciaList(objAcao, listaInserir);

            if (listaDeletar.Count > 0)
                InativarEvidenciaList(objAcao, listaDeletar);
        }

        public void GetEvidenciaConcluidaList(AcaoViewModel objAcao)
        {
            var listaEvidenciasConcluidasDB = BuscarListaEvidenciasConcluidas(objAcao.Id);

            var listaEvidenciasPathsEditadas = new List<Evidencia>();
            if (objAcao.ListaEvidenciaConcluida != null)
            {
                listaEvidenciasPathsEditadas = objAcao.ListaEvidenciaConcluida.ToList();
            }

            var listaInserir = listaEvidenciasPathsEditadas.Where(x => !listaEvidenciasConcluidasDB.Select(y => y.Path).ToList().Contains(x.Path)).ToList();

            var listaDeletar = listaEvidenciasConcluidasDB.Where(x => !listaEvidenciasPathsEditadas.Select(y => y.Path).Contains(x.Path)).ToList();

            if (listaInserir.Count > 0)
                InserirEvidenciaConcluidaList(objAcao, listaInserir);

            if (listaDeletar.Count > 0)
                InativarEvidenciaConcluidaList(objAcao, listaDeletar);
        }

        public void InserirEvidenciaList(AcaoViewModel objAcao, List<Evidencia> listaInserir)
        {
            AppColetaBusiness appColetaBusiness = new AppColetaBusiness();

            Acao objAcaoDB = new Acao();

            using (var db = new SgqDbDevEntities())
            {
                objAcaoDB = db.Acao.Where(x => x.Id == objAcao.Id).FirstOrDefault();
            }

            foreach (var evidenciaNaoConformidade in listaInserir)
            {
                var filePath = appColetaBusiness.SaveFileEvidenciaNaoConformidade(objAcaoDB.ParLevel1_Id, objAcaoDB.ParLevel2_Id, objAcaoDB.ParLevel3_Id, evidenciaNaoConformidade.Base64);
                appColetaBusiness.SaveEvidenciaNaoConformidade(new EvidenciaNaoConformidade() { Acao_Id = objAcao.Id, Path = filePath });
            }
        }

        public void InserirEvidenciaConcluidaList(AcaoViewModel objAcao, List<Evidencia> listaInserir)
        {
            AppColetaBusiness appColetaBusiness = new AppColetaBusiness();

            Acao objAcaoDB = new Acao();

            using (var db = new SgqDbDevEntities())
            {
                objAcaoDB = db.Acao.Where(x => x.Id == objAcao.Id).FirstOrDefault();
            }

            foreach (var evidenciaAcaoConcluida in listaInserir)
            {
                var filePath = appColetaBusiness.SaveFileEvidenciaAcaoConcluida(objAcaoDB.ParLevel1_Id, objAcaoDB.ParLevel2_Id, objAcaoDB.ParLevel3_Id, evidenciaAcaoConcluida.Base64);
                appColetaBusiness.SaveEvidenciaAcaoConcluida(new EvidenciaAcaoConcluida() { Acao_Id = objAcao.Id, Path = filePath });
            }
        }

        public void InativarEvidenciaList(AcaoViewModel objAcao, List<Evidencia> listaDeletar)
        {
            foreach (var item in listaDeletar)
            {
                try
                {
                    string sql = $@" UPDATE Pa.EvidenciaNaoConformidade 
                                        set IsActive = 0 
                                    where Id = @Id";

                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                        {
                            cmd.CommandType = CommandType.Text;
                            UtilSqlCommand.AddParameterNullable(cmd, "@Id", item.Id);

                            var id = Convert.ToInt32(cmd.ExecuteScalar());

                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        public void InativarEvidenciaConcluidaList(AcaoViewModel objAcao, List<Evidencia> listaDeletar)
        {
            foreach (var item in listaDeletar)
            {
                try
                {
                    string sql = $@" UPDATE Pa.EvidenciaAcaoConcluida 
                                        set IsActive = 0 
                                    where Id = @Id";

                    using (Factory factory = new Factory("DefaultConnection"))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, factory.connection))
                        {
                            cmd.CommandType = CommandType.Text;
                            UtilSqlCommand.AddParameterNullable(cmd, "@Id", item.Id);

                            var id = Convert.ToInt32(cmd.ExecuteScalar());

                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        public List<Evidencia> getEvidenciasDB(int acao_Id)
        {
            var query = $@"
                select * from Pa.EvidenciaNaoConformidade 
                where Acao_Id = {acao_Id}
                and IsActive = 1
            ";

            using (ADOFactory.Factory factory = new ADOFactory.Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<Evidencia>(query);
                return lista;
            }
        }

        public void GetNotificarList(AcaoViewModel objAcao)
        {
            var listaUsuarioNotificados = getUsuariosNotificados(objAcao);

            var listaIdsUsuarioEditados = objAcao.ListaNotificarAcao.Select(x => x.Id).ToList();

            var listaInserir = listaIdsUsuarioEditados.Where(x => !listaUsuarioNotificados.Select(y => y.UserSgq_Id).ToList().Contains(x)).ToList();

            var listaDeletar = listaUsuarioNotificados.Select(y => y.UserSgq_Id).ToList().Where(x => !listaIdsUsuarioEditados.Contains(x)).ToList();

            if (listaInserir.Count > 0)
                InserirNotificarList(objAcao, listaInserir);

            if (listaDeletar.Count > 0)
                InativarNotificarList(objAcao, listaDeletar);

        }

        public void InserirNotificarList(AcaoViewModel objAcao, List<int> listaInserir)
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

        public void InativarNotificarList(AcaoViewModel objAcao, List<int> listaDeletar)
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

        public List<AcaoXNotificarAcao> getUsuariosNotificados(AcaoViewModel objAcao)
        {
            var query = $@"
                select * from Pa.AcaoXNotificarAcao 
                where Acao_Id = {objAcao.Id}
                and IsActive = 1
            ";

            using (ADOFactory.Factory factory = new ADOFactory.Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<AcaoXNotificarAcao>(query);
                return lista;
            }
        }

        public void updateAcaoXNotificarAcao(char usuarioNotificar)
        {

            var queryUpdate = $@"

                update Pa.AcaoXNotificarAcao set 
                     User_Id = @User_Id
                   

                where Acao_Id = {usuarioNotificar}

            ";
        }

        public void UpdateAction(AcaoViewModel objAcao)
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

        [Route("GetById/{id}")]
        [HttpGet]
        public AcaoFormViewModel GetById(int id)
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
                 PAC.DataEmissao,
                 PAC.DataConclusao,
                 PAC.HoraEmissao,
                 PAC.HoraConclusao,
                 PAC.Referencia,
                 PAC.Responsavel,
                 PAC.Prioridade,
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
                 WHERE PAC.Id = {id}";

            using (ADOFactory.Factory factory = new ADOFactory.Factory("DefaultConnection"))
            {
                var acao = factory.SearchQuery<AcaoFormViewModel>(query).FirstOrDefault();
                acao.ListaResponsavel = BuscarListaResponsavel(acao.ParCompany_Id);
                acao.ListaNotificar = BuscarListaNotificar(acao.ParCompany_Id);
                acao.ListaNotificarAcao = BuscarListaNotificarAcao(acao.Id);

                return acao;
            }
        }

        [Route("GetFotosEvidenciaConcluida/{id}")]
        [HttpGet]
        public List<ImageEvidencia> GetFotosEvidenciaConcluida(int id)
        {
            var lista = new AcaoFormViewModel();

            var listaFotos = new List<ImageEvidencia>();

            lista.ListaEvidenciaConcluida = BuscarListaEvidenciasConcluidas(id);

            using (var db = new SgqDbDevEntities())
            {

                foreach (var item in lista.ListaEvidenciaConcluida)
                {
                    var foto = new ImageEvidencia();

                    string url = item.Path;

                    byte[] bytes;
                    //Verificar se no web.config a credencial do servidor de fotos
                    Exception exception = null;

                    bytes = FileHelper.DownloadPhoto(url
                   , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialUserServerPhoto
                   , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialPassServerPhoto
                   , out exception);

                    if (exception != null)
                        throw new Exception("Error: " + exception.ToClient());

                    foto.Byte = bytes;
                    foto.Path = item.Path;
                    listaFotos.Add(foto);
                }
            }
            return listaFotos;
        }


        [Route("GetFotosEvidencia/{id}")]
        [HttpGet]
        public List<ImageEvidencia> GetFotosEvidencia(int id)
        {
            var lista = new AcaoFormViewModel();

            var listaFotos = new List<ImageEvidencia>();

            ////download das imagens
            lista.ListaEvidencia = BuscarListaEvidencias(id);

            using (var db = new SgqDbDevEntities())
            {

                foreach (var item in lista.ListaEvidencia)
                {
                    var foto = new ImageEvidencia();

                    string url = item.Path;

                    byte[] bytes;
                    //Verificar se no web.config a credencial do servidor de fotos
                    Exception exception = null;

                    bytes = FileHelper.DownloadPhoto(url
                   , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialUserServerPhoto
                   , DicionarioEstaticoGlobal.DicionarioEstaticoHelpers.credentialPassServerPhoto
                   , out exception);

                    if (exception != null)
                        throw new Exception("Error: " + exception.ToClient());

                    foto.Byte = bytes;
                    foto.Path = item.Path;
                    listaFotos.Add(foto);
                }

                return listaFotos;
            }
        }

        public class ImageEvidencia
        {
            public byte[] Byte { get; set; }

            public string Path { get; set; }

        }

        private List<Evidencia> BuscarListaEvidencias(int acao_Id)
        {
            var lista = new List<Evidencia>();

            var query = $@"select * from Pa.EvidenciaNaoConformidade
                            where Acao_id = {acao_Id}
                            and IsActive = 1";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                lista = factory.SearchQuery<Evidencia>(query).ToList();
            }

            return lista;
        }

        private List<Evidencia> BuscarListaEvidenciasConcluidas(int acao_Id)
        {
            var lista = new List<Evidencia>();

            var query = $@"select * from Pa.EvidenciaAcaoConcluida
                            where Acao_id = {acao_Id}
                            and IsActive = 1";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                lista = factory.SearchQuery<Evidencia>(query).ToList();
            }

            return lista;
        }

        private List<NotificarViewModel> BuscarListaResponsavel(int ParCompany_Id)
        {
            var listaAuditor = GetUsersNotificarViewModelByCompany(ParCompany_Id);
            return listaAuditor;
        }

        private List<NotificarViewModel> BuscarListaNotificar(int ParCompany_Id)
        {
            var listaNotificar = GetUsersNotificarViewModelByCompany(ParCompany_Id);
            return listaNotificar;
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

        public List<NotificarViewModel> GetUsersNotificarViewModelByCompany(int ParCompany_Id)
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
                var retorno = factory.SearchQuery<NotificarViewModel>(query).ToList();
                return retorno;
            }
        }

        private List<UserSgq> GetUsersXAcao(int ParCompany_Id)
        {
            var query = $@"
SELECT
 US.Id AS Id, 
 US.FullName AS Name,
 US.Role AS Role
 FROM PA.AcaoXNotificarAcao PAAXN  WITH (NOLOCK)
 LEFT JOIN UserSgq US WITH (NOLOCK)
 ON US.Id = PAAXN.UserSgq_Id
 WHERE 1=1 AND US.isActive = 1 AND PAAXN.isActive = 1 
";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                var retorno = factory.SearchQuery<UserSgq>(query).ToList();
                return retorno;
            }
        }

        public class AcaoViewModel
        {
            public int Id { get; set; }
            public int ParLevel1_Id { get; set; }
            public string ParLevel1_Name { get; set; }
            public int ParLevel2_Id { get; set; }
            public string ParLevel2_Name { get; set; }
            public int ParLevel3_Id { get; set; }
            public string ParLevel3_Name { get; set; }
            public int ParCompany_Id { get; set; }
            public string ParCompany_Name { get; set; }
            public int ParDepartment_Id { get; set; }
            public string ParDepartment_Name { get; set; }
            public int ParDepartmentParent_Id { get; set; }
            public string ParDepartmentParent_Name { get; set; }
            public string ParCargo_Name { get; set; }
            public int ParCargo_Id { get; set; }
            public string Acao_Naoconformidade { get; set; }
            public string AcaoText { get; set; }
            public DateTime? DataEmissao { get; set; }
            public string _DataEmissao { get { return DataEmissao?.ToShortDateString(); } }
            public DateTime? DataConclusao { get; set; }
            public string _DataConclusao { get { return DataConclusao?.ToShortDateString(); } }
            public TimeSpan HoraEmissao { get; set; }
            public TimeSpan HoraConclusao { get; set; }
            public string Referencia { get; set; }
            public string Responsavel { get; set; }
            public string Notificar { get; set; }
            public string Emissor { get; set; }
            public int ParCluster_Id { get; set; }
            public int ParClusterGroup_Id { get; set; }

            [NotMapped]
            public List<Evidencia> ListaEvidencia { get; set; }

            [NotMapped]
            public List<Evidencia> ListaEvidenciaConcluida { get; set; }

            [NotMapped]
            public List<AcaoXNotificarAcao> ListaNotificarAcao { get; set; }

            [NotMapped]
            public List<string> EvidenciaNaoConformidade { get; set; }

            [NotMapped]
            public List<string> EvidenciaAcaoConcluida { get; set; }
            public string Prioridade { get; set; }
            public string Status { get; set; }
            public bool IsActive { get; set; }
            public string Responsavel_Name { get; set; }
        }

        public class Evidencia
        {
            public int Id { get; set; }

            public string Base64 { get; set; }

            public string Path { get; set; }
        }

        public class AcaoFormViewModel
        {
            public int Id { get; set; }
            public int ParLevel1_Id { get; set; }
            public string ParLevel1_Name { get; set; }
            public int ParLevel2_Id { get; set; }
            public string ParLevel2_Name { get; set; }
            public int ParLevel3_Id { get; set; }
            public string ParLevel3_Name { get; set; }
            public int ParCompany_Id { get; set; }
            public string ParCompany_Name { get; set; }
            public int ParDepartment_Id { get; set; }
            public string ParDepartment_Name { get; set; }
            public int ParDepartmentParent_Id { get; set; }
            public string ParDepartmentParent_Name { get; set; }
            public int ParCargo_Id { get; set; }
            public string ParCargo_Name { get; set; }
            public string Acao_Naoconformidade { get; set; }
            public string AcaoText { get; set; }
            public DateTime? DataEmissao { get; set; }
            public string _DataEmissao { get { return DataEmissao?.ToShortDateString(); } }
            public DateTime? DataConclusao { get; set; }
            public TimeSpan HoraEmissao { get; set; }
            public TimeSpan HoraConclusao { get; set; }
            public string Referencia { get; set; }
            public string Responsavel { get; set; }
            public string Notificar { get; set; }

            [NotMapped]
            public List<string> EvidenciaNaoConformidade { get; set; }

            [NotMapped]
            public List<FileStream> listaDeFotosEvidencia { get; set; }

            [NotMapped]
            public List<string> EvidenciaAcaoConcluida { get; set; }
            public string Prioridade { get; set; }
            public string Status { get; set; }
            public bool IsActive { get; set; }
            public string Responsavel_Name { get; set; }

            [NotMapped]
            public List<Evidencia> ListaEvidencia { get; set; }

            [NotMapped]
            public List<Evidencia> ListaEvidenciaConcluida { get; set; }


            public string Observacao { get; set; }
            public string Acao { get; set; }


            public List<NotificarViewModel> ListaResponsavel { get; set; } = new List<NotificarViewModel>();

            public List<NotificarViewModel> ListaPrioridade { get; set; } = new List<NotificarViewModel>()
            {
                new NotificarViewModel{ Id = 1, Nome = "Baixa"},
                new NotificarViewModel{ Id = 2, Nome = "Média"},
                new NotificarViewModel{ Id = 3, Nome = "Alta"},
            };
            public List<NotificarViewModel> ListaNotificar { get; set; } = new List<NotificarViewModel>();
            public List<NotificarViewModel> ListaNotificarAcao { get; set; } = new List<NotificarViewModel>();
        }

        public class NotificarViewModel
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }
    }
}
