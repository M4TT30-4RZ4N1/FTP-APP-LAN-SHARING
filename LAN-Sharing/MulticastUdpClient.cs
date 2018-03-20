using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LANSharing
{

    /// <summary>
    /// Multicast UdpClient wrapper with send and receive capabilities.
    /// Usage: pass local and remote multicast IPs and port to constructor.
    /// Use Send method to send data,
    /// subscribe to Received event to get notified about received data.
    /// </summary>
    public class MulticastUdpClient
    {
        UdpClient _udpclient;
        int _port;
        IPAddress _multicastIPaddress;
        IPAddress _localIPaddress;
        IPEndPoint _localEndPoint;
        IPEndPoint _remoteEndPoint;

        public MulticastUdpClient(IPAddress multicastIPaddress, int port, IPAddress localIPaddress = null)
        {
            // Store params
            _multicastIPaddress = multicastIPaddress;
            _port = port;
            _localIPaddress = localIPaddress;
            if (localIPaddress == null)
                _localIPaddress = IPAddress.Any;

            // Create endpoints
            _remoteEndPoint = new IPEndPoint(_multicastIPaddress, port);
            _localEndPoint = new IPEndPoint(_localIPaddress, port);

            // Create and configure UdpClient
            _udpclient = new UdpClient();
            // The following three lines allow multiple clients on the same PC
            _udpclient.ExclusiveAddressUse = false;
            _udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udpclient.ExclusiveAddressUse = false;
            // Bind, Join
            _udpclient.Client.Bind(_localEndPoint);
            _udpclient.JoinMulticastGroup(_multicastIPaddress, _localIPaddress);

            // Start listening for incoming data
            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }

        /// <summary>
        /// Send the buffer by UDP to multicast address
        /// </summary>
        /// <param name="bufferToSend"></param>
        public void SendMulticast(byte[] bufferToSend)
        {
            _udpclient.Send(bufferToSend, bufferToSend.Length, _remoteEndPoint);
        }

        /// <summary>
        /// Callback which is called when UDP packet is received
        /// </summary>
        /// <param name="ar"></param>
        private void ReceivedCallback(IAsyncResult ar)
        {
            // Get received data
            IPEndPoint sender = new IPEndPoint(0, 0);
            Byte[] receivedBytes = _udpclient.EndReceive(ar, ref sender);

            // fire event if defined
            if (UdpMessageReceived != null)
                UdpMessageReceived(this, new UdpMessageReceivedEventArgs() { Buffer = receivedBytes });

            // Restart listening for udp data packages
            _udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }

        /// <summary>
        /// Event handler which will be invoked when UDP message is received
        /// </summary>
        public event EventHandler<UdpMessageReceivedEventArgs> UdpMessageReceived;

        /// <summary>
        /// Arguments for UdpMessageReceived event handler
        /// </summary>
        public class UdpMessageReceivedEventArgs : EventArgs
        {
            public byte[] Buffer { get; set; }
        }

    }

}



