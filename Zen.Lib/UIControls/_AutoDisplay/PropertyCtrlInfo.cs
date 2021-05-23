using System.Reflection;
using Zen.Common.Def.Layout;
using Zen.Utilities.Generics;

namespace Zen.UIControls
{
    internal class PropertyCtrlInfo
    {
        public PropertyCtrlInfo()
        {
        }

        public ObjIdentifier Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public LayoutHint Layout
        {
            get { return _layout; }
            set { _layout = value; }
        }

        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
            set { _propertyInfo = value; }
        }
        public object ParentObject
        {
            get { return _parentObject; }
            set { _parentObject = value; }
        }

        /// <summary> data <-> control </summary>
        public ITypeBinder Binder
        {
            get { return _binder; }
            set { _binder = value; }
        }

        /// <summary> Property Type <-> Control Value Type </summary>
        public IPropertyValueConverter Converter
        {
            get { return _converter; }
            set { _converter = value; }
        }

        #region private data
        private ObjIdentifier _identifier;
        private LayoutHint _layout;

        private PropertyInfo _propertyInfo;
        private object _parentObject;

        private ITypeBinder _binder = null;
        private IPropertyValueConverter _converter;
        #endregion
    }
}
