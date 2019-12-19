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
                if (GetProperties(type).Any())
                {
                    _types.Add(type, type);
                }
                var exportAttributes = type.GetCustomAttributes<ExportAttribute>();
                foreach (var exportAttribute in exportAttributes)
                {
                    if (exportAttribute.Contract != null)
                    {
                        _types.Add(exportAttribute.Contract, type);
                    }
                    else
                    {
                        _types.Add(type, type);
                    }
                }
            }
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties().Where(p => p.GetCustomAttributes<ImportAttribute>() != null);
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
            if (!_types.ContainsKey(type))
                throw new TypeNotRegisteredException();
            Type dependType = _types[type];
            ConstructorInfo constructorInfo = GetConstructor(dependType);
            object instance = CreateFromConstructor(dependType, constructorInfo);

            if (dependType.GetCustomAttribute<ImportConstructorAttribute>() != null)
            {
                return instance;
            }

            var propertiesInfo = GetProperties(dependType);
            foreach (var property in propertiesInfo)
            {
                var resProperty = GetInstance(property.PropertyType);
                property.SetValue(instance, resProperty);
            }
            return instance;
        }

        private object CreateFromConstructor(Type dependType, ConstructorInfo constructorInfo)
        {
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            List<object> parametersInstances = new List<object>(parameters.Length);
            foreach (var param in parameters)
                parametersInstances.Add(GetInstance(param.ParameterType));
            object instance = Activator.CreateInstance(dependType, parametersInstances.ToArray());
            return instance;
        }

        private ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
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
