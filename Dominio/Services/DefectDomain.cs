﻿using System;
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
        public void MergeDefect(List<DefectDTO> listDefectDto)
        {
            if(listDefectDto != null)
            {
                foreach (DefectDTO defectDTO in listDefectDto)
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
            }            
            
        }

        public List<DefectDTO> GetDefects(int ParCompany_Id)
        {
            var today = DateTime.Now.Date;
            var tomorrow = DateTime.Now.AddDays(1).Date;

            //var list = _baseRepoDefect.GetAll().Where(r =>
            //    r.ParCompany_Id == ParCompany_Id &&
            //    r.Date.Date >= DateTime.Now.Date &&
            //    r.Date.Date < DateTime.Now.AddDays(1).Date).ToList();

            var list = Mapper.Map<List<DefectDTO>>(_baseRepoDefect.GetAll().Where(r =>
                r.ParCompany_Id == ParCompany_Id &&
                InsideFrequency(r.Date.Date, r.ParLevel1.ParFrequency_Id)));/*Clusters*/

            return list;
        }

        private bool InsideFrequency(DateTime currentDate,int Frequency_Id)
        {
            switch (Frequency_Id)
            {
                case 1: //Por Período
                    break;
                case 2: //Por Turno
                    break;
                case 3: //Diário
                    
                    break;
                case 4: //Semanal
                    break;
                case 5: //Quinzenal
                    break;
                case 6: //Mensal
                    break;
                default:
                    throw new ExceptionHelper(Frequency_Id + " Invalid Frequency.");
            }
            return false;
        }

        #endregion
    }
}