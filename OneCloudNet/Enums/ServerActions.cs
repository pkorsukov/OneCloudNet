﻿namespace OneCloudNet.Enums
{
    public enum Power
    {
        /// <summary>
        /// Enable power.
        /// </summary>
        PowerOn = 1,

        /// <summary>
        /// Disable power.
        /// </summary>
        PowerOff = 2,

        /// <summary>
        /// Shutdown server with OS tools.
        /// </summary>
        ShutDownGuestOS = 3,

        /// <summary>
        /// Reboot.
        /// </summary>
        PowerReboot = 4,
    }

    public enum NetworkAction
    {
        /// <summary>
        /// Connect server to network.
        /// </summary>
        AddNetwork = 5,

        /// <summary>
        /// Disconnect server from network.
        /// </summary>
        RemoveNetwork = 6
    }
}