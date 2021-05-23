using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

using Zen.Common.Def.Layout;
using Zen.UIControls.FileUI;
using Zen.UIControls.Layout;
using Zen.Utilities;
using Zen.Utilities.Algorithm;
using Zen.Utilities.Generics;

namespace Zen.UIControls
{
    using NumericRange = Zen.Common.Def.Twin<Decimal>;

    public sealed class PropertyEnvelope<T>
    {
        public PropertyEnvelope(T obj)
        {
            _object = obj;
        }

        [LayoutHint(ColumnCount = 2)]
        public T Data
        {
            get { return _object; }
            set { _object = value; }
        }
        private T _object;
    }

    #region PCV: Property Value Converter
    internal interface IPropertyValueConverter
    {
        object ToPropertyValue(object cv);
    }

    internal sealed class EnumPCV : IPropertyValueConverter
    {
        public EnumPCV(Type enumType)
        {
            _enumType = enumType;
        }

        public object ToPropertyValue(object cv)
        {
            return EnumConverter.ToValue(_enumType, cv.ToString());
        }
        public string ToControlValue(Enum pv)
        {
            return EnumConverter.ToName(pv);
        }

        #region private data
        private Type _enumType;
        #endregion
    }

    internal sealed class BitFlagsPCV : IPropertyValueConverter
    {
        public BitFlagsPCV(Type enumType)
        {
            _enumType = enumType;
        }

        public object ToPropertyValue(object cv)
        {
            return ToPropertyValue((IEnumerable<string>)cv);
        }
        public object ToPropertyValue(IEnumerable<string> cv)
        {
            return (object)(EnumConverter.ToValues(_enumType, cv));
        }
        public IEnumerable<string> ToControlValue(Type enumType)
        {
            return ToControlValue(Enum.GetNames(enumType));
        }
        public IEnumerable<string> ToControlValue(IEnumerable<int> pv)
        {
            return ToControlValue((IEnumerable)pv);
        }
        public IEnumerable<string> ToControlValue(IEnumerable pv)
        {
            return EnumConverter.ToNames(pv, true);
        }

        #region private data
        private Type _enumType;
        #endregion
    }

    internal sealed class ArrayPCV : IPropertyValueConverter
    {
        public ArrayPCV(Type itemType)
        {
            _itemType = itemType;
        }

        public object ToPropertyValue(object cv)
        {
            return (object)(CollConverter.ToArray((IEnumerable)cv, _itemType));
        }

        private Type _itemType;
    }

    internal sealed class ListPCV : IPropertyValueConverter
    {
        public ListPCV(Type itemType)
        {
            _itemType = itemType;
        }

        public object ToPropertyValue(object cv)
        {
            return (object)(CollConverter.ToList((IEnumerable)cv, _itemType));
        }

        private Type _itemType;
    }

    internal sealed class DecimalPCV : IPropertyValueConverter
    {
        public DecimalPCV(Type numericType)
        {
            _itemType = numericType;
        }

        public object ToPropertyValue(object cv)
        {
            Decimal? dcm = null;
            if (TypeInterrogator.IsStringType(cv.GetType()))
                dcm = Decimal.Parse(cv.ToString());
            else
                dcm = (Decimal)cv;

            if (_itemType == typeof(double))
                return Decimal.ToDouble((Decimal)dcm);
            else if (_itemType == typeof(float))
                return Decimal.ToSingle((Decimal)dcm);
            else if (_itemType == typeof(int))
                return Decimal.ToInt32((Decimal)dcm);
            else if (_itemType == typeof(long))
                return Decimal.ToInt64((Decimal)dcm);
            else if (_itemType == typeof(uint))
                return Decimal.ToUInt32((Decimal)dcm);
            else if (_itemType == typeof(ulong))
                return Decimal.ToUInt64((Decimal)dcm);
            else
                return null;
        }

        public Decimal ToControlValue(double pv)
        {
            return new Decimal(pv);
        }
        public Decimal ToControlValue(float pv)
        {
            return new Decimal(pv);
        }
        public Decimal ToControlValue(int pv)
        {
            return new Decimal(pv);
        }
        /// <summary> binary array of 32-bit signed integers </summary>
        public Decimal ToControlValue(int[] pv)
        {
            return new Decimal(pv);
        }
        public Decimal ToControlValue(long pv)
        {
            return new Decimal(pv);
        }
        public Decimal ToControlValue(uint pv)
        {
            return new Decimal(pv);
        }
        public Decimal ToControlValue(ulong pv)
        {
            return new Decimal(pv);
        }

