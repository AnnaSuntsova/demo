using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
	public class Container
	{
        private IDictionary<Type, Type> _types;

		public void AddAssembly(Assembly assembly)
		{
            Type[] types = assembly.GetExportedTypes();
            foreach (var type in types)
                AddType(type, type);
        }

		public void AddType(Type type)
		{
            _types.Add(type, type);
        }

		public void AddType(Type type, Type baseType)
		{
            _types.Add(baseType, type);

        }

		public object CreateInstance(Type type)
        {
            return GetInstance(type);
        }

        private object GetInstance(Type type)
        {
            ConstructorInfo constructorInfo = GetConstructor(type);
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            var parameterOfConstructor = new List<object>();
            foreach (var parameter in parameters)
                parameterOfConstructor.Add(parameter.ParameterType);
            object instance = Activator.CreateInstance(type, parameterOfConstructor);
            return instance;
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length==0)
            {
                throw new Exception("There are no constructors");
            }
            return constructors.First();   
        }

        public T CreateInstance<T>()
		{
            var type = typeof(T);
            return (T)GetInstance(type);
        }

		public void Sample()
		{
			var container = new Container();
			container.AddAssembly(Assembly.GetExecutingAssembly());

			var customerBLL = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));
			var customerBLL2 = container.CreateInstance<CustomerBLL>();

			container.AddType(typeof(CustomerBLL));
			container.AddType(typeof(Logger));
			container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
		}
	}
}
