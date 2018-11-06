using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ListaCollectionsLevel2XMotivosAtraso
    {
        public List<CollectionLevel2XMotivoAtraso> CollectionsLevel2XMotivosAtraso { get; set; }

        public bool DadosIsValid()
        {
            if (CollectionsLevel2XMotivosAtraso.Count <= 0)
            {
                return false;
            }

            foreach (var item in CollectionsLevel2XMotivosAtraso)
            {
                if(item.CollectionLevel2_Id <= 0)
                {
                    return false;
                }

                if (item.MotivoAtraso_Id <= 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
