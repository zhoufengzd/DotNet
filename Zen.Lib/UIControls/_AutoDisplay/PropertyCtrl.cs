using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Zen.Common.Def;
using Zen.Common.Def.Layout;
using Zen.UIControls.Layout;
using Zen.Utilities;
using Zen.Utilities.Generics;

namespace Zen.UIControls
{
    using CellBorderStyle = TableLayoutPanelCellBorderStyle;
    using NameCleaner = Zen.Utilities.Text.NameCleaner;
    using RowColumnLocation = Twin<int>;

    /// <summary>
    /// PropertyCtrl is a powerful but not super powerful control.
    /// It dynamically build & draw controls based on property type
    /// Workflow:   object -> property -> control
    ///             control -> property -> object

    /// ** TO DO **
    ///   Text box error mark:
    ///   Pair control (Left | Right)
    ///   Dictionary (1 key:1 value) control: almost
    ///   Master-Detail control: 
    ///   ListBox: add / remove support, multi selection
    ///   Advanced button: show options on demand
    ///   List of object page / wizard page: in progress
    ///   Draw section line: in progress. 
    /// </summary>
    public partial class PropertyCtrl : UserControl, IAutoControl<object>
    {
        public PropertyCtrl()
        {
            InitializeComponent();
        }

        public int SetObject(object obj)
        {
            return SetObject(obj, null, CellBorderStyle.None);
        }
        /// <summary>
        /// Set object and return leaf item count
        /// </summary>
        /// <param name="obj">Any object that exposes public properties</param>
        /// <param name="valueHints">Value hint by property name</param>
        public int SetObject(object obj, ObjValueLabelHint objHints, CellBorderStyle borderStyle)
        {
            if (_object == obj)
                return _controlMap.Count;

            _object = obj;
            _objHints = (objHints != null)? objHints: new ObjValueLabelHint();
            _borderStyle = borderStyle;

            if (_objType != obj.GetType())
            {
                BuildCompositeCtrl(_object.GetType(), _object, null);
                DrawControls();
                _objType = obj.GetType();
            }
            else // load data only, no need to rebuild / redraw controls
            {
                LoadComposite(_objType, obj, null);
            }

            return _controlMap.Count;
        }

        /// <summary>
        /// Doing data exchange: control ==> object
        /// </summary>
        public bool OnOk()
        {
            foreach (KeyValuePair<string, PropertyCtrlInfo> kv in _controlMap)
            {
                PropertyCtrlInfo ci = kv.Value;
                if (ci.Converter != null)
                    ci.PropertyInfo.SetValue(ci.ParentObject, ci.Converter.ToPropertyValue(ci.Binder.Value), null);
                else
                    ci.PropertyInfo.SetValue(ci.ParentObject, ci.Binder.Value, null);
            }

            return true;
        }
        public bool OnCancel()
        {
            return true;
        }

        #region private functions

        #region Build controls
        private void DrawControls()
        {
            _mainPage.Draw(_overarchingPanel);
        }

        private void BuildCompositeCtrl(Type tp, object obj, ObjIdentifier objId)
        {
            PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in properties)
            {
                Type unerLyingType = TypeInterrogator.GetUnderlyingType(pi.PropertyType);
                if (IsSkipped(pi, unerLyingType))
                    continue;

                if (IsLeafCtrl(unerLyingType))
                    BuildLeafCtrl(pi, unerLyingType, obj, objId);
                else if (IsCollectionCtrl(unerLyingType))
                    BuildCollectionCtrl(pi, pi.PropertyType, obj, objId);
                else
                    BuildCompositeCtrl(unerLyingType, pi.GetValue(obj, null), new ObjIdentifier(pi.Name, objId));
            }
        }

