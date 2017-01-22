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

namespace ALDBluetoothATConfig.UserControls
{
    /// <summary>
    /// Interaction logic for MyConsole.xaml
    /// </summary>
    public partial class MyConsole : UserControl
    {
        private SolidColorBrush headerBrush = new SolidColorBrush(Color.FromRgb(100, 255, 200));
        private SolidColorBrush contentBrush = new SolidColorBrush(Color.FromRgb(0x80, 0xB9, 0xFF));


        TextPointer lastCaretPosition;
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
            this.ChangeSelectionColor(this.contentBrush);
        }

        private void MoveToEnd()
        {
            this.richTxtConsole.Selection.Select(this.richTxtConsole.Document.ContentEnd, this.richTxtConsole.Document.ContentEnd);
            this.richTxtConsole.CaretPosition = this.richTxtConsole.Document.ContentEnd;
        }

        private void UpdateLastCaretPosition()
        {
            this.lastCaretPosition = this.richTxtConsole.CaretPosition.DocumentEnd;
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
            if (this.richTxtConsole.CaretPosition.Paragraph == null)
                return;

            int offset = this.GetOffset();

            this.richTxtConsole.IsReadOnly = offset < -2;
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

            int offset = this.GetOffset();

            if (e.Key == Key.Back)
                e.Handled = offset <= -2;
        }
    }
}
