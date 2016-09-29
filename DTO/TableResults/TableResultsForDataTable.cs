using DTO.DTO;
using System.Collections.Generic;

namespace DTO.TableResults
{
    public class TableResultsForDataTable
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
        public string Auditor { get; set; }
        public string ConformedIs { get; set; }
        public string Level03Id { get; set; }
        public string Level03Name { get; set; }
        public string Value { get; set; }
        public string ValueText { get; set; }

        public TableResultsForDataTable(List<ConsolidationLevel02DTO> dbData)
        {

            
        }

        public TableResultsForDataTable()
        {
        }

        public void RetornaCadaPertpertyEmArray<T>(List<T> obj)
        {
            foreach (var i in obj.GetType().GetProperties())
            {
                        var teste = i.Name + " = '" + i.GetValue(i, null) + "' ";

                //string[] temp = {
                //                        (DateTime.ParseExact(dataInicio, "yyyyMMdd", CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy"),
                //                        totalNc.ToString(),
                //                        totalAv.ToString(),
                //                        string.Format("{0:0.##}",totalAv > 0 ? (Convert.ToDouble(totalNc) /  Convert.ToDouble(totalAv)) * 100 : 0).Replace(",","."),
                //                      //  string.Format("{0:0.##}",(Convert.ToDouble(totalNc) /  Convert.ToDouble(totalAv)) * 100).Replace(",","."),
                //                        (DateTime.ParseExact(dataInicio, "yyyyMMdd", CultureInfo.InvariantCulture).Day).ToString(),
                //                    };

                //Array.Resize(ref final, final.Length + 1);
                //final[final.Length - 1] = temp;
            }


        }

        public List<TableResultsForDataTable> DataCollectionReportsProcessedResults(List<ConsolidationLevel01DTO> dbData)
        {
            var ListaResultDataCollectionReportsTable = new List<TableResultsForDataTable>();

            foreach (var conslL1 in dbData)
            {
                foreach (var conslL2 in conslL1.ConsolidationLevel02)
                {
                    foreach (var level02 in conslL2.CollectionLevel02)
                    {
                        foreach (var level03 in level02.CollectionLevel03)
                        {
                            var resultSet = new TableResultsForDataTable()
                            {
                                Departamento = conslL1.Department.Name,
                                Level01 = conslL1.Level01.Name,
                                Unit = conslL1.Unit.Name,

                                //Level02 = conslL2.Level02.Id.ToString(),
                                Level02name = conslL2.Level02.Name,
                                avaliado = level02.EvaluationNumber.ToString(),
                                Sample = level02.Sample.ToString(),
                                Shift = level02.Shift.ToString(),
                                Phase = level02.Phase.ToString(),
                                AddDate = level02.AddDate.ToString("MM/dd/yyyy"),
                                NotEvaluatedIs = level02.NotEvaluatedIs ? "Not Evaluated" : "Evaluated",
                                Auditor = level02.UserSgq.Name,

                                ConformedIs = level03.ConformedIs ? "Conform" : "Not Conform",
                                //Level03Id = level03.Id.ToString(),
                                Level03Name = level03.Level03.Name,
                                Value = string.Format("{0:N2}", level03.Value),
                                ValueText = level03.ValueText
                            };


                            ListaResultDataCollectionReportsTable.Add(resultSet);
                        }
                    }
                }

            }
            return ListaResultDataCollectionReportsTable;

        }



    }
}
