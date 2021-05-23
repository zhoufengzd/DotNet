
namespace Zen.UIControls
{
    using UserControl = System.Windows.Forms.UserControl;

    interface IValidationCtrl
    {
        bool OnOk();
        bool OnCancel();

        //bool ValidateValue();
        // --eee! All interface have to be public.
    }

    public class ValidationUserCtrlBase : UserControl, IValidationCtrl
    {
        public bool OnOk() { return ValidateValue(); }
        public bool OnCancel() { return true; }

        protected virtual bool ValidateValue() { return true; }
    }

}
