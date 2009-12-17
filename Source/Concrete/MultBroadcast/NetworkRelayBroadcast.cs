﻿/*=============================================================================
*
*	(C) Copyright 2007, Michael Carlisle (mike.carlisle@thecodeking.co.uk)
*
*   http://www.TheCodeKing.co.uk
*  
*	All rights reserved.
*	The code and information is provided "as-is" without waranty of any kind,
*	either expresed or implied.
*
*=============================================================================
*/
using System;
using System.Collections.Generic;
using System.Text;
using TheCodeKing.Net.Messaging.Concrete.MailSlot;

namespace TheCodeKing.Net.Messaging.Concrete.MultBroadcast
{
    /// <summary>
    /// This implementation is used to broadcast messages from other implementations across
    /// the network using an internal MailSlot.
    /// </summary>
    internal sealed class NetworkRelayBroadcast : IXDBroadcast
    {
        /// <summary>
        /// The encapsulated broadcast implementation that this instance provides
        /// network propagation for.
        /// </summary>
        private IXDBroadcast nativeBroadcast;
        /// <summary>
        /// The MailSlot implementation used to broadcast messages across the local network.
        /// </summary>
        private IXDBroadcast networkBroadcast;
        /// <summary>
        /// The base channel name used for propagating messages.
        /// </summary>
        internal const string NetworkPropagateChannel = "System.PropagateBroadcast";

        /// <summary>
        /// The default constructor used to wrap a native broadcast implementation.
        /// </summary>
        /// <param name="nativeBroadcast"></param>
        /// <param name="networkBroadcast"></param>
        internal NetworkRelayBroadcast(IXDBroadcast nativeBroadcast, IXDBroadcast networkBroadcast)
        {
            if (nativeBroadcast == null)
            {
                throw new ArgumentNullException("nativeBroadcast");
            }
            if (networkBroadcast == null)
            {
                throw new ArgumentNullException("networkBroadcast");
            }
            if (nativeBroadcast.GetType() == typeof(XDMailSlotBroadcast))
            {
                throw new ArgumentException("Cannot be of type XDMailSlotBroadcast.", "nativeBroadcast");
            }
            // the native broadcast that this implementation wrappers
            this.nativeBroadcast = nativeBroadcast;
            // the MailSlot broadcast implementation is used to send over the network
            this.networkBroadcast = networkBroadcast;
        }
        /// <summary>
        /// The IXDBroadcast implementation that additionally propagates messages
        /// over the local network as well as the local machine.
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="message"></param>
        public void SendToChannel(string channelName, string message)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(channelName, "The channel name must be defined");
            }
            if (message == null)
            {
                throw new ArgumentNullException(message, "The messsage packet cannot be null");
            }
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentException("The channel name may not contain the ':' character.", "channelName");
            }
            nativeBroadcast.SendToChannel(channelName, message);

            // broadcast system message over network
            networkBroadcast.SendToChannel(GetPropagateNetworkMailSlotName(nativeBroadcast), string.Concat(Environment.MachineName, ":" + channelName + ":", message));
        }
        /// <summary>
        /// Gets the unique network propagation MailSlot name.
        /// </summary>
        /// <param name="nativeBroadcast"></param>
        /// <returns></returns>
        internal static string GetPropagateNetworkMailSlotName(IXDBroadcast nativeBroadcast)
        {
            // each mode has a unique MailSlot ident for monitoring network traffic  
            return string.Concat(NetworkPropagateChannel, ".", nativeBroadcast.GetType().Name);
        }
    }
}
