﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Interfaces;

namespace Conformity.Infra.Data.Core.Repository.Log
{
    public class EntityTrackRepository
    {     
        private readonly LogEntityContext _dbContext;

        public EntityTrackRepository(LogEntityContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<EntityTrackViewModel> GetAll(string tabelaAlterada, int entidadeId)
        {
                var sql = $@"select 
	                    usuario.FullName AS UserName, 
	                    CAST(DAY(historico.RegisterDate) as VARCHAR(2)) + '-' +
						CAST(MONTH(historico.RegisterDate) as VARCHAR(2)) + '-' +
	                    CAST(YEAR(historico.RegisterDate) as VARCHAR(4)) + ' ' + 	
						CAST(DATEPART(HOUR, historico.RegisterDate) as VARCHAR(2)) + ':' +
						CAST(DATEPART(MINUTE, historico.RegisterDate) as VARCHAR(2)) AS UpdateDate,
	                    historico.OldValue, 
	                    historico.NewValue,
                        historico.FieldName
                    from LOG.EntityTrack historico with(nolock)
                    left join UserSgq usuario with(nolock) on
	                    usuario.id = historico.User_Id
                    where 1=1
                    and historico.TableName = '{tabelaAlterada}'
                    and historico.Entity_Id = '{entidadeId}'";
                return _dbContext.Database.SqlQuery<EntityTrackViewModel>(sql);
        }
    }
}
