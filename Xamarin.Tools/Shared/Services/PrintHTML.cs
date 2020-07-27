﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.Xamarin.Tools.Shared.Services
{
    public static class PrintHTML
    {
        /// <summary>
        /// Lazy-initialized file picker implementation
        /// </summary>
        private static Lazy<Services.Interfaces.IPrintHTML> implementation =
            new Lazy<Services.Interfaces.IPrintHTML>(Create, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current file picker plugin implementation to use
        /// </summary>
        public static Services.Interfaces.IPrintHTML Current
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
        private static Services.Interfaces.IPrintHTML Create()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
#if MONOANDROID
            return new Droid.Services.HtmlToPDF.PrintHTML();
#endif
#if __IOS__
            return new iOS.Services.PrintHTML();
#endif
#if WINDOWS_UWP
            return null;
#endif
#if NET462
            return new WPF.Services.PrintHTML();
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
