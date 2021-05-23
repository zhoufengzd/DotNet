using System;
using System.IO;
using System.Windows.Forms;
using Zen.Common.Def;
using Zen.UIControls.FileUI;

namespace Zen.FileProcessor
{
    using MessageBoxEx = Zen.UIControls.Misc.MessageBoxEx;

    /// <summary>
    /// Base class for text file based winform
    /// </summary>
    public partial class NotePadForm : Form
    {
        static readonly string DefaultTitle = "Notepad";
        static readonly string SavePromptMsg = "The text has changed. \r\nDo you want to save the changes?";

        public NotePadForm()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; this.Text = _title; }
        }

        #region protected functions
        /// <summary> Called during 'OnFormLoad' </summary>
        protected virtual void LoadCustomComponent()
        {
        }
        protected virtual void ShowOptionsDlg()
        {
            MessageBoxEx.ShowStop("Not implemented yet. ", Title);
        }
        #endregion

        #region Events
        #region Main
        private void OnFormLoad(object sender, EventArgs e)
        {
            this.SuspendLayout();
            LoadCustomComponent();
            this.ResumeLayout(true);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            TrySaveModified(sender, e);
        }
        #endregion

        #region Menu events

        #region File
        private void OnFileNew(object sender, EventArgs e)
        {
            TrySaveModified(sender, e);
            ResetState();
        }
        private void OnFileOpen(object sender, EventArgs e)
        {
            TrySaveModified(sender, e);

            string filePath = FileDirBrowser.BrowseFile(FileTypeFilter.TextFile, null);
            if (filePath == null)
                return;

            OpenFile(filePath);
        }
        private void OnFileSave(object sender, EventArgs e)
        {
            if (_filePath == null)
            {
                OnFileSaveAs(sender, e);
                return;
            }

            SaveFile(_filePath);
        }
        private void OnFileSaveAs(object sender, EventArgs e)
        {
            string fileName = (_filePath == null) ? null : Path.GetFileNameWithoutExtension(_filePath) + DateTime.Now.ToString("yyyyMMdd.hhmmss");
            string filePath = FileDirBrowser.SaveFile(FileTypeFilter.TextFile, fileName, null);
            if (filePath == null)
                return;

            SaveFile(filePath);
        }
        private void OnFileExit(object sender, EventArgs e)
        {
            TrySaveModified(sender, e);
            this.Close();
        }
        #endregion

        #region Edit
        private void OnEditUndo(object sender, EventArgs e)
        {
            _textBoxInput.Undo();
        }
        private void OnEditCut(object sender, EventArgs e)
        {
            _textBoxInput.Cut();
        }
        private void OnEditCopy(object sender, EventArgs e)
        {
            _textBoxInput.Copy();
        }
        private void OnEditPaste(object sender, EventArgs e)
        {
            _textBoxInput.Paste();
        }
        private void OnEditSelectAll(object sender, EventArgs e)
        {
            _textBoxInput.SelectAll();
        }
        #endregion

        #region View
        private void OnStatusBar(object sender, EventArgs e)
        {
            bool originallyVisibility = _statusStrip.Visible;
            _statusStrip.Visible = !originallyVisibility;

            if (originallyVisibility)
                _panelClient.Size = new System.Drawing.Size(_panelClient.Width, _panelClient.Height + _statusStrip.Height);
            else
                _panelClient.Size = new System.Drawing.Size(_panelClient.Width, _panelClient.Height - _statusStrip.Height);
        }
        #endregion

        #region Tool
        private void OnToolOptions(object sender, EventArgs e)
        {
            ShowOptionsDlg();
        }
        #endregion

        #endregion

        #region TextBox
        private void TextBox_TextChanged(object sender, System.EventArgs e)
        {
            _docModified = true;
        }

        private void TextBox_DragEnter(object sender, DragEventArgs e)
        {
            IDataObject obj = e.Data;

            if (obj.GetDataPresent(DataFormats.Text) || obj.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TextBox_DragDrop(object sender, DragEventArgs e)
        {
            IDataObject obj = e.Data;
            if (obj.GetDataPresent(DataFormats.Text))
            {
                _textBoxInput.Text = _textBoxInput.Text.Insert(_textBoxInput.SelectionStart, (string)obj.GetData(DataFormats.Text));
            }
            else if (obj.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])obj.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    string filePath = (string)(files[0]);
                    this.Activate();

                    TrySaveModified(sender, e);
                    OpenFile(filePath);
                }
            }
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDown = true;
        }

        private void TextBox_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                if (_textBoxInput.SelectionLength > 0)
                    _textBoxInput.DoDragDrop(_textBoxInput.Text, DragDropEffects.Copy);
                _mouseDown = false;
            }
        }
        #endregion
        #endregion

        #region private functions
        private void UpdateFilePath(string filePath)
        {
            _filePath = filePath;
            Text = (_filePath == null) ? Title : Path.GetFileName(_filePath) + " - " + Title;
        }

        private void TrySaveModified(object sender, EventArgs e)
        {
            if (!_docModified)
                return;

            if (MessageBoxEx.ShowYesNo(SavePromptMsg, Title) != DialogResult.Yes)
                return;

            OnFileSave(sender, e);
        }

        private void SaveFile(string filePath)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filePath);
                writer.Write(_textBoxInput.Text);
                writer.Close();

                UpdateFilePath(filePath);
                _docModified = false;
            }
            catch (Exception err)
            {
                MessageBoxEx.ShowError(err.Message, Title);
            }
        }
        private void OpenFile(string filePath)
        {
            try
            {
                StreamReader reader = new StreamReader(filePath);
                _textBoxInput.Text = reader.ReadToEnd();
                reader.Close();

                UpdateFilePath(filePath);
                _docModified = false;
            }
            catch (Exception err)
            {
                MessageBoxEx.ShowError(err.Message, Title);
            }
        }

        private void ResetState()
        {
            _textBoxInput.Clear();
            _docModified = false;
            UpdateFilePath(null);
        }
        #endregion

        #region private data
        private string _title = DefaultTitle;
        private string _filePath = null;
        private bool _docModified = false;
        private bool _mouseDown = false;
        #endregion
    }
}