        private void BuildLeafCtrl(PropertyInfo pi, Type tp, object parentObj, ObjIdentifier parentId)
        {
            PropertyCtrlInfo ci = BuildControlInfo(pi, parentObj);
            Page page = (ci.Layout.IsAdvanced) ? Macros.SafeGet(ref _advancedPage) : _mainPage;

            object piValue = (parentObj == null) ? null : pi.GetValue(parentObj, null);
            if (TypeInterrogator.IsNumericType(tp))
            {
                DecimalPCV converter = new DecimalPCV(tp);
                ci.Converter = converter;

                NumericUpDownHint upDownHint = AttributeInterrogator.GetAttribute<NumericUpDownHint>(pi);
                if (upDownHint != null)
                {
                    ci.Binder = new NumericUpDownSingleBinder((piValue == null) ? Decimal.MinValue : Decimal.Parse(piValue.ToString()), upDownHint);
                }
                else
                {
                    TextBoxHint tbHint = AttributeInterrogator.GetAttribute<TextBoxHint>(pi);
                    if (tbHint == null)
                        tbHint = new TextBoxHint();
                    tbHint.CharsAccepted = TextBoxHint.Numbers;
                    ci.Binder = new TextBoxBinder((piValue == null) ? null : piValue.ToString(), tbHint);
                }
            }
            else if (TypeInterrogator.IsBoolType(tp))
            {
                ci.Binder = new CheckBoxBinder(ci.Layout.Label, (piValue == null) ? false : (bool)piValue);
                ci.Layout.Label = null;
                ci.Layout.Section = Section.Footer;
            }
            else if (TypeInterrogator.IsDateTimeType(tp))
            {
                ci.Binder = new DateTimePickerBinder((piValue == null) ? DateTime.Today : (DateTime)piValue, null);
            }
            else if (TypeInterrogator.IsEnumType(tp))
            {
                const int MaxRadioButtonCount = 3;

                IEnumerable<string> names = EnumConverter.ToNames(tp, true);
                if (CollConverter.GetCount(names) <= MaxRadioButtonCount)
                {
                    ci.Binder = new RadioButtonBinder(tp, (piValue == null) ? -1 : (int)piValue);
                }
                else
                {
                    EnumPCV converter = new EnumPCV(tp);
                    ci.Binder = new ComboBoxBinder(piValue.ToString(), names, ComboBoxStyle.DropDownList);
                    ci.Converter = converter;
                }
            }
            else if (TypeInterrogator.IsStringType(tp))
            {
                if (_objHints.ValueHints != null && _objHints.ValueHints.ContainsKey(pi.Name))
                {
                    ci.Binder = new ComboBoxBinder((piValue == null) ? null : piValue.ToString(), _objHints.ValueHints[pi.Name], ComboBoxStyle.DropDown);
                }
                else
                {
                    LayoutHint lhint = AttributeInterrogator.GetAttribute<LayoutHint>(pi);
                    ci.Binder = new TextBoxBinder((piValue == null) ? null : piValue.ToString(), lhint);
                }
            }
            else if (tp == typeof(System.Data.DataTable))
            {
                ci.Binder = new DataGridViewBinder((piValue == null) ? null : piValue as System.Data.DataTable);
                ci.Layout.ColumnCount = 2;
            }
            else if (TypeInterrogator.IsDictionaryType(tp))
            {
                DictionaryPCV converter = new DictionaryPCV();
                Dictionary<string, string> converted = (piValue == null) ? null : converter.ToControlValue(piValue);
                ci.Binder = new DictionaryCtrlBinder(converted, _objHints.DictionaryHint);
                ci.Converter = converter;

                ci.Layout.Label = null;
                ci.Layout.ColumnCount = 2;
                ci.Layout.RowCount = (converted != null) ? (int)((converted.Count + 1) * 1.5) : 2;
            }
            AddControlInfo(page, ci, parentId);
        }

