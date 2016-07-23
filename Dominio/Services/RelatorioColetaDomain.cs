using Dominio.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.DTO;
using DTO.Helpers;
using Dominio.Interfaces.Repositories;
using AutoMapper;

namespace Dominio.Services
{
    public class RelatorioColetaDomain : IRelatorioColetaDomain
    {
        #region Construtor
        private IRelatorioColetaRepository _relatorioColetaRepository;
        private IRepositoryBase<Level1> _repoLevel1;
        private IRepositoryBase<Level2> _repoLevel2;
        private IRepositoryBase<Level3> _repoLevel3;

        public RelatorioColetaDomain(IRelatorioColetaRepository relatorioColetaRepository,
            IRepositoryBase<Level1> repoLevel1,
            IRepositoryBase<Level2> repoLevel2,
            IRepositoryBase<Level3> repoLevel3)
        {
            _relatorioColetaRepository = relatorioColetaRepository;
            _repoLevel1 = repoLevel1;
            _repoLevel2 = repoLevel2;
            _repoLevel3 = repoLevel3;
        }
        #endregion


        public GenericReturn<List<ColetaDTO>> GetColetas()
        {
            try
            {
                var queryResult = Mapper.Map<List<ColetaDTO>>(_relatorioColetaRepository.GetColetas());
                GetNames(queryResult);
                return new GenericReturn<List<ColetaDTO>>(queryResult);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<ColetaDTO>>(e, "Cannot retrieve data.");
            }
        }

        private void GetNames(List<ColetaDTO> queryResult)
        {
            var l1 = _repoLevel1.GetAll();
            var l2 = _repoLevel2.GetAll();
            var l3 = _repoLevel3.GetAll();
            
            foreach (var i in queryResult)
            {
                i.Level1Name = l1.FirstOrDefault(r => r.Id == i.Id_Level1).Name;
                i.Level2Name = l2.FirstOrDefault(r => r.Id == i.Id_Level2).Name;
                i.Level3Name = l3.FirstOrDefault(r => r.Id == i.Id_Level3).Name;
            }
            
        }

       
    }
}
