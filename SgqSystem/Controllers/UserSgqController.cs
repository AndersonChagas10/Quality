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

namespace SgqSystem.Controllers
{
    public class UserSgqController : Controller
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        private IBaseDomain<ParCompany, ParCompanyDTO> _baseDomainParCompany;
        private IBaseDomain<UserSgq, UserSgqDTO> _baseDomainUserSgq;

        public UserSgqController(IBaseDomain<ParCompany, ParCompanyDTO> baseDomainParCompany,
            IBaseDomain<UserSgq, UserSgqDTO> baseDomainUserSgq)
        {
            _baseDomainParCompany = baseDomainParCompany;
            _baseDomainUserSgq = baseDomainUserSgq;

            ViewBag.listaParCompany = _baseDomainParCompany.GetAll();
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
                _baseDomainUserSgq.AddOrUpdate(userSgqDto);
            }
            else
            {
                userSgqDto.AlterDate = DateTime.Now;

                if(userSgqDto.Password == null)
                {
                    UserSgq dummy = db.UserSgq.Find(userSgqDto.Id);
                    userSgqDto.Password = dummy.Password;
                }               
            }
            _baseDomainUserSgq.AddOrUpdate(userSgqDto);

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
