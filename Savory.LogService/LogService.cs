using Savory.LogService.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Savory.LogService.Contract.WriteLog;
using Savory.LogService.Repository;
using Savory.LogService.Contract.WriteLog.Request;
using Savory.LogService.Repository.Entity;
using System.Data.SqlClient;
using System.Configuration;
using System.ServiceModel;
using System.IO;

namespace Savory.LogService
{
    public class LogService : ServiceBase, ILogService
    {
        ServiceHost host = null;

        LogEventRepository logEventRepository;

        public LogService()
        {
            logEventRepository = new LogEventRepository();
        }

        #region ServiceBase Members
        protected override void OnStart(string[] args)
        {
            try
            {
                host = new ServiceHost(this.GetType());

                host.Open();
            }
            catch (Exception ex)
            {
                string fileName = string.Format("OnStartException_{0:yyyyMMddHHmmss}", DateTime.Now);

                File.WriteAllText(fileName, ex.ToString());
            }
        }

        protected override void OnStop()
        {
            try
            {
                host.Close();
            }
            catch (Exception ex)
            {
                string fileName = string.Format("OnStopException_{0:yyyyMMddHHmmss}", DateTime.Now);

                File.WriteAllText(fileName, ex.ToString());
            }
        }
        #endregion

        #region ILogService Members

        public WriteLogResponse WriteLog(WriteLogRequest writeLogRequest)
        {
            WriteLogResponse response = new WriteLogResponse();

            if (writeLogRequest != null)
            {
                response.Guid = writeLogRequest.Guid;

                if (writeLogRequest.LogEvent != null)
                {
                    var entity = ToEntity(writeLogRequest.Guid, writeLogRequest.LogEvent);

                    using (var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SavoryLogDB"].ConnectionString))
                    {
                        sqlConn.Open();

                        logEventRepository.Insert(entity, sqlConn);
                    }
                }
            }

            return response;
        }

        #endregion

        private LogEventEntity ToEntity(Guid guid, LogEvent logEvent)
        {
            LogEventEntity entity = new LogEventEntity { Guid = guid };

            if (logEvent != null)
            {
                if (logEvent.Header != null)
                {
                    entity.AppId = logEvent.Header.AppId;
                    entity.LogTypeId = (int)logEvent.Header.LogEventType;
                    entity.Source = logEvent.Header.Source;
                    entity.CreateTime = logEvent.Header.CreateTime;
                }

                if (logEvent.Body != null)
                {
                    entity.Title = logEvent.Body.Title;
                    entity.Content = logEvent.Body.Detail;
                }
            }

            return entity;
        }
    }
}
