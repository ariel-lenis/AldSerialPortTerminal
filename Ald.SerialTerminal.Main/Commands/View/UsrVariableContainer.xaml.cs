using Ald.SerialTerminal.Device.Configuration;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ald.SerialTerminal.Main.Commands.View
{
    /// <summary>
    /// Interaction logic for UsrVariableContainer.xaml
    /// </summary>
    public partial class UsrVariableContainer : UserControl, ICommandVariable
    {
        DeviceCommandVariable variable;
        DOnCommandVariableChanged listener;
        Control control;

        public string VariableValue
        {
            get
            {
                if (this.control is ComboBox)
                {
                    var comboBox = (this.control as ComboBox);
                    if (comboBox.SelectedValue == null)
                    {
                        return null;
                    }

                    return ((KeyValuePair<string, string>)comboBox.SelectedValue).Key;
                }

                return (this.control as TextBox).Text;
            }
        }

        public DeviceCommandVariable Variable
        {
            get
            {
                return this.variable;
            }
        }

        public UsrVariableContainer()
        {
            InitializeComponent();
        }

        public void SetVariableInformation(DeviceCommandVariable variable)
        {
            this.variable = variable;

            this.txtName.Text = PrepareVariable(variable.Variable) + ":";

            if (variable.Map.Count > 0)
            {
                this.PrepareMap();
            }
            else
            {
                this.PrepareText();
            }
        }

        private string PrepareVariable(string variable)
        {
            return variable.ToUpper()[0] + variable.ToLower().Substring(1);
        }

        private void PrepareText()
        {
            TextBox textContent = new TextBox();

            textContent.ToolTip = this.variable.Regex;
            textContent.HorizontalAlignment = HorizontalAlignment.Stretch;
            textContent.VerticalAlignment = VerticalAlignment.Stretch;
            textContent.Width = double.NaN;
            textContent.Height = double.NaN;
            textContent.VerticalContentAlignment = VerticalAlignment.Center;

            this.control = textContent;

            textContent.TextChanged += TextContent_TextChanged;

            this.gridContainer.Children.Add(textContent);
        }

        private void TextContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool validation = this.Validate();
            
            this.listener?.Invoke(this, validation, (this.control as TextBox).Text);

            this.ShowValidation(validation);
        }

        private void ShowValidation(bool validation)
        {
            var textBox = this.control as TextBox;

            if (!validation)
            {
                textBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 200, 200));
            }
            else
            {
                textBox.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void PrepareMap()
        {
            ComboBox comboMap = new ComboBox();

            comboMap.ItemsSource = this.variable.Map;
            //comboMap.DisplayMemberPath = "Value";
            comboMap.SelectedIndex = 0;

            comboMap.HorizontalAlignment = HorizontalAlignment.Stretch;
            comboMap.VerticalAlignment = VerticalAlignment.Stretch;
            comboMap.VerticalContentAlignment = VerticalAlignment.Center;

            comboMap.Width = double.NaN;
            comboMap.Height = double.NaN;

            this.control = comboMap;

            this.gridContainer.Children.Add(comboMap);
        }

        public bool Validate()
        {
            string value = this.VariableValue;

            return this.variable.Validate(value);
        }

        public void SetOnCommandVariableChangedEvent(DOnCommandVariableChanged listener)
        {
            this.listener = listener;
        }
    }
}
