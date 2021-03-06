﻿/*=============================================================================
*
*	(C) Copyright 2013, Michael Carlisle (mike.carlisle@thecodeking.co.uk)
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
using TheCodeKing.Utils.IO;
using TheCodeKing.Utils.IoC;
using TheCodeKing.Utils.Serialization;
using XDMessaging.IdentityProviders;
using XDMessaging.IO;
using XDMessaging.Specialized;

namespace XDMessaging.IoC
{
    public sealed class SimpleIocContainerBootstrapper
    {
        #region Constants and Fields

        private static Lazy<IocContainer> instance =
            new Lazy<IocContainer>(() => new SimpleIocContainer(c => new IocActivator(c), (c) => new SimpleIocScanner(c)).Initialize(Configure), true);

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public static IocContainer GetInstance()
        {
            return instance.Value;
        }

        internal static IocContainer GetInstance(bool clear)
        {
            if (clear)
            {
                instance = new Lazy<IocContainer>(() => new SimpleIocContainer(c => new IocActivator(c),
                                                                               c => new SimpleIocScanner(c)).
                                                            Initialize(Configure), true);
            }
            return GetInstance();
        }

        #endregion

        #region Methods

        public static void Configure(IocContainer container)
        {
            const string binarySerializer = "Binary";
            const string jsonSerializer = "Json";
            container.Register<IIdentityProvider, UniqueIdentityProvider>();
            container.Register<ISerializer, JsonSerializer>(jsonSerializer);
            container.Register<ISerializer, BinaryBase64Serializer>(binarySerializer);
            container.Register<ISerializer>(
                () => new SpecializedSerializer(container.Resolve<ISerializer>(binarySerializer),
                                                container.Resolve<ISerializer>(jsonSerializer)));

            try
            {
                container.Scan.ScanAllAssemblies("XDMessaging.*.dll");  
            }
            catch (IocScannerException e)
            {
                throw new XDMessagingException("Error loading transport assembly. Grant read/list contents permissions on the application directory. If executing from a network share, add <loadFromRemoteSources enabled=\"true\" /> to the runtime section of the app.config.", e);
            }

        }

        #endregion
    }
}