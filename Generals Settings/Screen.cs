using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Generals_Manager
{
    /// <summary>
    /// http://stackoverflow.com/a/10041189
    /// </summary>
    internal static class Screen
    {
        /// <summary>
        /// Get the current resolution of this screen.
        /// </summary>
        /// <returns>The current resolution of this screen.</returns>
        public static string GetCurrentResolution()
        {
            return Format(GetSystemMetrics(0x00), GetSystemMetrics(0x01));
        }

        /// <summary>
        /// Get all resolutions supported by this computer's screens.
        /// </summary>
        /// <returns>A list of strings with the supported resolutions.<br/>
        /// An empty list if no resolutions are supported.
        /// </returns>
        public static List<string> GetResolutions()
        {
            var resolutions = new List<string>();

            try
            {
                var devMode = new DEVMODE();
                int i = 0;

                while (EnumDisplaySettings(null, i, ref devMode))
                {
                    resolutions.Add(Format(devMode.dmPelsWidth, devMode.dmPelsHeight));
                    i++;
                }

                resolutions = resolutions.Distinct().ToList();
            }
            catch (Exception e)
            {
                // Log exception
            }

            return resolutions;
        }

        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        private static string Format(int width, int height)
        {
            return string.Format("{0} {1}", width, height);
        }

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [StructLayout(LayoutKind.Sequential)]
        internal struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;

            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;

            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
    }
}