        private void BuildCollectionCtrl(PropertyInfo pi, Type tp, object parentObj, ObjIdentifier parentId)
        {
            PropertyCtrlInfo ci = BuildControlInfo(pi, parentObj);
            Page page = (ci.Layout.IsAdvanced) ? Macros.SafeGet(ref _advancedPage) : _mainPage;

            object piValue = (parentObj == null) ? null : pi.GetValue(parentObj, null);

            Type itemT = TypeInterrogator.GetItemType(tp);
            if (TypeInterrogator.IsSingleValueType(itemT))
            {
                if (TypeInterrogator.IsEnumType(itemT))
                {
                    BitFlagsPCV converter = new BitFlagsPCV(itemT);
                    ci.Binder = new CheckedListBoxBinder(converter.ToControlValue(itemT), converter.ToControlValue((IEnumerable)piValue));
                    ci.Converter = converter;
                }
                else if (TypeInterrogator.IsStringType(itemT))
                {
                    if (TypeInterrogator.IsArrayType(tp))
                        ci.Converter = new ArrayPCV(itemT);
                    else if (TypeInterrogator.IsListType(tp))
                        ci.Converter = new ListPCV(itemT);

                    if (_objHints.ValueHints != null && _objHints.ValueHints.ContainsKey(pi.Name))
                        ci.Binder = new ListBoxBinder(_objHints.ValueHints[pi.Name], (IEnumerable<string>)piValue);
                    else
                        ci.Binder = new ListBoxBinder((IEnumerable<string>)piValue);
                }
                else if (TypeInterrogator.IsNumericType(itemT))
                {
                    if (TypeInterrogator.IsArrayType(tp))
                        ci.Converter = new ArrayPCV(itemT);
                    else if (TypeInterrogator.IsListType(tp))
                        ci.Converter = new ListPCV(itemT);

                    if (_objHints.ValueHints != null && _objHints.ValueHints.ContainsKey(pi.Name))
                        ci.Binder = new ListBoxBinder(_objHints.ValueHints[pi.Name], CollConverter.ToStringArray((IEnumerable)piValue));
                    else
                        ci.Binder = new ListBoxBinder(CollConverter.ToStringArray((IEnumerable)piValue));
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            else
            {
                ci.Binder = new ObjCollectionBinder((IEnumerable)piValue, null);
            }

            AddControlInfo(page, ci, parentId);
        }

        private void AddControlInfo(Page page, PropertyCtrlInfo ci, ObjIdentifier parentId)
        {
            Debug.Assert(ci.Binder.Control != null);

            switch (ci.Layout.Section)
            {
                case Section.Header:
                    page.HeadCtrls.Add(ci); break;
                case Section.Footer:
                    page.FootCtrls.Add(ci); break;
                case Section.Body:
                default:
                    page.BodyCtrls.Add(ci); break;
            }

            ci.Identifier = new ObjIdentifier(ci.PropertyInfo.Name, parentId);
            _controlMap.Add(ci.Identifier.FullName, ci);
        }

        private PropertyCtrlInfo BuildControlInfo(PropertyInfo pi, object parentObj)
        {
            PropertyCtrlInfo ci = new PropertyCtrlInfo();
            ci.PropertyInfo = pi;
            ci.ParentObject = parentObj;

            ci.Layout = AttributeInterrogator.GetAttribute<LayoutHint>(pi);
            if (ci.Layout == null)
                ci.Layout = new LayoutHint();

            if (_objHints.LabelHints != null && _objHints.LabelHints.ContainsKey(pi.Name))
                ci.Layout.Label = _objHints.LabelHints[pi.Name];
            if (string.IsNullOrEmpty(ci.Layout.Label))
                ci.Layout.Label =  NameCleaner.ToPhrase(pi.Name);

            return ci;
        }

        private bool IsLeafCtrl(Type tp)
        {
            return (tp == typeof(System.Data.DataTable) || TypeInterrogator.IsDictionaryType(tp) ||
                TypeInterrogator.IsSingleValueType(tp));
        }

        private bool IsCollectionCtrl(Type tp)
        {
            return TypeInterrogator.IsCollectionType(tp);
        }
        #endregion

        #region Load data
        private void LoadComposite(Type tp, object typeObj, string objName)
        {
            PropertyInfo[] properties = tp.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in properties)
            {
                Type unerLyingType = TypeInterrogator.GetUnderlyingType(pi.PropertyType);
                if (IsSkipped(pi, unerLyingType))
                    continue;

                if (IsLeafCtrl(unerLyingType) ||IsCollectionCtrl(unerLyingType))
                {
                    PropertyCtrlInfo pci = _controlMap[PropertyInterrogator.GetFullName(pi)];
                    pci.ParentObject = typeObj;
                    pci.Binder.Value = pi.GetValue(typeObj, null);
                }
                else
                {
                    LoadComposite(unerLyingType, pi.GetValue(typeObj, null), pi.Name);
                }
            }
        }
        #endregion

        /// <summary>
        /// Return true if any of the following conditions is satisfied:
        ///   1. Property is Delegate / Event
        ///   2. Can not read or write
        ///   3. Marked with XmlIgnore
        /// </summary>
        private bool IsSkipped(PropertyInfo pi, Type tp)
        {
            if (TypeInterrogator.IsDelegateType(tp) || TypeInterrogator.IsEventType(tp))
                return true;

            if (!pi.CanRead || !pi.CanWrite) // skip readonly properties. may revisit this
                return true;

            System.ComponentModel.BrowsableAttribute brw = AttributeInterrogator.GetAttribute<System.ComponentModel.BrowsableAttribute>(pi);
            if (brw != null && !brw.Browsable)
                return true;

            return false;
        }
        #endregion

        #region private data
        private object _object;
        private Type _objType;

        private Page _mainPage = new Page();
        private Page _advancedPage = null;
        private CellBorderStyle _borderStyle = CellBorderStyle.None;
        private Dictionary<string, PropertyCtrlInfo> _controlMap = new Dictionary<string, PropertyCtrlInfo>();

        private ObjValueLabelHint _objHints;
        #endregion
    }

