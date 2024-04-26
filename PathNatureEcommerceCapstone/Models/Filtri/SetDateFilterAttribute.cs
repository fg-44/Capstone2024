using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PathNatureEcommerceCapstone.Models.Filtri
{
        public class SetDateFilterAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.ActionParameters.ContainsKey("model"))
                {
                    var model = filterContext.ActionParameters["model"];

                    var createdDateProperty = model.GetType().GetProperty("CreatedDate");
                    if (createdDateProperty != null && createdDateProperty.CanWrite)
                    {
                        createdDateProperty.SetValue(model, DateTime.Now);
                    }

                    var modifiedDateProperty = model.GetType().GetProperty("ModifiedDate");
                    if (modifiedDateProperty != null && modifiedDateProperty.CanWrite)
                    {
                        modifiedDateProperty.SetValue(model, DateTime.Now);
                    }
                }

                base.OnActionExecuting(filterContext);
            }
        }
}
