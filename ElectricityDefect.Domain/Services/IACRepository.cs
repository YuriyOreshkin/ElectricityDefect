using ElectricityDefect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDefect.Domain.Services
{
    public interface IACRepository:IDisposable
    {
        IQueryable<OBEKT> GetOBEKTs();

        IQueryable<SH> GetSHs(long[] n_obs);
        SH GetSH(long n_sh);

        IQueryable<TEST_SETI> GetTestByPeriod(long[] n_shs,DateTime begin, DateTime end);

        IQueryable<TEST_SETI> GetTestOutOfRange(IEnumerable<ELECTRICITYLIMITS> limits, DateTime begin, DateTime end);

        IQueryable<TEST_SETI> GetDefects(IEnumerable<ELECTRICITYLIMITS> limits);
    }
}
