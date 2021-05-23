using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Zen.UIControls.Layout;

namespace Zen.UIControls
{

    /// Common used style:
    ///   autosize? margin, button location, auto scroll, tab index

    /// <summary>
    /// Helper class to build container control(s)
    ///   Tab Page / Layout Panel / Grid
    ///   Container: + list of tab pages
    ///   Logic: margin / autosize / tab index
    ///      Allow recursive callback
    /// </summary>
    public sealed class CtrlBuilder
    {
        public static TabControl BuildTabPages(IEnumerable<string> tabNames)
        {
            TabControl tc = BuildContainer<TabControl>(Point.Empty, Size.Empty, true);

            int tabIndex = 0;
            foreach (string tabName in tabNames)
            {
                TabPage page = BuildTabPage(tabName, tc.Size, tabIndex++);
                tc.Controls.Add(page);
                page.ResumeLayout(false);
            }

            tc.ResumeLayout(false);
            return tc;
        }

        public static TabPage BuildTabPage(string tabName, Size parentSize, int tabIndex)
        {
            TabPage tabPage = BuildContainer<TabPage>(new Point(2, 22), new Size((parentSize.Width - 4), (parentSize.Height - 24)), true);

            tabPage.Name = tabName;
            tabPage.Text = tabPage.Name;
            tabPage.TabIndex = tabIndex;
            tabPage.UseVisualStyleBackColor = true;

            tabPage.ResumeLayout(false);
            return tabPage;
        }

        public static void BuildCollectionCtrl<T>(IEnumerable<T> objCollection)
        {
            BuildWizardPage();
        }

        /// <summary>
        /// To do: define wizard (flow -> page direction)
        /// </summary>
        public static void BuildWizard()
        {
            BuildWizardPage();
        }
        public static void BuildWizardPage()
        {
        }

        #region private functions
        private static ControlT BuildContainer<ControlT>(Point location, Size size, bool suspendLayout) where ControlT : Control, new()
        {
            ControlT ctrl = new ControlT();
            if (suspendLayout)
                ctrl.SuspendLayout();

            ctrl.Location = location;
            ctrl.Size = size;

            ctrl.Anchor = UIConst.AutoSize;
            ctrl.Padding = Padding.Empty;

            return ctrl;
        }
        #endregion
    }
}
