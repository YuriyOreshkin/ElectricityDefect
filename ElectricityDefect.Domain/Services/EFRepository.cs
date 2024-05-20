using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using ElectricityDefect.Domain.Entities;

namespace ElectricityDefect.Domain.Services
{

    public partial class DBEntities : DbContext
    {
        public DBEntities()
            : base("name=EDEntities")
        {
        }

        public virtual DbSet<ELECTRICITYLIMITS> LIMITs { get; set; }
        public virtual DbSet<INCIDENT> INCIDENTs { get; set; }
        //public virtual DbSet<ORDER> Orders { get; set; }

        //public virtual DbSet<PAYMENT> Payments { get; set; }

        //public virtual DbSet<USER> Users { get; set; }
        //public virtual DbSet<ROLE> Roles { get; set; }
    }

    public class EFRepository : IRepository, IDisposable
    {
        private DBEntities db = new DBEntities();
        private bool disposed = false;
        public IQueryable<ELECTRICITYLIMITS> GetLIMITS()
        {
              return db.LIMITs;
            
        }

        public IQueryable<INCIDENT> GetINCIDENTS()
        {
            return db.INCIDENTs;
        }


        public void Add(ELECTRICITYLIMITS limit)
        {
            db.LIMITs.Add(limit);
           
        }


        public void Delete(ELECTRICITYLIMITS limit)
        {
            db.LIMITs.Remove(limit);
            
        }

        public void Update(ELECTRICITYLIMITS limit)
        {
            db.Entry(limit).State = EntityState.Modified;
           
        }
        public void Add(INCIDENT incident)
        {
            db.INCIDENTs.Add(incident);
            db.SaveChanges();

        }

        public void Update(INCIDENT incident)
        {
            db.Entry(incident).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(INCIDENT incident)
        {
            db.INCIDENTs.Remove(incident);
            db.SaveChanges();
        }
        public void SaveChanges()
        {
            db.SaveChanges();

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
