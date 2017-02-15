using System.Collections.Generic;

namespace PlanoDeAcaoCore
{
    public interface ICrudPa<T>
    {
        List<T> Listar();
        T Salvar();
        T Update();
        void IsValid();

    }
}
