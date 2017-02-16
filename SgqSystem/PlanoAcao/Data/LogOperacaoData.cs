using SgqSystem.PlanoAcao.Model;
using System.Data.Entity;

namespace PA.Data
{
    public class LogOperacaoData
    {

        #region LOG OPERAÇÃO

        public void SalvarLogOperacao(LogOperacaoPA log)
        {
            using (var db = new SgqSystem.PlanoAcao.Model.Entities())
            {
                db.LogOperacaoPA.Add(log);
                db.Entry(log).State = EntityState.Added;
                db.SaveChanges();

                db.Dispose();
            }
        }

        #endregion


    }
}
