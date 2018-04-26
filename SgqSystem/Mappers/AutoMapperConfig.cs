using AutoMapper;
using Dominio;
using SgqSystem.Mappers.ParamsBrasil;

namespace SgqSystem.Mappers
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            Mapper.Initialize(x =>
            {
                x.AddProfile<CorrectiveActionMapperProfile>();
                x.AddProfile<UserMapperProfile>();
                x.AddProfile<ParCompanyXUserSgqMapperProfile>();
                x.AddProfile<PeriodMapperProfile>();
                x.AddProfile<ShiftMapperProfile>(); //PROBLEMA NA TABELA, FORA DE PADRão, MOCKADO.
                x.AddProfile<UnitMapperProfile>();
                x.AddProfile<UnitUserMapperProfile>();
                x.AddProfile<ParCompanyXStructureMapperProfile>();
                x.AddProfile<ParStructureMapperProfile>();
                x.AddProfile<ParStructureGroupMapperProfile>();
                x.AddProfile<ParCompanyClusterProfile>();
                x.AddProfile<ParGoalMapperProfile>();
                x.AddProfile<ParClusterXModuleMapperProfile>();
                /*
                 * Par Level1 Mapping
                 */
                x.AddProfile<ParLevel1MapperProfile>();
                x.AddProfile<ParConsolidationTypeMapperProfile>();
                x.AddProfile<ParFrequencyMapperProfile>();
                x.AddProfile<ParClusterMapperProfile>();
                x.AddProfile<ParLevel1XClusterMapperProfile>();
                x.AddProfile<ParHeaderFieldMapperProfile>();
                x.AddProfile<ParLevelDefinitonMapperProfile>();
                x.AddProfile<ParFieldTypeMapperProfile>();
                x.AddProfile<ParDepartmentMapperProfile>();
                x.AddProfile<ParLevelDefinitonMapperProfile>();
                x.AddProfile<ParMultipleValuesMapperProfile>();
                x.AddProfile<ParLevel1XHeaderFieldMapperProfile>();
                x.AddProfile<ParLevel3Level2Level1MapperProfile>();
                x.AddProfile<ParCriticalLevelMapperProfile>();
                x.AddProfile<ParLevel2Level1MapperProfile>();
                x.AddProfile<ParScoreTypeMapperProfile>();

                /*
                * Par Level2 Mapping
                */
                x.AddProfile<ParLevel2MapperProfile>();
                x.AddProfile<ParLevel3GroupMapperProfile>();
                x.AddProfile<ParLocalMapperProfile>();
                x.AddProfile<ParCounterMapperProfile>();
                x.AddProfile<ParCounterLocalMapperProfile>();
                x.AddProfile<ParRelapseMapperProfile>();
                x.AddProfile<ParNotConformityRuleMapperProfile>();
                x.AddProfile<ParNotConformityRuleXLevelMapperProfile>();
                x.AddProfile<ParCompanyMapperProfile>();
                x.AddProfile<ParEvaluationMapperProfile>();
                x.AddProfile<ParSampleMapperProfile>();
                x.AddProfile<ParLevel2XHeaderFieldMapperProfile>();


                /*
                 *Par Level 3 Mapping
                 */
                x.AddProfile<ParLevel3MapperProfile>();
                x.AddProfile<ParLevel3EvaluationSampleMapperProfile>();
                x.AddProfile<ParLevel3ValueMapperProfile>();

                x.AddProfile<ParLevel3inputTypeMapperProfile>();
                x.AddProfile<ParMeasurementUnitMapperProfile>();
                x.AddProfile<ParLevel3BoolFalseMapperProfile>();
                x.AddProfile<ParLevel3BoolTrueMapperProfile>();
                x.AddProfile<ParLevel3Level2MapperProfile>();

                /*
                 Exemplo para configuração do Auto Mapper.
                 */
                x.AddProfile<ExampleMapperProfile>();


                /*ParamsBrasil*/

                x.AddProfile<ParLevel2ControlCompanyMapperProfile>();
                x.AddProfile<ResultLevel3MapperProfile>();
                x.AddProfile<RoleMapperProfile>();
                x.AddProfile<DefectMapperProfile>();
                x.AddProfile<UserSgqMapperProfile>();
                x.AddProfile<ParClusterGroupMapperProfile>();



                /*Manutencao*/
                //x.AddProfile<DimManutencaoColetaITsMapperProfile>();

                /*Email Content*/
                x.AddProfile<EmailContentMapperProfile>();

                x.AddProfile<RoleUserSgqMapperProfile>();

               



                ///*PA............*/
                //x.AddProfile<AcompanhamentoTarefaProfile>();
                //x.AddProfile<CabecalhoProfile>();
                //x.AddProfile<CampoProfile>();
                //x.AddProfile<ConfiguracaoEmailProfile>();
                //x.AddProfile<EmpresaProfile>();
                //x.AddProfile<GrupoCabecalhoProfile>();
                //x.AddProfile<GrupoProjetoProfile>();
                //x.AddProfile<LogOperacaoProfile>();
                //x.AddProfile<MultiplaEscolhaProfile>();
                //x.AddProfile<ParticipanteProfile>();
                //x.AddProfile<ProjetoProfile>();
                //x.AddProfile<TarefaProfile>();
                //x.AddProfile<VinculoCampoCabecalhoProfile>();
                //x.AddProfile<VinculoCampoTarefaProfile>();
                //x.AddProfile<VinculoParticipanteMultiplaEscolhaProfile>();
                //x.AddProfile<VinculoParticipanteProjetoProfile>();

            });

        }

    }
}