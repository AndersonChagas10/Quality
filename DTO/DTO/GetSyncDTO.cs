using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTO.BaseEntity;

namespace DTO.DTO
{
    public class GetSyncDTO
    {
        //public List<CollectionLevel02DTO> CollectionLevel02 { get; set; }
        //public List<CollectionLevel03DTO> CollectionLevel03 { get; set; }
        public List<ConsolidationLevel01DTO> ConsolidationLevel01 { get; set; }
        //public List<ConsolidationLevel02DTO> ConsolidationLevel02 { get; set; }
        public string html { get; set; }

        public void MakeHtml()
        {

            /*<div 
                class="level01Result" 
                level01id="3" 
                unidadeid="1" 
                date="08262016" 
                datetime="08/26/2016 16:03" 
                shift="1" 
                period="1" 
                reaudit="false" 
                reauditnumber="0" 
                totalevaluate="0" 
                sidewitherros="0" 
                more3defects="0" 
                lastevaluate="0" 
                lastsample="0" 
                biasedunbiased="0" 
                evaluate="1" 
                completed="completed">
                </div>
            */
            var divMaster = "<div>";
            foreach (var i in ConsolidationLevel01.FirstOrDefault().GetType().GetProperties())
            {
                if (i.PropertyType.BaseType.Name.Equals("EntityBase"))
                    foreach (var x in i.GetType().GetProperties())
                    {
                        divMaster += x.Name + " = '" + x.GetValue(i, null) + "' ";
                    }

                divMaster += i.Name + " = '" + i.GetValue(ConsolidationLevel01.FirstOrDefault(), null) + "' ";
            }

            divMaster += "</div>";
        }
    }
}
