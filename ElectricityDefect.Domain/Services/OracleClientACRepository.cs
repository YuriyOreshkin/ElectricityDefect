using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ElectricityDefect.Domain.Entities;
using System.Data.Entity.Core.Objects;
using System.Data.OracleClient;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;
using System.Text.RegularExpressions;


namespace ElectricityDefect.Domain.Services
{

    
   

    public class OracleClientACRepository : IACRepository, IDisposable
    {
        OracleConnection conn;
        private bool disposed = false;
        public OracleClientACRepository(string _connectionString)
        {

            string connectionString = _connectionString; //"User Id=CNT;Password=CNT;Data Source=10.48.157.71:1521/orcl";
            conn = new OracleConnection(connectionString);
            conn.Open();
            //SetSchema("CNT");
        }

        private void SetSchema(string schema)
        {
            string query = "ALTER SESSION SET CURRENT_SCHEMA="+schema;
            OracleCommand command = new OracleCommand(query, conn);
            command.ExecuteNonQuery();
        }
        public IQueryable<OBEKT> GetOBEKTs()
        {
                List<OBEKT> obekts = new List<OBEKT>();
                string query = "SELECT N_OB,TXT,(select Count(N_SH) from SH where n_ob=OBEKT.N_OB) SHS FROM OBEKT WHERE SYB_RNK=3";
                OracleCommand command = new OracleCommand(query, conn);
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var obekt = new OBEKT
                    {
                        N_OB = reader.GetInt64(0),
                        TXT = reader.GetString(1),
                        SHS =reader.GetInt32(2)
                    };
                    obekts.Add(obekt);
                }
                
                reader.Close();

                return obekts.AsQueryable();
            
        }
        public IQueryable<SH> GetSHs(long[] n_obs)
        {
                List<SH> result = new List<SH>();
                string query = "SELECT SH.N_SH,SH.N_OB,FID.TXT FROM SH LEFT OUTER JOIN FID on FID.N_FID=SH.N_FID and FID.N_OB=SH.N_OB ";
                if (n_obs != null && n_obs.Count()>0) 
                {
                    query += " WHERE SH.N_OB in (" + String.Join(",",n_obs.Select(n=>n.ToString()))+")";
                }
                OracleCommand command = new OracleCommand(query, conn);
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var sh = new SH
                    {
                        N_SH= reader.GetInt64(0),
                        N_OB = reader.GetInt64(1),
                        TXT = reader.GetString(2)
                    };
                    result.Add(sh);
                }

                reader.Close();

