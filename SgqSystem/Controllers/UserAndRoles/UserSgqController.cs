using AutoMapper;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.DTO.Params;
using DTO.Helpers;
using Helper;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [HandleController()]
    public class UserSgqController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        private IBaseDomain<ParCompany, ParCompanyDTO> _baseDomainParCompany;
        private IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> _baseDomainParCompanyXUserSgq;
        private IBaseDomain<UserSgq, UserSgqDTO> _baseDomainUserSgq;
        private IBaseDomain<RoleSGQ, RoleSGQDTO> _baseDomainRoleSGQ;

        public UserSgqController(IBaseDomain<ParCompany, ParCompanyDTO> baseDomainParCompany,
            IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> baseDomainParCompanyXUserSgq,
            IBaseDomain<UserSgq, UserSgqDTO> baseDomainUserSgq,
            IBaseDomain<RoleSGQ, RoleSGQDTO> baseDomainRoleSGQ
            )
        {
            _baseDomainParCompany = baseDomainParCompany;
            _baseDomainUserSgq = baseDomainUserSgq;
            _baseDomainParCompanyXUserSgq = baseDomainParCompanyXUserSgq;
            _baseDomainRoleSGQ = baseDomainRoleSGQ;

            ViewBag.listaParCompany = _baseDomainParCompany.GetAll();
            var listaRoleSGQ = _baseDomainRoleSGQ.GetAll();

            foreach (var roleSgq in listaRoleSGQ)
            {
                roleSgq.Role = roleSgq.Role.Trim();
            }

            ViewBag.listaRoleSGQ = listaRoleSGQ;
        }

        // GET: UserSgq
        public ActionResult Index()
        {
            return View(db.UserSgq.ToList());
        }

        // GET: UserSgq/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserSgq userSgq = db.UserSgq.Find(id);
            if (userSgq == null)
            {
                return HttpNotFound();
            }
            return View(userSgq);
        }

        /// <summary>
        /// Tela de Perfil Usuário
        /// </summary>
        /// <param name="motivo">Quando obtiver um motivo, é por cause da expiração da senha</param>
        /// <returns></returns>
        public ActionResult Perfil(string motivo = "")
        {
            ViewBag.Title = "Perfil";

            ViewBag.Motivo = motivo;

            var idUsuario = Guard.GetUsuarioLogado_Id(ControllerContext.HttpContext);

            var userSgq = db.UserSgq.Where(x => x.Id == idUsuario).FirstOrDefault();

            List<ParCompanyXUserSgq> parCompanyXUserSgq = db.ParCompanyXUserSgq.Where(x => x.UserSgq_Id == idUsuario).ToList();

            var model = new UserViewModel()
            {
                Id = userSgq.Id,
                Name = userSgq.Name,
                FullName = userSgq.FullName,
                Email = userSgq.Email,
                Roles = userSgq.Role == null ? new string[0] : userSgq.Role.Split(';'),
                Password = userSgq.Password,
                Phone = userSgq.Phone,
                Empresa = parCompanyXUserSgq != null ? (from comp in parCompanyXUserSgq select new EmpresaDTO { Role = comp.Role, Nome = comp.ParCompany.Name }).ToList() : new List<EmpresaDTO>()
            };

            return View(model);
        }

        // GET: UserSgq/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserSgq/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Save(UserSgqDTO userSgqDto)
        {

            if (userSgqDto.Id == 0)
            {
                userSgqDto.AddDate = DateTime.Now;
                userSgqDto.Password = "123";
            }
            else
            {
                userSgqDto.AlterDate = DateTime.Now;

                if (userSgqDto.Password == null)
                {
                    UserSgq dummy = db.UserSgq.Find(userSgqDto.Id);
                    userSgqDto.Password = dummy.Password;
                }
            }

            if (userSgqDto.ListRole != null)
            {
                string roles = string.Join("; ", userSgqDto.ListRole);
                userSgqDto.Role = roles;
            }

            IEnumerable<int> listParCompany = userSgqDto.ListParCompany_Id;
            userSgqDto.ParCompany_Id = userSgqDto.ListParCompany_Id.FirstOrDefault();
            userSgqDto = _baseDomainUserSgq.AddOrUpdate(userSgqDto);

            _baseDomainParCompanyXUserSgq.ExecuteSql("DELETE FROM ParCompanyXUserSgq WHERE UserSgq_Id = " + userSgqDto.Id);

            foreach (int ParCompany_id in listParCompany)
            {
                ParCompanyXUserSgqDTO parCompanyXUserSgqDTO = new ParCompanyXUserSgqDTO();
                parCompanyXUserSgqDTO.Id = 0;
                parCompanyXUserSgqDTO.UserSgq_Id = userSgqDto.Id;
                parCompanyXUserSgqDTO.ParCompany_Id = ParCompany_id;

                _baseDomainParCompanyXUserSgq.AddOrUpdate(parCompanyXUserSgqDTO);
            }

        }

        // GET: UserSgq/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserSgq userSgq = db.UserSgq.Find(id);
            if (userSgq == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // GET: UserSgq/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserSgq userSgq = db.UserSgq.Find(id);
            if (userSgq == null)
            {
                return HttpNotFound();
            }
            return View(userSgq);
        }

        // POST: UserSgq/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserSgq userSgq = db.UserSgq.Find(id);
            db.UserSgq.Remove(userSgq);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void ResetSessionTimer()
        {
            try
            {
                HttpCookie currentUserCookie = Request.Cookies["webControlCookie"];
                if (currentUserCookie != null)
                {
                    currentUserCookie.Expires = DateTime.Now.AddHours(1);
                    Response.SetCookie(currentUserCookie);
                }
            }
            catch (Exception)
            {
            }
        }

        [HttpPost]
        public bool VerificarCookieExiste()
        {
            try
            {
                HttpCookie currentUserCookie = Request.Cookies["webControlCookie"];
                if (currentUserCookie != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public string AlterarSenha(UserViewModel model)
        {
            try
            {
                var userSgq = db.UserSgq.Where(x => x.Id == model.Id).FirstOrDefault();

                var senhaAntiga = userSgq.Password;

                if (Guard.Descriptografar3DES(senhaAntiga).Equals(model.SenhaAntiga))
                {
                    userSgq.AlterDate = DateTime.Now;
                    userSgq.PasswordDate = DateTime.Now.AddMonths(2);
                    userSgq.Password = Guard.Criptografar3DES(model.Password);

                    db.Entry(userSgq).State = EntityState.Modified;

                    db.SaveChanges();

                    #region Criar cookie novamente

                    //create a cookie
                    HttpCookie myCookie = new HttpCookie("webControlCookie");

                    //Add key-values in the cookie
                    myCookie.Values.Add("userId", userSgq.Id.ToString());
                    myCookie.Values.Add("userName", userSgq.Name);


                    if (userSgq.PasswordDate != null)
                    {
                        myCookie.Values.Add("passwordDate", userSgq.PasswordDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                    }
                    else if (userSgq.AlterDate != null)
                    {
                        myCookie.Values.Add("alterDate", userSgq.AlterDate.GetValueOrDefault().ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        myCookie.Values.Add("alterDate", "");
                    }

                    myCookie.Values.Add("addDate", userSgq.AddDate.ToString("dd/MM/yyyy"));

                    if (userSgq.Role != null)
                        myCookie.Values.Add("roles", userSgq.Role.Replace(';', ',').ToString());//"admin, teste, operacional, 3666,344, 43434,...."
                    else
                        myCookie.Values.Add("roles", "");

                    if (userSgq.ParCompanyXUserSgq != null)
                        if (userSgq.ParCompanyXUserSgq.Any(r => r.Role != null))
                            myCookie.Values.Add("rolesCompany", string.Join(",", userSgq.ParCompanyXUserSgq.Select(n => n.Role).Distinct().ToArray()));
                        else
                            myCookie.Values.Add("rolesCompany", "");

                    //set cookie expiry date-time. Made it to last for next 12 hours.
                    myCookie.Expires = DateTime.Now.AddMinutes(60);

                    //Most important, write the cookie to client.
                    Response.Cookies.Add(myCookie);

                    #endregion

                }
                else
                {
                    return Resources.Resource.old_password_incorrect;
                }
            }
            catch (Exception)
            {
                return Resources.Resource.try_again_contact_support;
            }
            return "";
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
