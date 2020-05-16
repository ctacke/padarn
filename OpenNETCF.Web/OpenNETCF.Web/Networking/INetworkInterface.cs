using System;

namespace OpenNETCF.Net.NetworkInformation
{
    internal interface INetworkInterface
    {
        PhysicalAddress GetPhysicalAddress();
        string Id { get; }
        string Name { get; }
    }
}
