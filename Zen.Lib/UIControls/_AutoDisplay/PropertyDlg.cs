using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Zen.Common.Def;

namespace Zen.UIControls
{
    
    /// <summary>
    /// </summary>
    public partial class PropertyDlg : Form
    {
        public PropertyDlg()
            :this(null)
        {
        }
        public PropertyDlg(string title)
        {
            InitializeComponent();
            if (title != null)
                base.Text = title;
        }

        public void AddOption(object options)
        {
            AddOption(options, null);
        }
        public void AddOption(object options, ObjValueLabelHint hint)
        {
            if (hint == null)
                hint = new ObjValueLabelHint();

            if (string.IsNullOrEmpty(hint.Category))
                hint.Category = Zen.Utilities.Text.NameCleaner.ToPhrase(options.GetType().Name);
            _optionsList.Add(new Pair<object, ObjValueLabelHint>(options, hint));
        }

        #region private functions
        private void OnFormLoad(object sender, EventArgs e)
        {
            LoadCustomComponent();
        }

        private void OnOK(object sender, EventArgs e)
        {
            foreach (PropertyCtrl optCtrl in _optionsCtrls)
                optCtrl.OnOk();

            Close();
            DialogResult = DialogResult.OK;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            foreach (PropertyCtrl optCtrl in _optionsCtrls)
                optCtrl.OnCancel();

            Close();
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region Dynamically load custom UI controls
        /// <summary>
        ///   Load TabControl if multiple options supplied, or PropertyGrid for single option
        /// </summary>
        private void LoadCustomComponent()
        {
            this.SuspendLayout();
            {
                if (_optionsList.Count > 1)
                    LoadTabControl(_optionsList);
                else
                    LoadGrid(_optionsList[0]);
            }
            this.ResumeLayout(false);

            this.Size = new Size(Size.Width, Size.Height + _panelOptions.Height * _maxLeafCount);
        }

        private void LoadGrid(Pair<object, ObjValueLabelHint> opt)
        {
            _propertyGridDefault = BuildOptionControl<PropertyCtrl>();
            _optionsCtrls.Add(_propertyGridDefault);

            _propertyGridDefault.SuspendLayout();
            AddProperty(_propertyGridDefault, opt);
            _propertyGridDefault.ResumeLayout(false);
        }

        private void LoadTabControl(List<Pair<object, ObjValueLabelHint>> optionsList)
        {
            _tabControlOptions = BuildOptionControl<TabControl>();
            _tabControlOptions.SuspendLayout();

            int tabIndex = 0;
            foreach (Pair<object, ObjValueLabelHint> opt in optionsList)
            {
                _tabControlOptions.Controls.Add(BuildTabPage(_tabControlOptions.Size, tabIndex, opt));
                tabIndex++;
            }

            _tabControlOptions.SelectedIndex = 0;
            _tabControlOptions.ResumeLayout(false);
        }

        private TabPage BuildTabPage(Size parentSize, int tabIndex, Pair<object, ObjValueLabelHint> opt)
        {
            int pageWidth = parentSize.Width - 4;
            int pageHeight = parentSize.Height - 24;

            TabPage tabPage = new TabPage();
            tabPage.SuspendLayout();

            tabPage.Name = opt.Y.Category;
            tabPage.Size = new Size(pageWidth, pageHeight);
            tabPage.Location = new Point(2, 22);
            tabPage.Text = tabPage.Name;
            tabPage.Padding = new Padding(0, 0, 0, 0);
            tabPage.TabIndex = tabIndex;
            tabPage.UseVisualStyleBackColor = true;

            tabPage.Controls.Add(BuildPropertyGrid(pageWidth, pageHeight, opt));

            tabPage.ResumeLayout(false);
            return tabPage;
        }

        private PropertyCtrl BuildPropertyGrid(int width, int height, Pair<object, ObjValueLabelHint> opt)
        {
            PropertyCtrl grid = new PropertyCtrl();
            _optionsCtrls.Add(grid);
            grid.SuspendLayout();

            grid.Location = new Point(0, 0);
            grid.Size = new Size(width, height);
            grid.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            AddProperty(grid, opt);

            grid.ResumeLayout(false);
            return grid;
        }

        /// <summary>
        /// Use _panelOptions size / location to build the real options control 
        /// </summary>
        private ControlT BuildOptionControl<ControlT>() where ControlT : Control, new()
        {
            ControlT ctrl = new ControlT();
            ctrl.Dock = DockStyle.Fill;
            ctrl.TabIndex = 0;
            ctrl.Margin = new Padding(0, 0, 0, 0);
            ctrl.Padding = new Padding(0, 0, 0, 0);

            _panelOptions.Controls.Add(ctrl);
            return ctrl;
        }

        private void AddProperty(PropertyCtrl grid, Pair<object, ObjValueLabelHint> opt)
        {
            int leafCount = grid.SetObject(opt.X, opt.Y, TableLayoutPanelCellBorderStyle.None);
            if (_maxLeafCount < leafCount)
                _maxLeafCount = leafCount;
        }
        #endregion

        #region private data
        #region Dynamic UI controls
        private TabControl _tabControlOptions;      // Multi page options        
        private PropertyCtrl _propertyGridDefault;  // Single page options
        private int _maxLeafCount = 1;
        #endregion

        private List<Pair<object, ObjValueLabelHint>> _optionsList = new List<Pair<object, ObjValueLabelHint>>();
        private List<PropertyCtrl> _optionsCtrls = new List<PropertyCtrl>();
        #endregion
    }

}