        #region private data
        private Type _itemType;
        #endregion
    }

    internal sealed class DictionaryPCV : IPropertyValueConverter
    {
        public DictionaryPCV()
        {
        }

        public object ToPropertyValue(object cv)
        {
            Dictionary<string, string> converted = (Dictionary<string, string>)cv;
            if (_keyType == typeof(string) && _valueType == typeof(string))
                return converted;

            IDictionary dictionary = (IDictionary)GenBuilder.BuildInstance(typeof(Dictionary<,>), new Type[] { _keyType, _valueType });
            foreach (KeyValuePair<string, string> kv in converted)
                dictionary.Add((new StrAdapter(kv.Key)), (new StrAdapter(kv.Value)));

            return dictionary;
        }
        public Dictionary<string, string> ToControlValue(object dict)
        {
            IDictionary dictionary = dict as IDictionary;
            if (dictionary == null)
            {
                Debug.Assert(false);
                return null;
            }

            Type[] keyValueTypes = dict.GetType().GetGenericArguments();
            _keyType = keyValueTypes[0];
            _valueType = keyValueTypes[1];

            if (_keyType == typeof(string) && _valueType == typeof(string))
                return dict as Dictionary<string, string>;

            Dictionary<string, string> converted = new Dictionary<string, string>(dictionary.Count);
            IDictionaryEnumerator kv = dictionary.GetEnumerator();
            while (kv.MoveNext())
                converted.Add(kv.Key.ToString(), kv.Value.ToString());

            return converted;
        }

        #region private data
        private Type _keyType;
        private Type _valueType;
        #endregion
    }

    #endregion

    #region Type Binder

    #region Base Binder
    /// <summary>
    /// Bind object and build control(s) for given type
    ///   Control <-> Object | Type | Value
    /// </summary>
    internal interface ITypeBinder
    {
        Type Type { get; }
        Control Control { get; }
        Object Value { get; set; }
    }

    /// <summary>
    /// Build control and update {control <==> value}
    /// </summary>
    internal abstract class BinderBase : ITypeBinder
    {
        public Type Type
        {
            get { return _type; }
        }
        public Control Control
        {
            get { return _control; }
        }
        public object Value
        {
            get
            {
                ControlToValue();
                return _value;
            }
            set
            {
                if (_value == value)
                    return;

                if (ValueToControl(value))
                    _value = value;
            }
        }

        #region abstract functions
        protected abstract void ControlToValue();
        protected abstract bool ValueToControl(object value);
        #endregion

        #region protected data
        protected Type _type;
        protected object _value;
        protected Control _control;
        #endregion
    }
    #endregion

    #region Binder by control type

    /// <summary>
    /// Bind bool value
    /// </summary>
    internal sealed class CheckBoxBinder : BinderBase
    {
        public CheckBoxBinder(string label)
            : this(label, false)
        {
        }
        public CheckBoxBinder(string label, bool value)
        {
            Init(label, value);
        }

        #region Base override
        protected override void ControlToValue()
        {
            _value = ((CheckBox)_control).Checked;
        }
        protected override bool ValueToControl(object value)
        {
            ((CheckBox)_control).Checked = (bool)value;
            return true;
        }
        #endregion

        #region private functions
        private void Init(string label, bool value)
        {
            if (_control != null)
                return;

            CheckBox ckBox = new CheckBox();
            ckBox.AutoSize = true;
            ckBox.Text = label;

            _control = ckBox;
            Value = value;
        }
        #endregion
    }

    internal sealed class ComboBoxBinder : BinderBase
    {
        public ComboBoxBinder()
            : this(null, null, ComboBoxStyle.DropDown)
        {
        }
        public ComboBoxBinder(string value, IEnumerable<string> hints, ComboBoxStyle style)
        {
            if (style == ComboBoxStyle.DropDownList)
                Debug.Assert(Finder.Find(hints, value));
            Init(hints, style, value);
        }

