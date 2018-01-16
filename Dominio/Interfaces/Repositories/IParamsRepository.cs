using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IParamsRepository
    {
        void SaveParLevel1(ParLevel1 saveParamLevel1, List<ParHeaderField> listaParHEadField, List<ParLevel1XCluster> listaParLevel1XCluster, List<int> removerHeadField, List<ParCounterXLocal> listaParCounterLocal, List<ParNotConformityRuleXLevel> listNonCoformitRule, List<ParRelapse> listaReincidencia, List<ParGoal> listParGoal);

        void SaveParLevel2(ParLevel2 saveParamLevel2, List<ParLevel3Group> listaParLevel3Group, List<ParCounterXLocal> listParCounterXLocal, List<ParNotConformityRuleXLevel> saveParamNotConformityRuleXLevel, List<ParEvaluation> saveParamEvaluation, List<ParSample> saveParamSample, List<ParRelapse> listParRelapse);

        void RemoveParLevel3Group(ParLevel3Group paramLevel03group);

        void SaveParLocal(ParLocal paramLocal);
        void SaveParCounter(ParCounter paramCounter);
        void SaveParCounterXLocal(ParCounterXLocal paramCounterLocal);
        void SaveParRelapse(ParRelapse paramRelapse);
        void SaveParNotConformityRule(ParNotConformityRule paramNotConformityRule);
        void SaveParNotConformityRuleXLevel(ParNotConformityRuleXLevel paramNotConformityRule);
        void SaveParCompany(ParCompany paramCompany);

        void SaveParLevel3Level2(ParLevel3Level2 paramLevel3Level2);
        void SaveParLevel3(ParLevel3 saveParamLevel3, List<ParLevel3Value> listSaveParamLevel3Value, List<ParRelapse> listParRelapse, List<ParLevel3Level2> parLevel3Level2pontos, int level1Id);
        void ExecuteSql(string sql);
        ParLevel2XHeaderField SaveParHeaderLevel2(ParLevel2XHeaderField parLevel2XHeaderField);

        ParHeaderField SaveParHeaderDuplicate(ParHeaderField parHeaderField);

        void SaveVinculoL3L2L1(int idLevel1, int idLevel2, int idLevel3, int? userId, int? companyId);
    }
}
