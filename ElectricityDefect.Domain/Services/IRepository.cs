using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDefect.Domain.Services
{
    public interface IRepository: IDisposable
    {
        IQueryable<ELECTRICITYLIMITS> GetLIMITS();
        IQueryable<INCIDENT> GetINCIDENTS();
        void Add(ELECTRICITYLIMITS limit);
        void Update(ELECTRICITYLIMITS limit);
        void Delete(ELECTRICITYLIMITS limit);

        void Add(INCIDENT incident);
        void Update(INCIDENT incident);
        void Delete(INCIDENT incident);
        void SaveChanges();
        //IQueryable<ORDER> Orders { get; }
        //void AddOrder(ORDER order);
        //void UpdateOrder(ORDER order);
        //void DeleteOrder(ORDER order);

        //IQueryable<USER> Users { get; }
        //void AddUser(USER user);
        //void UpdateUser(USER user);
        //void DeleteUser(USER user);
        //IQueryable<ROLE> Roles { get; }

        //IQueryable<PAYMENT> Payments { get; }
        //void AddPayment(PAYMENT payment);


    }
}
