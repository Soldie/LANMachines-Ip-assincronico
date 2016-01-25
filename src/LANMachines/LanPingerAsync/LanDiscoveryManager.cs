﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LanDiscovery
{
    /// <summary>
    /// Lan discovery manager - identifies lan machines
    /// using multiple techniques.
    /// </summary>
    public class LanDiscoveryManager
    {
        #region Public Methods

        /// <summary>
        /// Retrieve a list of network machines.
        /// </summary>
        /// <returns>List of network IP addresses.</returns>
        public List<IPAddress> GetNetworkMachines()
        {
            List<IPAddress> lanMachinesList = getLanPingResults();
            List<IPAddress> activeMachines = getArpScanResults();

            foreach (IPAddress lanIp in lanMachinesList)
            {
                if (!activeMachines.Contains(lanIp))
                {
                    activeMachines.Add(lanIp);
                } // end if
            } // end foreach

            activeMachines.Sort(new IPAddressComparer());

            foreach (IPAddress address in activeMachines)
            {
                try
                {
                    IPHostEntry entry = Dns.GetHostEntry(address);
                    Console.WriteLine(entry.HostName);
                }
                catch (Exception)
                {
                    Console.WriteLine("Unable to find host name: " + address.ToString());
                } // end try-catch
            }

            return activeMachines;
        } // end method

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the list of machines which responded to the lan ping test.
        /// </summary>
        /// <returns>List of lan ping responding.</returns>
        private List<IPAddress> getLanPingResults()
        {
            using (lanPinger_m = new LanPingerAsync())
            {
                List<IPAddress> respondingIpAddresses = lanPinger_m.GetActiveMachineAddresses();
                if (respondingIpAddresses == null)
                {
                    return new List<IPAddress>();
                } // end if

                return lanPinger_m.GetActiveMachineAddresses();
            } // end using
        } // end method

        /// <summary>
        /// Get the list of machines which responded to the arp request.
        /// </summary>
        /// <returns>List of arp responders.</returns>
        private List<IPAddress> getArpScanResults()
        {
            using (arpScanner_m = new ArpScanner())
            {
                return arpScanner_m.GetRespondingMachines();
            } // end using
        } // end method

        #endregion

        #region Private Data

        // Lan ping handler.
        private LanPingerAsync lanPinger_m;
        // Arp request handler.
        private ArpScanner arpScanner_m;

        #endregion

    } // end class
} // end namespace
