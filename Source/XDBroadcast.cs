/*=============================================================================
*
*	(C) Copyright 2011, Michael Carlisle (mike.carlisle@thecodeking.co.uk)
*
*   http://www.TheCodeKing.co.uk
*  
*	All rights reserved.
*	The code and information is provided "as-is" without waranty of any kind,
*	either expressed or implied.
*
*=============================================================================
*/
using System;
using System.Collections.Generic;
using TheCodeKing.Net.Messaging.Concrete.IOStream;
using TheCodeKing.Net.Messaging.Concrete.MailSlot;
using TheCodeKing.Net.Messaging.Concrete.MultiBroadcast;
using TheCodeKing.Net.Messaging.Concrete.WindowsMessaging;
using TheCodeKing.Net.Messaging.Helpers;

namespace TheCodeKing.Net.Messaging
{
    /// <summary>
    ///   Class used to broadcast messages to other applications listening
    ///   on a particular channel.
    /// </summary>
    public static class XDBroadcast
    {
        private static readonly SerializerHelper serializerHelper;

        static XDBroadcast()
        {
            serializerHelper = new SerializerHelper();
        }

        #region Public Methods

        /// <summary>
        ///   Creates an instance of IXDBroadcast with the otption to propagate over the local network.
        /// </summary>
        /// <param name = "mode">The broadcast mode.</param>
        /// <param name = "propagateNetwork">true to propagate messages over the local network.</param>
        /// <returns></returns>
        public static IXDBroadcast CreateBroadcast(XDTransportMode mode, bool propagateNetwork)
        {
            // MailSlots can communicate over a network by default, so
            // no need to use the NetworkRelayBroadcast instance for this type.
            if (mode == XDTransportMode.MailSlot)
            {
                return new XDMailSlotBroadcast(serializerHelper, propagateNetwork);
            }
            IXDBroadcast broadcast = CreateBroadcast(mode);
            if (propagateNetwork)
            {
                broadcast = new XDMultiBroadcast(broadcast, new NetworkRelayBroadcast(serializerHelper, CreateBroadcast(XDTransportMode.MailSlot, true), mode));
            }
            return broadcast;
        }

        /// <summary>
        ///   Creates a concrete instance of IXDBroadcast used to broadcast messages to 
        ///   other processes in one or more modes.
        /// </summary>
        /// <param name = "modes">One or more transport mechanisms to use for interprocess communication.</param>
        /// <returns>The concreate instance of IXDBroadcast</returns>
        public static IXDBroadcast CreateBroadcast(params XDTransportMode[] modes)
        {
            if (modes.Length == 0)
            {
                throw new ArgumentException("At least one transport mode must be defined.");
            }
            if (modes.Length == 1)
            {
                switch (modes[0])
                {
                    case XDTransportMode.IOStream:
                        return new XDIOStreamBroadcast(serializerHelper);
                    case XDTransportMode.MailSlot:
                        return new XDMailSlotBroadcast(serializerHelper, false);
                    default:
                        return new XDWinMsgBroadcast(serializerHelper);
                }
            }

            // ensure only one of each type added
            var singleItems = new Dictionary<XDTransportMode, IXDBroadcast>();
            foreach (var mode in modes)
            {
                // only add one of each mode
                if (!singleItems.ContainsKey(mode))
                {
                    singleItems.Add(mode, CreateBroadcast(mode));
                }
            }
            return new XDMultiBroadcast(singleItems.Values);
        }

        #endregion
    }
}