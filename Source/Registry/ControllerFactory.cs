using System;
using System.Collections.Generic;

namespace Pagan.Registry
{
    public class ControllerFactory: IControllerFactory
    {
        private readonly Dictionary<Type, Controller> _controllers;
        private readonly IDbConfiguration _dbConfig;

        public ControllerFactory(IDbConfiguration dbConfig)
        {
            _controllers = new Dictionary<Type, Controller>();
            _dbConfig = dbConfig;
        }

        public Controller<T> GetController<T>()
        {
            Controller controller;
            if (!_controllers.TryGetValue(typeof (T), out controller))
                _controllers[typeof (T)] = controller = new Controller<T>(this, _dbConfig);

            return (Controller<T>) controller;
        }
    }
}