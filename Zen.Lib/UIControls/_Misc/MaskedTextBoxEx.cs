using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Zen.UIControls
{
    public sealed class MaskedTextBoxEx : MaskedTextBox
    {
        public MaskedTextBoxEx()
        {
        }

        /// <summary>
        /// Accepted or not accepted chars are both exposed if one set is smaller
        ///   Don't set both of them (no need and will fire assertions.
        /// </summary>
        public char[] CharsAccepted
        {
            set
            {
                Debug.Assert(_notAccepted == null);
                _accepted = new List<char>(value);
                _accepted.Sort();
            }
        }
        public char[] CharsNotAccepted
        {
            set
            {
                Debug.Assert(_accepted == null);
                _notAccepted = new List<char>(value);
                _notAccepted.Sort();
            }
        }

        #region Base override
        /// <summary>
        /// Restricts the entry of characters
        /// #1. Check if any known valid key strokes defined 
        ///     Backspace key is OK
        ///     [control] | [alt] key combinations is OK
        /// #2. Check if any known invalid key strokes pressed
        /// #3. Otherwise, let it pass
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            char keyInput = e.KeyChar;
            bool accepted = true;
            if (_accepted != null)
            {
                accepted = (_accepted.BinarySearch(keyInput) > 0) || (e.KeyChar == '\b') ||
                    ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0);
            }
            else if (_notAccepted != null)
            {
                accepted = (_notAccepted.BinarySearch(keyInput) < 0);
            }

            e.Handled = accepted ? false : true;
        }
        #endregion

        #region private data
        private List<char> _accepted;
        private List<char> _notAccepted;
        #endregion
    }

}