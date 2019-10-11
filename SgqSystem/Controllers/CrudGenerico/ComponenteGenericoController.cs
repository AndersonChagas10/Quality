using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dominio;
using Dominio.AppViewModel;

namespace SgqSystem.Controllers
{
    public class ComponenteGenericoController : BaseController
    {
        private SgqDbDevEntities db = new SgqDbDevEntities();

        // GET: ComponenteGenerico
        public ActionResult Index()
        {
            return View(db.ComponenteGenerico.ToList());
        }

        // GET: ComponenteGenerico/Create
        public ActionResult Edit(int? id)
        {
            var componenteGenerico = new ComponenteGenericoViewModel();
            componenteGenerico.ComponentesGenericosColuna = new List<ComponenteGenericoColuna>();

            if (id != null && id > 0)
            {
                componenteGenerico.ComponenteGenerico = db.ComponenteGenerico.Find(id);
                componenteGenerico.ComponentesGenericosColuna = db.ComponenteGenericoColuna.Include("ComponenteGenericoTipoColuna").Where(x => x.ComponenteGenerico_Id == id).ToList();
            }

            ViewBag.ComponentesGenericosTipoColuna = db.ComponenteGenericoTipoColuna.Where(x => x.IsActive).ToList();

            return View(componenteGenerico);
        }

