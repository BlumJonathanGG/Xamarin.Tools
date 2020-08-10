﻿using Plugin.Xamarin.Tools.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Xamarin.Tools.Shared.Services
{
    public static class PDFSaveAndOpen
    {
        /// <summary>
        /// Lazy-initialized file picker implementation
        /// </summary>
        private static Lazy<IPDFSaveAndOpen> implementation =
            new Lazy<IPDFSaveAndOpen>(CreateDataShare, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current file picker plugin implementation to use
        /// </summary>
        public static IPDFSaveAndOpen Current
        {
            get
            {
                var ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }

                return ret;
            }
        }

        /// <summary>
        /// Creates file picker instance for the platform
        /// </summary>
        /// <returns>file picker instance</returns>
        private static IPDFSaveAndOpen CreateDataShare()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0||NETCOREAPP
            return null;
#else
#if MONOANDROID
            return new Droid.Services.PDFSaveAndOpen();
#endif
#if __IOS__
            return new iOS.Services.PDFSaveAndOpen();
#endif
#if WINDOWS_UWP
            return null;
#endif
#if NET462
            return null;
#endif
#endif
        }

        /// <summary>
        /// Returns new exception to throw when implementation is not found. This is the case when
        /// the NuGet package is not added to the platform specific project.
        /// </summary>
        /// <returns>exception to throw</returns>
        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException(
                "This functionality is not implemented in the portable version of this assembly. You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
}
