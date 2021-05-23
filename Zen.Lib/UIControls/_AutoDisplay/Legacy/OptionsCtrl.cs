using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace Zen.UIControls
{
    /// Build Tab Page
    ///    Pre condition: Parent container
    ///    # of tab pages added
    /// Build wizard page   

    /// <summary>
    /// Generic options control: support built in system types & user types
    ///   To Do: upgrade with home grown property grid, to handle
    ///     1. File / directory path; 2. Radio buttons; 3.etc.
    ///   12:39 AM 10/31/2009: above now are supported in PropertyCtrl.cs
    /// </summary>
    public partial class OptionsCtrl : UserControl
    {
        public OptionsCtrl()
        {
            InitializeComponent();
            _optionsList = new List<OptionInfo>();
        }

        public void AddOption(object options)
        {
            AddOption(options, TypeToCategory(options.GetType()));
        }
        private void AddOption(object options, string category)
        {
            MakeBrowsable(options.GetType());
            _optionsList.Add(new OptionInfo(options, category, null, null));
        }

        public void UpdateCtrl()
        {
            LoadCustomComponent();
        }
        #region private functions

        #region Event handler
        #endregion

        #region Dynamically load custom UI controls
        /// <summary>
        ///   Load TabControl if multiple options supplied, or PropertyGrid for single option
        /// </summary>
        private void LoadCustomComponent()
        {
            this.SuspendLayout();

            if (_optionsList.Count > 1)
                LoadTabControl(_optionsList);
            else
                LoadGrid(_optionsList[0]);

            this.ResumeLayout(false);
        }

        private void LoadGrid(OptionInfo opt)
        {
            _propertyGridDefault = BuildOptionControl<PropertyGrid>();
            _propertyGridDefault.SuspendLayout();

            _propertyGridDefault.ToolbarVisible = false;
            _propertyGridDefault.SelectedObject = opt.Options;

            _propertyGridDefault.ResumeLayout(false);
        }

        private void LoadTabControl(List<OptionInfo> optionsList)
        {
            _tabControlOptions = BuildOptionControl<TabControl>();
            _tabControlOptions.SuspendLayout();

            int tabIndex = 0;
            foreach (OptionInfo opt in optionsList)
            {
                _tabControlOptions.Controls.Add(BuildTabPage(_tabControlOptions.Size, tabIndex, opt));
                tabIndex++;
            }

            _tabControlOptions.SelectedIndex = 0;
            _tabControlOptions.ResumeLayout(false);
        }

        private TabPage BuildTabPage(Size parentSize, int tabIndex, OptionInfo opt)
        {
            int pageWidth = parentSize.Width - 4;
            int pageHeight = parentSize.Height - 24;

            TabPage tabPage = new TabPage();
            tabPage.SuspendLayout();

            tabPage.Name = opt.Category;
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

        private PropertyGrid BuildPropertyGrid(int width, int height, OptionInfo opt)
        {
            PropertyGrid grid = new PropertyGrid();
            grid.SuspendLayout();

            grid.Location = new Point(0, 0);
            grid.Size = new Size(width, height);
            grid.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            grid.ToolbarVisible = false;
            grid.SelectedObject = opt.Options;

            grid.ResumeLayout(false);
            return grid;
        }

        /// <summary>
        /// Use _panelOptions size / location to build the real options control 
        /// </summary>
        private ControlT BuildOptionControl<ControlT>() where ControlT : Control, new()
        {
            ControlT ctrl = new ControlT();
            ctrl.Location = _panelOptions.Location;
            ctrl.Size = new Size(_panelOptions.Size.Width, _panelOptions.Size.Height);
            ctrl.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            ctrl.TabIndex = 0;
            ctrl.Padding = new Padding(0, 0, 0, 0);

            this.Controls.Add(ctrl);

            return ctrl;
        }
        #endregion

        #region internal helper functions
        /// <summary>
        /// Make option browsable in the property grid by providing default converter
        /// </summary>
        private void MakeBrowsable(Type optionType)
        {
            PropertyInfo[] properties = optionType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                // PropertyGrid has built-in support for system types.
                //   Need special attention to System.Collections.Generic
                Type propertyType = property.PropertyType;
                if (propertyType.FullName.IndexOf("System.") == 0 || Zen.Utilities.TypeInterrogator.IsValueType(propertyType))
                    continue;

                UpdateConverterAttribute(propertyType);
                MakeBrowsable(propertyType);
            }
        }

        private void UpdateConverterAttribute(Type optionType)
        {
            TypeConverterAttribute converter = new TypeConverterAttribute(typeof(OptionsDefaultConverter));
            CategoryAttribute category = new CategoryAttribute(optionType.ToString());

            Attribute[] attributes = new Attribute[] { null, null };
            AttributeCollection oldAttributes = TypeDescriptor.GetAttributes(optionType);
            if (!oldAttributes.Contains(converter))
                attributes[0] = converter;
            if (!oldAttributes.Contains(category))
                attributes[1] = category;

            if (attributes[0] != null)
                TypeDescriptor.AddAttributes(optionType, attributes);
        }

        private string TypeToCategory(Type tp)
        {
            return Zen.Utilities.Text.NameCleaner.ToPhrase(tp.Name);
        }
        #endregion

        #endregion

        #region private data
        #region Dynamic UI controls
        private TabControl _tabControlOptions;      // Multi page options        
        private PropertyGrid _propertyGridDefault;  // Single page options
        #endregion

        private List<OptionInfo> _optionsList;
        #endregion
    }

    #region Options Dialog supporter
    internal sealed class OptionInfo
    {
        public OptionInfo(object options)
            : this(options, null, null, null)
        {
        }
        public OptionInfo(object options, string category, Dictionary<string, IEnumerable<string>> valueHints, Dictionary<string, string> lableHints)
        {
            _options = options;
            _category = category;
            _valueHints = valueHints;
            _labelHints = lableHints;
        }

        public object Options
        {
            get { return _options; }
            set { _options = value; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public Dictionary<string, IEnumerable<string>> ValueHints
        {
            get { return _valueHints; }
            set { _valueHints = value; }
        }
        public Dictionary<string, string> LabelHints
        {
            get { return _labelHints; }
            set { _labelHints = value; }
        }

        #region private data
        private object _options;
        private string _category;
        private Dictionary<string, IEnumerable<string>> _valueHints;
        private Dictionary<string, string> _labelHints;
        #endregion
    }

    /// <summary>
    /// Helper class to make options expandable in the propertyGrid.
    /// </summary>
    internal sealed class OptionsDefaultConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return string.Empty;
        }
    }
    #endregion

}