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
        public AcaoViewModel Post([System.Web.Http.FromBody] AcaoInputModel objAcao)
        {
            try
            {

                //salva os campos comuns da ação
                AtualizarValoresDaAcao(objAcao);

                //salva/deleta a listagem de usuario no campo notificar
                AtualizarUsuariosASeremNotificadosDaAcao(objAcao);

                //salva/deleta a listagem de imagens de evidencias
                RetornarListaDeEvidencias(objAcao);

                RetornarListaDeEvidenciasConcluidas(objAcao);

                if (objAcao.Responsavel != null)
                {
                    PrepararEEnviarEmail(objAcao);
                }

            }
            catch (Exception e)
            {
                return null;
            }

            return new AcaoViewModel() { Id = objAcao.Id};
        }

        [Route("Post/Acompanhamento/{id}")]
        [HttpPost]
        public AcaoViewModel Post([FromUri] int id, [FromBody] AcompanhamentoAcaoInputModel objAcompanhamentoAcao)
        {
            try
            {
                var usuarioLogado = base.GetUsuarioLogado();

                var listaNotificar = objAcompanhamentoAcao.ListaNotificar
                    .Select(x =>
                        new AcompanhamentoAcaoXNotificar()
                        {
                            UserSgq_Id = x.Id
                        }
                    ).ToList();

                var acompanhamento = new AcompanhamentoAcao()
                {
                    ListaNotificar = listaNotificar,
                    Observacao = objAcompanhamentoAcao.Observacao,
                    Status = objAcompanhamentoAcao.Status,
                    Acao_Id = id,
                    UserSgq_Id = usuarioLogado.Id
                };

                using (SgqDbDevEntities db = new SgqDbDevEntities())
                {
                    db.AcompanhamentoAcao.Add(acompanhamento);
                    db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                return null;
            }

            return null;
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
                acao.ListaAcompanhamento = BuscarAcompanhamento(acao.Id);

                return acao;
            }
        }

        [Route("GetFotosEvidenciaConcluida/{id}")]
        [HttpGet]
        public List<ImagemDaEvidenciaViewModel> GetFotosEvidenciaConcluida(int id)
        {
            var lista = new AcaoFormViewModel();

            var listaFotos = new List<ImagemDaEvidenciaViewModel>();

            lista.ListaEvidenciaConcluida = BuscarListaEvidenciasConcluidas(id);

            using (var db = new SgqDbDevEntities())
            {

                foreach (var item in lista.ListaEvidenciaConcluida)
                {
                    var foto = new ImagemDaEvidenciaViewModel();

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
        public List<ImagemDaEvidenciaViewModel> GetFotosEvidencia(int id)
        {
            var lista = new AcaoFormViewModel();

            var listaFotos = new List<ImagemDaEvidenciaViewModel>();

            ////download das imagens
            lista.ListaEvidencia = BuscarListaEvidencias(id);

            using (var db = new SgqDbDevEntities())
            {

                foreach (var item in lista.ListaEvidencia)
                {
                    var foto = new ImagemDaEvidenciaViewModel();

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

        #region ViewModel
        public class ImagemDaEvidenciaViewModel
        {
            public byte[] Byte { get; set; }

            public string Path { get; set; }

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
            public List<EvidenciaViewModel> ListaEvidencia { get; set; }

            [NotMapped]
            public List<EvidenciaViewModel> ListaEvidenciaConcluida { get; set; }

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

        public class EvidenciaViewModel
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
            public List<EvidenciaViewModel> ListaEvidencia { get; set; }

            [NotMapped]
            public List<EvidenciaViewModel> ListaEvidenciaConcluida { get; set; }


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

            public List<AcompanhamentoAcaoViewModel> ListaAcompanhamento { get; set; } = new List<AcompanhamentoAcaoViewModel>();
        }

        public class NotificarViewModel
        {
            public int Id { get; set; }
            public string Nome { get; set; }
        }

        public class AcompanhamentoAcaoViewModel
        {
            public string Observacao { get; set; }
            public DateTime DataRegistro { get; set; }
            public string DataRegistroFormatada { get { return DataRegistro.ToString("dd/MM/yyyy HH:mm"); } }
            public List<NotificarViewModel> ListaNotificar { get; set; }
            public Dominio.Enums.Enums.EAcaoStatus Status { get; set; }
            public string Responsavel { get; set; }
            public string StatusName { get { return Enum.GetName(typeof(Dominio.Enums.Enums.EAcaoStatus), Status); } }
        }
        #endregion

        #region InputModel
        public class AcompanhamentoAcaoInputModel
        {
            public string Observacao { get; set; }
            public DateTime DataRegistro { get { return DateTime.Now; } }
            public List<NotificarViewModel> ListaNotificar { get; set; }
            public Dominio.Enums.Enums.EAcaoStatus Status { get; set; }
        }

        public class AcaoInputModel
        {
            public int Id { get; set; }
            public string Acao_Naoconformidade { get; set; }
            public string AcaoText { get; set; }
            public DateTime? DataConclusao { get; set; }
            public TimeSpan HoraConclusao { get; set; }
            public string Referencia { get; set; }
            public string Responsavel { get; set; }

            [NotMapped]
            public List<EvidenciaViewModel> ListaEvidencia { get; set; }

            [NotMapped]
            public List<EvidenciaViewModel> ListaEvidenciaConcluida { get; set; }

            [NotMapped]
            public List<AcaoXNotificarAcao> ListaNotificarAcao { get; set; }

            public string Prioridade { get; set; }
            public string Status { get; set; }
        }
        #endregion

        #region AcaoService
        private void PrepararEEnviarEmail(AcaoInputModel acao)
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

        private void RetornarListaDeEvidencias(AcaoInputModel objAcao)
        {
            var listaEvidenciasDB = RetornarEvidenciasDaAcao(objAcao.Id);
            var listaEvidenciasPathsEditadas = new List<EvidenciaViewModel>();

            if (objAcao.ListaEvidencia != null)
            {
                listaEvidenciasPathsEditadas = objAcao.ListaEvidencia.ToList();
            }

            var listaInserir = listaEvidenciasPathsEditadas.Where(x => !listaEvidenciasDB.Select(y => y.Path).ToList().Contains(x.Path)).ToList();

            var listaDeletar = listaEvidenciasDB.Where(x => !listaEvidenciasPathsEditadas.Select(y => y.Path).Contains(x.Path)).ToList();

            if (listaInserir.Count > 0)
                VincularEvidenciasAAcao(objAcao, listaInserir);

            if (listaDeletar.Count > 0)
                InativarEvidencias(listaDeletar);
        }

        private void RetornarListaDeEvidenciasConcluidas(AcaoInputModel objAcao)
        {
            var listaEvidenciasConcluidasDB = BuscarListaEvidenciasConcluidas(objAcao.Id);

            var listaEvidenciasPathsEditadas = new List<EvidenciaViewModel>();
            if (objAcao.ListaEvidenciaConcluida != null)
            {
                listaEvidenciasPathsEditadas = objAcao.ListaEvidenciaConcluida.ToList();
            }

            var listaInserir = listaEvidenciasPathsEditadas.Where(x => !listaEvidenciasConcluidasDB.Select(y => y.Path).ToList().Contains(x.Path)).ToList();

            var listaDeletar = listaEvidenciasConcluidasDB.Where(x => !listaEvidenciasPathsEditadas.Select(y => y.Path).Contains(x.Path)).ToList();

            if (listaInserir.Count > 0)
                VincularEvidenciasAAcaoConcluida(objAcao, listaInserir);

            if (listaDeletar.Count > 0)
                InativarEvidenciasDaAcaoConcluida(listaDeletar);
        }

        private void VincularEvidenciasAAcao(AcaoInputModel objAcao, List<EvidenciaViewModel> listaInserir)
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

        private void VincularEvidenciasAAcaoConcluida(AcaoInputModel objAcao, List<EvidenciaViewModel> listaInserir)
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

        private List<EvidenciaViewModel> RetornarEvidenciasDaAcao(int acao_Id)
        {
            var query = $@"
                select * from Pa.EvidenciaNaoConformidade 
                where Acao_Id = {acao_Id}
                and IsActive = 1
            ";

            using (ADOFactory.Factory factory = new ADOFactory.Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<EvidenciaViewModel>(query);
                return lista;
            }
        }

        private void AtualizarUsuariosASeremNotificadosDaAcao(AcaoInputModel objAcao)
        {
            var listaUsuarioNotificados = RetornarUsuariosASeremNotificadosDaAcao(objAcao);

            var listaIdsUsuarioEditados = objAcao.ListaNotificarAcao.Select(x => x.Id).ToList();

            var listaInserir = listaIdsUsuarioEditados.Where(x => !listaUsuarioNotificados.Select(y => y.UserSgq_Id).ToList().Contains(x)).ToList();

            var listaDeletar = listaUsuarioNotificados.Select(y => y.UserSgq_Id).ToList().Where(x => !listaIdsUsuarioEditados.Contains(x)).ToList();

            if (listaInserir.Count > 0)
                VincularUsuariosASeremNotificadosAAcao(objAcao, listaInserir);

            if (listaDeletar.Count > 0)
                InativarUsuariosASeremNotificadosAAcao(objAcao, listaDeletar);

        }

        private void VincularUsuariosASeremNotificadosAAcao(AcaoInputModel objAcao, List<int> listaInserir)
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

        private void InativarUsuariosASeremNotificadosAAcao(AcaoInputModel objAcao, List<int> listaDeletar)
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

        private List<AcaoXNotificarAcao> RetornarUsuariosASeremNotificadosDaAcao(AcaoInputModel objAcao)
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

        private void AtualizarValoresDaAcao(AcaoInputModel objAcao)
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

        private List<EvidenciaViewModel> BuscarListaEvidencias(int acao_Id)
        {
            var lista = new List<EvidenciaViewModel>();

            var query = $@"select * from Pa.EvidenciaNaoConformidade
                            where Acao_id = {acao_Id}
                            and IsActive = 1";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                lista = factory.SearchQuery<EvidenciaViewModel>(query).ToList();
            }

            return lista;
        }

        private List<EvidenciaViewModel> BuscarListaEvidenciasConcluidas(int acao_Id)
        {
            var lista = new List<EvidenciaViewModel>();

            var query = $@"select * from Pa.EvidenciaAcaoConcluida
                            where Acao_id = {acao_Id}
                            and IsActive = 1";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                lista = factory.SearchQuery<EvidenciaViewModel>(query).ToList();
            }

            return lista;
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
            using (SgqDbDevEntities db = new SgqDbDevEntities())
            {
                listaAcompanhamentoAcaoViewModel = db.AcompanhamentoAcao
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
            }

            return listaAcompanhamentoAcaoViewModel;
        }
        #endregion

        #region UserSgqService
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
        #endregion

        #region EvidenciasService
        private void InativarEvidencias(List<EvidenciaViewModel> listaInativar)
        {
            foreach (var item in listaInativar)
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

        private void InativarEvidenciasDaAcaoConcluida(List<EvidenciaViewModel> listaInativar)
        {
            foreach (var item in listaInativar)
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
        #endregion
    }
}
