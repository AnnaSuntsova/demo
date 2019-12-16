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
        private IDictionary<Type, Type> _types = new Dictionary<Type, Type>();

		public void AddAssembly(Assembly assembly)
		{
            Type[] types = assembly.GetExportedTypes();
            foreach (var type in types)
            {
                var constImportAttribute = type.GetCustomAttribute<ImportConstructorAttribute>();
                if (constImportAttribute != null)
                {
                    _types.Add(type, type);
                }
                else
                {
                    throw new NoConstructorAttributes("No constructor attributes for import");
                }

                if (GetProperties (type))
                {
                    _types.Add(type, type);
                }

                var exportAttributes = type.GetCustomAttributes<ExportAttribute>();
                if (exportAttributes == null)
                {
                    throw new NoConstructorAttributes("No constructor attributes for export");
                }
                foreach (var exportAttribute in exportAttributes)
                {
                    _types.Add(type, type);
                }
            }                
        }

        private bool GetProperties(Type type)
        {
            var property = type.GetProperties().Where(p => p.GetCustomAttributes<ImportAttribute>() != null);
            return property.Any();
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
                throw new ConstructorNotFoundException("There are no constructors");
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
