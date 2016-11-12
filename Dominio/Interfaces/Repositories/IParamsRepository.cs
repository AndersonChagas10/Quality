using System.Collections.Generic;

namespace Dominio.Interfaces.Repositories
{
    public interface IParamsRepository
    {
        void SaveParLevel1(ParLevel1 saveParamLevel1, List<ParHeaderField> listaParHEadField, List<ParLevel1XCluster> listaParLevel1XCluster, List<int> removerHeadField, List<int> removerCluster, List<int> removeCounter, List<ParCounterXLocal> listaParCounterLocal, ParNotConformityRuleXLevel nonCoformitRule, List<ParRelapse> listaReincidencia, List<int> removeReincidencia);
        void SaveParLevel2(ParLevel2 saveParamLevel2, List<ParLevel3Group> listaParLevel3Group, List<ParCounterXLocal> listParCounterXLocal, ParNotConformityRuleXLevel saveParamNotConformityRuleXLevel, ParEvaluation saveParamEvaluation, ParSample saveParamSample, List<ParRelapse> listParRelapse, List<int> listParRelapseRemove);
        

        void RemoveParLevel3Group(ParLevel3Group paramLevel03group);

        void SaveParLocal(ParLocal paramLocal);
        void SaveParCounter(ParCounter paramCounter);
        void SaveParCounterXLocal(ParCounterXLocal paramCounterLocal);
        void SaveParRelapse(ParRelapse paramRelapse);
        void SaveParNotConformityRule(ParNotConformityRule paramNotConformityRule);
        void SaveParNotConformityRuleXLevel(ParNotConformityRuleXLevel paramNotConformityRule);
        void SaveParCompany(ParCompany paramCompany);

        void SaveParLevel3(ParLevel3 saveParamLevel3, ParLevel3Value saveParamLevel3Value, List<ParRelapse> listParRelapse, List<int> listParRelapseRemove);
        void SaveParLevel3Level2(ParLevel3Level2 paramLevel3Level2);
        //void SaveParLevel3Group(List<ParLevel3Group> paramLevel3Group, int ParLevel2_Id);
    }
}
