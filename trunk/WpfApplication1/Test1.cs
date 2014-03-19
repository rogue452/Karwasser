using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Net.NetworkInformation;

namespace project
{
    class Test1
    {
         void Main()
        {
               
        }
         public static String do_test()
         {
              var defaultGateway =
              from nics in NetworkInterface.GetAllNetworkInterfaces()
              from props in nics.GetIPProperties().GatewayAddresses
              where nics.OperationalStatus == OperationalStatus.Up
              select props.Address.ToString();

              return defaultGateway.First(); 
         
         }

        /*     
                foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())  
        {  
            Console.WriteLine(f.Name);  
            IPInterfaceProperties ipInterface = f.GetIPProperties();  
            foreach (UnicastIPAddressInformation unicastAddress in ipInterface.UnicastAddresses)  
            {
                System.Diagnostics.Debug.WriteLine(unicastAddress.Address == null ? "No subnet defined" : unicastAddress.Address.ToString());  
               // System.Diagnostics.Debug.WriteLine(unicastAddress.IPv4Mask == null ? "No subnet defined" : unicastAddress.IPv4Mask.ToString());  
            }  
        }  
        Console.ReadLine(); 
            }*/





                /*var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

                foreach (ManagementObject mo in mc.GetInstances())
                {
                    string[] gateways = mo["DefaultIPGateway"] as string[];
                    if (gateways != null)
                    {
                        string[] ipv6_gateways = gateways.Where(g => g.Contains(":")).ToArray();
                        if (ipv6_gateways.Length > 0)
                        {
                            foreach (string g in ipv6_gateways)
                            {
                                System.Diagnostics.Debug.WriteLine("IPV6 Default Gateway : {0}", g);
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No default gateway for IPV6 was detected");
                        }
                    }
                }

                Console.ReadKey();
            }*/
        }
}



