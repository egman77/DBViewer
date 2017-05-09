using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace DBViewer.Model.Core
{
    internal class ModelInfo
    {
        public string AssemblyFile;
        public string ClassName;

        public ModelInfo(string assembly,string className)
        {
            this.AssemblyFile = assembly;
            this.ClassName = className;
        }

        public object CreateModel()
        {
            Assembly assembly = Assembly.LoadFile(Path.Combine(Application.StartupPath ,AssemblyFile));
            return assembly.CreateInstance(ClassName);
        }
    }
}
