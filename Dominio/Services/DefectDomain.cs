using System;
using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using DTO.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Services
{
    public class DefectDomain : IDefectDomain
    {
        #region Construtor

        private IBaseRepository<Defect> _baseRepoDefect;

        public DefectDomain(
            IBaseRepository<Defect> baseRepoDefect
            )
        {
            _baseRepoDefect = baseRepoDefect;
        }
        
        #endregion

        #region Metodos

        /// <summary>
        /// Se valido, classe defectDTO.example (Example) é salva em Banco de dados.
        /// 
        /// 
        /// </summary>
        /// <param name="defectDTO"></param>
        /// <returns></returns>
        public List<Defect> MergeDefect(List<DefectDTO> listDefectDto)
        {
            foreach(DefectDTO defectDTO in listDefectDto)
            {
                List<Defect> defects = _baseRepoDefect.GetAll().Where(r =>
                r.CurrentEvaluation == defectDTO.CurrentEvaluation &&
                r.ParCompany_Id == defectDTO.ParCompany_Id &&
                r.ParLevel1_Id == defectDTO.ParLevel1_Id).ToList();

                if (defects.Count == 0)
                {
                    Defect defect = Mapper.Map<Defect>(defectDTO);
                    defectDTO.AddDate = DateTime.Now;
                    defectDTO.Active = true;

                    _baseRepoDefect.Add(defect);
                }
                else
                {
                    var defect = defects.FirstOrDefault();

                    defect.Evaluations += defectDTO.Evaluations;
                    defect.Defects += defectDTO.Defects;
                    defect.AlterDate = DateTime.Now;

                    _baseRepoDefect.Update(defect);
                }
            }

            return _baseRepoDefect.GetAll().Where(r =>
                r.ParCompany_Id == listDefectDto.FirstOrDefault().ParCompany_Id).ToList();
        }
        
        #endregion
    }
}