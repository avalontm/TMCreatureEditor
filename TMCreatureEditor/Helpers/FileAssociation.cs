using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace TMCreatureEditor.Helpers
{
    public class FileAssociation
    {
        [DllImport("Kernel32.dll")]
        static extern uint GetShortPathName(string lpszLongPath, [Out] StringBuilder lpszShortPath, uint cchBuffer);

        // Associate file extension with progID, description, icon and application
        public static void Associate(string extension, string progID, string description, string icon, string application)
        {
            try
            {
                Registry.ClassesRoot.CreateSubKey(extension).SetValue("", progID);

                if (!string.IsNullOrEmpty(progID))
                {
                    using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(progID))
                    {
                        if (!string.IsNullOrEmpty(description))
                        {
                            key.SetValue("", description);
                        }
                        if (!string.IsNullOrEmpty(icon))
                        {
                            key.CreateSubKey("DefaultIcon").SetValue("", ToShortPathName(icon));
                        }
                        if (!string.IsNullOrEmpty(application))
                        {
                            key.CreateSubKey(@"Shell\Open\Command").SetValue("", ToShortPathName(application) + " \"%1\"");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Return true if extension already associated in registry
        public static bool IsAssociated(string extension)
        {
            return (Registry.ClassesRoot.OpenSubKey(extension, false) != null);
        }


        // Return short path format of a file name
        static string ToShortPathName(string longName)
        {
            StringBuilder s = new StringBuilder(1000);
            uint iSize = (uint)s.Capacity;
            uint iRet = GetShortPathName(longName, s, iSize);
            return s.ToString();
        }
    }
}
