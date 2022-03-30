using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TMCreatureEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string file { private set; get; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            foreach (string arg in e.Args)
            {
                file = arg;
                break;
            }
        }
    }
}
