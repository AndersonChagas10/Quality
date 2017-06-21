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

        private List<Obra> _mock { get; set; }
        private List<Torre> _mock2 { get; set; }
        private List<Pavimento> _mock3 { get; set; }
        private List<Apartamento> _mock4 { get; set; }
        private List<Comodo> _mock5 { get; set; }

        [HttpPost]
        [Route("Grafico1")]
        public List<Obra> Grafico1([FromBody] DataCarrierFormulario form)
        {
            CriaMockG1(form);
            return _mock;
            //return _list;
        }

        [HttpPost]
        [Route("Grafico2")]
        public List<Torre> Grafico2([FromBody] DataCarrierFormulario form)
        {
            CriaMockG2(form);
            //return _mock;
            return _mock2;
        }

        [HttpPost]
        [Route("Grafico3")]
        public List<Pavimento> Grafico3([FromBody] DataCarrierFormulario form)
        {
            CriaMockG3(form);
            return _mock3;
        }

        [HttpPost]
        [Route("Grafico4")]
        public List<Apartamento> Grafico4([FromBody] DataCarrierFormulario form)
        {
            CriaMockG4(form);
            return _mock4;
        }

        [HttpPost]
        [Route("Grafico5")]
        public List<Comodo> Grafico5([FromBody] DataCarrierFormulario form)
        {
            CriaMockG5(form);
            return _mock5;
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
                "\n pc.Name as 'obraNome'" +
                "\n ,count(*) as 'folhasVerificadas'" +
                "\n ,(sum(case when Defects <> 0 then 1 else 0 end) * 100) / count(*) as 'porNC'" +
                "\n ,sum(case when Defects = 0 then 1 else 0 end) as 'conforme'" +
                "\n ,sum(case when Defects <> 0 then 1 else 0 end) as 'nconforme'" +
                "\n ,0 as 'meta'" +
                "\n from CollectionLevel2 cl2" +
                "\n LEFT JOIN ParCompany pc on cl2.UnitId = pc.Id" +
                "\n GROUP BY pc.Name";


                using (var db = new SgqDbDevEntities())
                {
                    _mock = db.Database.SqlQuery<Obra>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        private void CriaMockG2(DataCarrierFormulario form)
        {
            _mock2 = new List<Torre>();

            _mock2.Add(new Torre()
            {
                folhasVerificadas = 100M,
                porNC = 90M,
                conforme = 70M,
                nconforme = 20M,
                meta = 40M,
                torreNome = "Torre 1"
            });

            _mock2.Add(new Torre()
            {
                folhasVerificadas = 100M,
                porNC = 90M,
                conforme = 70M,
                nconforme = 20M,
                meta = 40M,
                torreNome = "Torre 2"
            });
        }

        private void CriaMockG3(DataCarrierFormulario form)
        {
            _mock3 = new List<Pavimento>();

            _mock3.Add(new Pavimento()
            {
                folhasVerificadas = 100M,
                porNC = 90M,
                conforme = 70M,
                nconforme = 20M,
                meta = 40M,
                pavimentoNome = "PAV 1"
            });

            _mock3.Add(new Pavimento()
            {
                folhasVerificadas = 100M,
                porNC = 90M,
                conforme = 70M,
                nconforme = 20M,
                meta = 40M,
                pavimentoNome = "PAV 2"
            });
        }

        private void CriaMockG4(DataCarrierFormulario form)
        {
            _mock4 = new List<Apartamento>();

            _mock4.Add(new Apartamento()
            {
                folhasVerificadas = 100M,
                porNC = 90M,
                conforme = 70M,
                nconforme = 20M,
                meta = 40M,
                apartamentoNome = "Apartamento 1"
            });

            _mock4.Add(new Apartamento()
            {
                folhasVerificadas = 100M,
                porNC = 90M,
                conforme = 70M,
                nconforme = 20M,
                meta = 40M,
                apartamentoNome = "Apartamento 2"
            });
        }

        private void CriaMockG5(DataCarrierFormulario form)
        {
            _mock5 = new List<Comodo>();

            _mock5.Add(new Comodo()
            {
                folhasVerificadas = 100M,
                porNC = 90M,
                conforme = 70M,
                nconforme = 20M,
                meta = 40M,
                comodoNome = "Banheiro 1"
            });

            _mock5.Add(new Comodo()
            {
                folhasVerificadas = 100M,
                porNC = 90M,
                conforme = 70M,
                nconforme = 20M,
                meta = 40M,
                comodoNome = "Banheiro 2"
            });
        }
    }

    public class Obra
    {
        public int folhasVerificadas { get; set; }
        public int porNC { get; set; }
        public int conforme { get; set; }
        public int meta { get; set; }
        public int nconforme { get; set; }
        public string obraNome { get; set; }
    }

    public class Torre
    {
        public decimal folhasVerificadas { get; set; }
        public decimal porNC { get; set; }
        public decimal conforme { get; set; }
        public decimal nconforme { get; set; }
        public decimal meta { get; set; }
        public string torreNome { get; set; }
    }

    public class Pavimento
    {
        public decimal folhasVerificadas { get; set; }
        public decimal porNC { get; set; }
        public decimal conforme { get; set; }
        public decimal nconforme { get; set; }
        public decimal meta { get; set; }
        public string pavimentoNome { get; set; }
    }

    public class Apartamento
    {
        public decimal folhasVerificadas { get; set; }
        public decimal porNC { get; set; }
        public decimal conforme { get; set; }
        public decimal nconforme { get; set; }
        public decimal meta { get; set; }
        public string apartamentoNome { get; set; }
    }

    public class Comodo
    {
        public decimal folhasVerificadas { get; set; }
        public decimal porNC { get; set; }
        public decimal conforme { get; set; }
        public decimal nconforme { get; set; }
        public decimal meta { get; set; }
        public string comodoNome { get; set; }
    }
}
