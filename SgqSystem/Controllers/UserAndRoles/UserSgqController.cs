using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.DTO.Params;
using DTO.Helpers;
using Helper;
using SgqSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SgqSystem.Controllers
{
    [CustomAuthorize]
    public class UserSgqController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        private IBaseDomain<ParCompany, ParCompanyDTO> _baseDomainParCompany;
        private IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> _baseDomainParCompanyXUserSgq;
        private IBaseDomain<UserSgq, UserDTO> _baseDomainUserSgq;
        private IBaseDomain<RoleUserSgq, RoleUserSgqDTO> _baseDomainRoleUserSGQ;

        public UserSgqController(IBaseDomain<ParCompany, ParCompanyDTO> baseDomainParCompany,
            IBaseDomain<ParCompanyXUserSgq, ParCompanyXUserSgqDTO> baseDomainParCompanyXUserSgq,
            IBaseDomain<UserSgq, UserDTO> baseDomainUserSgq,
            IBaseDomain<RoleUserSgq, RoleUserSgqDTO> baseDomainRoleUserSGQ
            )
        {
            _baseDomainParCompany = baseDomainParCompany;
            _baseDomainUserSgq = baseDomainUserSgq;
            _baseDomainParCompanyXUserSgq = baseDomainParCompanyXUserSgq;
            _baseDomainRoleUserSGQ = baseDomainRoleUserSGQ;

            ViewBag.listaParCompany = _baseDomainParCompany.GetAll();
            var listaRoleSGQ = _baseDomainRoleUserSGQ.GetAll();

            foreach (var roleSgq in listaRoleSGQ)
                roleSgq.Name = roleSgq.Name.Trim();

            ViewBag.listaRoleSGQ = listaRoleSGQ;
        }

        // GET: UserSgq
        public ActionResult Index()
        {
            return View(_baseDomainUserSgq.GetAllNoLazyLoad().ToList());
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
                Roles = userSgq.Role == null ? new string[0] : userSgq.Role.Split(','),
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
        public bool Save(UserDTO userSgqDto)
        {
            
            ValidaUserSgqDto(userSgqDto);

            if (!ModelState.IsValid)
                return false;

            //Se for Estados Unidos ou Canada e for um novo usuário
            if ((DTO.GlobalConfig.Eua == true || DTO.GlobalConfig.Canada == true) && userSgqDto.Id == 0)
            {
                if ((userSgqDto.Password == "") || (userSgqDto.Password == null))
                    userSgqDto.Password = "USERUSA";
            }

            userSgqDto.ParCompany_Id = userSgqDto.ListParCompany_Id.FirstOrDefault();
            if (userSgqDto.Id == 0)
            {
                userSgqDto.AddDate = DateTime.Now;
                userSgqDto.Password = Guard.EncryptStringAES(userSgqDto.Password);
            }
            else
            {
                userSgqDto.AlterDate = DateTime.Now;

                if (userSgqDto.Password == null)
                {
                    UserSgq dummy = db.UserSgq.Find(userSgqDto.Id);
                    userSgqDto.Password = dummy.Password;
                }
                else
                {
                    userSgqDto.Password = Guard.EncryptStringAES(userSgqDto.Password);
                }
            }

            /*Roles*/
            if (userSgqDto.ListRole != null)
            {
                string roles = string.Join(",", userSgqDto.ListRole);
                userSgqDto.Role = roles;
            }
            var ListParCompany_Id = userSgqDto.ListParCompany_Id;
            userSgqDto = _baseDomainUserSgq.AddOrUpdate(userSgqDto, true);

            ///*Empresas do usuario*/
            _baseDomainParCompanyXUserSgq.ExecuteSql("DELETE FROM ParCompanyXUserSgq WHERE UserSgq_Id = " + userSgqDto.Id);
            if(ListParCompany_Id != null)
                foreach (int ParCompany_id in ListParCompany_Id)
                    _baseDomainParCompanyXUserSgq.AddOrUpdate(new ParCompanyXUserSgqDTO() { Id = 0, UserSgq_Id = userSgqDto.Id, ParCompany_Id = ParCompany_id }, true);

            return true;

        }

        // GET: UserSgq/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDTO userSgq = _baseDomainUserSgq.GetById(id.GetValueOrDefault());
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

                if (Guard.DecryptStringAES(senhaAntiga).Equals(model.SenhaAntiga))
                {
                    userSgq.AlterDate = DateTime.Now;
                    userSgq.PasswordDate = DateTime.Now.AddMonths(2);
                    userSgq.Password = Guard.EncryptStringAES(model.Password);

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

        private void ValidaUserSgqDto(UserDTO userSgqDto)
        {
            if (userSgqDto.Id > 0)
            {
                if (db.UserSgq.Where(r => r.Name == userSgqDto.Name && r.Id != userSgqDto.Id).ToList().Count() > 0)
                    ModelState.AddModelError("Name", Resources.Resource.repeated_username);
            }
            else if (db.UserSgq.Where(r => r.Name == userSgqDto.Name).ToList().Count() > 0)
            {
                ModelState.AddModelError("Name", Resources.Resource.repeated_username);
            }
        }
    }
}
