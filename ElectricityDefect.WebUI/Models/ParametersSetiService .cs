using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.Common.CommandTrees.ExpressionBuilder;
using System.Runtime;
using System.IO;
using ElectricityDefect.Domain.Services;
using ElectricityDefect.Domain.Entities;
using System.Web.UI.WebControls;
using System.Web.Http.Results;

namespace ElectricityDefect.WebUI.Models
{
    public class ParametersSetiService
    {

        private IRepository entities;
       
        private IACRepository ac_repo;
        public ParametersSetiService(IACRepository _ac_repo, IRepository _entities)
        {
            entities = _entities;
            ac_repo = _ac_repo;
        }

        private TreePointViewModel ConvertToViewModel(OBEKT obekt)
        {
            return new TreePointViewModel
            {
                id = -obekt.N_OB,
                name = String.Format("{0} - {1} {2}", obekt.N_OB.ToString(), obekt.TXT, obekt.SHS > 0 ? "("+obekt.SHS.ToString()+")":""),
                hasChildren = obekt.SHS > 0
            };
        }
        private TreePointViewModel ConvertToViewModel(SH sh)
        {
            return new TreePointViewModel
            {
                id = sh.N_SH,
                name = String.Format("{0} - {1}", sh.N_SH.ToString(), sh.TXT),
                hasChildren = false
            };
        }

     

        public IQueryable<FullTestSetiViewModel> GetTestsOutOfRange(long[] points, DateTime datebegin, DateTime dateend)
        {
            var result = Enumerable.Empty<FullTestSetiViewModel>();
            var shs = GetObjectsSHs(points);
            var limits = entities.GetLIMITS().Where(w => shs.Contains(w.N_SH));
            result =FullTestSetiViewModel(ac_repo.GetTestOutOfRange(limits, datebegin, dateend).ToList());
            return result.AsQueryable();
        }
        public IQueryable<FullTestSetiViewModel> GetDefects()
        {
           
            var defects = new List<TEST_SETI>();
            var limits = entities.GetLIMITS().ToList();
            if (limits.Count() > 0)
            {
                ac_repo.GetDefects(limits).ToList().ForEach(o =>
                {
                    if (!entities.GetINCIDENTS().Any(n => n.N_SH == o.N_SH && o.TESTTIME >= n.BEGIN && o.TESTTIME <= n.END))
                    {
                        defects.Add(o);
                    }
                }); 
            }
            var result = FullTestSetiViewModel(defects);

            return result.AsQueryable();
        }

        public IncidentViewModel GetIncident(long n_sh,DateTime testtime)
        {
            return entities.GetINCIDENTS().Where(w => w.N_SH == n_sh && (testtime >= w.BEGIN && testtime <= w.END)).Select(s=> new IncidentViewModel() { id=s.id, comment = s.COMMENT, begin =s.BEGIN, end=s.END }).FirstOrDefault();
        }

        private long[] GetObjectsSHs(long[] points) 
        {
            var shs = points.Where(p => p > 0).ToArray();
            var objekts = points.Where(p => p < 0).Select(s => Math.Abs(s));
            if (objekts.Count() > 0)
            {
                shs = shs.Concat(ac_repo.GetSHs(objekts.ToArray()).Select(s => s.N_SH).ToArray()).Distinct().ToArray();
            }

            return shs;
        }

