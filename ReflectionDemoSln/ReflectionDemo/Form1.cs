using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReflectionDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ListExecutingAssemblies();
            ListExecutingAssembliesThroughReflection();

        }

        private void ListExecutingAssembliesThroughReflection()
        {
            try
            {
                var currentFormType = this.GetType();
                currentFormType.InvokeMember("ListExecutingAssemblies", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null,
                    this, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ListExecutingAssemblies()
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly singleLoadedAssembly in loadedAssemblies.AsQueryable().OrderBy(p=>p.GetName().Name))
            {
                var assemblyNode = 
                    trvAssemblies.Nodes.Add(singleLoadedAssembly.GetName().Name);
                foreach (var singleTypeInAssembly in singleLoadedAssembly.GetTypes().AsQueryable().OrderBy(p=>p.Name))
                {
                    var typeNode = assemblyNode.Nodes.Add(singleTypeInAssembly.Name);
                    var propertyNode = typeNode.Nodes.Add("Properties");
                    var methodsNode = typeNode.Nodes.Add("Methods");
                    foreach (var singlePropertyInType in singleTypeInAssembly.GetProperties().OrderBy(p=>p.Name))
                    {
                        propertyNode.Nodes.Add(singlePropertyInType.Name);
                    }
                    foreach (var singleMethodInType in singleTypeInAssembly.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic |
                        BindingFlags.Public).OrderBy(p=>p.Name))
                    {
                        var singleMethodNode = methodsNode.Nodes.Add(singleMethodInType.Name);
                        var methodParamethersNode = singleMethodNode.Nodes.Add("Parameters");
                        foreach (var singleParameter in singleMethodInType.GetParameters())
                        {
                            string parameterInfo = string.Format("{0},{1}", singleParameter.Name, singleParameter.ParameterType);
                            methodParamethersNode.Nodes.Add(parameterInfo);
                        }
                        
                    }
                }
            }
        }
    }
}
