using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ald.SerialTerminal.Main.UserControls
{
    /// <summary>
    /// Interaction logic for MyConsole.xaml
    /// </summary>
    public partial class MyConsole : UserControl
    {
        private SolidColorBrush headerBrush = new SolidColorBrush(Color.FromRgb(100, 255, 200));
        private SolidColorBrush contentBrush = new SolidColorBrush(Color.FromRgb(0x80, 0xB9, 0xFF));

        public delegate void DCommandDispatch(object who, string command);

        public event DCommandDispatch OnCommandDispatch;

        TextPointer lastCaretPosition;
        int editionZoneStartIndex;

        private List<string> lastCommands = new List<string>();
        private int lastCommandIndex = 0;

        public MyConsole()
        {
            InitializeComponent();
            this.UpdateLastCaretPosition();
            this.AddHeader();            
        }

        private void ChangeSelectionColor(SolidColorBrush targetBrush)
        {
            this.richTxtConsole.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, targetBrush);
        }

        private void AddHeader()
        {
            this.richTxtConsole.Selection.Select(this.richTxtConsole.Document.ContentEnd, this.richTxtConsole.Document.ContentEnd);
            this.richTxtConsole.CaretPosition = this.richTxtConsole.Document.ContentEnd;


            this.ChangeSelectionColor(this.headerBrush);

            this.richTxtConsole.Selection.Text = ("\n" + Environment.UserName + ">");

            this.ChangeSelectionColor(this.headerBrush);

            //this.richTxtConsole.AppendText("\n" + Environment.UserName + ">");

            //this.richTxtConsole.Selection.Select(this.richTxtConsole.Document.ContentEnd, this.richTxtConsole.Document.ContentEnd);
            //this.richTxtConsole.CaretPosition = this.richTxtConsole.Document.ContentEnd;

            this.richTxtConsole.CaretPosition = this.richTxtConsole.Document.ContentEnd;

            this.UpdateLastCaretPosition();
            this.editionZoneStartIndex = this.GetCaretPositionAsPlainText();

            this.ChangeSelectionColor(this.contentBrush);

            
        }

        private void MoveToEnd()
        {
            this.richTxtConsole.Selection.Select(this.richTxtConsole.Document.ContentEnd, this.richTxtConsole.Document.ContentEnd);
            this.richTxtConsole.CaretPosition = this.richTxtConsole.Document.ContentEnd;
            this.richTxtConsole.ScrollToEnd();
        }

        private void UpdateLastCaretPosition()
        {
            this.lastCaretPosition = this.richTxtConsole.CaretPosition.DocumentEnd.GetPositionAtOffset(0);
        }

        public void SetText(string content)
        {
            this.richTxtConsole.SelectAll();
            this.richTxtConsole.Selection.Text = content;
            this.AddHeader();
        }

        private void MenuItemClearConsole_Click(object sender, RoutedEventArgs e)
        {
            this.richTxtConsole.SelectAll();
            this.richTxtConsole.Selection.Text = "";
            this.AddHeader();
        }

        private void richTxtConsole_SelectionChanged(object sender, RoutedEventArgs e)
        {
            /*
            if (this.richTxtConsole.CaretPosition.Paragraph == null)
                return;

            int offset = this.GetOffset();

            this.richTxtConsole.IsReadOnly = offset < -2;
            */
        }

        private int GetOffset()
        {

            var endDocument = this.lastCaretPosition;
            var caretPosition = this.richTxtConsole.CaretPosition;

            return endDocument.GetOffsetToPosition(caretPosition);

        }

        internal void AppendResponse(string strNew)
        {

            this.MoveToEnd();

            this.richTxtConsole.Selection.Text = strNew;
            this.ChangeSelectionColor(this.contentBrush);
                       
            this.AddHeader();
            this.UpdateLastCaretPosition();
            this.MoveToEnd();
        }

        private void richTxtConsole_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void richTxtConsole_PreviewKeyDown(object sender, KeyEventArgs e)
        {            
            if (this.richTxtConsole.CaretPosition.Paragraph == null)
                return;

            List<Key> specialKeys = new List<Key>() { Key.PageUp, Key.PageDown, Key.End };

            if (specialKeys.Contains(e.Key))
            {
                return;
            }

            if (e.Key == Key.Up || e.Key == Key.Down)
            {

                e.Handled = true;
                return;
            }

            bool editableZone = IsOnEditableZone(e.Key == Key.Back || e.Key == Key.Left);

            if (editableZone)
            {
                this.ChangeSelectionColor(this.contentBrush);

                if (e.Key == Key.Enter)
                {
                    this.DispatchCommand();
                    e.Handled = true;
                    return;
                }
            }            

            e.Handled = !editableZone;
        }

        private void DispatchCommand()
        {
            TextPointer start = this.richTxtConsole.Document.ContentStart;
            TextPointer end = this.richTxtConsole.Document.ContentEnd;

            TextRange allRange = new TextRange(start, end);

            string command = allRange.Text.Substring(editionZoneStartIndex);

            
            if (command.EndsWith("\r\n"))
            {
                command = command.Substring(0, command.Length - 2);
            }
            

            this.lastCommands.Add(command);
            this.OnCommandDispatch?.Invoke(this, command);

            //this.AddHeader();
        }

        int GetCaretPositionAsPlainText()
        {
            TextPointer start = this.richTxtConsole.Document.ContentStart;
            TextPointer caret = this.richTxtConsole.CaretPosition;
            TextRange range = new TextRange(start, caret);

            int indexInText = range.Text.Length;

            return indexInText;
        }

        private bool IsOnEditableZone(bool isBackSpace)
        {
            int currentPosition = this.GetCaretPositionAsPlainText();

            if (isBackSpace)
            {
                // Prevent first character deletion
                return currentPosition > this.editionZoneStartIndex;
            }

            return currentPosition >= this.editionZoneStartIndex;
        }

        private void MenuItemCopySelection_Click(object sender, RoutedEventArgs e)
        {
            this.richTxtConsole.Copy();
        }

        public void SendCommand(string command)
        {
            this.richTxtConsole.CaretPosition = this.richTxtConsole.Document.ContentEnd;

            this.UpdateLastCaretPosition();
            this.editionZoneStartIndex = this.GetCaretPositionAsPlainText();

            this.ChangeSelectionColor(this.contentBrush);

            this.richTxtConsole.Selection.Text = command;

            this.DispatchCommand();
        }
    }
}
