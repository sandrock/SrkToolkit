
namespace SrkToolkit.Xaml.Controls
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    public class Button : System.Windows.Controls.Button
    {
        #region Dependency property: Command

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
              "Command",
              typeof(ICommand),
              typeof(Button),
              new PropertyMetadata(null));

        #endregion

        #region Dependency property: CommandParameter

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
              "CommandParameter",
              typeof(object),
              typeof(Button),
              new PropertyMetadata(null));

        #endregion

        ////public Button()
        ////    : base()
        ////{
        ////}

        protected override void OnClick()
        {
            base.OnClick();

            this.ExecuteCommand();
        }

        private void ExecuteCommand()
        {
            if (this.Command != null)
            {
                this.Command.Execute(this.CommandParameter);
            }
        }
    }
}
