using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ListaCollectionsLevel2XMotivosAtraso
    {
        public List<CollectionLevel2XParReason> CollectionsLevel2XMotivosAtraso { get; set; }

        public bool DadosIsValid()
        {
            if (CollectionsLevel2XMotivosAtraso.Count <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
