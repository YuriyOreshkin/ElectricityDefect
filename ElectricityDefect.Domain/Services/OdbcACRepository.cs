using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ElectricityDefect.Domain.Entities;
using System.Data.Entity.Core.Objects;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.Odbc;

namespace ElectricityDefect.Domain.Services
{

   

    public class OdbcACRepository : IACRepository, IDisposable
    {
      
        private bool disposed = false;
        
        public IQueryable<OBEKT> OBEKTS
        {
            get
            {
                List<OBEKT> obekts = new List<OBEKT>();
                string connectionString = "DSN=71;";
                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    
                    string query = "SELECT N_OB,TXT FROM OBEKT";
                    OdbcCommand command = new OdbcCommand(query, conn);
                    conn.Open();
                    OdbcDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var obekt = new OBEKT
                        {
                            N_OB = reader.GetInt64(0),
                            TXT = reader.GetString(1)
                        };
                        obekts.Add(obekt);
                    }

                    reader.Close();
                }
                return obekts.AsQueryable();
            }
        }


        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                   
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

      
    }
}
