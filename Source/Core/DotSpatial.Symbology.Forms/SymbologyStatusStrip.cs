// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A pre-configured status strip with a thread safe Progress function.
    /// </summary>
    [ToolboxItem(false)]
    public partial class SymbologyStatusStrip : StatusStrip, IProgressHandler
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbologyStatusStrip"/> class which has a built in, thread safe Progress handler.
        /// </summary>
        public SymbologyStatusStrip()
        {
            InitializeComponent();
        }

        #endregion

        private delegate void UpdateProg(int percent, string message);

        #region Properties

        /// <summary>
        /// Gets or sets the progress bar. By default, the first ToolStripProgressBar that is added to the tool strip.
        /// </summary>
        public ToolStripProgressBar ProgressBar { get; set; }

        /// <summary>
        /// Gets or sets the progress label. By default, the first ToolStripStatusLabel that is added to the tool strip.
        /// </summary>
        public ToolStripStatusLabel ProgressLabel { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This method is thread safe so that people calling this method don't cause a cross-thread violation
        /// by updating the progress indicator from a different thread.
        /// </summary>
        /// <param name="percent">The integer percent from 0 to 100.</param>
        /// <param name="message">A message including the percent information if wanted.</param>
        public void Progress(int percent, string message)
        {
            if (InvokeRequired)
            {
                UpdateProg prg = UpdateProgress;
                BeginInvoke(prg, percent, message);
            }
            else
            {
                UpdateProgress(percent, message);
            }
        }

        /// <summary>
        /// Resets the progress. This method is thread safe so that people calling this method don't cause a cross-thread violation
        /// by updating the progress indicator from a different thread.
        /// </summary>
        public void Reset()
        {
            Progress(0, string.Empty);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStrip.ItemAdded"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemEventArgs"/> that contains the event data.</param>
        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            base.OnItemAdded(e);

            if (ProgressBar != null)
            {
                if (e.Item is ToolStripProgressBar pb)
                {
                    ProgressBar = pb;
                }
            }

            if (ProgressLabel == null)
            {
                if (e.Item is ToolStripStatusLabel sl)
                {
                    ProgressLabel = sl;
                }
            }
        }

        private void UpdateProgress(int percent, string message)
        {
            if (ProgressBar != null)
                ProgressBar.Value = percent;
            if (ProgressLabel != null)
                ProgressLabel.Text = message;

            // hack: I think there is a bug somewhere if we need to call DoEvents at the end of this event handler.
            Application.DoEvents();
        }
        
        #endregion
    }
}