﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

using LanDiscovery;

namespace LanMachines
{
    class Program
    {
        internal static void Main(string[] args)
        {
            using (LanPingerAsync asyncPinger = new LanPingerAsync())
            {
                List<string> activeAddresses = asyncPinger.GetActiveMachines();

                foreach (string address in activeAddresses)
                {
                    Console.WriteLine(address);
                }
            } // end using

            Console.WriteLine("LAN ping complete");
            Console.ReadKey();
        } // end method
    } // end class
} // end namespace
