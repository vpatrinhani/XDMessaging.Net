﻿/*=============================================================================
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

namespace TheCodeKing.Utils.IoC
{
    public interface IocContainer
    {
        #region Public Methods

        IoCScanner Scan { get; }

        bool IsRegistered(Type type);
        bool IsRegistered(Type type, string name);

        void Register(Type abstractType, Type concreteType);
        void Register(Type abstractType, Type concreteType, string name);
        void Register(Type abstractType, Func<object> factory);
        void Register(Type abstractType, Func<object> factory, string name);
        
        void RegisterInstance(Type type, object instance);
        void RegisterInstance(Type type, object instance, string name);

        object Resolve(Type type);
        object Resolve(Type type, string name);

        #endregion
    }
}