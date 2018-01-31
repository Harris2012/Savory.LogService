using Savory.LogService.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savory.LogService.Repository
{
    public class LogEventRepository
    {
        public void Insert(LogEventEntity entity, SqlConnection sqlConn)
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandText = "InsertLogEvent";
            sqlCmd.CommandType = CommandType.StoredProcedure;

            #region Prepare Parameters
            sqlCmd.Parameters.AddWithValue(@"Guid", entity.Guid);
            sqlCmd.Parameters.AddWithValue(@"AppId", entity.AppId);
            sqlCmd.Parameters.AddWithValue(@"LogTypeId", entity.LogTypeId);
            sqlCmd.Parameters.AddWithValue(@"Source", entity.Source);
            sqlCmd.Parameters.AddWithValue(@"CreateTime", entity.CreateTime);
            sqlCmd.Parameters.AddWithValue(@"Title", entity.Title);
            if (entity.Content == null)
            {
                sqlCmd.Parameters.AddWithValue(@"Content", DBNull.Value);
            }
            else
            {
                sqlCmd.Parameters.AddWithValue(@"Content", entity.Content);
            }

            #endregion

            var count = sqlCmd.ExecuteNonQuery();

            if (count == 0)
            {
                throw new Exception("插入数据失败");
            }
        }
    }
}
