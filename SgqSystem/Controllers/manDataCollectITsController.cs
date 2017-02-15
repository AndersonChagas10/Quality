using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio;
using DTO.DTO.Params;
using Dominio.Interfaces.Services;
using DTO.Helpers;
using SgqSystem.Secirity;
using System;
using DTO.DTO.Manutencao;
using System.Collections.Generic;

namespace SgqSystem.Controllers
{
    [CustomAuthorize(Roles = "somentemanutencao-sgq")]
    [FilterUnit]
    public class manDataCollectITsController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        private IBaseDomain<ParCompany, ParCompanyDTO> _baseDomainParCompany;
        private IBaseDomain<ParFrequency, ParFrequencyDTO> _baseDomainParFrequency;
        private IBaseDomain<ParMeasurementUnit, ParMeasurementUnitDTO> _baseDomainParMeasurementUnit;
        private IBaseDomain<DimManutencaoColetaITs, DimManutencaoColetaITsDTO> _dimManutencaoColetaITs;

        public manDataCollectITsController(IBaseDomain<ParCompany, ParCompanyDTO> baseDomainParCompany,
                    IBaseDomain<ParFrequency, ParFrequencyDTO> baseDomainParFrequency,
                    IBaseDomain<ParMeasurementUnit, ParMeasurementUnitDTO> baseDomainParMeasurementUnit,
                    IBaseDomain<DimManutencaoColetaITs, DimManutencaoColetaITsDTO> dimManutencaoColetaITs)
        {
            _baseDomainParCompany = baseDomainParCompany;
            _baseDomainParFrequency = baseDomainParFrequency;
            _baseDomainParMeasurementUnit = baseDomainParMeasurementUnit;
            _dimManutencaoColetaITs = dimManutencaoColetaITs;

            ViewBag.listaParCompany = _baseDomainParCompany.GetAll();
            ViewBag.listaParFrequency = _baseDomainParFrequency.GetAll();
            ViewBag.listaParMeasurementUnit = _baseDomainParMeasurementUnit.GetAll();
            ViewBag.listaDataDataType = _dimManutencaoColetaITs.GetAll();

        }

        // GET: manDataCollectITs
        public ActionResult Index()
        {
            return View(db.ManDataCollectIT.ToList());
        }

        // GET: manDataCollectITs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManDataCollectIT manDataCollectIT = db.ManDataCollectIT.Find(id);
            if (manDataCollectIT == null)
            {
                return HttpNotFound();
            }
            return View(manDataCollectIT);
        }

        // GET: manDataCollectITs/Create
        public ActionResult Create()
        {
            return View(new ManDataCollectIT() { AmountData = 0 });
        }

        // POST: manDataCollectITs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, AddDate, AlterDate, ReferenceDatetime, UserSGQ_Id, ParCompany_Id, ParFrequency_Id, DimManutencaoColetaITs_Id, AmountData, IsActive, Comments")] ManDataCollectIT manDataCollectIT)
        {
            manDataCollectIT.IsActive = true;
            manDataCollectIT.UserSGQ_Id = Guard.GetUsuarioLogado_Id(HttpContext);

            if (ModelState.IsValid)
            {
                manDataCollectIT.AddDate = DateTime.Now;
                db.ManDataCollectIT.Add(manDataCollectIT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manDataCollectIT);
        }

        // GET: manDataCollectITs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManDataCollectIT manDataCollectIT = db.ManDataCollectIT.Find(id);
            if (manDataCollectIT == null)
            {
                return HttpNotFound();
            }
            return View(manDataCollectIT);
        }

        // POST: manDataCollectITs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, AddDate, AlterDate, ReferenceDatetime, UserSGQ_Id, ParCompany_Id, ParFrequency_Id, DimManutencaoColetaITs_Id, AmountData, IsActive, Comments")] ManDataCollectIT manDataCollectIT)
        {
            if (ModelState.IsValid)
            {
                manDataCollectIT.AlterDate = DateTime.Now;
                db.Entry(manDataCollectIT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manDataCollectIT);
        }

        // GET: manDataCollectITs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManDataCollectIT manDataCollectIT = db.ManDataCollectIT.Find(id);
            if (manDataCollectIT == null)
            {
                return HttpNotFound();
            }
            return View(manDataCollectIT);
        }

        // POST: manDataCollectITs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ManDataCollectIT manDataCollectIT = db.ManDataCollectIT.Find(id);
            db.ManDataCollectIT.Remove(manDataCollectIT);
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


        // GET: manDataCollectITs/Create
        [FormularioPesquisa(filtraUnidadePorUsuario = true)]
        public ActionResult Create2()
        {

            /**
             * Colocar os filtros de Query
             * 
             * SELECT DISTINCT Name FROM DimManColetaDados WHERE DimRealTarget = 'Real' and DimName is not null and name = 'M3AguaBoi_Real'
             * SELECT top 1 M3AguaBoi_Real as PerguntaIndicador FROM ManColetaDados WHERE Base_dateRef= '2017-02-14'--  -- DataDoDia
             * SELECT DISTINCT * FROM DimManColetaDados where DIMNAME is not null and name = 'Moral_CustoHorasExtrasParcial'
             * 
             **/

            //Lista com todos os indicadores
            //List<Indicador> Indicadores;

            string query = "SELECT DISTINCT DimName as Nome, Name as NomeReal FROM DimManColetaDados WHERE DimRealTarget = 'Real' and DimName is not null";

            var Indicadores = db.Database.SqlQuery<Indicador>(query).ToList();


            //Pergunta se existe o Indicador na data

            //foreach (var item in Indicadores)
            

            for (int i = 0; i < Indicadores.Count; i++)
            {
                query = "SELECT top 1 " + Indicadores[i].NomeReal + " as PerguntaIndicador FROM ManColetaDados WHERE Base_dateRef= '" + DateTime.Now.ToString("yyyy - MM - dd") + "' AND " + Indicadores[i].NomeReal + " IS NOT NULL ";

                Nullable<decimal> result = db.Database.SqlQuery<Nullable<decimal>>(query).FirstOrDefault();

                if (result != null)
                {
                    Indicadores.RemoveAt(i);
                }
            }

            ViewBag.Indicadores = Indicadores;

            return View(new ManDataCollectIT() { AmountData = 0 });
        }
    }

    public class Indicador
    {
        public string Nome { get; set; }
        public string NomeReal { get; set; }
    }

    public class Pergunta
    {
        public string PerguntaIndicador { get; set; }
    }
}
