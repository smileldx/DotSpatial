// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ZenithEditor.
    /// </summary>
    public class ZenithEditor : UITypeEditor
    {
        #region Fields

        private IWindowsFormsEditorService _dialogProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether we can widen the drop-down without having to close the drop down,
        /// widen the control, and re-open it again.
        /// </summary>
        public override bool IsDropDownResizable => true;

        #endregion

        #region Methods

        /// <summary>
        /// Edits the value by showing a slider control in the drop down.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">The ramp slider value.</param>
        /// <returns>Returns the ramp slider value.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _dialogProvider = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            RampSlider rs = new()
            {
                Maximum = 90,
                Minimum = 0,
                MaximumColor = Color.SteelBlue,
                MinimumColor = Color.Transparent,
                RampText = "Zenith",
                RampTextBehindRamp = false,
                Value = Convert.ToDouble(value),
                ShowValue = false,
                Width = 75,
                Height = 50
            };
            rs.ValueChanged += RsValueChanged;
            _dialogProvider?.DropDownControl(rs);
            return (float)rs.Value;
        }

        /// <summary>
        /// Sets the behavior to drop-down.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <returns>The UITypeEditorEditStyle.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private void RsValueChanged(object sender, EventArgs e)
        {
            _dialogProvider.CloseDropDown();
        }

        #endregion
    }
}