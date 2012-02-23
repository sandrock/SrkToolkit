using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;
using SrkToolkit.Xaml.Commands;

namespace SrkToolkit.Xaml.Behaviors {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// http://10rem.net/blog/2010/01/09/a-wpf-behavior-for-window-resize-events-in-net-35
    /// http://digitalmoosetracks.com/blog/wpf-transparent-window-with-resizing
    /// http://www.codeproject.com/KB/WPF/WPF_Window_Resizing.aspx
    /// </remarks>
    public class WindowControls : Behavior<Window> {

        #region Constants

        private const Int32 WM_EXITSIZEMOVE = 0x0232;
        private const Int32 WM_SIZING = 0x0214;
        private const Int32 WM_SIZE = 0x0005;
        private const Int32 SIZE_RESTORED = 0x0000;
        private const Int32 SIZE_MINIMIZED = 0x0001;
        private const Int32 SIZE_MAXIMIZED = 0x0002;
        private const Int32 SIZE_MAXSHOW = 0x0003;
        private const Int32 SIZE_MAXHIDE = 0x0004;
        private const Int32 WM_SYSCOMMAND = 0x112;

        //private Dictionary<ResizeDirection, Cursor> cursors = new Dictionary<ResizeDirection, Cursor>
        //{
        //    { ResizeDirection.Top, Cursors.SizeNS },
        //    { ResizeDirection.Bottom, Cursors.SizeNS },
        //    { ResizeDirection.Left, Cursors.SizeWE },
        //    { ResizeDirection.Right, Cursors.SizeWE },
        //    { ResizeDirection.TopLeft, Cursors.SizeNWSE },
        //    { ResizeDirection.BottomRight, Cursors.SizeNWSE },
        //    { ResizeDirection.TopRight, Cursors.SizeNESW },
        //    { ResizeDirection.BottomLeft, Cursors.SizeNESW },
        //};

        //private enum ResizeDirection {
        //    Left = 1,
        //    Right = 2,
        //    Top = 3,
        //    TopLeft = 4,
        //    TopRight = 5,
        //    Bottom = 6,
        //    BottomLeft = 7,
        //    BottomRight = 8,
        //}

        #endregion

        #region Bad stuff

        private HwndSourceHook _hook;
        private HwndSource source;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out PointAPI lpPoint);

        private struct PointAPI {
            public int X;
            public int Y;
        }

        #endregion

        #region Resize

        private bool resizeRight = false;
        private bool resizeLeft = false;
        private bool resizeUp = false;
        private bool resizeDown = false;

        private Dictionary<UIElement, short> leftElements = new Dictionary<UIElement, short>();
        private Dictionary<UIElement, short> rightElements = new Dictionary<UIElement, short>();
        private Dictionary<UIElement, short> upElements = new Dictionary<UIElement, short>();
        private Dictionary<UIElement, short> downElements = new Dictionary<UIElement, short>();

        private PointAPI resizePoint = new PointAPI();
        private Size resizeSize = new Size();
        private Point resizeWindowPoint = new Point();

        private delegate void RefreshDelegate();

        private Dispatcher dispatcher;

        #endregion

        #region Dependency property: LeftResizeElements

        public string LeftResizeElements {
            get { return (string)GetValue(LeftResizeElementsProperty); }
            set { SetValue(LeftResizeElementsProperty, value); }
        }

        public static readonly DependencyProperty LeftResizeElementsProperty =
            DependencyProperty.Register(
                "LeftResizeElements",
                typeof(string),
                typeof(WindowControls),
                new UIPropertyMetadata(null, OnLeftResizeElementsPropertyChanged));

        private static void OnLeftResizeElementsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var me = (WindowControls)d;
            var names = e.NewValue as string;

