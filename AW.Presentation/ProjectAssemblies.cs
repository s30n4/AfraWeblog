using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AW.Presentation
{
    public class ProjectAssemblies
    {
        public static Assembly DataLayer => Assembly.Load(new AssemblyName("AW.Entities"));

        public static Assembly ServiceLayer => Assembly.Load(new AssemblyName("AW.Application"));
    }
}
