using ALDBluetoothATConfig.Commands.Implementation;
using System;
using System.Collections.Generic;
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

namespace ALDBluetoothATConfig.Commands.View
{
    /// <summary>
    /// Interaction logic for UsrCommandAction.xaml
    /// </summary>
    public partial class UsrCommandAction : UserControl, IDeviceCommand
    {
        DeviceCommand commandInformation;
        DOnCommandAction onCommandAction;

        List<ICommandVariable> commandVariables;

        public UsrCommandAction()
        {
            InitializeComponent();
        }

        public void SetCommandInformation(DeviceCommand commandInformation)
        {
            this.commandInformation = commandInformation;

            this.btnAction.Content = commandInformation.ShortLabel;
            this.btnAction.ToolTip = commandInformation.Description;

            this.LoadVariables();
        }

        private void LoadVariables()
        {
            this.commandVariables = new List<ICommandVariable>();

            foreach (var variable in this.commandInformation.Variables)
            {
                this.AddNewVariable(variable);
            }

            this.RefreshValidation();
        }

        private void RefreshValidation()
        {
            this.btnAction.IsEnabled = this.ValidateVariables();
        }

        private void AddNewVariable(DeviceCommandVariable variable)
        {
            UsrVariableContainer container = new UsrVariableContainer();

            container.SetOnCommandVariableChangedEvent(new DOnCommandVariableChanged(this.OnCommandVariableChanged));

            container.HorizontalAlignment = HorizontalAlignment.Stretch;
            container.Width = double.NaN;

            container.SetVariableInformation(variable);

            this.stackControls.Children.Add(container);
            this.commandVariables.Add(container);
        }

        private void OnCommandVariableChanged(object sender, bool isValid, string newValue)
        {
            this.RefreshValidation();
        }

        private bool ValidateVariables()
        {
            bool result = true;

            foreach (var commandVariable in this.commandVariables)
            {
                if (!commandVariable.Validate())
                {
                    result = false;
                }
            }

            return result;
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            string command = this.GetCommand();

            this.onCommandAction?.Invoke(this, command);
        }

        private string GetCommand()
        {
            var values = new Dictionary<string, string>();

            foreach (var variable in this.commandVariables)
            {
                values.Add(variable.Variable.Variable, variable.VariableValue);
            }

            return this.commandInformation.CreateCommand(values);
        }

        public void SetOnCommandActionEvent(DOnCommandAction onCommandAction)
        {
            this.onCommandAction = onCommandAction;
        }
    }
}