        private IEnumerable<FullTestSetiViewModel> FullTestSetiViewModel(IEnumerable<TEST_SETI> tests)
        {
            //var query = from test in tests
            //            join lims in entities.GetLIMITS().GroupBy(n => n.N_SH).Select(g => new { N_SH = g.Key, LIMITS = g.Select(p => new { FASA = p.FASA, MIN = p.UMIN, MAX = p.UMAX }) }) on test.N_SH equals lims.N_SH
            //            into groups
            //            from gr in groups.DefaultIfEmpty()
            //            select new FullTestSetiViewModel(test)
            //            {
            //                uamin = gr != null ? gr.LIMITS != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 1) != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 1).MIN : null: null : null,
            //                uamax = gr != null ? gr.LIMITS != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 1) != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 1).MAX : null : null : null,
            //                ubmin = gr != null ? gr.LIMITS != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 2) != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 2).MIN : null : null : null,
            //                ubmax = gr != null ? gr.LIMITS != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 2) != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 2).MAX : null : null : null,
            //                ucmin = gr != null ? gr.LIMITS != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 3) != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 3).MIN : null : null : null,
            //                ucmax = gr != null ? gr.LIMITS != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 3) != null ? gr.LIMITS.FirstOrDefault(f => f.FASA == 3).MAX : null : null : null,
            //            };

            //return query;

            var query = from test in tests
                        from lims in entities.GetLIMITS().Where(n => n.N_SH == test.N_SH).GroupBy(n => n.N_SH).Select(g => new { N_SH = g.Key, LIMITS = g.Select(p => new { FASA = p.FASA, MIN = p.UMIN, MAX = p.UMAX }) }).DefaultIfEmpty()
                        from inc in entities.GetINCIDENTS().Where(i => i.N_SH == test.N_SH && i.BEGIN <= test.TESTTIME && test.TESTTIME <= i.END).DefaultIfEmpty()
                        select new FullTestSetiViewModel(test)
                        {
                            uamin = lims != null ? lims.LIMITS != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 1) != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 1).MIN : null : null : null,
                            uamax = lims != null ? lims.LIMITS != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 1) != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 1).MAX : null : null : null,
                            ubmin = lims != null ? lims.LIMITS != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 2) != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 2).MIN : null : null : null,
                            ubmax = lims != null ? lims.LIMITS != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 2) != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 2).MAX : null : null : null,
                            ucmin = lims != null ? lims.LIMITS != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 3) != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 3).MIN : null : null : null,
                            ucmax = lims != null ? lims.LIMITS != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 3) != null ? lims.LIMITS.FirstOrDefault(f => f.FASA == 3).MAX : null : null : null,
                            incident = inc != null
                        };

            return query;

        }
        public IQueryable<FullTestSetiViewModel> GetTestByPeriod(long[] points, DateTime datebegin, DateTime dateend)
        {
            var result = Enumerable.Empty<FullTestSetiViewModel>();
            var shs = GetObjectsSHs(points);

            result =FullTestSetiViewModel( ac_repo.GetTestByPeriod(shs, datebegin, dateend).ToList());

            return result.AsQueryable();
        }

        public IEnumerable<TreePointViewModel> GetTreePoints(int? id)
        {
            var result = Enumerable.Empty<TreePointViewModel>();

            if (!id.HasValue)
            {
                result = ac_repo.GetOBEKTs().OrderBy(o => o.N_OB).Select(s => ConvertToViewModel(s));

            }
            else
            {

                result = ac_repo.GetSHs(new long[] { Math.Abs((long)id) }).OrderBy(o => o.N_SH).Select(o => ConvertToViewModel(o));
            }
            return result;
        }

        public IEnumerable<ObjektViewModel> GetOBEKTs() 
        { 
            return ac_repo.GetOBEKTs().Select(s => new ObjektViewModel(s));
        }

        public IEnumerable<SHViewModel> GetSHs(long[] n_obs) 
        {
           return ac_repo.GetSHs(n_obs).Select(s => new SHViewModel(s));
        }

        private void ModifyLimit(long n_sh,short fasa,double? umin, double? umax) 
        {
            var limit = entities.GetLIMITS().Where(w => w.N_SH == n_sh && w.FASA == fasa).FirstOrDefault();
            if (limit == null) 
            {
                if (umin != null || umax != null)
                {
                    entities.Add(new ELECTRICITYLIMITS { N_SH = n_sh, FASA = fasa, UMIN = umin, UMAX = umax });
                }
            }
            else
            {
                if( umin == null &&  umax == null )
                {
                    entities.Delete(limit);

                }
                else
                {
                    limit.UMAX = umax;
                    limit.UMIN = umin;
                    entities.Update(limit);
                }
            }
        }


        public void Modify(LimitsSetiViewModel view, ModelStateDictionary modelState)
        {
            if (ValidateModel(view, modelState))
            {

                ModifyLimit(view.n_sh, 1, view.uamin, view.uamax);
                ModifyLimit(view.n_sh, 2, view.ubmin, view.ubmax);
                ModifyLimit(view.n_sh, 3, view.ucmin, view.ucmax);
                entities.SaveChanges();
            }
        }

        public void Delete(LimitsSetiViewModel view, ModelStateDictionary modelState)
        {

            var limits = entities.GetLIMITS().Where(l => l.N_SH == view.n_sh);

            foreach (var limit in limits) 
            {
                entities.Delete(limit);
            } 
                entities.SaveChanges();
            
        }


       

        private bool ValidateModel(LimitsSetiViewModel view, ModelStateDictionary modelState)
        {
            return true;
        }



        public void Dispose()
        {
            entities.Dispose();
        }


    }
}
   