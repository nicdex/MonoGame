﻿// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Eto.Forms;

namespace MonoGame.Tools.Pipeline
{
    public partial class PropertyGridTable : Scrollable
    {
        TreeGridView propertyGrid;

        private void InitializeComponent()
        {
            propertyGrid = new TreeGridView();

            Content = propertyGrid;
        }
    }
}

