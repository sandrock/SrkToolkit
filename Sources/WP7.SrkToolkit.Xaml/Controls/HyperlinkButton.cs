
namespace SrkToolkit.Xaml.Controls
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Represents a button control that displays a hyperlink.
    /// </summary>
    public class HyperlinkButton : System.Windows.Controls.HyperlinkButton
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
              typeof(HyperlinkButton),
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
              typeof(HyperlinkButton),
              new PropertyMetadata(null));

        #endregion

        protected override void OnClick()
        {
            base.OnClick();

            if (this.Command != null)
            {
                this.Command.Execute(this.CommandParameter);
            }
        }
    }
}