    internal sealed class Page
    {
        public Page()
        {
        }

        internal List<PropertyCtrlInfo> HeadCtrls
        {
            get { return _headCtrls; }
        }
        internal List<PropertyCtrlInfo> BodyCtrls
        {
            get { return _bodyCtrls; }
        }
        internal List<PropertyCtrlInfo> FootCtrls
        {
            get { return _footCtrls; }
        }

        internal void Draw(TableLayoutPanel container)
        {
            container.SuspendLayout();

            int rowIndex = 0;
            if (_headCtrls.Count > 0)
            {
                FlowLayoutPanel headPanel = BuildPanel<FlowLayoutPanel>(container, rowIndex++);
                DoFlowLayout(_headCtrls, headPanel);
            }

            if (_bodyCtrls.Count > 0)
            {
                TableLayoutPanel bodyPanel = BuildPanel<TableLayoutPanel>(container, rowIndex++);
                bodyPanel.CellBorderStyle = _borderStyle;
                DoGridLayout(_bodyCtrls, bodyPanel);
            }

            if (_footCtrls.Count > 0)
            {
                FlowLayoutPanel footerPanel = BuildPanel<FlowLayoutPanel>(container, rowIndex++);
                DoFlowLayout(_footCtrls, footerPanel);
            }

            container.ResumeLayout(false);
        }

        #region private functions
        private PanelT BuildPanel<PanelT>(TableLayoutPanel container, int rowIndex) where PanelT : Panel, new()
        {
            PanelT panel = new PanelT();
            panel.Anchor = UIConst.AutoSize;
            panel.AutoSize = true;
            panel.Dock = DockStyle.Fill;
            panel.Margin = new Padding(0);
            panel.Padding = new Padding(0);
            container.Controls.Add(panel, 0, rowIndex);

            return panel;
        }

        private void DoGridLayout(List<PropertyCtrlInfo> secionList, TableLayoutPanel container)
        {
            container.SuspendLayout();

            container.ColumnCount = 2;
            container.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            container.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            RowColumnLocation location = new RowColumnLocation(0, 0);
            foreach (PropertyCtrlInfo ci in secionList)
            {
                int rowSpan = ci.Layout.RowCount;
                int colSpan = ci.Layout.ColumnCount;
                location.Column = 0; // reset column location

                // Build label
                Label lbl = null;
                if (ci.Layout.Label != null)
                {
                    lbl = new Label();
                    lbl.AutoSize = true;
                    lbl.Text = ci.Layout.Label + ":";
                    lbl.Padding = new Padding(0, 3, 0, 0);
                    lbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                    if (_minLabelLength < ci.Layout.Label.Length)
                        _minLabelLength = ci.Layout.Label.Length;
                }

                // Add label
                if (lbl != null)
                {
                    container.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    if (colSpan > 1)
                    {
                        container.Controls.Add(lbl, location.Column, location.Row);
                        location.Row += 1;
                    }
                    else
                    {
                        container.Controls.Add(lbl, location.Column, location.Row);
                        location.Column++;
                    }
                }

                // Add control
                if (rowSpan > 1) // multi row control, prepare the space
                    container.RowStyles.Add(new RowStyle(SizeType.Absolute, UIDefault.RowHeight * rowSpan));
                else
                    container.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                container.Controls.Add(ci.Binder.Control, location.Column, location.Row);
                if (rowSpan > 1) // multi row control, set row span
                    container.SetRowSpan(ci.Binder.Control, rowSpan);
                if (colSpan > 1) // multi column control, set column span
                    container.SetColumnSpan(ci.Binder.Control, colSpan);

                location.Row += rowSpan;
            }

            container.ResumeLayout(false);
        }

        private void DoFlowLayout(List<PropertyCtrlInfo> secionList, FlowLayoutPanel container)
        {
            container.SuspendLayout();

            container.Padding = new Padding(3, 0, 0, 0);
            foreach (PropertyCtrlInfo ci in secionList)
            {
                container.Controls.Add(ci.Binder.Control);
            }

            container.ResumeLayout(false);
        }
        #endregion

        #region private data
        private List<PropertyCtrlInfo> _headCtrls = new List<PropertyCtrlInfo>();
        private List<PropertyCtrlInfo> _footCtrls = new List<PropertyCtrlInfo>();
        private List<PropertyCtrlInfo> _bodyCtrls = new List<PropertyCtrlInfo>();

        private CellBorderStyle _borderStyle = CellBorderStyle.None;
        private int _minLabelLength = 0;
        #endregion
    }
}