        #region Base override
        protected override void ControlToValue()
        {
            _value = _control.Text;
        }
        protected override bool ValueToControl(object value)
        {
            _control.Text = (string)value;
            return true;
        }
        #endregion

        #region private functions
        private void Init(IEnumerable<string> hints, ComboBoxStyle style, string value)
        {
            if (_control != null)
                return;

            ComboBox cbx = new ComboBox();
            cbx.AutoSize = true;
            cbx.DropDownStyle = style;
            cbx.Anchor = UIConst.AutoSize;

            if (hints != null)
            {
                foreach (string it in hints)
                    cbx.Items.Add(it);

                if (cbx.Items.Count > 0)
                    cbx.SelectedIndex = 0;
            }

            _control = cbx;
            Value = value;
        }
        #endregion
    }

    /// <summary>
    /// Allow user choose from enum bit flags or string collection
    /// </summary>
    internal sealed class CheckedListBoxBinder : BinderBase
    {
        public CheckedListBoxBinder(IEnumerable<string> items)
            : this(items, null)
        {
        }
        public CheckedListBoxBinder(IEnumerable<string> items, IEnumerable<string> checkedItems)
        {
            Init(items, checkedItems);
        }

        #region Base override
        protected override void ControlToValue()
        {
            List<string> checkedItems = new List<string>();
            foreach (object item in ((CheckedListBox)_control).CheckedItems)
                checkedItems.Add((string)item);

            _value = checkedItems;
        }

        protected override bool ValueToControl(object value)
        {
            CheckedListBox ckListBox = (CheckedListBox)_control;
            IEnumerable<string> checkedItems = (IEnumerable<string>)value;

            CheckedListBox.ObjectCollection items = ckListBox.Items;
            for (int ind = 0; ind < items.Count; ind++)
            {
                if (Finder.Find(checkedItems, (string)items[ind]))
                    ckListBox.SetItemChecked(ind, true);
            }

            return true;
        }
        #endregion

        #region private functions
        private void Init(IEnumerable<string> items, IEnumerable<string> checkedItems)
        {
            if (_control != null)
                return;

            CheckedListBox ckListBox = new CheckedListBox();
            ckListBox.AutoSize = true;
            ckListBox.Anchor = UIConst.AutoSize;
            foreach (string it in items)
                ckListBox.Items.Add(it);

            _control = ckListBox;
            Value = checkedItems;
        }
        #endregion
    }

    /// <summary>
    /// Allow user pick date, time, or both
    /// </summary>
    internal sealed class DateTimePickerBinder : BinderBase
    {
        public DateTimePickerBinder()
            : this(DateTime.MinValue, null)
        {
        }
        public DateTimePickerBinder(DateTime value, DateTimePickerHint hint)
        {
            Init(value, ((hint == null) ? new DateTimePickerHint() : hint));
        }

        #region Base override
        protected override void ControlToValue()
        {
            DateTime dt = DateTime.MinValue;
            if (_customPicker != null)
            {
                _value = _customPicker.Value;
            }
            else
            {
                int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, millisecond = 0;
                if (_datePicker != null)
                {
                    year = _datePicker.Value.Year;
                    month = _datePicker.Value.Month;
                    day = _datePicker.Value.Day;
                }
                if (_timePicker != null)
                {
                    hour = _datePicker.Value.Hour;
                    minute = _datePicker.Value.Minute;
                    second = _datePicker.Value.Second;
                    millisecond = _datePicker.Value.Millisecond;
                }
                _value = new DateTime(year, month, day, hour, minute, second, millisecond = 0);
            }
        }

        protected override bool ValueToControl(object value)
        {
            DateTime dt = (DateTime)value;
            if (dt == DateTime.MinValue)
                dt = DateTime.Today;

            if (_customPicker != null)
                _customPicker.Value = dt;
            if (_datePicker != null)
                _datePicker.Value = dt;
            if (_timePicker != null)
                _timePicker.Value = dt;

            return true;
        }
        #endregion

