﻿// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.ComponentModel;

namespace MonoGame.Tools.Pipeline
{
    partial class PropertyGridControl
    {
        List<object> _objects;

        public PropertyGridControl()
        {
            InitializeComponent();

            _objects = new List<object>();
        }

        private void BtnAbc_Click(object sender, EventArgs e)
        {
            propertyTable.Group = false;
            propertyTable.Update();
        }

        private void BtnGroup_Click(object sender, EventArgs e)
        {
            propertyTable.Group = true;
            propertyTable.Update();
        }

        public void SetObjects(List<IProjectItem> objects)
        {
            _objects = objects.Cast<object>().ToList();
            Reload();
        }

        public void Reload()
        {
            propertyTable.Clear();

            if (_objects.Count != 0)
                LoadProps(_objects);
            
            propertyTable.Update();
        }

        private bool CompareVariables(ref object a, object b, PropertyInfo p)
        {
            var prop = b.GetType().GetProperty(p.Name);
            if (prop == null)
                return false;

            if (a == null || !a.Equals(prop.GetValue(b)))
                a = null;

            return true;
        }

        private void LoadProps(List<object> objects)
        {
            var props = objects[0].GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var p in props)
            {
                var attrs = p.GetCustomAttributes(true);
                var browsable = true;
                var category = "Mics";

                foreach (var a in attrs)
                {
                    if (a is BrowsableAttribute)
                        browsable = (a as BrowsableAttribute).Browsable;
                    else if (a is CategoryAttribute)
                        category = (a as CategoryAttribute).Category;
                }

                object value = p.GetValue(objects[0], null);
                foreach (object o in objects)
                {
                    if (!CompareVariables(ref value, o, p))
                    {
                        browsable = false;
                        break;
                    }
                }

                if (!browsable)
                    continue;

                propertyTable.AddEntry(category, p.Name, value, (sender, e) =>
                {
                    var action = new UpdatePropertyAction(MainWindow.Instance, objects, p, sender);
                    MainWindow.Controller.AddAction(action);
                    action.Do();
                }, p.CanWrite);

                if (value is ProcessorTypeDescription)
                    LoadProcessorParams(_objects.Cast<ContentItem>().ToList());
            }
        }

        private void LoadProcessorParams(List<ContentItem> objects)
        {
            foreach (var p in objects[0].Processor.Properties)
            {
                object value = objects[0].ProcessorParams[p.Name];
                foreach (ContentItem o in objects)
                {
                    if (value == null || !value.Equals(o.ProcessorParams[p.Name]))
                    {
                        value = null;
                        break;
                    }
                }

                propertyTable.AddEntry("Processor Parameters", p.Name, value, (sender, e) =>
                {
                    var action = new UpdateProcessorAction(MainWindow.Instance, objects.Cast<ContentItem>().ToList(), p.Name, sender);
                    MainWindow.Controller.AddAction(action);
                    action.Do();
                }, true);
            }
        }
    }
}