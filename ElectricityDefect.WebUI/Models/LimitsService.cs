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
    public class LimitsService 
    {

        private IRepository entities;
       
        private IACRepository ac;
        public LimitsService(IRepository _entities, IACRepository _ac)
        {
            entities = _entities;
            ac = _ac;
        }


        private LimitsSetiViewModel ConvertToViewModel(long n_sh,IEnumerable<ELECTRICITYLIMITS> limits, LimitsSetiViewModel view)
        {
            var sh = ac.GetSH(n_sh);
            view.name_sh = sh != null ? sh.TXT: "";
            view.n_ob = sh != null ? sh.N_OB : 0;
            view.name_ob = sh != null ? sh.OBEKT != null ? sh.OBEKT.TXT: "" : "";
            view.n_sh = n_sh;
            view.uamax = limits.Any(f => f.FASA == 1) ? limits.Where(f => f.FASA == 1).FirstOrDefault().UMAX : (double?)null ;
            view.uamin = limits.Any(f => f.FASA == 1) ? limits.Where(f => f.FASA == 1).FirstOrDefault().UMIN : (double?)null;
            view.ubmax = limits.Any(f => f.FASA == 2) ? limits.Where(f => f.FASA == 2).FirstOrDefault().UMAX : (double?)null;
            view.ubmin = limits.Any(f => f.FASA == 2) ? limits.Where(f => f.FASA == 2 ).FirstOrDefault().UMIN : (double?)null;
            view.ucmax = limits.Any(f => f.FASA == 3) ? limits.Where(f => f.FASA == 3).FirstOrDefault().UMAX : (double?)null;
            view.ucmin = limits.Any(f => f.FASA == 3) ? limits.Where(f => f.FASA == 3).FirstOrDefault().UMIN : (double?)null;
            
            return view;

        }
       

        public IList<LimitsSetiViewModel> GetAll()
        {
            IList<LimitsSetiViewModel> result  = entities.GetLIMITS().ToList().GroupBy(g=>g.N_SH).Select(s=> new { N_SH = s.Key, LIMITS=s.Select(p=>p)  }).Select(l => ConvertToViewModel(l.N_SH,l.LIMITS, new LimitsSetiViewModel())).ToList();

            return result;
        }
        public LimitsSetiViewModel GetSHLimits(long n_sh)
        {
            LimitsSetiViewModel  result = entities.GetLIMITS().Where(l=>l.N_SH == n_sh).ToList().GroupBy(g => g.N_SH).Select(s => new { N_SH = s.Key, LIMITS = s.Select(p => p) }).Select(l => ConvertToViewModel(l.N_SH, l.LIMITS, new LimitsSetiViewModel())).ToList().FirstOrDefault();
            return result;
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
   