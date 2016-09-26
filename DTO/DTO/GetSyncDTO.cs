using DTO.TableResults;
using System.Collections.Generic;
using System.Linq;

namespace DTO.DTO
{
    public class GetSyncDTO
    {
        public List<TableResultsForDataTable> ConsolidationLevel01 { get; set; }
        public string html { get; set; }

        public void MakeHtml()
        {
            var divMaster = "<div>";
            if (ConsolidationLevel01.FirstOrDefault() != null)
            {
                foreach (var i in ConsolidationLevel01.FirstOrDefault().GetType().GetProperties())
                {
                    if (i.PropertyType.BaseType.Name.Equals("EntityBase"))
                        foreach (var x in i.GetType().GetProperties())
                        {
                            divMaster += x.Name + " = '" + x.GetValue(i, null) + "' ";
                        }

                    divMaster += i.Name + " = '" + i.GetValue(ConsolidationLevel01.FirstOrDefault(), null) + "' ";
                }
            }
            divMaster += "</div>";
        }
    }
}
