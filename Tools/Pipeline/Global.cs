﻿// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using Eto;
using Eto.Drawing;
using Eto.Forms;

namespace MonoGame.Tools.Pipeline
{
    static partial class Global
    {
        public static string DesktopEnvironment { get; private set; }
        public static bool UseHeaderBar { get; private set; }
        public static bool Unix { get; private set; }

        static Global()
        {
#if LINUX
            Global.DesktopEnvironment = Environment.GetEnvironmentVariable("XDG_CURRENT_DESKTOP");
            UseHeaderBar = Gtk.Global.MajorVersion >= 3 && Gtk.Global.MinorVersion >= 16 && Global.DesktopEnvironment == "GNOME";
#else
            DesktopEnvironment = "OSX";
#endif

            Unix = Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX;
        }

        public static string NotAllowedCharacters
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                {
                    if (Global.DesktopEnvironment == "OSX")
                        return ":";
                    
                    return "/";
                }

                return "/?<>\\:*|\"";

            }
        }

        public static bool CheckString(string s)
        {
            var notAllowed = Path.GetInvalidFileNameChars();

            for (int i = 0; i < notAllowed.Length; i++)
                if (s.Contains(notAllowed[i].ToString()))
                    return false;

            return true;
        }

        public static void ShowOpenWithDialog(string filePath)
        {
            try
            {
                PlatformShowOpenWithDialog(filePath);
            }
            catch
            {
                MainWindow.Instance.ShowError("Error", "The current platform does not have this dialog implemented.");
            }
        }

        public static Bitmap GetDirectoryIcon(bool exists)
        {
            try
            {
                return PlatformGetDirectoryIcon(exists);
            }
            catch { }

            return exists ? Bitmap.FromResource("TreeView.Folder.png") : Bitmap.FromResource("TreeView.FolderMissing.png");
        }

        public static Bitmap GetFileIcon(string path, bool exists)
        {
            try
            {
                return PlatformGetFileIcon(path, exists);
            }
            catch { }

            return exists ? Bitmap.FromResource("TreeView.File.png") : Bitmap.FromResource("TreeView.FileMissing.png");
        }
    }
}