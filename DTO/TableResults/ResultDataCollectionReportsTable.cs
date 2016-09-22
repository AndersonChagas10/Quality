using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.TableResults
{
    public class ResultDataCollectionReportsTable
    {

        public string Departamento { get; set; }
        public string Level01 { get; set; }
        public string Unit { get; set; }
        public string Level02 { get; set; }
        public string Level02name { get; set; }
        public string avaliado { get; set; }
        public string Sample { get; set; }
        public string Shift { get; set; }
        public string Phase { get; set; }
        public string AddDate { get; set; }
        public string NotEvaluatedIs { get; set; }
        public string ConformedIs { get; set; }
        public string Level03Id { get; set; }
        public string Level03Name { get; set; }
        public string Value { get; set; }
        public string ValueText { get; set; }

        public ResultDataCollectionReportsTable(List<ConsolidationLevel02DTO> dbData)
        {
            //foreach (var z in dbData)
            //{
            //    foreach (var zz in z.CollectionLevel02) {

            //    }
            //}
            //foreach (var i in dbDataCollectionLevel02)
            //{
            //    foreach (var ii in i.CollectionLevel03)
            //    {
            //        Departamento = 
            //    }
            //}
        }
        public ResultDataCollectionReportsTable(List<ConsolidationLevel01DTO> dbData)
        {
            foreach (var o in dbData)
            {
                foreach (var oo in o.consolidationLevel02DTO)
                {
                    foreach (var ooo in oo.collectionLevel02DTO)
                    {
                        Departamento = o.Department.Name;

                    }
                }

            }


            //retorno['Departamento'] = o.Department.Name;
            //retorno['Level01'] = o.Level01.Name;
            //retorno['Unit'] = o.Unit.Name;

            //retorno['Level02'] = oo.Level02Id;
            //retorno['Level02name'] = oo.Name;
            //retorno['avaliado'] = oo.EvaluationNumber;
            //retorno['Sample'] = oo.Sample;
            //retorno['Shift'] = oo.Shift;
            //retorno['Phase'] = oo.Phase;
            //retorno['AddDate'] = new Date(oo.AddDate).toLocaleDateString();
            //retorno['NotEvaluatedIs'] = oo.NotEvaluatedIs;

            //retorno["ConformedIs"] = ooo.ConformedIs;
            //retorno["Level03Id"] = ooo.Level03Id;
            //retorno["Level03Name"] = ooo.Name;
            //retorno["Value"] = ooo.Value;
            //retorno["ValueText"] = ooo.ValueText;

        }

    }
}
