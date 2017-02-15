using System.Collections.Generic;

namespace PlanoAcaoCore
{
    public interface ICrudPa<T>
    {
        List<T> Listar();
        T Salvar();
        T Update();
        void IsValid();

    }
}
