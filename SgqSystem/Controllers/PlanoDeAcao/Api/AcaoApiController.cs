using ADOFactory;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SgqSystem.Controllers.Api
{
    [RoutePrefix("api/AcaoApi")]
    public class AcaoApiController : BaseApiController
    {
        [Route("Get")]
        [HttpGet]
        public IEnumerable<AcaoViewModel> Get()
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
 PAC.Notificar,
 PAC.EvidenciaNaoConformidade,
 PAC.EvidenciaAcaoConcluida,
 PAC.Prioridade,
 PAC.Status,
 PAC.IsActive,
 US.Name AS Responsavel_Name
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
 LEFT JOIN ParCargo PCG  WITH (NOLOCK)
 ON PCG.Id = PAC.ParCargo_Id
 LEFT JOIN UserSgq US WITH (NOLOCK)
 ON US.Id = PAC.Responsavel"
 ;

            using (ADOFactory.Factory factory = new ADOFactory.Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<AcaoViewModel>(query);
                return lista;
            }
        }

        /*[Route("Post")]
        [HttpPost]
        public IHttpActionResult SetAction(List<Acao> listaObjAcoes)
        {
            AppColetaBusiness appColetaBusiness = new AppColetaBusiness();

            try
            {
                foreach (var acao in listaObjAcoes)
                {
                    appColetaBusiness.SaveAction(acao);

                    foreach (var usuarioNotificado in acao.Notificar)
                    {
                        appColetaBusiness.SaveAcaoXNotificarAcao(new AcaoXNotificarAcao() { Acao_Id = acao.Id, UserSgq_Id = usuarioNotificado });
                    }
                }

            }
            catch (Exception e)
            {
                return null;
            }


            return Ok();
        }*/

        [Route("Get/{id}")]
        [HttpGet]
        public AcaoFormViewModel Get(int id)
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
 PAC.Notificar,
 PAC.EvidenciaNaoConformidade,
 PAC.EvidenciaAcaoConcluida,
 PAC.Prioridade,
 PAC.Status,
 PAC.IsActive,
 US.Name AS Responsavel_Name
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
 LEFT JOIN ParCargo PCG  WITH (NOLOCK)
 ON PCG.Id = PAC.ParCargo_Id
 LEFT JOIN UserSgq US WITH (NOLOCK)
 ON US.Id = PAC.Responsavel
WHERE PAC.Id = {id}
";

            using (ADOFactory.Factory factory = new ADOFactory.Factory("DefaultConnection"))
            {
                var lista = factory.SearchQuery<AcaoFormViewModel>(query).FirstOrDefault();
                //Falta as listas de Evidências (Arquivos e Fotos)
                lista.ListaResponsavel = BuscarListaResponsavel(lista.ParCompany_Id);
                lista.ListaNotificar = BuscarListaNotificar(lista.ParCompany_Id);
                lista.ListaNotificarAcao = BuscarListaNotificarAcao(lista.ParCompany_Id);

                return lista;
            }
        }

        private List<NotificarViewModel> BuscarListaResponsavel(int ParCompany_Id)
        {
            var listaAuditor = GetUsersByCompany(ParCompany_Id);
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
            var listaNotificar = GetUsersByCompany(ParCompany_Id);
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

        private List<NotificarViewModel> BuscarListaNotificarAcao(int ParCompany_Id)
        {
            var listaAuditor = GetUsersXAcao(ParCompany_Id);
            List<NotificarViewModel> listaNotificarAcao = new List<NotificarViewModel>();

            foreach (var item in listaAuditor)
            {
                if (item.Role != null && item.Role.ToLower().Contains("Auditor".ToLower()))
                {
                    listaNotificarAcao.Add(listaAuditor.Select(x => new NotificarViewModel()
                    {
                        Id = item.Id,
                        Nome = item.Name
                    }).FirstOrDefault());
                }
            }
            return listaNotificarAcao;
        }

        public List<UserSgq> GetUsersByCompany(int ParCompany_Id)
        {
            var query = $@"
SELECT * 
FROM UserSgq
WHERE 1=1 AND isActive = 1 AND id 
IN(
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
            public string ParCargo_Name { get; set; }
            public int ParCargo_Id { get; set; }
            public string Acao_Naoconformidade { get; set; }
            public string AcaoText { get; set; }
            public DateTime DataEmissao { get; set; }
            public DateTime DataConclusao { get; set; }
            public TimeSpan HoraEmissao { get; set; }
            public TimeSpan HoraConclusao { get; set; }
            public string Referencia { get; set; }
            public string Responsavel { get; set; }
            public string Notificar { get; set; }
            public string EvidenciaNaoConformidade { get; set; }
            public string EvidenciaAcaoConcluida { get; set; }
            public string Prioridade { get; set; }
            public string Status { get; set; }
            public bool IsActive { get; set; }
            public string Responsavel_Name { get; set; }
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
            public int ParCargo_Id { get; set; }
            public string ParCargo_Name { get; set; }
            public string Acao_Naoconformidade { get; set; }
            public string AcaoText { get; set; }
            public DateTime DataEmissao { get; set; }
            public DateTime DataConclusao { get; set; }
            public TimeSpan HoraEmissao { get; set; }
            public TimeSpan HoraConclusao { get; set; }
            public string Referencia { get; set; }
            public string Responsavel { get; set; }
            public string Notificar { get; set; }
            public string EvidenciaNaoConformidade { get; set; }
            public string EvidenciaAcaoConcluida { get; set; }
            public string Prioridade { get; set; }
            public string Status { get; set; }
            public bool IsActive { get; set; }
            public string Responsavel_Name { get; set; }

            public string Observacao { get; set; }
            public string Acao { get; set; }
            public List<NotificarViewModel> ListaEvidencia { get; set; } = new List<NotificarViewModel>();
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