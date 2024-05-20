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

namespace ElectricityDefect.Domain.Services
{

    public partial class DBEntities : DbContext
    {
        public DBEntities()
            : base("name=OracleDbContext")        {
            //"name=OracleDbContext"
        }

        public virtual DbSet<OBEKT> OBEKTs { get; set; }
        
    }

    public class EFACRepository : IACRepository, IDisposable
    {
        private DBEntities db;
        private bool disposed = false;
        public EFACRepository()
        {
            db = new DBEntities();
        }
        public IQueryable<OBEKT> OBEKTS
        {
            get
            {
                return db.OBEKTs;
            }
        }


        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
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
