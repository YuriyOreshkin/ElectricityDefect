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
using System.Web.Helpers;
using System.Web.Http.Results;

namespace ElectricityDefect.WebUI.Models
{
    public class IncidentService
    {

        private IRepository entities;
       
        private IACRepository ac;
        public IncidentService(IRepository _entities, IACRepository _ac)
        {
            entities = _entities;
            ac = _ac;
        }


        public IncidentViewModel ConvertToViewModel(INCIDENT incident, IncidentViewModel view)
        {
            var sh = ac.GetSH(incident.N_SH);
            view.name_sh = sh != null ? sh.TXT: "";
            view.n_ob = sh != null ? sh.N_OB : 0;
            view.name_ob = sh != null ? sh.OBEKT != null ? sh.OBEKT.TXT: "" : "";
            view.n_sh = incident.N_SH;
            view.begin = incident.BEGIN;
            view.end = incident.END;
            view.comment = incident.COMMENT;
            view.id = incident.id;
            
            return view;

        }
       

        public IList<IncidentViewModel> GetAll()
        {
            IList<IncidentViewModel> result  = entities.GetINCIDENTS().ToList().Select(i => ConvertToViewModel(i, new IncidentViewModel())).ToList();

            return result;
        }


        public void Create(IncidentViewModel view, ModelStateDictionary modelState)
        {
            if (ValidateModel(view, modelState))
            {
                var entity = view.ToEntity(new INCIDENT());
                entities.Add(entity);
                view.id = entity.id;
                view = ConvertToViewModel(entity, view);
            }
        }

        public void Update(IncidentViewModel view, ModelStateDictionary modelState)
        {
            if (ValidateModel(view, modelState))
            {

                var entity = entities.GetINCIDENTS().Where(i => i.id == view.id).FirstOrDefault();
                if (entity == null)
                {
                    modelState.AddModelError("incident", String.Format("Событие не обнаружено в базе данных!"));
                }
                else
                {
                    //TODO Validate not found
                    entity =view.ToEntity(entity);


                }
                entities.Update(entity);
                
            }
        }

        public void Delete(IncidentViewModel view, ModelStateDictionary modelState)
        {

            var entity = entities.GetINCIDENTS().Where(i => i.id == view.id).FirstOrDefault();
            if (entity == null)
            {
                modelState.AddModelError("incident", String.Format("Событие не обнаружено в базе данных!"));
            }

            else
            {
                //TODO Validate not found
                entities.Delete(entity);

            }



        }




    private bool ValidateModel(IncidentViewModel view, ModelStateDictionary modelState)
        {

            return true;
        }



        public void Dispose()
        {
            entities.Dispose();
        }


    }
}
   