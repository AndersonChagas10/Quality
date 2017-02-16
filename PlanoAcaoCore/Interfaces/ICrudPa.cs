using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public interface ICrudPa<T>
    {
        //List<T> Listar();
       
        void IsValid();
        void AddOrUpdate();
    }
}
