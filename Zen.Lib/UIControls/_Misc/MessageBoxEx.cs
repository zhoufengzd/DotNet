using System.Windows.Forms;

namespace Zen.UIControls.Misc
{
    /// <summary>
    /// List a few most commonly used messageboxes 
    ///     to save the headache of memorizing the message box style.
    /// </summary>
    public sealed class MessageBoxEx
    {
        public static DialogResult ShowInfo(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static DialogResult ShowError(string errorMsg, string title)
        {
            return MessageBox.Show(errorMsg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static DialogResult ShowStop(string errorMsg, string title)
        {
            return MessageBox.Show(errorMsg, title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        public static DialogResult ShowYesNo(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static DialogResult ShowYesNoCancel(string msg, string title)
        {
            return MessageBox.Show(msg, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
    }
}
