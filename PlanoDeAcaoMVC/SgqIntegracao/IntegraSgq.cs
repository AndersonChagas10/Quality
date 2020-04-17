using ADOFactory;
using DTO;
using DTO.DTO;
using DTO.DTO.Params;
using PlanoAcaoCore;
using PlanoDeAcaoMVC.SgqIntegracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PlanoDeAcaoMVC
{
    /// <summary>
    /// Disponibiliza as DDL's
    /// Level1, Level2, Level3, Undiade e Quem do SGQ.
    /// </summary>
    public class IntegraSgq : AuthorizeAttribute
    {
        private static DateTime? quandoExecutou = null;
        /// <summary>
        /// Disponibiliza as DDL's
        /// Level1, Level2, Level3, Undiade e Quem do SGQ.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var parCompanies = new List<ParCompanyDTO>();
            var usersSGQ = new List<UserDTO>();

            using (var db = new ConexaoSgq().db)
            {
                var level1 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel1 WHERE IsActive = 1").ToList();
                var level2 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel2 WHERE IsActive = 1").ToList();
                var level3 = db.SearchQuery<ParLevel1DTO>("Select * from parlevel3 WHERE IsActive = 1").ToList();
                usersSGQ = db.SearchQuery<UserDTO>("Select * from usersgq");
                parCompanies = db.SearchQuery<ParCompanyDTO>("Select * from parcompany WHERE IsActive = 1").ToList();

                filterContext.Controller.ViewBag.Level1 = level1;
                filterContext.Controller.ViewBag.Level2 = level2;
                filterContext.Controller.ViewBag.Level3 = level3;
            }

            if (quandoExecutou == null || (quandoExecutou != null && quandoExecutou?.AddDays(1) > DateTime.Now))
            {
                InserirUnidades(parCompanies);

                if (GlobalConfig.SESMT)
                    UpdateName(usersSGQ);

                UpdateSgqId(usersSGQ);
                InserirNovosUsuarios(usersSGQ);
                quandoExecutou = DateTime.Now;
            }

            filterContext.Controller.ViewBag.Unidade = PlanoAcaoCore.Pa_Unidade.Listar().OrderBy(r => r.Name);
            filterContext.Controller.ViewBag.Quem = PlanoAcaoCore.Pa_Quem.Listar().OrderBy(r => r.Name);

        }

        private static void InserirUnidades(List<ParCompanyDTO> parcompany)
        {
            var pa_unidades = PlanoAcaoCore.Pa_Unidade.Listar();

            if (parcompany.Count() > 0)
            {
                var iterator = parcompany.Where(r => !pa_unidades.Any(c => c.Name.Equals(r.Initials)) && r.Initials != null).ToList();

                foreach (var i in iterator)
                {
                    var unidadeInsert = new PlanoAcaoCore.Pa_Unidade() { Name = i.Initials, Description = i.Name, Sgq_Id = i.Id };
                    PlanoAcaoCore.Pa_BaseObject.SalvarGenerico(unidadeInsert);
                }
            }
        }

        //Insere os Ids do UserSgq na tabela Pa_Quem - provavelmente só irá rodar uma unica vez
        private void UpdateSgqId(List<UserDTO> userSgq)
        {
            var usersPA = Pa_Quem.Listar().Where(x => x.UserSgq_Id == null || x.UserSgq_Id == 0).ToList();

            if (usersPA.Count > 0)
            {
                foreach (var item in usersPA)
                {
                    try
                    {
                        if (GlobalConfig.SESMT)
                            item.UserSgq_Id = userSgq.Where(x => x.FullName == item.Name).FirstOrDefault().Id;
                        else
                            item.UserSgq_Id = userSgq.Where(x => x.Name == item.Name).FirstOrDefault().Id;

                        Pa_BaseObject.SalvarGenerico(item);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }

            }
        }

        //Somente para o Sesmt (troca o name para fullname)
        private void UpdateName(List<UserDTO> userSgq)
        {
            var usersPA = Pa_Quem.Listar();

            var usersToUpdate = new List<Pa_Quem>();

            foreach (var userPA in usersPA)
            {
                var userToChange = userSgq.Where(x => x.Name == userPA.Name).FirstOrDefault();

                if (userToChange != null)
                {
                    userPA.Name = userToChange.FullName;
                    userPA.UserSgq_Id = userPA.UserSgq_Id == null ? 0 : userPA.UserSgq_Id;
                    userPA.AlterDate = DateTime.Now;
                }

                Pa_BaseObject.SalvarGenerico(userPA);
            }
        }

        private void InserirNovosUsuarios(List<UserDTO> usersSgq)
        {
            var usersPA = Pa_Quem.Listar();

            usersSgq = usersSgq.Where(userGQ => !usersPA.Any(userPA => userPA.UserSgq_Id.Equals(userGQ.Id) && userPA.Name != null)).ToList();

            foreach (var userSgq in usersSgq)
            {
                var nameToInsert = "";

                if (GlobalConfig.SESMT)
                    nameToInsert = userSgq.FullName;
                else
                    nameToInsert = userSgq.Name;

                var userInsert = new Pa_Quem() { Name = nameToInsert, UserSgq_Id = userSgq.Id, AddDate = DateTime.Now };

                Pa_BaseObject.SalvarGenerico(userInsert);
            }
        }
    }
}
