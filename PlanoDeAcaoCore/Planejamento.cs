using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanoDeAcaoCore
{
    public class Planejamento : ICrudPa<Planejamento>
    {
       public int Id { get; set; }
       public DateTime AddDate { get; set; }
       public DateTime AlterDate { get; set; }
       public int Diretoria_Id { get; set; }
       public int Gerencia_Id { get; set; }
       public int Coordenacao_Id { get; set; }
       public int Missao_Id { get; set; }
       public int Visao_Id { get; set; }
       public int TemaAssunto_Id { get; set; }
       public int Indicadores_Id { get; set; }
       public int Iniciativa_Id { get; set; }
       public int ObjetivoGerencial_Id { get; set; }
       public string Dimensao { get; set; }
       public string Objetivo { get; set; }
       public decimal ValorDe { get; set; }
       public decimal ValorPara { get; set; }
       public DateTime DataInicio { get; set; }
       public DateTime DataFim { get; set; }

        public void IsValid()
        {
            throw new NotImplementedException();
        }

        public List<Planejamento> Listar()
        {
            IsValid();
            using (var db = new FactoryADO(""))
            {

            }
        }

        public Planejamento Salvar()
        {
            IsValid();
            throw new NotImplementedException();
        }

        public Planejamento Update()
        {
            IsValid();
            throw new NotImplementedException();
        }
    }
}