                return result.AsQueryable();
            
        }
        public IQueryable<TEST_SETI> GetTestByPeriod(long[] n_shs,DateTime begin, DateTime end)
        {
                List<TEST_SETI> result = new List<TEST_SETI>();
            string query = "SELECT TEST.N_SH,TESTTIME,UA,UB,UC,FID.TXT,SH.N_OB,OBEKT.TXT FROM (SELECT N_SH,TESTTIME,UA,UB,UC FROM TEST_SETI " +
                            " WHERE TESTTIME>=:begin AND TESTTIME<=:end";
                               
            if (n_shs != null )
            {
                if (n_shs.Count() > 0)
                {
                    query += " AND N_SH in (" + String.Join(",", n_shs.Select(n => n.ToString())) + ")";
                }
                else
                {
                    return result.AsQueryable();
                }
            }

            query += ") TEST LEFT OUTER JOIN SH on SH.N_SH = TEST.N_SH" +
                     " LEFT OUTER JOIN FID on FID.N_FID = SH.N_FID and FID.N_OB = SH.N_OB" +
                     " LEFT OUTER JOIN OBEKT on OBEKT.N_OB = SH.N_OB";

            OracleCommand command = new OracleCommand(query, conn);
                command.Parameters.Add(new OracleParameter("begin", begin));
                command.Parameters.Add(new OracleParameter("end", end));
            OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var test = FromReader(reader);
                result.Add(test);
                }

                reader.Close();

              return result.AsQueryable();
            
        }
        private void FasaConditions(dynamic lim, short i,ref List<string> upar)
        {
            var fasa = "ABC";
                
                if (lim != null)
                {
                    upar.Add(String.Format("U{0} is null", fasa[i - 1]));
                    var min = lim.MIN;
                    if (min != null)
                    {
                        upar.Add(String.Format("U{0}<{1}", fasa[i - 1], min.ToString().Replace(",", ".")));
                    }
                    var max = lim.MAX;
                    if (max != null)
                    {
                        upar.Add(String.Format("U{0}>{1}", fasa[i - 1], max.ToString().Replace(",", ".")));
                    }

                }
            
        }

        public IQueryable<TEST_SETI> GetDefects(IEnumerable<ELECTRICITYLIMITS> limits) 
        {
            List<TEST_SETI> result = new List<TEST_SETI>();
            List<string> upar = new List<string>();
            
            foreach (var limit in limits.GroupBy(n => n.N_SH).Select(g => new { N_SH = g.Key, LIMITS = g.Select(p => new { FASA = p.FASA, MIN = p.UMIN, MAX = p.UMAX }) }))
            {
                string query =
                     "SELECT TEST.N_SH,TESTTIME,UA,UB,UC,FID.TXT,SH.N_OB,OBEKT.TXT FROM (SELECT SETI.N_SH, SETI.TESTTIME, UA, UB, UC FROM TEST_SETI SETI, (SELECT N_SH, MAX(TESTTIME) TESTTIME FROM TEST_SETI" +
                     " WHERE N_SH = :n_sh";

                upar.Clear();
                for (short i = 1; i <= 3; i++)
                {
                    var lim = limit.LIMITS.FirstOrDefault(l => l.FASA == i);
                    FasaConditions(lim,i, ref upar);
                }
                query += upar.Count() > 0 ? " AND (" + String.Join(" OR ", upar) + ")" : "";
                query += " GROUP BY N_SH) DEF WHERE SETI.N_SH = DEF.N_SH AND SETI.TESTTIME = DEF.TESTTIME) TEST " +
                        " LEFT OUTER JOIN SH on SH.N_SH = TEST.N_SH" +
                        " LEFT OUTER JOIN FID on FID.N_FID = SH.N_FID and FID.N_OB = SH.N_OB" +
                        " LEFT OUTER JOIN OBEKT on OBEKT.N_OB = SH.N_OB";

                OracleCommand command = new OracleCommand(query, conn);
                command.Parameters.Add(new OracleParameter("n_sh", limit.N_SH));
               
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var test = FromReader(reader);
                    result.Add(test);
                }

                reader.Close();
            }

            return result.AsQueryable();
        }

        public IQueryable<TEST_SETI> GetTestOutOfRange(IEnumerable<ELECTRICITYLIMITS> limits,DateTime begin, DateTime end)
        {
            List<TEST_SETI> result = new List<TEST_SETI>();
            List<string> upar = new List<string>();
            
            foreach (var limit in limits.GroupBy(n=>n.N_SH).Select(g=> new { N_SH =g.Key, LIMITS= g.Select(p=> new { FASA=p.FASA, MIN=p.UMIN , MAX= p.UMAX }) }))
            {
                string query = "SELECT SETI.N_SH,TESTTIME,UA,UB,UC,FID.TXT,SH.N_OB,OBEKT.TXT FROM TEST_SETI SETI" +
                    " LEFT OUTER JOIN SH on SH.N_SH = SETI.N_SH"+
                    " LEFT OUTER JOIN FID on FID.N_FID = SH.N_FID and FID.N_OB = SH.N_OB"+
                    " LEFT OUTER JOIN OBEKT on OBEKT.N_OB = SH.N_OB"+
                    " WHERE TESTTIME>=:begin AND TESTTIME<=:end" +
                    " AND SETI.N_SH =:n_sh";
                upar.Clear();
                for (short i= 1; i<= 3; i++)
                {
                    var lim = limit.LIMITS.FirstOrDefault(l => l.FASA == i);
                    FasaConditions(lim, i, ref upar);
                }
            

                query += upar.Count()>0 ? " AND ("+String.Join(" OR ", upar)+")" : "";


                OracleCommand command = new OracleCommand(query, conn);
                command.Parameters.Add(new OracleParameter("n_sh", limit.N_SH));
                command.Parameters.Add(new OracleParameter("begin", begin));
                command.Parameters.Add(new OracleParameter("end", end));
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var test = FromReader(reader);
                    result.Add(test);
                }

                reader.Close();
            }

            return result.AsQueryable();

        }

        private TEST_SETI FromReader(OracleDataReader reader)
        {
            return new TEST_SETI
            {
                N_SH = reader.GetInt64(0),
                TESTTIME = reader.GetDateTime(1),
                UA = reader.IsDBNull(2) ? 0 : reader.GetDouble(2),
                UB = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                UC = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                SH = new SH
                {
                    N_SH = reader.GetInt64(0),
                    N_OB = reader.GetInt64(6),
                    TXT = reader.GetString(5),
                    OBEKT = new OBEKT
                    {
                        N_OB = reader.GetInt64(6),
                        TXT = reader.GetString(7)
                    }
                }
            };
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

      

        public SH GetSH(long n_sh)
        {
            string query = "SELECT SH.N_SH,SH.N_OB,FID.TXT,OBEKT.TXT FROM SH LEFT OUTER JOIN FID on FID.N_FID=SH.N_FID and FID.N_OB=SH.N_OB  LEFT OUTER JOIN OBEKT on OBEKT.N_OB=SH.N_OB WHERE SH.N_SH =:n_sh";
            
            OracleCommand command = new OracleCommand(query, conn);
            command.Parameters.Add(new OracleParameter("n_sh", n_sh));
            OracleDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                return  new SH
                {
                    N_SH = reader.GetInt64(0),
                    N_OB = reader.GetInt64(1),
                    TXT = reader.GetString(2),
                    OBEKT = new OBEKT { 
                        N_OB = reader.GetInt64(1),
                        TXT = reader.GetString(3)
                    }
                };
              
            }

            reader.Close();

            return null;
        }
    }
}