        // POST: ComponenteGenerico/Create
        [HttpPost]
        public ActionResult Edit(ComponenteGenericoViewModel collection)
        {
            try
            {

                SaveOrUpdateComponenteGenerico(collection.ComponenteGenerico);
                SaveOrUpdateComponenteGenericoColuna(collection);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private ComponenteGenerico SaveOrUpdateComponenteGenerico(ComponenteGenerico componenteGenerico)
        {
            using (var db = new SgqDbDevEntities())
            {

                if (componenteGenerico.Id <= 0)
                {
                    componenteGenerico.AddDate = DateTime.Now;
                    db.ComponenteGenerico.Add(componenteGenerico);

                }
                else
                {
                    componenteGenerico.AlterDate = DateTime.Now;
                    db.Entry(componenteGenerico).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();
            }

            return componenteGenerico;
        }

        private List<ComponenteGenericoColuna> SaveOrUpdateComponenteGenericoColuna(ComponenteGenericoViewModel collection)
        {
            using (var db = new SgqDbDevEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                foreach (var componenteGenericoColuna in collection.ComponentesGenericosColuna)
                {
                    componenteGenericoColuna.ComponenteGenerico = null;
                    componenteGenericoColuna.ComponenteGenericoTipoColuna = null;

                    if (componenteGenericoColuna.Id > 0 && componenteGenericoColuna.ComponenteGenerico_Id > 0)
                    {
                        componenteGenericoColuna.ComponenteGenerico_Id = collection.ComponenteGenerico.Id;
                        componenteGenericoColuna.AlterDate = DateTime.Now;
                        db.Entry(componenteGenericoColuna).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        componenteGenericoColuna.ComponenteGenerico_Id = collection.ComponenteGenerico.Id;
                        componenteGenericoColuna.AddDate = DateTime.Now;
                        db.ComponenteGenericoColuna.Add(componenteGenericoColuna);
                    }
                }

                db.SaveChanges();
            }

            return collection.ComponentesGenericosColuna;
        }

        //Componente Generico Valor
        public ActionResult List(int id)
        {
            var retorno = new ComponenteGenericoValorViewModel();

            var colunas = db.ComponenteGenericoColuna.Where(x => x.IsActive && x.ComponenteGenerico_Id == id).ToList();
            var dados = db.ComponenteGenericoValor.Where(x => x.ComponenteGenerico_Id == id && x.IsActive).ToList();

            retorno.Colunas = colunas;
            retorno.Valores = dados;
            retorno.ComponenteGenerico = db.ComponenteGenerico.Find(id);

            return View(retorno);
        }

        public ActionResult EditValor(int id, int? idValor)
        {
            var retorno = new ComponenteGenericoValorViewModel();

            var colunas = db.ComponenteGenericoColuna.Where(x => x.IsActive && x.ComponenteGenerico_Id == id).ToList();
            var dados = db.ComponenteGenericoValor.Where(x => x.ComponenteGenerico_Id == id && x.SaveId == idValor && x.IsActive).ToList();

            retorno.Colunas = colunas;
            retorno.Valores = dados;
            retorno.ComponenteGenerico = db.ComponenteGenerico.Find(id);

            db.Configuration.LazyLoadingEnabled = false;
            ViewBag.Level3 = db.ParLevel3.Where(x => x.IsActive).ToList();

            retorno.Tabelas = getTabelasVinculadas(colunas);

            return View(retorno);
        }

        [HttpPost]
        public ActionResult EditValor(ComponenteGenericoValorViewModel componenteGenericoValores)
        {

            try
            {
                SaveOrUpdateComponenteGenericoValor(componenteGenericoValores.Valores);

                return RedirectToAction("List", new { id = componenteGenericoValores.ComponenteGenerico.Id });
            }
            catch (Exception ex)
            {
                if (componenteGenericoValores.Valores.FirstOrDefault().SaveId == 0)
                    return RedirectToAction("EditValor", new { id = componenteGenericoValores.ComponenteGenerico.Id });

                else
                    return RedirectToAction("EditValor", new { id = componenteGenericoValores.ComponenteGenerico.Id, idValor = componenteGenericoValores.Valores.FirstOrDefault().SaveId });

            }
        }

        private List<ComponenteGenericoValor> SaveOrUpdateComponenteGenericoValor(List<ComponenteGenericoValor> componenteGenericoValores)
        {

            int hash = 0;

            foreach (var componenteGenericoValor in componenteGenericoValores)
            {

                componenteGenericoValor.ComponenteGenerico = null;
                componenteGenericoValor.ComponenteGenericoColuna = null;

                if (componenteGenericoValor.Id == 0)//ADD
                {

                    componenteGenericoValor.AddDate = DateTime.Now;

                    if (hash == 0)
                        hash = DateTime.Now.GetHashCode();

                    componenteGenericoValor.IsActive = true;
                    if(componenteGenericoValor.SaveId == 0)
                        componenteGenericoValor.SaveId = hash;

                    db.ComponenteGenericoValor.Add(componenteGenericoValor);

                }
                else //update
                {
                    componenteGenericoValor.AlterDate = DateTime.Now;
                    componenteGenericoValor.IsActive = true;
                    db.Entry(componenteGenericoValor).State = System.Data.Entity.EntityState.Modified;
                }
            }

            db.SaveChanges();

            return componenteGenericoValores;
        }

        [HttpPost]
        public bool DeleteValor(int id, int? idValor)
        {
            try
            {
                var dados = db.ComponenteGenericoValor.Where(x => x.IsActive && x.SaveId == idValor && x.ComponenteGenerico_Id == id).ToList();
                dados.ForEach(x => x.IsActive = false);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private List<TabelaVinculada> getTabelasVinculadas(List<ComponenteGenericoColuna> colunas)
        {
            var tabelasVinculadas = new List<TabelaVinculada>();

            foreach (var coluna in colunas)
            {

                if (coluna.ComponenteGenericoTipoColuna_Id == 9)
                {
                    var valores = getValoresColunas(coluna);

                    if (valores.Count > 0)
                    {
                        var tabela = new TabelaVinculada();
                        tabela.Tabela = coluna.Name;
                        tabela.Select = valores;
                        tabelasVinculadas.Add(tabela);
                    }

                }
            }

            return tabelasVinculadas;

        }

        private List<GenericSelect> getValoresColunas(ComponenteGenericoColuna coluna)
        {

            var valores = new List<GenericSelect>();

            string[] TabelaVinculo;

            try
            {
                TabelaVinculo = coluna.TabelaVinculo.Split(':');
            }
            catch (Exception ex)
            {
                return valores;
            }

            if (TabelaVinculo == null || TabelaVinculo.Length < 3)
            {
                return valores;
            }

            var nomeTabela = TabelaVinculo[0];
            var value = TabelaVinculo[1];
            var text = TabelaVinculo[2];

            var sql = $@"SELECT CAST({value} as Varchar(MAX)) as Value, CAST({text} as Varchar(MAX)) as Text FROM {nomeTabela}";

            try
            {
                valores = db.Database.SqlQuery<GenericSelect>(sql).ToList();
            }
            catch (Exception ex)
            {
                return valores;
            }

            return valores;

        }
    }
}
