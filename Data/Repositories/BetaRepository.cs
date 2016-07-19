using Data.ComplexType;
using Dominio.Entities;
using Dominio.Interfaces.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Data.Repositories
{

    /// <summary>
    /// BetaRepository é uma Classe criada para Sprint-1 do SgqGlobal, esta classe serve como exemplo para construção das demais sprints.
    /// </summary>
    public class BetaRepository : RepositoryBase<ResultOld>, IBetaRepository
    {

        #region Construtor e atributos privados

        public BetaRepository(DbContextSgq _db)
            : base(_db)
        {
        }

        private DbSet<Operacao> _Operacao { get { return db.Set<Operacao>(); } }
        private DbSet<Monitoramento> _Monitoramento { get { return db.Set<Monitoramento>(); } }
        private DbSet<Tarefa> _Tarefa { get { return db.Set<Tarefa>(); } }

        #endregion

        #region Coleta De Dados

        /// <summary>
        /// Salva o resultado de uma coleta de dados na tabela Result old.
        /// </summary>
        /// <param name="r">ResultOld com os parametros validados.</param>
        public void Salvar(ResultOld r)
        {
            Add(r);
        }

        /// <summary>
        /// Salva uma lista de resultados de coletas de dados na tabela Result old recursivamente.
        /// </summary>
        /// <param name="list">Lista de ResultOld com os parametros validados.</param>
        public void SalvarLista(List<ResultOld> list)
        {
            AddAll(list);
        }

        #endregion

        #region Busca de Dados

        /// <summary>
        /// Busca quantidade de Não conformidades para Nivel1 agrupada por Avaliação e Amostra. No caso deste metodo, na Sprint-1 não foi contemplado Parametrização para estes dados além da descrita anteriormente.
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="dateInit"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<ResultOld> GetNcPorIndicador(int indicadorId, string dateInit, string dateEnd)
        {

            /*
             
                SELECT 
                id_operacao
                , SUM(Evaluate)"Evaluate"
                , SUM(NotConform) "NotConform"
                FROM(
                SELECT id_operacao
                , Case when sum(Evaluate) > 0 Then 1 ELSE 0 end "Evaluate" 
                , CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 END "NotConform" FROM ResultOld GROUP BY 
                numero1,
                Id_Monitoramento,
                numero2,
                Id_Operacao
                ) ind
                GROUP BY 
                Id_Operacao
            
            */

            //select CAST(getdate() as Date) >>> VERIFICAR PARA TRUNCAR DATA.

            var query = string.Format("SELECT" +
                " id_operacao, " +
                " CONVERT(DECIMAL(16,4),SUM(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4),SUM(NotConform)) 'NotConform' " +
                " FROM( select id_operacao," +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 end 'Evaluate' , " +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 end 'NotConform'" +
                " FROM ResultOld " +
                " WHERE AddDate BETWEEN  '{0} 00:00:00' AND '{1} 23:59:59' " +
                " GROUP BY " +
                " numero1," +
                " numero2," +
                " Id_Monitoramento ," +
                " Id_Operacao) ind GROUP BY Id_Operacao", dateInit, dateEnd);

            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).OrderByDescending(r => r.NotConform / r.Evaluate * 100).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        /// <summary>
        /// Busca quantidade de Não conformidades para Nivel2 agrupada por Avaliação e Amostra. No caso deste metodo, na Sprint-1 não foi contemplado Parametrização para estes dados além da descrita anteriormente.
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="dateInit"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<ResultOld> GetNcPorMonitoramento(int indicadorId, string dateInit, string dateEnd)
        {

            /*
             */

            var query = string.Format("SELECT id_operacao," +
                " Id_Monitoramento," +
                " CONVERT(DECIMAL(16,4), sum(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4), sum(NotConform)) 'NotConform' " +
                " FROM(" +
                " SELECT id_operacao, " +
                " Id_Monitoramento, " +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 END 'Evaluate'," +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 END 'NotConform' " +
                " FROM ResultOld " +
                " WHERE Id_Operacao = {0} " +
                " AND AddDate BETWEEN  '{1} 00:00:00' AND '{2} 23:59:59' " +
                " AND NotConform > 0" +
                " GROUP BY numero1, " +
                " numero2, " +
                " Id_Operacao, " +
                " Id_Monitoramento)" +
                " ind GROUP BY Id_Operacao," +
                " Id_Monitoramento" +
                " ORDER BY NotConform desc"
                , indicadorId, dateInit, dateEnd);


            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        /// <summary>
        /// Busca quantidade de Não conformidades para Nivel3. No caso deste metodo, na Sprint-1 não foi contemplado Parametrização.
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="monitoramentoId"></param>
        /// <param name="dateInit"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<ResultOld> GetNcPorTarefa(int indicadorId, int monitoramentoId, string dateInit, string dateEnd)
        {

            /*
             */

            var query = string.Format("SELECT " +
                                        "id_operacao" +
                                        ", Id_Monitoramento " +
                                        ", Id_Tarefa " +
                                        ", CONVERT(DECIMAL(16,4), SUM(Evaluate)) AS 'Evaluate'" +
                                        ", CONVERT(DECIMAL(16,4), SUM(NotConform)) AS 'NotConform'" +
                                        " FROM ResultOld" +
                                        " WHERE Id_Monitoramento = {0} AND Id_Operacao = {1}" +
                                        " AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59' " +
                                        " AND NotConform > 0" +
                                        //" AND  numero1 = (select MAX(numero1) from ResultOld where Id_Operacao = {1} AND Id_Monitoramento = {0} AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59')" +
                                        //" AND numero2 = (select MAX(numero2) from ResultOld where Id_Operacao = {1} AND Id_Monitoramento = {0} AND AddDate BETWEEN  '{2} 00:00:00' AND '{3} 23:59:59')" +
                                        " GROUP BY" +
                                        " id_operacao" +
                                        ", Id_Monitoramento" +
                                        ", Id_Tarefa" +
                                        " ORDER BY NotConform desc", monitoramentoId, indicadorId, dateInit, dateEnd);

            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        /// <summary>
        /// Busca quantidade de Não conformidades para Nivel3 somente da ultima avaliação realizada de um indicador. No caso deste metodo, na Sprint-1 não foi contemplado Parametrização. Este método foi construido e depois modificado para a funcionalidade da tela de Coleta de dados > monitoramentos: coluna Defects.
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="dateInit"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<ResultOld> GetNcPorMonitoramentoJelsafa(int indicadorId, string dateInit, string dateEnd)
        {

            /*
             */

            var query = string.Format("SELECT id_operacao," +
                " Id_Monitoramento," +
                " CONVERT(DECIMAL(16,4), sum(Evaluate)) 'Evaluate', " +
                " CONVERT(DECIMAL(16,4), sum(NotConform)) 'NotConform' " +
                " FROM(" +
                " SELECT id_operacao, " +
                " Id_Monitoramento, " +
                " CASE WHEN SUM(Evaluate) > 0 THEN 1 ELSE 0 END 'Evaluate'," +
                " CASE WHEN SUM(NotConform) > 0 THEN 1 ELSE 0 END 'NotConform' " +
                " FROM ResultOld " +
                " WHERE Id_Operacao = {0} " +
                " AND AddDate BETWEEN  '{1} 00:00:00' AND '{2} 23:59:59' " +
                //" AND NotConform > 0" +
                " GROUP BY numero1, " +
                " numero2, " +
                " Id_Operacao, " +
                " Id_Monitoramento)" +
                " ind GROUP BY Id_Operacao," +
                " Id_Monitoramento" +
                " ORDER BY NotConform desc", indicadorId, dateInit, dateEnd);


            var queryResult = db.Database.SqlQuery<RetornoQueryIndicadoresRelBate>(query).ToList();

            List<ResultOld> resultsList = RetornoQueryIndicadoresRelBateToResultOld(queryResult);

            return resultsList;
        }

        #endregion

        #region Auxiliares

        /// <summary>
        /// Metodo que faz a conversão de uma complexType para Model, devido utilização de uma query distinta de Linq e Lambda, pela complexibilidade de query, foi necessário realizar o procedimento para reaproveitar a Classe ja definida.
        /// </summary>
        /// <param name="queryResult">Resultado de uma query com headers compatíveis com: ResultOld</param>
        /// <returns></returns>
        private List<ResultOld> RetornoQueryIndicadoresRelBateToResultOld(List<RetornoQueryIndicadoresRelBate> queryResult)
        {
            var resultsList = new List<ResultOld>();

            foreach (var i in queryResult)
            {

                resultsList.Add(new ResultOld()
                {
                    Evaluate = i.Evaluate,
                    NotConform = i.NotConform,
                    Id_Operacao = i.Id_operacao,
                    Id_Monitoramento = i.Id_Monitoramento,
                    Id_Tarefa = i.Id_tarefa,
                    Operacao = db.indicadores.FirstOrDefault(r => r.Id == i.Id_operacao).Name,
                    Monitoramento = db.Monitoramentos.Where(r => r.Id == i.Id_Monitoramento).Select(r => r.Name ?? "").FirstOrDefault(),
                    Tarefa = db.Tarefas.Where(r => r.Id == i.Id_tarefa).Select(r => r.Name ?? "").FirstOrDefault()

                });
            }

            return resultsList;
        }

        /// <summary>
        /// A tabela resultados não possui no momento relacionamentos externos de Foreingin Keys, foi necessário esta validação por como foi forçado relacionamento via INTEGER o banco não verifica automáticamente.
        /// </summary>
        /// <param name="r">ResultOld Objeto.</param>
        public void ValidaFkResultado(ResultOld r)
        {
            if (_Operacao.FirstOrDefault(z => z.Id == r.Id_Operacao) == null)
                throw new ExceptionHelper("Id Invalido para Operação");

            if (_Monitoramento.FirstOrDefault(z => z.Id == r.Id_Monitoramento) == null)
                throw new ExceptionHelper("Id Invalido para Monitoramento");

            if (_Tarefa.FirstOrDefault(z => z.Id == r.Id_Tarefa) == null)
                throw new ExceptionHelper("Id Invalido para Tarefa");
        }

        #endregion

    }

  
}
