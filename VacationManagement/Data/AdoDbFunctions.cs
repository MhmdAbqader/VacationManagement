using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace VacationManagement.Data
{
    public static class AdoDbFunctions
    {
        public static DataTable SqlDataTableMyExtensionMethod(this DbContext context , string query) 
        {

            DbConnection _cn = context.Database.GetDbConnection();
            using(var cmd = _cn.CreateCommand()) 
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
            

            var dt = new DataTable();

            if(_cn.State.Equals(ConnectionState.Closed))
                _cn.Open();

            using (var reader = cmd.ExecuteReader())
            {
                    dt.Load(reader);
            }

            if (_cn.State.Equals(ConnectionState.Open))
                _cn.Close();

            return dt;
            }
        }
    }
}
