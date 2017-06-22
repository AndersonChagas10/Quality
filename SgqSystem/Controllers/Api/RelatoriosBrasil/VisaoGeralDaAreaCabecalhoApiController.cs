using Dominio;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.RelatoriosBrasil
{
    [RoutePrefix("api/VisaoGeralDaAreaCabecalho")]
    public class VisaoGeralDaAreaCabecalhoApiController : ApiController
    {

        private List<Generic> _return { get; set; }

        [HttpPost]
        [Route("Grafico1")]
        public List<Generic> Grafico1([FromBody] DataCarrierFormulario form)
        {
            CriaMockG1(form);
            return _return;
            //return _list;
        }

        [HttpPost]
        [Route("Grafico2")]
        public List<Generic> Grafico2([FromBody] DataCarrierFormulario form)
        {
            CriaMockG2(form);
            //return _mock;
            return _return;
        }

        [HttpPost]
        [Route("Grafico3")]
        public List<Generic> Grafico3([FromBody] DataCarrierFormulario form)
        {
            CriaMockG3(form);
            return _return;
        }

        [HttpPost]
        [Route("Grafico4")]
        public List<Generic> Grafico4([FromBody] DataCarrierFormulario form)
        {
            CriaMockG4(form);
            return _return;
        }

        [HttpPost]
        [Route("Grafico5")]
        public List<Generic> Grafico5([FromBody] DataCarrierFormulario form)
        {
            CriaMockG5(form);
            return _return;
        }

        private void CriaMockG1(DataCarrierFormulario form)
        {
            //_mock = new List<Obra>();

            //_mock.Add(new Obra()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    obraNome = "Ytoara"
            //});

            //_mock.Add(new Obra()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    obraNome = "Castro Alves"
            //});

            try
            {
                string query = "select " +
                "\n pc.Name as 'nome'" +
                "\n , pc.Id as 'regId'" +
                "\n ,count(*) as 'folhasVerificadas'" +
                "\n ,(sum(case when Defects <> 0 then 1 else 0 end) * 100) / count(*) as 'porNC'" +
                "\n ,sum(case when Defects = 0 then 1 else 0 end) as 'conforme'" +
                "\n ,sum(case when Defects <> 0 then 1 else 0 end) as 'nconforme'" +
                "\n ,0 as 'meta'" +
                "\n from CollectionLevel2 cl2" +
                "\n LEFT JOIN ParCompany pc on cl2.UnitId = pc.Id" +
                "\n GROUP BY pc.Name, pc.Id";

                using (var db = new SgqDbDevEntities())
                {
                    _return = db.Database.SqlQuery<Generic>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void CriaMockG2(DataCarrierFormulario form)
        {
            //_mock2 = new List<Torre>();

            //_mock2.Add(new Torre()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    torreNome = "Torre 1"
            //});

            //_mock2.Add(new Torre()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    torreNome = "Torre 2"
            //});

            try
            {

                var query =
                "select " +
                "\n pmv.Name as 'nome'" +
                "\n ,count(*) as 'folhasVerificadas'" +
                "\n ,(sum(case when cl2.Defects <> 0 then 1 else 0 end) * 100) / count(*) as 'porNC'" +
                "\n ,sum(case when cl2.Defects = 0 then 1 else 0 end) as 'conforme'" +
                "\n ,sum(case when cl2.Defects <> 0 then 1 else 0 end) as 'nconforme'" +
                "\n ,0 as 'meta'" +
                "\n from CollectionLevel2 cl2" +
                "\n LEFT JOIN CollectionLevel2XParHeaderField cl2xphf On cl2xphf.CollectionLevel2_Id = cl2.Id" +
                "\n LEFT JOIN ParHeaderField phf on phf.Id = cl2xphf.ParHeaderField_Id" +
                "\n LEFT JOIN ParMultipleValues PMV (nolock)on cl2xphf.Value = cast(PMV.Id as varchar(500))" +
                "\n Where cl2.UnitId = 1 and phf.Name = 'TORRE / PERIFERIA'" +
                "\n GROUP BY pmv.Name";

                using (var db = new SgqDbDevEntities())
                {
                    _return = db.Database.SqlQuery<Generic>(query).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CriaMockG3(DataCarrierFormulario form)
        {
            //_mock3 = new List<Pavimento>();

            //_mock3.Add(new Pavimento()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    pavimentoNome = "PAV 1"
            //});

            //_mock3.Add(new Pavimento()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    pavimentoNome = "PAV 2"
            //});

            try
            {

                var query =
                "select " +
                "\n pmv.Name as 'nome'" +
                "\n ,count(*) as 'folhasVerificadas'" +
                "\n ,(sum(case when cl2.Defects <> 0 then 1 else 0 end) * 100) / count(*) as 'porNC'" +
                "\n ,sum(case when cl2.Defects = 0 then 1 else 0 end) as 'conforme'" +
                "\n ,sum(case when cl2.Defects <> 0 then 1 else 0 end) as 'nconforme'" +
                "\n ,0 as 'meta'" +
                "\n from CollectionLevel2 cl2" +
                "\n LEFT JOIN CollectionLevel2XParHeaderField cl2xphf On cl2xphf.CollectionLevel2_Id = cl2.Id" +
                "\n LEFT JOIN ParHeaderField phf on phf.Id = cl2xphf.ParHeaderField_Id" +
                "\n LEFT JOIN ParMultipleValues PMV (nolock)on cl2xphf.Value = cast(PMV.Id as varchar(500))" +
                "\n Where cl2.UnitId = 1 and phf.Name = 'PAVIMENTO TIPO'" +
                "\n GROUP BY pmv.Name";

                using (var db = new SgqDbDevEntities())
                {
                    _return = db.Database.SqlQuery<Generic>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void CriaMockG4(DataCarrierFormulario form)
        {
            //_mock4 = new List<Apartamento>();

            //_mock4.Add(new Apartamento()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    apartamentoNome = "Apartamento 1"
            //});

            //_mock4.Add(new Apartamento()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    apartamentoNome = "Apartamento 2"
            //});

            try
            {

                var query =
                "select " +
                "\n pmv.Name as 'nome'" +
                "\n ,count(*) as 'folhasVerificadas'" +
                "\n ,(sum(case when cl2.Defects <> 0 then 1 else 0 end) * 100) / count(*) as 'porNC'" +
                "\n ,sum(case when cl2.Defects = 0 then 1 else 0 end) as 'conforme'" +
                "\n ,sum(case when cl2.Defects <> 0 then 1 else 0 end) as 'nconforme'" +
                "\n ,0 as 'meta'" +
                "\n from CollectionLevel2 cl2" +
                "\n LEFT JOIN CollectionLevel2XParHeaderField cl2xphf On cl2xphf.CollectionLevel2_Id = cl2.Id" +
                "\n LEFT JOIN ParHeaderField phf on phf.Id = cl2xphf.ParHeaderField_Id" +
                "\n LEFT JOIN ParMultipleValues PMV (nolock)on cl2xphf.Value = cast(PMV.Id as varchar(500))" +
                "\n Where cl2.UnitId = 1 and phf.Name = 'APARTAMENTO / ÁREA'" +
                "\n GROUP BY pmv.Name";

                using (var db = new SgqDbDevEntities())
                {
                    _return = db.Database.SqlQuery<Generic>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CriaMockG5(DataCarrierFormulario form)
        {
            //_mock5 = new List<Comodo>();

            //_mock5.Add(new Comodo()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    comodoNome = "Banheiro 1"
            //});

            //_mock5.Add(new Comodo()
            //{
            //    folhasVerificadas = 100M,
            //    porNC = 90M,
            //    conforme = 70M,
            //    nconforme = 20M,
            //    meta = 40M,
            //    comodoNome = "Banheiro 2"
            //});

            try
            {
                var query =
                "select " +
                "\n pmv.Name as 'nome'" +
                "\n ,count(*) as 'folhasVerificadas'" +
                "\n ,(sum(case when cl2.Defects <> 0 then 1 else 0 end) * 100) / count(*) as 'porNC'" +
                "\n ,sum(case when cl2.Defects = 0 then 1 else 0 end) as 'conforme'" +
                "\n ,sum(case when cl2.Defects <> 0 then 1 else 0 end) as 'nconforme'" +
                "\n ,0 as 'meta'" +
                "\n from CollectionLevel2 cl2" +
                "\n LEFT JOIN CollectionLevel2XParHeaderField cl2xphf On cl2xphf.CollectionLevel2_Id = cl2.Id" +
                "\n LEFT JOIN ParHeaderField phf on phf.Id = cl2xphf.ParHeaderField_Id" +
                "\n LEFT JOIN ParMultipleValues PMV (nolock)on cl2xphf.Value = cast(PMV.Id as varchar(500))" +
                "\n Where cl2.UnitId = 1 and phf.Name = 'CÔMODOS'" +
                "\n GROUP BY pmv.Name";

                using (var db = new SgqDbDevEntities())
                {
                    _return = db.Database.SqlQuery<Generic>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Generic
    {
        public int folhasVerificadas { get; set; }
        public int porNC { get; set; }
        public int conforme { get; set; }
        public int meta { get; set; }
        public int nconforme { get; set; }
        public string nome { get; set; }
        public int regId { get; set; }
    }
}
