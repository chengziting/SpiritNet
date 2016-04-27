using SpiritNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace SpiritNet.Core.FactoryModule
{
    public class FactoryModule:Autofac.Module
    {
        public string Mode
        {
            get;
            set;
        }

        protected override void Load(ContainerBuilder builder)
        {
            CommonFunctions.LoadAssembly(
               (file =>
                   builder.RegisterAssemblyTypes(file)
                       .AsImplementedInterfaces()),
               "SpiritNet.Dal.dll");


            CommonFunctions.LoadAssembly(
               (assembly =>
                   builder.RegisterAssemblyTypes(assembly)
                       .AsImplementedInterfaces()
                       .PropertiesAutowired()),
               "SpiritNet.BLL.dll");
        }
    }
}
