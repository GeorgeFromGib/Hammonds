using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace HammondsIBMS_2.Setup
{
    //public class UnityDependencyResolver : IDependencyResolver
    //{
    //    private readonly IUnityContainer _container;

    //    public UnityDependencyResolver(IUnityContainer container)
    //    {
    //        _container = container;
    //    }

    //    public object GetService(Type serviceType)
    //    {
    //        try
    //        {
    //            return _container.Resolve(serviceType);
    //            // if (serviceType.IsAbstract || serviceType.IsInterface)
    //            //{
    //            //    return null;
    //            //}
    //            // return instance;
    //        }
    //        catch (Exception)
    //        {
    //            return null;
    //        }
    //    }

    //    public IEnumerable<object> GetServices(Type serviceType)
    //    {
    //        return _container.ResolveAll(serviceType);
    //    }
    //}
}