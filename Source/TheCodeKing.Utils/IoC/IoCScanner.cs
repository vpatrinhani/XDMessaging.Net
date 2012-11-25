﻿/*=============================================================================
*
*	(C) Copyright 2007, Michael Carlisle (mike.carlisle@thecodeking.co.uk)
*
*   http://www.TheCodeKing.co.uk
*  
*	All rights reserved.
*	The code and information is provided "as-is" without waranty of any kind,
*	either expressed or implied.
*
*=============================================================================
*/
using System.Reflection;

namespace TheCodeKing.Utils.IoC
{
    public interface IoCScanner
    {
        /// <summary>
        ///   Scan assembly for embedded assembies.
        /// </summary>
        /// <param name="assembly">The assembly the scan.</param>
        void ScanEmbeddedAssemblies(Assembly assembly);

        /// <summary>
        ///   Scan assemblies at the same location as the executing assembly for concrete XD implementations.
        /// </summary>
        void ScanAllAssemblies();

        /// <summary>
        ///   Scan assemblies at given location for concrete XD implementations.
        /// </summary>
        /// <param name = "location">The search location.</param>
        void ScanAllAssemblies(string location);
    }
}