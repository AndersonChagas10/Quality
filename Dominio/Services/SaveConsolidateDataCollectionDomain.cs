using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO;
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

        //private IBaseRepository<CollectionLevel02> _baseRepoCollectionL2;
        //private IBaseRepository<CollectionLevel03> _baseRepoCollectionL3;
        //private ICollectionLevel02Repo _collectionLevel02Repo;
        private IBaseRepository<ConsolidationLevel01> _baseRepoConsolidationL1;
        private IBaseRepository<ConsolidationLevel02> _baseRepoConsolidationL2;
        private IBaseRepository<CollectionHtml> _baseRepoCollectionHtml;
        private IBaseRepository<CorrectiveAction> _baseRepoCorrectiveAction;
        private IGetDataResultRepository<ConsolidationLevel01> _consolidationLevel01RepositoryGET;
        private IGetDataResultRepository<ConsolidationLevel02> _consolidationLevel02RepositoryGET;
        private IGetDataResultRepository<CollectionLevel02> _collectionLevel02RepositoryGET;
        private IGetDataResultRepository<CollectionLevel03> _collectionLevel03RepositoryGET;
        private IGetDataResultRepository<CollectionHtml> _baseRepoCollectionHtmlGET;

        /*Save*/
        private ISaveCollectionRepo _saveCollectionRepo;
        private List<ConsolidationLevel01> _consolidationLevel01ToSave;
        private List<ConsolidationLevel02> _consolidationLevel02ToSave;
        //private List<ConsolidationLevel02DTO> _consolidationLevel02DTOToSave;
        private List<CollectionLevel02> _collectionLevel02ToSave;
        private List<CollectionLevel03> _collectionLevel03ToSave;
        private List<CorrectiveAction> _correctiveActionToSave;

        #endregion

        #region Construtor

        public SaveConsolidateDataCollectionDomain(
            //ICollectionLevel02Repo collectionLevel02Repo,
            //IBaseRepository<CollectionLevel02> baseRepoCollectionL2,
            //IBaseRepository<CollectionLevel03> baseRepoCollectionL3,
            IBaseRepository<ConsolidationLevel01> baseRepoConsolidationL1,
            IBaseRepository<ConsolidationLevel02> baseRepoConsolidationL2,
            IBaseRepository<CollectionHtml> baseRepoCollectionHtml,
            IBaseRepository<CorrectiveAction> baseRepoCorrectiveAction,
            IGetDataResultRepository<ConsolidationLevel01> consolidationLevel01RepositoryGET,
            IGetDataResultRepository<ConsolidationLevel02> consolidationLevel02RepositoryGET,
            IGetDataResultRepository<CollectionLevel02> collectionLevel02RepositoryGET,
            IGetDataResultRepository<CollectionLevel03> collectionLevel03RepositoryGET,
            IGetDataResultRepository<CollectionHtml> baseRepoCollectionHtmlGET,
            ISaveCollectionRepo saveCollectionRepo
            )
        {
            //_collectionLevel02Repo = collectionLevel02Repo;
            //_baseRepoCollectionL2 = baseRepoCollectionL2;
            //_baseRepoCollectionL3 = baseRepoCollectionL3;
            _consolidationLevel01RepositoryGET = consolidationLevel01RepositoryGET;
            _consolidationLevel02RepositoryGET = consolidationLevel02RepositoryGET;
            _collectionLevel02RepositoryGET = collectionLevel02RepositoryGET;
            _collectionLevel03RepositoryGET = collectionLevel03RepositoryGET;
            _baseRepoCollectionHtmlGET = baseRepoCollectionHtmlGET;
            _baseRepoCollectionHtml = baseRepoCollectionHtml;
            _baseRepoConsolidationL1 = baseRepoConsolidationL1;
            _baseRepoConsolidationL2 = baseRepoConsolidationL2;
            _baseRepoCorrectiveAction = baseRepoCorrectiveAction;

            /*Save*/
            _saveCollectionRepo = saveCollectionRepo;
            _consolidationLevel01ToSave = new List<ConsolidationLevel01>();
            _consolidationLevel02ToSave = new List<ConsolidationLevel02>();
            _collectionLevel02ToSave = new List<CollectionLevel02>();
            _collectionLevel03ToSave = new List<CollectionLevel03>();
            _correctiveActionToSave = new List<CorrectiveAction>();
        }

        #endregion

        /// <summary>
        /// Recebe um SyncDTO com objeto ConsolidationLevel01DTO, com todas a properties Virtual vinculadas ao Objeto ConsolidationLevel01 do entity devidamente preenchidas, para serem inseridas no DataBase.
        /// 
        /// RN1: Integridade do Banco de Dados - Valida todos os objetos que serão inseridos com "self validation" para garantir integridade do DataBase.
        /// RN2: Integridade do Banco de Dados - Deve Salvar os objetos obedecendo a ordem de hierarquia de "Foreign key".
        /// RN3: Debug - Cronometro para debug do tempo de "commit".
        /// RN4: Debug - Retorno Genério disponibiliza saida sem erro, porem todo retorno deve ser tratado pelo "call site", mensagens automáticas são geradas.
        /// RN5: Debug - Sucesso "Susscess! All Data Saved for: ....."
        /// RN6: Debug - Erro "Cannot sync Data: ....."
        /// RN7: Banco de Dados - Verifica itens DUPLICADOS collection Lelve02  e level03.
        /// RN8: Banco de Dados -  Salva level03 em lote.
        /// 
        /// </summary>
        /// <param name="obj">SyncDTO com objeto Root Completo</param>
        /// <returns></returns>
        public GenericReturn<SyncDTO> SetDataToSincyAuditConsolidated(SyncDTO obj)
        {
            GenericReturn<SyncDTO> feedback = new GenericReturn<SyncDTO>("Susscess! All Data Saved for: ");
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
                    level01Consolidation = AddConsolidationLevel01(level01Consolidation);
                    ConsolidationLevel02 level02Consolidation;
                    foreach (var consolidationLevel02Dto in i.consolidationLevel02DTO)
                    {
                        VerificaDuplicados(i);//RN7
                        foreach (var collectionLevel02Dto in i.collectionLevel02DTO)
                        {
                            level02Consolidation = AddConsolidationLevel02(level01Consolidation, consolidationLevel02Dto, collectionLevel02Dto.Level02Id, i.UnitId);
                            AddCorrectiveAction(obj.ListToSaveCA, collectionLevel02Dto);
                            CollectionLevel02 collectionLevel02 = AddCollectionLevel02CollectionLevel03(collectionLevel02Dto, level01Consolidation.Level01Id, level02Consolidation.Id);
                        }
                    }
                }

                /*Salvando os itens */
                _saveCollectionRepo.SaveAllLevel(_collectionLevel02ToSave
                    , _collectionLevel03ToSave, _correctiveActionToSave);

                foreach (var preencheObjetoParaAtualizarTela in obj.ListToSave)
                {
                    preencheObjetoParaAtualizarTela.collectionLevel02DTO = Mapper.Map<List<CollectionLevel02DTO>>(_collectionLevel02ToSave);
                    preencheObjetoParaAtualizarTela.collectionLevel02DTO.ForEach(r => r.collectionLevel03DTO = r.CollectionLevel03);
                }

                #endregion

                #region Retorno

                long elapsedMs = ParaCronometro(watch);
                feedback = PreencheFeedBackPt2(obj, saving, elapsedMs);//RN4
                #endregion

            }
            catch (Exception e)
            {
                new CreateLog(new Exception("Cannot sync Data: ", e), obj);
            }

            return feedback; //RN4 E RN5

        }

        /// <summary>
        /// Metodo que insere/altera registros como duplicadas 
        /// caso DOIS tablets enviem a mesma informação devido a 
        /// SINCRONIZAÇÃO DE TELA ATRASADA ou outros fatores, isto somente é realizado se o ID do elemento for para INSERÇÃO.
        /// 
        /// </summary>
        /// <param name="CollectionLevel02Verificar"></param>
        /// <param name="level01Id"></param>
        private void VerificaDuplicados(ConsolidationLevel01DTO i)
        {
            var collectionAVerificarDuplicidade = i.collectionLevel02DTO.Where(r => r.Id == 0);
            if (collectionAVerificarDuplicidade.Count() > 0)
            {
                var verificar = Mapper.Map<IEnumerable<CollectionLevel02>>(collectionAVerificarDuplicidade);
                _collectionLevel02RepositoryGET.SetDuplicated(verificar, i.Level01Id);//RN1
                var duplicadoInsercao = verificar.Where(r => r.Duplicated == true);
                if (duplicadoInsercao.Count() > 0)
                    foreach (var dup in duplicadoInsercao)
                        i.collectionLevel02DTO.FirstOrDefault(r => r.Level02Id == dup.Level02Id).Duplicated = true;//RN2
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

            try //RN3
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
                    return new GenericReturn<SyncDTO>("Susscess! Sync.");

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
        /// 
        /// RN1 : Banco De Dados (Integridade): 
        /// Contrutor que possui validação do objeto, dentro deste contrutor devem constar as RNs para cada objeto a ser inserido no BD, 
        /// caso haja incosistencia, o mesmo deve expedir uma exception, ou exceptionhelper parando a execução, 
        /// e prevenindo a entrada de arquivos não válidos no DB.
        /// 
        /// </summary>                          
        /// <param name="obj"></param>          
        private static void CriaListaDeObjectsToSave(SyncDTO obj)
        {
            foreach (var i in obj.Root)
            {
                obj.ListToSave.Add(i.ValidateAndCreateDtoConsolidationLevel01DTO());
                if (i.correctiveactioncomplete != null)
                    obj.ListToSaveCA = i.makeCA();
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
        private ConsolidationLevel01 AddConsolidationLevel01(ConsolidationLevel01 level01Consolidation)
        {
            try //RN3
            {
                var consolidacaoLevel01Existente = _consolidationLevel01RepositoryGET.GetExistentLevel01Consollidation(level01Consolidation); //RN1

                if (consolidacaoLevel01Existente != null) //RN2
                {
                    level01Consolidation = consolidacaoLevel01Existente;
                }

                //_consolidationLevel01ToSave.Add(level01Consolidation);
                _baseRepoConsolidationL1.AddOrUpdate(level01Consolidation);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar consoidação level01", e);
            }

            return level01Consolidation;
        }

        /// <summary>
        /// Salva ConsolidationLevel02
        /// 
        /// RN1: Banco de dados - Procura Consolidação Level02 existente no DIA, caso exista sobrescreve.
        /// RN2: Banco de dados - Salva ConsolidationLevel02 Com Id salvo préviamente em ConsolidationLelve01.
        /// RN3: Banco de dados - Consolida dados.
        /// RN4: Debug - Try/Catch para Log.
        /// 
        /// </summary>
        /// <param name="level01Consolidation"></param>
        /// <returns></returns>
        private ConsolidationLevel02 AddConsolidationLevel02(ConsolidationLevel01 level01Consolidation, ConsolidationLevel02DTO consolidationLevel02DTO, int level02Id, int unitId)
        {
            ConsolidationLevel02 level02Consolidation = new ConsolidationLevel02();

            try //RN4
            {
                level02Consolidation = Mapper.Map<ConsolidationLevel02>(consolidationLevel02DTO);
                level02Consolidation.Level01ConsolidationId = level01Consolidation.Id;
                level02Consolidation.Level02Id = level02Id; //RN2
                level02Consolidation.UnitId = unitId; //RN2

                var consolidacaoExistente = _consolidationLevel02RepositoryGET.GetExistentLevel02Consollidation(level02Consolidation, level01Consolidation); //RN2

                if (consolidacaoExistente != null) //RN3
                {
                    level02Consolidation = consolidacaoExistente;
                }

                //var list = new List<CollectionLevel02DTO>();
                //list.Add(listaCollectionLevel02);
                //_consolidationLevel02ToSave.Add(level02Consolidation);
                _baseRepoConsolidationL2.AddOrUpdate(level02Consolidation);

                //var aa = Mapper.Map<ConsolidationLevel02DTO>(level02Consolidation);
                //level02Consolidation.CollectionLevel02 = Mapper.Map<List<CollectionLevel02>>(list);
                //_consolidationLevel02DTOToSave.Add(aa);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar consoidação level02", e);
            }

            return level02Consolidation;
        }

        /// <summary>
        /// Salva Corrective action, se alguem level02 possuis a property CorrectiveActionId > 0
        /// 
        /// RN1: DataBaase - Deve conter FK CollectionLevel02 ID válida.
        /// RN2: DataBaase - Salva Corrective action, se alguem level02 possuis a property CorrectiveActionId > 0
        /// RN3: DataBaase - Salva somente se corrective action não existir.
        /// RN4: HOTFIX - Limpa properties VIRTUAL do objeto a ser salvo.
        /// RN5: Debug/Retorno - Deve atualizar DTO principalmente ID.
        /// RN6: Debug - Try/Catch para Log.
        /// 
        /// </summary>
        /// <param name="objListToSaveCA"></param>
        /// <param name="x"></param>
        /// <param name="collectionLevel02Id"></param>
        private void AddCorrectiveAction(List<CorrectiveActionDTO> objListToSaveCA, CollectionLevel02DTO collectionLevel02Dto)
        {
            try //RN6
            {
                if (collectionLevel02Dto.CorrectiveActionId > 0) //RN2
                {

                    if (_baseRepoCorrectiveAction.GetById(collectionLevel02Dto.CorrectiveActionId) == null)//RN3
                    {
                        var CaToSaveDTO = objListToSaveCA.FirstOrDefault(z => z.idcorrectiveaction == collectionLevel02Dto.CorrectiveActionId);
                        var CA = Mapper.Map<CorrectiveAction>(CaToSaveDTO);
                        //CA.CollectionLevel02Id = collectionLevel02Id;//RN1

                        CA.UserSgq = null;//RN4
                        CA.UserSgq1 = null;
                        CA.UserSgq2 = null;
                        CA.CollectionLevel02 = null;

                        _correctiveActionToSave.Add(CA);
                        collectionLevel02Dto.CorrectiveActionSaved = Mapper.Map<CorrectiveActionDTO>(CA);//RN5

                        var correctiveToSave = new List<CorrectiveAction>();
                        correctiveToSave.Add(CA);

                        /**/
                        collectionLevel02Dto.CorrectiveAction = Mapper.Map<List<CorrectiveActionDTO>>(correctiveToSave);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao salvar Corrective Action", e);
            }
        }

        /// <summary>
        /// 
        /// RN1: Debug - Try/Catch para Log.
        /// RN2: DataBase - Deve conter foregin keys consistentes Level01.
        /// RN3: DataBase - Deve utilizar a FK de ConsolidationLevel02 Previamente salva.
        /// RN4: HOTFIX - Deve limpar elementos de properties "VIRTUAL" do objeto.
        /// RN5: DataBase - Deve atualizar o ID no DTO para posterior PARA RETORNO em tela.
        /// RN6: DataBase - Deve Salvar CollectionLevel02.
        /// RN7: DataBase - Deve referenciar DUPLICATED para CollectionLevel03 caso haja.
        /// RN8: DataBase - Deve referenciar CollectionLevel02 ID para CollectionLevel03.
        /// RN9: DataBase - Deve ADICIONAR elementos do CollectionLevel03 na lista de inserção/update para ser salvo posteriormente.
        /// 
        /// </summary>
        /// <param name="collectionLevel02DTO"></param>
        /// <param name="level01Id"></param>
        /// <param name="level02ConsolidationId"></param>
        /// <returns></returns>
        private CollectionLevel02 AddCollectionLevel02CollectionLevel03(CollectionLevel02DTO collectionLevel02DTO, int level01Id, int level02ConsolidationId)
        {
            CollectionLevel02 collectionLevel02 = new CollectionLevel02();
            try //RN1
            {
                collectionLevel02DTO.Level01Id = level01Id; //RN2
                collectionLevel02DTO.ConsolidationLevel02Id = level02ConsolidationId; //RN3
                collectionLevel02 = Mapper.Map<CollectionLevel02>(collectionLevel02DTO);

                collectionLevel02.CollectionLevel03 = null; //RN4
                collectionLevel02.Level01 = null;
                collectionLevel02.Level02 = null;
                collectionLevel02.UserSgq = null;
                collectionLevel02.ConsolidationLevel02 = null;

                _collectionLevel02ToSave.Add(collectionLevel02);//RN5
                //collectionLevel02DTO.Id = collectionLevel02.Id; //RN6

                //foreach (var i in collectionLevel02DTO.collectionLevel03DTO)
                //{

                //    i.Duplicated = collectionLevel02.Duplicated; //RN7
                //    //i.CollectionLevel02Id = collectionLevel02.Id; // RN8
                //    _collectionLevel03ToSave.Add(Mapper.Map<CollectionLevel03>(i)); //RN9
                //}

                collectionLevel02.CollectionLevel03 = Mapper.Map<List<CollectionLevel03>>(collectionLevel02DTO.collectionLevel03DTO);

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao salvar CollectionLevel02", e);
            }

            return collectionLevel02;
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