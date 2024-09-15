using System;
using System.Collections.Generic;

namespace Services
{
   
    public interface IService { }

    public class ServiceLocator
    {
        private static ServiceLocator _instance;
        private Dictionary<Type, IService> _services;

        private ServiceLocator()
        {
            _services = new Dictionary<Type, IService>();
        }

        public static ServiceLocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceLocator();
                }
                return _instance;
            }
        }

        public void RegisterService<T>(T service) where T : IService
        {
            _services[typeof(T)] = service;
        }

        public T GetService<T>() where T : IService
        {
            if (_services.TryGetValue(typeof(T), out IService service))
            {
                return (T)service;
            }
            throw new Exception($"Service of type {typeof(T)} not registered.");
        }

        public void UnregisterService<T>() where T : IService
        {
            _services.Remove(typeof(T));
        }
    }
}