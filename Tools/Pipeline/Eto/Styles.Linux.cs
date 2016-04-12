﻿// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using Eto;
using Eto.GtkSharp.Forms;
using Eto.GtkSharp.Forms.Controls;
using Eto.GtkSharp.Forms.ToolBar;

namespace MonoGame.Tools.Pipeline
{
    static partial class Gtk3Wrapper
    {
        public const string gtklibpath = "libgtk-3.so.0";

        [DllImport(gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gtk_header_bar_new();

        [DllImport(gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_window_set_titlebar(IntPtr window, IntPtr widget);

        [DllImport(gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_header_bar_pack_start(IntPtr bar, IntPtr child);

        [DllImport(gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_header_bar_pack_end(IntPtr bar, IntPtr child);

        [DllImport(gtklibpath, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gtk_header_bar_set_show_close_button(IntPtr handle, bool show);
    }

    public static class Styles
    {
        public static void Load()
        {
            Style.Add<DialogHandler>("HeaderBar", h =>
            {
                var title = h.Title;
                var headerBar = Gtk3Wrapper.gtk_header_bar_new();
                Gtk3Wrapper.gtk_window_set_titlebar(h.Control.Handle, headerBar);
                h.Title = title;

                if (h.AbortButton.Text == "Close")
                {
                    Gtk3Wrapper.gtk_header_bar_set_show_close_button(headerBar, true);
                    return;
                }

                var defButton = (Gtk.Button)h.DefaultButton.ControlObject;
                defButton.StyleContext.AddClass("suggested-action");

                Gtk3Wrapper.gtk_header_bar_pack_end(headerBar, defButton.Handle);
                Gtk3Wrapper.gtk_header_bar_pack_start(headerBar, ((Gtk.Button)h.AbortButton.ControlObject).Handle);
            });

            Style.Add<LabelHandler>("Wrap", h => h.Control.MaxWidthChars = 55);

            Style.Add<LinkButtonHandler>("Center", h => h.Control.SetAlignment(0.5f, 0.5f));

            Style.Add<ToolBarHandler>("ToolBar", h =>
            {
                h.Control.ToolbarStyle = Gtk.ToolbarStyle.Icons;
                h.Control.IconSize = Gtk.IconSize.SmallToolbar;
            });
        }
    }
}