        #region private functions
        private void Init(DateTime value, DateTimePickerHint hint)
        {
            if (_control != null)
                return;

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = FlowDirection.LeftToRight;
            panel.AutoSize = true;

            if (hint.Format != null) // Custom picker
            {
                _customPicker = new DateTimePicker();
                _customPicker.Format = DateTimePickerFormat.Custom;
                _customPicker.CustomFormat = hint.Format;
                panel.Controls.Add(_customPicker);
            }
            else
            {
                if ((hint.Mode & DateTimeMode.DateOnly) == DateTimeMode.DateOnly)
                {
                    _datePicker = new DateTimePicker();
                    _datePicker.Format = DateTimePickerFormat.Short;
                    panel.Controls.Add(_datePicker);
                }

                if ((hint.Mode & DateTimeMode.TimeOnly) == DateTimeMode.TimeOnly)
                {
                    _timePicker = new DateTimePicker();
                    _timePicker.Format = DateTimePickerFormat.Time;
                    panel.Controls.Add(_timePicker);
                }
            }

            _control = panel;
            Value = value;
        }

        #endregion

        #region private data
        private DateTimePicker _datePicker;
        private DateTimePicker _timePicker;
        private DateTimePicker _customPicker;
        #endregion
    }

    /// <summary>
    /// Bind to single numeric value
    /// </summary>
    internal sealed class NumericUpDownSingleBinder : BinderBase
    {
        public NumericUpDownSingleBinder(Decimal value, NumericUpDownHint hint)
        {
            Init(value, ((hint == null) ? new NumericUpDownHint() : hint));
        }

        #region Base override
        protected override void ControlToValue()
        {
            _value = ((NumericUpDown)_control).Value;
        }

        protected override bool ValueToControl(object value)
        {
            ((NumericUpDown)_control).Value = (new StrAdapter(value.ToString()));
            return true;
        }
        #endregion

        #region private functions
        private void Init(Decimal value, NumericUpDownHint hint)
        {
            if (_control != null)
                return;

            NumericUpDown numCtrl = new NumericUpDown();
            numCtrl.AutoSize = true;
            numCtrl.Minimum = hint.Minimum;
            numCtrl.Maximum = hint.Maximum;
            numCtrl.Increment = hint.Increment;

            _control = numCtrl;
            Value = value;
        }
        #endregion
    }

    /// <summary>
    /// Bind to pair values
    /// </summary>
    internal sealed class NumericUpDownPairBinder : BinderBase
    {
        public NumericUpDownPairBinder()
            : this(new NumericRange(Decimal.MinValue, Decimal.MinValue), null)
        {
        }
        public NumericUpDownPairBinder(NumericRange range, NumericUpDownHint hint)
        {
            Init(range, ((hint == null) ? new NumericUpDownHint() : hint));
        }

        #region Base override
        protected override void ControlToValue()
        {
            NumericRange range = (NumericRange)_value;
            range.Min = _numericFrom.Value;
            range.Max = _numericTo.Value;
        }

        protected override bool ValueToControl(object value)
        {
            NumericRange range = (NumericRange)value;

            if (range.From != Decimal.MinValue)
                _numericFrom.Value = range.From;
            if (range.To != Decimal.MinValue)
                _numericTo.Value = range.To;

            return true;
        }
        #endregion

        #region private functions
        private void Init(NumericRange value, NumericUpDownHint hint)
        {
            if (_control != null)
                return;

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = FlowDirection.LeftToRight;
            panel.AutoSize = true;

            Label lblFrom = new Label();
            lblFrom.Text = "From";
            panel.Controls.Add(lblFrom);

            _numericFrom = new NumericUpDown();
            _numericFrom.Minimum = hint.Minimum;
            _numericFrom.Maximum = hint.Maximum;
            _numericFrom.Increment = hint.Increment;
            panel.Controls.Add(_numericFrom);

            Label lblTo = new Label();
            lblTo.Text = "To";
            panel.Controls.Add(lblTo);

            _numericTo = new NumericUpDown();
            _numericTo.Minimum = hint.Minimum;
            _numericTo.Maximum = hint.Maximum;
            _numericTo.Increment = hint.Increment;
            panel.Controls.Add(_numericTo);

            _control = panel;
            Value = value;
        }
        #endregion

        #region private data
        private NumericUpDown _numericFrom;
        private NumericUpDown _numericTo;
        #endregion
    }

