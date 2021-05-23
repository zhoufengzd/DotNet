using System.Windows.Forms;

namespace Zen.UIControls
{
    using SerializationMgr = Zen.Utilities.Proc.ConfigurationMgr;

    public sealed class DialogInvoker
    {
        public static DialogResult ShowDialog<T>(T obj, string title, SerializationMgr serializationMgr)
        {
            PropertyDlg dlg = new PropertyDlg(title);
            dlg.AddOption(new PropertyEnvelope<T>(obj));
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK && serializationMgr != null)
                serializationMgr.SaveProfile<T>(obj, title);

            return result;
        }

    }
}
