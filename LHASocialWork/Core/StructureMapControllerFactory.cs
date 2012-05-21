using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace LHASocialWork.Core
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                base.GetControllerInstance(requestContext, controllerType);
            return ObjectFactory.GetInstance(controllerType) as Controller;
        }
    }
}