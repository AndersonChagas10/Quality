﻿using AutoMapper;
using Dominio.Interfaces.Repositories;
using Dominio.Interfaces.Services;
using DTO.DTO;
using DTO.Helpers;
using System;
using System.Collections.Generic;
using DTO;
using System.Linq;

namespace Dominio.Services
{
    public class CorrectiveActionDomain : ICorrectiveActionDomain
    {
        #region Construtor
        private ICorrectiveActionRepository _correctiveActionRepository;
        private IBaseRepository<UserSgq> _baseUserSgqRepo;
        private IEnumerable<UserSgq> _userSgq;
        private IBaseRepository<Level01> _baseLevel01Repo;
        private IBaseRepository<Level02> _baseLevel02Repo;
        private IBaseRepository<CorrectiveAction> _baseCorrectiveAction;
        private IEnumerable<Level01> _listLevel01;
        private IEnumerable<Level02> _listLevel02;

        public CorrectiveActionDomain(ICorrectiveActionRepository correctiveActionRepository,
            IBaseRepository<UserSgq> baseUserSgqRepo,
            IBaseRepository<Level01> baseLevel01Repo,
            IBaseRepository<Level02> baseLevel02Repo,
            IBaseRepository<CorrectiveAction> baseCorrectiveAction
            )
        {
            _baseCorrectiveAction = baseCorrectiveAction;
            _baseLevel01Repo = baseLevel01Repo;
            _baseLevel02Repo = baseLevel02Repo;
            _baseUserSgqRepo = baseUserSgqRepo;
            _correctiveActionRepository = correctiveActionRepository;
            _userSgq = _baseUserSgqRepo.GetAll();
            _listLevel01 = _baseLevel01Repo.GetAll();
            _listLevel02 = _baseLevel02Repo.GetAll();
        }
        #endregion

        public string falhaGeral { get { return "It was not possible retrieve any data."; } }

        public GenericReturn<List<CorrectiveActionDTO>> GetCorrectiveAction(DataCarrierFormulario data)
        {
            try
            {
                var result = _correctiveActionRepository.GetCorrectiveAction(data).ToList();

                //Guard.CheckListNullOrEmpty(result, "There`s no corrective actions in database to retrieve.");

                var resultMapped = Mapper.Map<List<CorrectiveActionDTO>>(result);

                foreach (var i in resultMapped)
                {
                    i.NameSlaughter = _userSgq.FirstOrDefault(r => r.Id == i.SlaughterId).Name;
                    i.NameTechinical = _userSgq.FirstOrDefault(r => r.Id == i.TechinicalId).Name;
                    i.AuditorName = _userSgq.FirstOrDefault(r => r.Id == i.AuditorId).Name;

                    var original = result.FirstOrDefault(r => r.Id == i.Id);
                    i.Level01Id = original.CollectionLevel02.Level01Id;
                    i.Level02Id = original.CollectionLevel02.Level02Id;

                    i.level02Name = _listLevel02.FirstOrDefault(r => r.Id == i.Level02Id).Name;
                    i.level01Name = _listLevel01.FirstOrDefault(r => r.Id == i.Level01Id).Name;

                }

                return new GenericReturn<List<CorrectiveActionDTO>>(resultMapped);
            }
            catch (Exception e)
            {
                return new GenericReturn<List<CorrectiveActionDTO>>(e, falhaGeral);
            }
        }

        public GenericReturn<CorrectiveActionDTO> SalvarAcaoCorretiva(CorrectiveActionDTO dto)
        {
            try
            {
                dto.ValidaDataCorrectiveActionDTO();
                var correctiveActionLevels = dto.CorrectiveActionLevels;
                dto.CorrectiveActionLevels = new List<CorrectiveActionLevelsDTO>();

                var entitie = Mapper.Map<CorrectiveAction>(dto);
                entitie = _correctiveActionRepository.SalvarAcaoCorretiva(entitie);


                //foreach (var item in correctiveActionLevels)
                //{
                //    item.ValidaCorrectiveActionLevels();
                //    item.CorrectiveActionId = entitie.Id;
                //    var entitieLevels = Mapper.Map<CorrectiveActionLevels>(item);
                //    entitieLevels = _correctiveActionRepository.SalvarAcaoCorretivaLevels(entitieLevels);
                //}

                return new GenericReturn<CorrectiveActionDTO>(dto);
            }
            catch (Exception e)
            {
                return new GenericReturn<CorrectiveActionDTO>(e, falhaGeral);
            }
        }

        public GenericReturn<CorrectiveActionDTO> VerificarAcaoCorretivaIncompleta(CorrectiveActionDTO dto)
        {
            try
            {
                var entitie = _correctiveActionRepository.VerificarAcaoCorretivaIncompleta(Mapper.Map<CorrectiveAction>(dto));
                return new GenericReturn<CorrectiveActionDTO>(Mapper.Map<CorrectiveActionDTO>(entitie));
            }
            catch (Exception e)
            {
                return new GenericReturn<CorrectiveActionDTO>(e, falhaGeral);
            }
        }

        public GenericReturn<CorrectiveActionDTO> GetCorrectiveActionById(int id)
        {
            try
            {
                var result = _baseCorrectiveAction.GetById(id);

                if (result == null)
                    throw new ExceptionHelper("Cannot get corrective action from id: " + id);

                var resultMapped = Mapper.Map<CorrectiveActionDTO>(result);

                resultMapped.NameSlaughter = _userSgq.FirstOrDefault(r => r.Id == result.SlaughterId).Name;
                resultMapped.NameTechinical = _userSgq.FirstOrDefault(r => r.Id == result.TechinicalId).Name;
                resultMapped.AuditorName = _userSgq.FirstOrDefault(r => r.Id == result.AuditorId).Name;

                resultMapped.Level01Id = result.CollectionLevel02.Level01Id;
                resultMapped.Level02Id = result.CollectionLevel02.Level02Id;

                resultMapped.PeriodName = result.CollectionLevel02.Period.ToString();
                resultMapped.ShiftName = result.CollectionLevel02.Shift.ToString();

                resultMapped.level02Name = _listLevel02.FirstOrDefault(r => r.Id == resultMapped.Level02Id).Name;
                resultMapped.level01Name = _listLevel01.FirstOrDefault(r => r.Id == resultMapped.Level01Id).Name;

                return new GenericReturn<CorrectiveActionDTO>(resultMapped);
            }
            catch (Exception e)
            {
                return new GenericReturn<CorrectiveActionDTO>(e, "Cannot get corrective action from id: " + id);
            }
        }
    }
}