    /// <summary>
    /// Selection is easier than CheckedListBox
    /// </summary>
    internal sealed class ListBoxBinder : BinderBase
    {
        public ListBoxBinder(IEnumerable<string> items)
            : this(items, null)
        {
        }
        public ListBoxBinder(IEnumerable<string> items, IEnumerable<string> selectedItems)
        {
            Init(items, selectedItems);
        }

        #region Base override
        protected override void ControlToValue()
        {
            List<string> selectedItems = new List<string>();
            foreach (object item in ((ListBox)_control).SelectedItems)
                selectedItems.Add((string)item);

            _value = selectedItems;
        }

        protected override bool ValueToControl(object value)
        {
            if (value == null)
                return false;

            ListBox lb = (ListBox)_control;
            lb.SelectionMode = SelectionMode.MultiSimple;
            IEnumerable<string> selectedItems = (IEnumerable<string>)value;

            ListBox.ObjectCollection items = lb.Items;
            for (int ind = 0; ind < items.Count; ind++)
            {
                if (Finder.Find(selectedItems, (string)items[ind]))
                    lb.SetSelected(ind, true);
            }

            return true;
        }
        #endregion

        #region private functions
        private void Init(IEnumerable<string> items, IEnumerable<string> selectedItems)
        {
            if (_control != null)
                return;

            ListBox lb = new ListBox();
            lb.AutoSize = true;
            lb.Anchor = UIConst.AutoSize;
            lb.SelectionMode = SelectionMode.MultiSimple;

            if (items != null)
            {
                foreach (string it in items)
                    lb.Items.Add(it);
            }

            _control = lb;
            Value = selectedItems;
        }
        #endregion
    }

    /// <summary>
    /// Map enum values to radio buttons
    /// </summary>
    internal sealed class RadioButtonBinder : BinderBase
    {
        public RadioButtonBinder(Type enumType)
            : this(enumType, -1)
        {
        }
        public RadioButtonBinder(Type enumType, int value)
        {
            Debug.Assert(TypeInterrogator.IsEnumType(enumType));

            Init(enumType, value);
        }

        #region Base override
        protected override void ControlToValue()
        {
            int index = 0;
            foreach (RadioButton btn in _radioButtons)
            {
                if (btn.Checked)
                {
                    _value = _valueArray.GetValue(index);
                    break;
                }

                index++;
            }
        }

        protected override bool ValueToControl(object value)
        {
            int index = ValueToIndex((int)value);
            if (index < 0)
                return false;

            _radioButtons[index].Checked = true;
            return true;
        }
        #endregion

        #region private functions
        private void Init(Type enumType, int value)
        {
            if (_control != null)
                return;

            _type = enumType;
            _valueArray = Enum.GetValues(_type);

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.FlowDirection = FlowDirection.LeftToRight;
            panel.AutoSize = true;

            int index = 0;
            _radioButtons = new RadioButton[_valueArray.Length];
            foreach (object enumValue in _valueArray)
            {
                RadioButton btn = new RadioButton();
                btn.AutoSize = true;
                btn.Text = Zen.Utilities.Text.NameCleaner.ToPhrase(enumValue.ToString());
                panel.Controls.Add(btn);

                _radioButtons[index] = btn;
                index++;
            }

            _control = panel;
            Value = value;
        }

        private int ValueToIndex(int value)
        {
            int index = 0;
            foreach (object enumValue in _valueArray)
            {
                if ((int)enumValue == value)
                    return index;

                index++;
            }

            return -1;
        }
        #endregion

        #region private data
        private Array _valueArray;
        private RadioButton[] _radioButtons;
        #endregion
    }

    /// <summary>
    /// String, Numeric, File / Directory path
    /// </summary>
    internal sealed class TextBoxBinder : BinderBase
    {
        public TextBoxBinder()
            : this(null, null)
        {
        }
        public TextBoxBinder(string value, LayoutHint hint)
        {
            Init(value, hint);
        }

        #region Base override
        protected override void ControlToValue()
        {
            _value = _control.Text;
        }
        protected override bool ValueToControl(object value)
        {
            _control.Text = (string)value;
            return true;
        }
        #endregion

