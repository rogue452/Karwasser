using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;



using System.Net.NetworkInformation;
using System.Net;

namespace project
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
            NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface network in networks)
            {

                if (network.NetworkInterfaceType.ToString().Contains("Ethernet"))
                {
                    ////Code for getting IP address
                    Console.WriteLine("DNS Addresses");
                    foreach (IPAddress entry in network.GetIPProperties().DnsAddresses)
                    {
                        Console.WriteLine(entry.ToString());
                    }

                    //Code for getting IP address
                    Console.WriteLine("Anycast Addresses");
                    foreach (IPAddressInformation entry in network.GetIPProperties().AnycastAddresses)
                    {
                        Console.WriteLine(entry.ToString());
                    }

                    //Code for getting IP address
                    Console.WriteLine("DHCPServer Addresses");
                    foreach (IPAddress entry in network.GetIPProperties().DhcpServerAddresses)
                    {
                        Console.WriteLine(entry.ToString());
                    }

                    //Code for getting IP address
                    Console.WriteLine("Gateway Addresses");
                    foreach (GatewayIPAddressInformation entry in network.GetIPProperties().GatewayAddresses)
                    {
                        Console.WriteLine(entry.Address.ToString());
                    }

                    //Code for getting IP address
                    Console.WriteLine("Multicast Addresses");
                    foreach (MulticastIPAddressInformation entry in network.GetIPProperties().MulticastAddresses)
                    {
                        Console.WriteLine(entry.Address.ToString());
                    }

                    //Code for getting IP address
                    Console.WriteLine("Unicast Addresses");
                    foreach (UnicastIPAddressInformation entry in network.GetIPProperties().UnicastAddresses)
                    {
                        Console.WriteLine(entry.Address.ToString());
                    }
                }
            }

            Console.ReadKey(true);
        }
    }
}


/*namespace IPAddressProgram
{
  class Program
  {
    static void Main(string[] args)
    {
      NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();

      foreach (NetworkInterface network in networks)
      {

        if (network.NetworkInterfaceType.ToString().Contains("Ethernet"))
        {
          ////Code for getting IP address
          Console.WriteLine("DNS Addresses");
          foreach (IPAddress entry in network.GetIPProperties().DnsAddresses)
          {
            Console.WriteLine(entry.ToString());
          }

          //Code for getting IP address
          Console.WriteLine("Anycast Addresses");
          foreach (IPAddressInformation entry in network.GetIPProperties().AnycastAddresses)
          {
            Console.WriteLine(entry.ToString());
          }

          //Code for getting IP address
          Console.WriteLine("DHCPServer Addresses");
          foreach (IPAddress entry in network.GetIPProperties().DhcpServerAddresses)
          {
            Console.WriteLine(entry.ToString());
          }

          //Code for getting IP address
          Console.WriteLine("Gateway Addresses");
          foreach (GatewayIPAddressInformation entry in network.GetIPProperties().GatewayAddresses)
          {
            Console.WriteLine(entry.Address.ToString());
          }

          //Code for getting IP address
          Console.WriteLine("Multicast Addresses");
          foreach (MulticastIPAddressInformation entry in network.GetIPProperties().MulticastAddresses)
          {
            Console.WriteLine(entry.Address.ToString());
          }

          //Code for getting IP address
          Console.WriteLine("Unicast Addresses");
          foreach (UnicastIPAddressInformation entry in network.GetIPProperties().UnicastAddresses)
          {
            Console.WriteLine(entry.Address.ToString());
          }
        }
      }

      Console.ReadKey(true);
    }
  }
}*/