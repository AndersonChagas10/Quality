using Conformity.Domain.Core.DTOs;
using Conformity.Domain.Core.Entities.Log;
using Conformity.Domain.Core.Interfaces;
using Conformity.Infra.CrossCutting;
using Conformity.Infra.Data.Core.Repository.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Conformity.Application.Core.Log
{
    public class LogErrorService
    {
        private readonly LogErrorRepository _repository;
        public LogErrorService(LogErrorRepository repository) 
        {
            _repository = repository;
        }

        public void TryRegister(Exception ex, object obj = null)
        {
            Register(ex, obj);
        }

        public void Register(Exception ex, object obj = null)
        {
            try
            {
                LogError error = new LogError();

                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame?.GetFileLineNumber();

                //monta o objeto com as informações do log
                error.AddDate = DateTime.Now;
                error.Line = line ?? 0;
                error.Method = frame?.GetMethod().Name;
                error.Controller = frame?.GetMethod().DeclaringType?.Name;
                error.Object = obj?.GetType() != typeof(string) ? SerializationHelper.ToJson(obj).ToString() : "";
                error.Object = error.Object.Substring(0, error.Object.Length > 900 ? 900 : error.Object.Length);
                error.StackTrace = ex.ToClient();
                error.StackTrace = error.StackTrace.Substring(0, error.StackTrace.Length > 900 ? 900 : error.StackTrace.Length);

                _repository.Add(error);
            }
            catch (Exception e)
            {
                try
                {
                    Register(e);
                }
                catch { }
            }

        }

    }
}
