using ADOFactory;
using PlanoAcaoCore;
using System;

namespace PlanoDeAcaoMVC.SgqIntegracao
{
    public class ConexaoSgq : IDisposable
    {
        public Factory db;
        public ConexaoSgq() {
            db = new Factory(Conn.dataSource2, Conn.catalog2, Conn.pass2, Conn.user2);
        }

        public void Dispose()
        {
            
        }
    }
}