        #region private functions
        private void Init(string value, LayoutHint hint)
        {
            if (_control != null)
                return;

            BrowserHint browserHint = hint as BrowserHint;
            TextBoxHint tbHint = hint as TextBoxHint;
            if (browserHint != null)
            {
                FileDirBrowserCtrl fbc = new FileDirBrowserCtrl();
                fbc.AutoSize = true;
                fbc.Options = browserHint.BrowserOptions;
                _control = fbc;
            }
            else if (tbHint != null)
            {
                MaskedTextBoxEx tb = new MaskedTextBoxEx();
                if (tbHint.CharsAccepted != null)
                    tb.CharsAccepted = tbHint.CharsAccepted;
                if (tbHint.CharsNotAccepted != null)
                    tb.CharsNotAccepted = tbHint.CharsNotAccepted;

                if (tbHint.RowCount > 1)
                    tb.Multiline = true;
                if (tbHint.IsReadonly)
                    tb.ReadOnly = true;

                _control = tb;
            }
            else
            {
                TextBox tb = new TextBox();
                tb.AutoSize = true;
                _control = tb;
            }

            _control.Anchor = UIConst.AutoSize;
            Value = value;
        }
        #endregion
    }

    /// <summary>
    /// Collection of objects -> button --> Object View Page
    /// </summary>
    internal sealed class ObjCollectionBinder : BinderBase
    {
        public ObjCollectionBinder()
            : this(null, null)
        {
        }
        public ObjCollectionBinder(IEnumerable collection, LayoutHint hint)
        {
            Init(collection, hint);
        }

        #region Base override
        protected override void ControlToValue()
        {
            //_value = ((TextBox)_control).Text;
        }
        protected override bool ValueToControl(object value)
        {
            //((TextBox)_control).Text = (string)value;
            return true;
        }
        #endregion

        #region private functions
        private void Init(IEnumerable collection, LayoutHint hint)
        {
            if (_control != null)
                return;

            Button btn = new Button();
            btn.AutoSize = true;
            btn.Anchor = AnchorStyles.Left;
            btn.Text = "Collection...";
            btn.Click += new System.EventHandler(OnClick);

            _control = btn;
            Value = collection;
        }

        private void OnClick(object sender, EventArgs e)
        {
            CollectionDlg dlg = new CollectionDlg();
            dlg.AddOption((IEnumerable)_value);
            dlg.ShowDialog();
        }
        #endregion
    }

    /// <summary>
    /// DataGridView: DataTable Binder
    /// </summary>
    internal sealed class DataGridViewBinder : BinderBase
    {
        public DataGridViewBinder()
            : this(null)
        {
        }
        public DataGridViewBinder(DataTable value)
        {
            Init(value);
        }

        #region Base override
        protected override void ControlToValue()
        {
            //_value = _control.Text;
        }
        protected override bool ValueToControl(object value)
        {
            ((DataGridView)_control).DataSource = (DataTable)value;
            return true;
        }
        #endregion

        #region private functions
        private void Init(DataTable value)
        {
            if (_control != null)
                return;

            _control = new DataGridView();
            _control.AutoSize = true;
            _control.Anchor = UIConst.AutoSize;
            Value = value;
        }
        #endregion
    }

    /// <summary>
    /// DictionaryCtrlBinder: Bind to string Dictionary
    /// </summary>
    internal sealed class DictionaryCtrlBinder : BinderBase
    {
        public DictionaryCtrlBinder()
            : this(null, null)
        {
        }
        public DictionaryCtrlBinder(Dictionary<string, string> value, DictionaryHint hint)
        {
            Init(value, hint);
        }

        #region Base override
        protected override void ControlToValue()
        {
            ((DictionaryCtrl)_control).OnOk();
        }
        protected override bool ValueToControl(object value)
        {
            ((DictionaryCtrl)_control).SetObject((Dictionary<string, string>)value, _hint, TableLayoutPanelCellBorderStyle.None);
            return true;
        }
        #endregion

        #region private functions
        private void Init(Dictionary<string, string> value, DictionaryHint hint)
        {
            if (_control != null)
                return;

            if (value == null)
                return;

            _hint = hint;
            _control = new DictionaryCtrl();
            _control.Anchor = UIConst.AutoSize;
            Value = value;
        }
        #endregion

        #region private data
        private DictionaryHint _hint;
        #endregion
    }

    #endregion

    #endregion
}
