using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace Dominio.Services
{
    /// <summary>
    /// Classe que cuida da Inserção de Dados a Nível de Regra de Negóciode da Coleta do SgqGlobal.
    /// </summary>
    public class SaveConsolidateDataCollectionDomain : ISaveConsolidateDataCollectionDomain
    {

        #region Variaveis

        private IBaseRepository<ConsolidationLevel01> _baseRepoConsolidationL1;
        private IBaseRepository<ConsolidationLevel02> _baseRepoConsolidationL2;
        private IBaseRepository<CollectionLevel02> _baseRepoCollectionL2;
        private ICollectionLevel02Repo _collectionLevel02Repo;
        private IBaseRepository<CollectionLevel03> _baseRepoCollectionL3;
        private IBaseRepository<CollectionHtml> _baseRepoCollectionHtml;
        private IBaseRepository<CorrectiveAction> _baseRepoCorrectiveAction;
        private IGetDataResultRepository<ConsolidationLevel01> _consolidationLevel01RepositoryGET;
        private IGetDataResultRepository<ConsolidationLevel02> _consolidationLevel02RepositoryGET;
        private IGetDataResultRepository<CollectionLevel02> _collectionLevel02RepositoryGET;
        private IGetDataResultRepository<CollectionLevel03> _collectionLevel03RepositoryGET;
        private IGetDataResultRepository<CollectionHtml> _baseRepoCollectionHtmlGET;

        #endregion

        #region Construtor

        public SaveConsolidateDataCollectionDomain(
            IBaseRepository<ConsolidationLevel01> baseRepoConsolidationL1,
            IBaseRepository<ConsolidationLevel02> baseRepoConsolidationL2,
            IBaseRepository<CollectionLevel02> baseRepoCollectionL2,
            IBaseRepository<CollectionLevel03> baseRepoCollectionL3,
            IBaseRepository<CollectionHtml> baseRepoCollectionHtml,
            IBaseRepository<CorrectiveAction> baseRepoCorrectiveAction,
            IGetDataResultRepository<ConsolidationLevel01> consolidationLevel01RepositoryGET,
            IGetDataResultRepository<ConsolidationLevel02> consolidationLevel02RepositoryGET,
            IGetDataResultRepository<CollectionLevel02> collectionLevel02RepositoryGET,
            IGetDataResultRepository<CollectionLevel03> collectionLevel03RepositoryGET,
            IGetDataResultRepository<CollectionHtml> baseRepoCollectionHtmlGET,
            ICollectionLevel02Repo collectionLevel02Repo
            )
        {
            _collectionLevel02Repo = collectionLevel02Repo;
            _consolidationLevel01RepositoryGET = consolidationLevel01RepositoryGET;
            _consolidationLevel02RepositoryGET = consolidationLevel02RepositoryGET;
            _collectionLevel02RepositoryGET = collectionLevel02RepositoryGET;
            _collectionLevel03RepositoryGET = collectionLevel03RepositoryGET;
            _baseRepoCollectionHtmlGET = baseRepoCollectionHtmlGET;
            _baseRepoCollectionHtml = baseRepoCollectionHtml;
            _baseRepoConsolidationL1 = baseRepoConsolidationL1;
            _baseRepoConsolidationL2 = baseRepoConsolidationL2;
            _baseRepoCollectionL2 = baseRepoCollectionL2;
            _baseRepoCollectionL3 = baseRepoCollectionL3;
            _baseRepoCorrectiveAction = baseRepoCorrectiveAction;
        }

        #endregion

        /// <summary>
        /// Recebe um SyncDTO com objeto ConsolidationLevel01DTO, com todas a properties Virtual vinculadas ao Objeto ConsolidationLevel01 do entity devidamente preenchidas, para serem inseridas no DataBase.
        /// RN1: Integridade do Banco de Dados - Valida todos os objetos que serão inseridos com "self validation" para garantir integridade do DataBase.
        /// RN2: Integridade do Banco de Dados - Deve Salvar os objetos obedecendo a ordem de hierarquia de "Foreign key".
        /// A DEFINIR: Integridade do Banco de Dados - Rollback um das várias entidades apresentem problemas ao serem salvas (pelo fato de existir 'sync = false' em tela, o objeto recebera um "ressend", o objeto pode ser salvo como duplicado hoje).
        /// RN3: Debug - Cronometro para debug do tempo de "commit".
        /// RN4: Debug - Retorno Genério disponibiliza saida sem erro, porem todo retorno deve ser tratado pelo "call site", mensagens automáticas são geradas.
        /// RN5: Sucesso - "Susscess! All Data Saved for: ....."
        /// RN6: Erro - "Cannot sync Data: ....."
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO obj)
        {

            try
            {

                #region Validação e criação de objetos.

                if (obj.Root.Count == 0)
                    throw new ExceptionHelper("Impossible to Sync data. The Sync list is empty.");

                obj.ListToSave = new List<ConsolidationLevel01DTO>();
                obj.ListToSaveCA = new List<CorrectiveActionDTO>();

                CriaListaDeObjectsToSave(obj); //RN1

                #endregion
                Stopwatch watch = IniciaCronometro(); //RN3
                #region Loop Save

                var saving = 0;

                foreach (var i in obj.ListToSave) //RN2
                {
                    ConsolidationLevel01 level01Consolidation = Mapper.Map<ConsolidationLevel01>(i);
                    PrencheFeedaBackPt1(out saving, level01Consolidation);
                    level01Consolidation = SalvaConsolidationLevel01(level01Consolidation);
                    ConsolidationLevel02 level02Consolidation;

                    foreach (var consolidationLevel02Dto in i.consolidationLevel02DTO)
                    {
                        foreach (var collectionLevel02Dto in i.collectionLevel02DTO)
                        {
                            level02Consolidation = SalvaConsolidationLevel02(level01Consolidation, consolidationLevel02Dto, collectionLevel02Dto.Level02Id);
                            CollectionLevel02 collectionLevel02 = SalvaCollectionLevel02(collectionLevel02Dto, level01Consolidation.Level01Id, level02Consolidation.Id);
                            SalvaCorrectiveAction(obj.ListToSaveCA, collectionLevel02Dto, collectionLevel02.Id);
                            SalvaCollectionLevel03(collectionLevel02Dto, collectionLevel02);
                        }
                    }
                }

                #endregion

                #region Retorno

                long elapsedMs = ParaCronometro(watch);
                GenericReturn<SyncDTO> feedback = PreencheFeedBackPt2(obj, saving, elapsedMs);//RN4
                return feedback; //RN4 E RN5
                #endregion

            }
            catch (Exception e)
            {
                return new GenericReturn<SyncDTO>(e, "Cannot sync Data: ", obj); //RN6
            }
            finally
            {
                #region NotImplemented

                #endregion
            }

        }

        /// <summary>
        /// Salva o Html para merge em frontend.
        /// RN1: Banco De Dados - Distingue Unidade e Shift.
        /// RN2: Banco De Dados - Atualiza elementos de mesma Unidade e Shift.
        /// RN2: Retorno - Generico, deve ser tratado pelo web caller.
        /// </summary>
        /// <param name="objToSync"></param>
        /// <returns></returns>
        public GenericReturn<SyncDTO> SaveHtml(SyncDTO objToSync)
        {

            try
            {
                var html = new CollectionHtml()
                {
                    Html = objToSync.html,
                    Period = objToSync.CollectionHtml.Period,
                    Shift = objToSync.CollectionHtml.Shift,
                    CollectionDate = objToSync.CollectionHtml.CollectionDate,
                    UnitId = objToSync.CollectionHtml.UnitId
                };
                var elemento = _baseRepoCollectionHtml.GetAll() //RN1
                    .FirstOrDefault(r => r.UnitId == objToSync.idUnidade
                        && r.Shift == objToSync.CollectionHtml.Shift
                    //&& r.Period == objToSync.CollectionHtml.Period
                    );
                if (elemento.IsNull() && (objToSync.html.IsNull()))
                    return new GenericReturn<SyncDTO>("Susscess! Sync.");//RN3

                if (elemento.IsNotNull())
                {
                    html.Id = elemento.Id;
                    html.AddDate = elemento.AddDate;
                    _baseRepoCollectionHtml.Dettach(elemento);
                }

                _baseRepoCollectionHtml.AddOrUpdate(html); //RN2

                return new GenericReturn<SyncDTO>("Susscess! Sync."); //RN3
            }
            catch (Exception e)
            {
                return new GenericReturn<SyncDTO>(e, "Cannot sync HTML."); //RN3
            }
            finally
            {
                #region NotImplemented

                #endregion
            }

        }

        #region Auxiliares e Validações
        
        /// <summary>
        /// Cria e valida Objeto DTO com "self Validation"
        /// </summary>
        /// <param name="obj"></param>
        private static void CriaListaDeObjectsToSave(SyncDTO obj)
        {
            //Contrutor que possui validação do objeto, dentro deste contrutor devem constar as RNs para cada objeto a ser inserido no BD, 
            //caso haja incosistencia, o mesmo deve expedir uma exception, ou exceptionhelper parando a execução, 
            //e prevenindo a entrada de arquivos não válidos no DB.
            foreach (var i in obj.Root)
            {
                obj.ListToSave.Add(i.ValidateAndCreateDtoConsolidationLevel01DTO());
                if (i.correctiveactioncomplete != null)
                    obj.ListToSaveCA = i.makeCA();
            }
        }

        /// <summary>
        /// Salva Collection level03 em loop de 1 em 1 elemento:
        /// RN's contempladas:
        /// RN1: Integridade de Banco de Dados - Deve salvar a CollectionLevel02Id corretamente com cabeçado de tabela salvo anteriormente em SalvaCollectionLevel02(..).
        /// RN2: Integridade de Banco de Dados - Deve salvar a Level03Id corretamente com cabeçado de tabela definido no Db.    
        /// RN3: Duplicado - Ao registrar um elemento CollectionLevel02 como DUPLICADO, o CollectionLevel03 referente a este precisa ser atualizado com os dados do CollectionLevel02.    
        /// RN4: Duplicado - Deve validar elemento Duplicado caso o CollectionLevel02 deste mesmo seja duplicado anteriormente em "SalvaCollectionLevel02(..)" e o mesmo seja um REGISTRO DE INSERÇÃO.
        /// RN5: Debug - Deve o objeto DTO é atualizado com dados do objeto salvo, o mesmo é disponiblizado em console JavaScript após o termino do ajax.
        /// RN6: Debug - O cabeçalho de exceção deve ser definido.
        /// Params: Object CollectionLevel02DTO, Object CollectionLevel02.
        /// </summary>
        /// <param name="collectionLevel02Dto"></param>
        /// <param name="collectionLevel02"></param>
        private void SalvaCollectionLevel03(CollectionLevel02DTO collectionLevel02Dto, CollectionLevel02 collectionLevel02)
        {
            try
            {
                List<CollectionLevel03> saved = new List<CollectionLevel03>();
                foreach (var i in collectionLevel02Dto.collectionLevel03DTO)
                {
                    //throw new NullReferenceException("teste");

                    if (i.CollectionLevel02Id == 0)
                        i.CollectionLevel02Id = collectionLevel02.Id;//RN1 e RN4

                    CollectionLevel03 collectionLevel03 = Mapper.Map<CollectionLevel03>(i);//RN2

                    if (collectionLevel03.Id == 0)
                        _collectionLevel03RepositoryGET.SetDuplicated(collectionLevel03, collectionLevel02);//RN3

                    if (collectionLevel03.Id > 0)
                    {
                        collectionLevel03.CollectionLevel02 = null;
                        collectionLevel03.Level03 = null;
                        _baseRepoCollectionL3.Update(collectionLevel03);
                    }
                    else
                        _baseRepoCollectionL3.Add(collectionLevel03);

                    saved.Add(collectionLevel03);
                }
                collectionLevel02Dto.collectionLevel03DTO = Mapper.Map<List<CollectionLevel03DTO>>(saved);//RN5

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao salvar Colleta do level03", e); //RN6
            }
        }

        /// <summary>
        /// Salva Corrective action, se alguem level02 possuis a property CorrectiveActionId > 0
        /// RN1: DataBaase - Deve conter FK CollectionLevel02 ID válida.
        /// RN2: DataBaase - Salva Corrective action, se alguem level02 possuis a property CorrectiveActionId > 0
        /// RN3: DataBaase - Salva somente se corrective action não existir.
        /// RN4: HOTFIX - Limpa properties VIRTUAL do objeto a ser salvo.
        /// RN5: Debug/Retorno - Deve atualizar DTO principalmente ID.
        /// RN6: Debug - Try/Catch para Log.
        /// </summary>
        /// <param name="objListToSaveCA"></param>
        /// <param name="x"></param>
        /// <param name="collectionLevel02Id"></param>
        private void SalvaCorrectiveAction(List<CorrectiveActionDTO> objListToSaveCA, CollectionLevel02DTO collectionLevel02Dto, int collectionLevel02Id)
        {
            try //RN6
            {
                if (collectionLevel02Dto.CorrectiveActionId > 0) //RN2
                {
                    if (_baseRepoCorrectiveAction.GetById(collectionLevel02Dto.CorrectiveActionId) == null)//RN3
                    {
                        var CaToSaveDTO = objListToSaveCA.FirstOrDefault(z => z.idcorrectiveaction == collectionLevel02Dto.CorrectiveActionId);
                        var CA = Mapper.Map<CorrectiveAction>(CaToSaveDTO);
                        CA.CollectionLevel02Id = collectionLevel02Id;//RN1

                        CA.UserSgq = null;//RN4
                        CA.UserSgq1 = null;
                        CA.UserSgq2 = null;
                        CA.CollectionLevel02 = null;

                        _baseRepoCorrectiveAction.AddOrUpdate(CA);
                        collectionLevel02Dto.CorrectiveActionSaved = Mapper.Map<CorrectiveActionDTO>(CA);//RN5
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao salvar Corrective Action", e);
            }
        }

        /// <summary>
        /// RN1: DataBase - Deve conter foregin keys consistentes Level01 e ConsolidationLevel02.
        /// RN2: DataBase - Deve procurar por elementos duplicados.
        /// RN3: DataBase - Deve utilizar a FK de ConsolidationLevel02 Previamente salva.
        /// RN3: DataBase - Deve atualizar o ID no DTO para posterior save do CollectionLevel03.
        /// RN5: HOTFIX - Deve limpar elementos de properties "VIRTUAL" do objeto.
        /// RN6: Debug - Try/Catch para Log.
        /// </summary>
        /// <param name="collectionLevel02DTO"></param>
        /// <param name="level01Id"></param>
        /// <param name="level02ConsolidationId"></param>
        /// <returns></returns>
        private CollectionLevel02 SalvaCollectionLevel02(CollectionLevel02DTO collectionLevel02DTO, int level01Id
            , int level02ConsolidationId)
        {
            try //RN6
            {
                collectionLevel02DTO.Level01Id = level01Id; //RN01
                collectionLevel02DTO.ConsolidationLevel02Id = level02ConsolidationId;//RN03
                CollectionLevel02 collectionLevel02 = Mapper.Map<CollectionLevel02>(collectionLevel02DTO);

                if (collectionLevel02.Id == 0) //RN2
                    _collectionLevel02RepositoryGET.SetDuplicated(collectionLevel02);

                collectionLevel02.CollectionLevel03 = null; //RN5
                collectionLevel02.Level01 = null;
                collectionLevel02.Level02 = null;
                collectionLevel02.UserSgq = null;
                collectionLevel02.ConsolidationLevel02 = null;

                _baseRepoCollectionL2.AddOrUpdate(collectionLevel02);

                collectionLevel02DTO.Id = collectionLevel02.Id; //RN3
                return collectionLevel02;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao salvar CollectionLevel02", e);
            }
        }

        /// <summary>
        /// Salva ConsolidationLevel02
        /// RN1: Banco de dados - Procura Consolidação Level02 existente no DIA, caso exista sobrescreve.
        /// RN2: Banco de dados - Salva ConsolidationLevel02 Com Id salvo préviamente em ConsolidationLelve01.
        /// RN3: Banco de dados - Consolida dados.
        /// RN4: Debug - Try/Catch para Log.
        /// </summary>
        /// <param name="level01Consolidation"></param>
        /// <returns></returns>
        private ConsolidationLevel02 SalvaConsolidationLevel02(ConsolidationLevel01 level01Consolidation, ConsolidationLevel02DTO consolidationLevel02DTO, int level02Id)
        {
            try //RN4
            {
                ConsolidationLevel02 level02Consolidation = Mapper.Map<ConsolidationLevel02>(consolidationLevel02DTO);
                level02Consolidation.Level01ConsolidationId = level01Consolidation.Id;
                level02Consolidation.Level02Id = level02Id; //RN2

                var consolidacaoExistente = _consolidationLevel02RepositoryGET.GetExistentLevel02Consollidation(level02Consolidation, level01Consolidation); //RN2

                if (consolidacaoExistente != null) //RN3
                {
                    level02Consolidation = consolidacaoExistente;
                }

                _baseRepoConsolidationL2.AddOrUpdate(level02Consolidation);
                return level02Consolidation;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar consoidação level02", e);
            }
        }

        /// <summary>
        /// Salva ConsolidationLevel01
        /// RN1: Banco de dados - Procura Consolidação Level01 existente no DIA, caso exista sobrescreve.
        /// RN2: Banco de dados - Consolida dados.
        /// RN3: Debug - Try/Catch para Log.
        /// </summary>
        /// <param name="level01Consolidation"></param>
        /// <returns></returns>
        private ConsolidationLevel01 SalvaConsolidationLevel01(ConsolidationLevel01 level01Consolidation)
        {
            try //RN3
            { 
                var consolidacaoLevel01Existente = _consolidationLevel01RepositoryGET.GetExistentLevel01Consollidation(level01Consolidation); //RN1

                if (consolidacaoLevel01Existente != null) //RN2
                {
                    level01Consolidation = consolidacaoLevel01Existente;
                }

                _baseRepoConsolidationL1.AddOrUpdate(level01Consolidation);
                return level01Consolidation;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar consoidação level01", e);
            }
        }

        /// <summary>
        /// Feedback com dados para controle de Desenvolvimento após salvar. 
        /// </summary>
        /// <param name="saving"></param>
        /// <param name="level01Consolidation"></param>
        private static void PrencheFeedaBackPt1(out int saving, ConsolidationLevel01 level01Consolidation)
        {

            if (level01Consolidation.Level01Id == 3) { }
                saving = 3;
            if (level01Consolidation.Level01Id == 1)
                saving = 1;
            if (level01Consolidation.Level01Id == 2)
                saving = 2;
        }

        /// <summary>
        /// Feedback com dados para controle de Desenvolvimento após salvar.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="saving"></param>
        /// <param name="elapsedMs"></param>
        /// <returns></returns>
        private static GenericReturn<SyncDTO> PreencheFeedBackPt2(SyncDTO obj, int saving, long elapsedMs)
        {
            var savingText = "";
            if (saving == 3) { }
            savingText = "CFF (Cut, Fold and Flaps)";
            if (saving == 1)
                savingText = "HTP";
            if (saving == 2)
                savingText = "Carcass Contamination Audit";

            var feedback = new GenericReturn<SyncDTO>("Susscess! All Data Saved for: " + savingText + ", in: " + elapsedMs + " ms", obj); //,
            feedback.IdSaved = saving;
            return feedback;
        }

        /// <summary>
        /// Para Cronometro.
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
        private static long ParaCronometro(Stopwatch watch)
        {
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return elapsedMs;
        }

        /// <summary>
        /// Inicia Cronometro.
        /// </summary>
        /// <returns></returns>
        private static Stopwatch IniciaCronometro()
        {
            return Stopwatch.StartNew();
        }

        #endregion

    }
}