            OnResizeElementsPropertyChanged(me, names, me.leftElements);
        }

        #endregion

        #region Dependency property: RightResizeElements

        public string RightResizeElements {
            get { return (string)GetValue(RightResizeElementsProperty); }
            set { SetValue(RightResizeElementsProperty, value); }
        }

        public static readonly DependencyProperty RightResizeElementsProperty =
            DependencyProperty.Register(
                "RightResizeElements",
                typeof(string),
                typeof(WindowControls),
                new UIPropertyMetadata(null, OnRightResizeElementsPropertyChanged));

        private static void OnRightResizeElementsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var me = (WindowControls)d;
            var names = e.NewValue as string;

            OnResizeElementsPropertyChanged(me, names, me.rightElements);
        }

        #endregion

        #region Dependency property: TopResizeElements

        public string TopResizeElements {
            get { return (string)GetValue(TopResizeElementsProperty); }
            set { SetValue(TopResizeElementsProperty, value); }
        }

        public static readonly DependencyProperty TopResizeElementsProperty =
            DependencyProperty.Register(
                "TopResizeElements",
                typeof(string),
                typeof(WindowControls),
                new UIPropertyMetadata(null, OnTopResizeElementsPropertyChanged));

        private static void OnTopResizeElementsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var me = (WindowControls)d;
            var names = e.NewValue as string;

            OnResizeElementsPropertyChanged(me, names, me.upElements);
        }

        #endregion

        #region Dependency property: BottomResizeElements

        public string BottomResizeElements {
            get { return (string)GetValue(BottomResizeElementsProperty); }
            set { SetValue(BottomResizeElementsProperty, value); }
        }

        public static readonly DependencyProperty BottomResizeElementsProperty =
            DependencyProperty.Register(
                "BottomResizeElements",
                typeof(string),
                typeof(WindowControls),
                new UIPropertyMetadata(null, OnBottomResizeElementsPropertyChanged));

        private static void OnBottomResizeElementsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var me = (WindowControls)d;
            var names = e.NewValue as string;

            OnResizeElementsPropertyChanged(me, names, me.downElements);
        }

        private static void OnResizeElementsPropertyChanged(WindowControls me, string names, Dictionary<UIElement, short> list) {
            if (!string.IsNullOrEmpty(names) && me.AssociatedObject != null) {
                foreach (var name in names.Split(' ', ',', ';')) {
                    var elem = me.AssociatedObject.FindName(name) as UIElement;
                    if (elem != null) {
                        if (!me.IsResizerRegistered(elem))
                            me.ConnectMouseHandlers(elem);
                        if (!list.ContainsKey(elem))
                            list.Add(elem, 0);
                    }
                }
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Maximize/windowize command.
        /// To be bound in the view.
        /// </summary>
        public ICommand MaximizeCommand {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this.maximizeCommand ?? (this.maximizeCommand = new RelayCommand(OnMaximize)); }
        }
        private ICommand maximizeCommand;

        private void OnMaximize() {
            this.AssociatedObject.WindowState = (this.AssociatedObject.WindowState == WindowState.Normal)
                ? WindowState.Maximized : WindowState.Normal;
        }

        /// <summary>
        /// Minimize command.
        /// To be bound in the view.
        /// </summary>
        public ICommand MinimizeCommand {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this.minimizeCommand ?? (this.minimizeCommand = new RelayCommand(OnMinimize)); }
        }
        private ICommand minimizeCommand;

        private void OnMinimize() {
            this.AssociatedObject.WindowState = (this.AssociatedObject.WindowState == WindowState.Minimized)
                ? WindowState.Normal : WindowState.Minimized;
        }

        /// <summary>
        /// Close command.
        /// To be bound in the view.
        /// </summary>
        public ICommand CloseCommand {
            [System.Diagnostics.DebuggerStepThrough]
            get { return this.closeCommand ?? (this.closeCommand = new RelayCommand(OnClose)); }
        }
        private ICommand closeCommand;

        private void OnClose() {
            this.AssociatedObject.Close();
        }

        #endregion

        #region Events

        public event EventHandler Resized;
        public event EventHandler Resizing;
        public event EventHandler Maximized;
        public event EventHandler Minimized;
        public event EventHandler Restored;

        #endregion

        #region Init and cleanup

        protected override void OnAttached() {
            base.OnAttached();

            this.dispatcher = this.AssociatedObject.Dispatcher;

            this.AssociatedObject.Loaded += (s, e) => this.WireUpWndProc();
            this.AssociatedObject.MouseLeftButtonDown += this.OnInputDown;
            this.AssociatedObject.TouchDown += this.OnInputDown;
        }

        void OnInputDown(object sender, RoutedEventArgs e) {
            if (sender is Window && !this.AssociatedObject.AreAnyTouchesOver) {
                this.AssociatedObject.DragMove();
                e.Handled = true;
            }
        }

        protected override void OnDetaching() {
            this.RemoveWndProc();

            foreach (var item in new Dictionary<UIElement, short>[] { this.leftElements, this.rightElements, this.upElements, this.downElements }) {
                foreach (var i in item) {
                    if (i.Key == null)
                        continue;

                    i.Key.MouseLeftButtonDown += this.OnResizeElementMouseLeftButtonDown;
                    i.Key.MouseEnter += this.OnResizeElementMouseEnter;
                    i.Key.MouseLeave += this.OnResizeElementMouseLeave;
                }

                item.Clear();
            }

            base.OnDetaching();
        }

        private void WireUpWndProc() {
            this.source = HwndSource.FromVisual(this.AssociatedObject) as HwndSource;

            if (this.source != null) {
                this._hook = new HwndSourceHook(this.WndProc);
                this.source.AddHook(this._hook);
            }

            OnResizeElementsPropertyChanged(this, this.LeftResizeElements, this.leftElements);
            OnResizeElementsPropertyChanged(this, this.RightResizeElements, this.rightElements);
            OnResizeElementsPropertyChanged(this, this.TopResizeElements, this.upElements);
            OnResizeElementsPropertyChanged(this, this.BottomResizeElements, this.downElements);
        }

        private void RemoveWndProc() {
            if (this.source != null) {
                this.source.RemoveHook(this._hook);
            }
        }

        #endregion

        #region Bad stuff (again)

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private IntPtr WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled) {
            IntPtr result = IntPtr.Zero;

            switch (msg) {
                case WM_SIZING:
                    OnResizing();
                    break;

                case WM_SIZE: 
                    int param = wParam.ToInt32();
                    switch (param) {
                        case SIZE_RESTORED:
                            OnRestored();
                            break;
                        case SIZE_MINIMIZED:
                            OnMinimized();
                            break;
                        case SIZE_MAXIMIZED:
                            OnMaximized();
                            break;
                        case SIZE_MAXSHOW:
                            break;
                        case SIZE_MAXHIDE:
                            break;
                    }
                    break;
                case WM_EXITSIZEMOVE:
                    OnResized();
                    break;
            }

            return result;
        }

        #endregion

        #region Event raisers

        private void OnResizing() {
            var handler = this.Resizing;
            if (handler != null)
                handler(this.AssociatedObject, EventArgs.Empty);
        }

        private void OnResized() {
            var handler = this.Resized;
            if (handler != null)
                handler(this.AssociatedObject, EventArgs.Empty);
        }

        private void OnRestored() {
            var handler = this.Restored;
            if (handler != null)
                handler(this.AssociatedObject, EventArgs.Empty);
        }

        private void OnMinimized() {
            var handler = this.Minimized;
            if (handler != null)
                handler(this.AssociatedObject, EventArgs.Empty);
        }

        private void OnMaximized() {
            var handler = this.Maximized;
            if (handler != null)
                handler(this.AssociatedObject, EventArgs.Empty);
        }

        #endregion

        #region Window resize

        public bool IsResizerRegistered(UIElement element) {
            return this.leftElements.ContainsKey(element) || this.rightElements.ContainsKey(element) ||
                   this.upElements.ContainsKey(element)   || this.downElements.ContainsKey(element);
        }

        public void AddResizerRight(UIElement element) {
            this.ConnectMouseHandlers(element);
            this.rightElements.Add(element, 0);
        }

        public void AddResizerLeft(UIElement element) {
            this.ConnectMouseHandlers(element);
            this.leftElements.Add(element, 0);
        }

        public void AddResizerUp(UIElement element) {
            this.ConnectMouseHandlers(element);
            this.upElements.Add(element, 0);
        }

        public void AddResizerDown(UIElement element) {
            this.ConnectMouseHandlers(element);
            this.downElements.Add(element, 0);
        }

        public void AddResizerRightDown(UIElement element) {
            this.ConnectMouseHandlers(element);
            this.rightElements.Add(element, 0);
            this.downElements.Add(element, 0);
        }

        public void AddResizerLeftDown(UIElement element) {
            this.ConnectMouseHandlers(element);
            this.leftElements.Add(element, 0);
            this.downElements.Add(element, 0);
        }

        public void AddResizerRightUp(UIElement element) {
            this.ConnectMouseHandlers(element);
            this.rightElements.Add(element, 0);
            this.upElements.Add(element, 0);
        }

        public void AddResizerLeftUp(UIElement element) {
            this.ConnectMouseHandlers(element);
            this.leftElements.Add(element, 0);
            this.upElements.Add(element, 0);
        }

        private void ConnectMouseHandlers(UIElement element) {
            element.MouseLeftButtonDown += this.OnResizeElementMouseLeftButtonDown;
            element.MouseEnter += this.OnResizeElementMouseEnter;
            element.MouseLeave += this.OnResizeElementMouseLeave;
        }

        private void OnResizeElementMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            e.Handled = true;
            GetCursorPos(out this.resizePoint);
            this.resizeSize = new Size(this.AssociatedObject.Width, this.AssociatedObject.Height);
            this.resizeWindowPoint = new Point(this.AssociatedObject.Left, this.AssociatedObject.Top);

            UIElement sourceSender = (UIElement)sender;
            this.resizeLeft = this.leftElements.ContainsKey(sourceSender);
            this.resizeRight = this.rightElements.ContainsKey(sourceSender);
            this.resizeUp = this.upElements.ContainsKey(sourceSender);
            this.resizeDown = this.downElements.ContainsKey(sourceSender);

            var t = new Thread(new ThreadStart(this.UpdateSizeLoop));
            t.Name = "Mouse Position Poll Thread";
            t.Start();
        }

        private void UpdateSizeLoop() {
            try {
                while (resizeDown || resizeLeft || resizeRight || resizeUp) {
                    this.dispatcher.Invoke(DispatcherPriority.Render, new RefreshDelegate(UpdateSize));
                    this.dispatcher.Invoke(DispatcherPriority.Render, new RefreshDelegate(UpdateMouseDown));
                    //Thread.Sleep(2);
                }

                this.dispatcher.Invoke(DispatcherPriority.Render, new RefreshDelegate(SetArrowCursor));
            } catch {
            }
        }

        private void UpdateSize() {
            PointAPI p = new PointAPI();
            GetCursorPos(out p);

            if (resizeRight) {
                this.AssociatedObject.Width = Math.Abs(this.resizeSize.Width - (resizePoint.X - p.X)) + 8;
            }

            if (resizeDown) {
                this.AssociatedObject.Height = Math.Abs(resizeSize.Height - (resizePoint.Y - p.Y)) + 8;
            }

            if (resizeLeft) {
                this.AssociatedObject.Width = Math.Abs(resizeSize.Width + (resizePoint.X - p.X)) + 8;
                this.AssociatedObject.Left = resizeWindowPoint.X - (resizePoint.X - p.X);
            }

            if (resizeUp) {
                this.AssociatedObject.Height = Math.Abs(resizeSize.Height + (resizePoint.Y - p.Y)) + 8;
                this.AssociatedObject.Top = resizeWindowPoint.Y - (resizePoint.Y - p.Y);
            }
        }

        private void UpdateMouseDown() {
            if (Mouse.LeftButton == MouseButtonState.Released) {
                resizeRight = false;
                resizeLeft = false;
                resizeUp = false;
                resizeDown = false;
            }
        }

        private void OnResizeElementMouseEnter(object sender, MouseEventArgs e) {
            bool resizeRight = false;
            bool resizeLeft = false;
            bool resizeUp = false;
            bool resizeDown = false;

            UIElement sourceSender = (UIElement)sender;
            resizeLeft = this.leftElements.ContainsKey(sourceSender);
            resizeRight = this.rightElements.ContainsKey(sourceSender);
            resizeUp = this.upElements.ContainsKey(sourceSender);
            resizeDown = this.downElements.ContainsKey(sourceSender);

            if ((resizeLeft && resizeDown) || (resizeRight && resizeUp)) {
                this.SetNESWCursor(sender, e);
            } else if ((resizeRight && resizeDown) || (resizeLeft && resizeUp)) {
                this.SetNWSECursor(sender, e);
            } else if (resizeLeft || resizeRight) {
                this.SetWECursor(sender, e);
            } else if (resizeUp || resizeDown) {
                this.SetNSCursor(sender, e);
            }
        }

        private void SetWECursor(object sender, MouseEventArgs e) {
            this.AssociatedObject.Cursor = Cursors.SizeWE;
        }

        private void SetNSCursor(object sender, MouseEventArgs e) {
            this.AssociatedObject.Cursor = Cursors.SizeNS;
        }

        private void SetNESWCursor(object sender, MouseEventArgs e) {
            this.AssociatedObject.Cursor = Cursors.SizeNESW;
        }

        private void SetNWSECursor(object sender, MouseEventArgs e) {
            this.AssociatedObject.Cursor = Cursors.SizeNWSE;
        }

        private void OnResizeElementMouseLeave(object sender, MouseEventArgs e) {
            if (!resizeDown && !resizeLeft && !resizeRight && !resizeUp) {
                this.AssociatedObject.Cursor = Cursors.Arrow;
            }
        }

        private void SetArrowCursor() {
            this.AssociatedObject.Cursor = Cursors.Arrow;
        }

        #endregion
    }
}
