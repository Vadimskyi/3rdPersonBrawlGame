using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Utils
{
    public static class Services
    {
        private static Dictionary<Type, object> _services;

        static Services()
        {
            _services = new Dictionary<Type, object>();
        }

        public static void RegisterService<T>(T service)
        {
            _services.Add(typeof(T), service);
        }

        public static T Get<T>()
        {
            try
            {
                return (T)_services[typeof(T)];
            }
            catch (Exception e)
            {
                throw new NotImplementedException("Service not available!");
            }
        }
    }
}
