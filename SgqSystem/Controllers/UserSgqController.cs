using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using DTO.DTO;
using AutoMapper;
using System.Collections.Generic;

namespace SgqSystem.Controllers
{
    public class UserSgqController : BaseController
    {
        [HandleController()]
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
