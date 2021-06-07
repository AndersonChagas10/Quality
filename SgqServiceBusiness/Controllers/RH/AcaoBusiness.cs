using ADOFactory;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgqServiceBusiness.Controllers.RH
{
    public class AcaoBusiness
    {
        public Acao GetBy(int id)
        {
            Acao acao;

            using (Factory factory = new Factory("DefaultConnection"))
            {
                acao = factory.SearchQuery<Acao>($"Select * from Pa.Acao where id = {id}").SingleOrDefault();
            }

            acao.ParLevel1 = new ParLevel1Business().GetBy(acao.ParLevel1_Id);
            acao.ParLevel2 = new ParLevel2Business().GetBy(acao.ParLevel2_Id);
            acao.ParLevel3 = new ParLevel3Business().GetBy(acao.ParLevel3_Id);
            acao.ParCargo = new ParCargoBusiness().GetBy(acao.ParCargo_Id);
            acao.ParCompany = new ParCompanyBusiness().GetBy(acao.ParCompany_Id);
            acao.ParDepartment = new ParDepartmentBusiness().GetBy(acao.ParDepartment_Id); //Sessão
            acao.ParDepartmentParent = new ParDepartmentBusiness().GetBy(acao.ParDepartmentParent_Id); //Centro de custo
            acao.ResponsavelUser = new UserSgqBusiness().GetBy(acao.Responsavel.Value); 
            acao.ParCluster = new ParClusterBusiness().GetBy(acao.ParCluster_Id);
            acao.ParClusterGroup = new ParClusterGroupBusiness().GetBy(acao.ParClusterGroup_Id);
            acao.NotificarUsers = GetNotificarUsersBy(acao.Id);
            acao.EvidenciaAcaoConcluida = GetEvidenciaAcaoConcluidaBy(acao.Id);
            acao.EvidenciaNaoConformidade = GetEvidenciaNaoConformidadeBy(acao.Id);
            acao.EmissorUser = new UserSgqBusiness().GetBy(acao.Emissor);

            return acao;
        }

        public IEnumerable<UserSgq> GetNotificarUsersBy(int acao_Id)
        {
            IEnumerable<UserSgq> usuarios;

            string queryNotificar = $@"SELECT
                            	U.*
                            FROM UserSgq U
                            INNER JOIN Pa.AcaoXNotificarAcao AXN
                            	ON AXN.UserSgq_Id = u.Id
                            WHERE 1 = 1 
                            AND AXN.IsActive = 1
                            AND AXN.Acao_Id = {acao_Id}";

            using (Factory factory = new Factory("DefaultConnection"))
            {
                usuarios = factory.SearchQuery<UserSgq>(queryNotificar).ToList();
            }

            return usuarios;
        }

        public IEnumerable<string> GetEvidenciaAcaoConcluidaBy(int acao_Id)
        {
            //Trazer fotos em base64
            return new string[] { };
        }


        private IEnumerable<string> GetEvidenciaNaoConformidadeBy(int acao_Id)
        {
            //Trazer fotos em base64
            return new string[] { };
        }
    }
}
