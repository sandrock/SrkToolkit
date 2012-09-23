
namespace SrkToolkit.Xaml.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    /// <summary>
    /// Permits to focus the next input control when pressing enter.
    /// A command can be associated instead of a next control for execution.
    /// </summary>
    public class InputEnterBehavior : Behavior<Control>, IDisposable
    {
        /// <summary>
        /// Indicates the object was disposed.
        /// </summary>
        private bool disposed;

        #region Dependency property: Command

        /// <summary>
        /// Gets or sets the command to execute when Enter key is pressed.
        /// This is a dependency property.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// The dependency property for the <see cref="Command"/> property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(InputEnterBehavior),
                new PropertyMetadata(null));

        #endregion

        #region Dependency property: CommandParameter

        /// <summary>
        /// Gets or sets the command parameter.
        /// This is a dependency property.
        /// </summary>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// The dependency property for the <see cref="CommandParameter"/> property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(InputEnterBehavior),
                new PropertyMetadata(null));

        #endregion

        #region Dependency property: NextControl

        /// <summary>
        /// Gets or sets the next control to focus when Enter key is pressed.
        /// This is a dependency property.
        /// </summary>
        public Control NextControl
        {
            get { return (Control)GetValue(NextControlProperty); }
            set { SetValue(NextControlProperty, value); }
        }

        /// <summary>
        /// The dependency property for the <see cref="NextControl"/> property.
        /// </summary>
        public static readonly DependencyProperty NextControlProperty =
            DependencyProperty.Register(
              "NextControl",
              typeof(Control),
              typeof(InputEnterBehavior),
              new PropertyMetadata(null));

        #endregion

        #region Dependency property: AcceptsEmpty

        /// <summary>
        /// Gets or sets a value indicating whether to accept an empty input value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to accept an empty input value; otherwise, <c>false</c>.
        /// </value>
        public bool AcceptsEmpty
        {
            get { return (bool)GetValue(AcceptsEmptyProperty); }
            set { SetValue(AcceptsEmptyProperty, value); }
        }

        /// <summary>
        /// The dependency property for the <see cref="AcceptsEmpty"/> property.
        /// </summary>
        public static readonly DependencyProperty AcceptsEmptyProperty =
            DependencyProperty.Register(
              "AcceptsEmpty",
              typeof(bool),
              typeof(InputEnterBehavior),
              new PropertyMetadata(false));

        #endregion

        #region Dependency property: FocusOnLoaded

        /// <summary>
        /// Gets or sets a value indicating whether to focus on this element when loaded.
        /// This is a dependency property.
        /// </summary>
        public bool FocusOnLoaded
        {
            get { return (bool)GetValue(FocusOnLoadedProperty); }
            set { SetValue(FocusOnLoadedProperty, value); }
        }

        /// <summary>
        /// The dependency property for the <see cref="FocusOnLoaded"/> property.
        /// </summary>
        public static readonly DependencyProperty FocusOnLoadedProperty =
            DependencyProperty.Register(
                "FocusOnLoaded",
                typeof(bool),
                typeof(InputEnterBehavior),
                new PropertyMetadata(false, OnFocusOnLoadedPropertyChanged));

        #endregion

        /// <summary>
        /// Occurs when [execute event].
        /// </summary>
        public event EventHandler ExecuteEvent;

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases managed and - optionally - unmanaged resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Cleanup();
                }

                this.disposed = true;
            }
        }

        #endregion

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            if (this.AssociatedObject is TextBox)
            {
                var textBox = (TextBox)this.AssociatedObject;

                textBox.KeyUp += this.OnKeyUp;
            }
            else if (this.AssociatedObject is PasswordBox)
            {
                var passBox = (PasswordBox)this.AssociatedObject;

                passBox.KeyUp += this.OnKeyUp;
            }

            if (this.AssociatedObject is Control)
            {
                var ctrl = (Control)this.AssociatedObject;

                ctrl.Loaded += this.OnControlLoaded;
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            this.Cleanup();

            base.OnDetaching();
        }

        /// <summary>
        /// Called when some dependency property values changes.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnFocusOnLoadedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (InputEnterBehavior)d;
            bool newValue = (bool)e.NewValue;

            if (newValue)
                me.OnControlLoaded(me, new RoutedEventArgs());
        }

        private void Cleanup()
        {
            if (this.AssociatedObject is TextBox)
            {
                var textBox = (TextBox)this.AssociatedObject;
                textBox.KeyUp -= this.OnKeyUp;
            }
            else if (this.AssociatedObject is PasswordBox)
            {
                var passBox = (PasswordBox)this.AssociatedObject;
                passBox.KeyUp -= this.OnKeyUp;
            }

            if (this.AssociatedObject is Control)
            {
                var ctrl = (Control)this.AssociatedObject;

                ctrl.Loaded -= this.OnControlLoaded;
            }

            this.ExecuteEvent = null;
        }

        /// <summary>
        /// Called when [key up].
        /// </summary>
        /// <param name="e">The event args.</param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            var isEnterKey = e.Key == Key.Enter || e.PlatformKeyCode == 10;
            bool isEmpty = false;

            var inputTb = this.AssociatedObject as TextBox;
            if (!this.AcceptsEmpty && inputTb != null)
            {
                isEmpty = string.IsNullOrWhiteSpace(inputTb.Text);
            }
            var inputPb = this.AssociatedObject as PasswordBox;
            if (!this.AcceptsEmpty && inputPb != null)
            {
                isEmpty = string.IsNullOrWhiteSpace(inputPb.Password);
            }

            // force databinding
            BindingExpression binding = null;
            if (inputTb != null)
                binding = inputTb.GetBindingExpression(TextBox.TextProperty);
            else if (inputPb != null)
                binding = inputPb.GetBindingExpression(PasswordBox.PasswordProperty);
            if (binding != null)
                binding.UpdateSource();

            // if any enter key is pressed
            if (isEnterKey)
            {
                // don't do anything if the input must be filled
                if (isEmpty)
                    return;
                
                if (this.NextControl != null)
                {
                    // focus next control the best way we can
                    this.NextControl.Focus();

                    var tb = this.NextControl as TextBox;
                    if (tb != null)
                        tb.SelectAll();

                    var pb = this.NextControl as PasswordBox;
                    if (pb != null)
                        pb.SelectAll();
                }
                else if (this.Command != null)
                {
                    // execute the command
                    this.Command.Execute(this.CommandParameter);
                }
                else
                {
                    // raise the event
                    var handler = this.ExecuteEvent;
                    if (handler != null)
                        handler(sender, e);
                }
            }
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            if (this.FocusOnLoaded && this.AssociatedObject is Control)
            {
                this.Dispatcher.BeginInvoke(() => ((Control)this.AssociatedObject).Focus());
            }
        }
    }
}
