using System;
using System.ServiceModel;
using ININ.Alliances.DotNetIvr;

namespace ININ.Alliances.DotNetIvrDotNetIvrHost
{
    class Program
    {
        private static DotNetIvrService _service;
        private static ServiceHost _serviceHost;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Initializing...");

                // Initialize new service instance
                _service = new DotNetIvrService();

                // Create the service host based on the service instance
                _serviceHost = new ServiceHost(_service);

                // Open the service host (bind/listen on configured addresses)
                _serviceHost.Open();

                Console.WriteLine("Service is up and running");
                foreach (var url in _serviceHost.BaseAddresses)
                {
                    Console.WriteLine("Base address: " + url.AbsoluteUri);
                }

                Console.WriteLine("Press enter to stop service ");
                Console.ReadLine();

                _serviceHost.Close();
            }
            catch (AddressAccessDeniedException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("This is typically resolved by running the follong command in a command prompt run with admin rights:");
                Console.WriteLine("netsh http add urlacl url=http://+:8000/DotNetIvrService user=DOMAIN\\username");
                Console.WriteLine("");
                Console.WriteLine("Press enter to quit ");
                Console.ReadLine();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Press enter to quit ");
                Console.ReadLine();
            }
        }
    }
}
