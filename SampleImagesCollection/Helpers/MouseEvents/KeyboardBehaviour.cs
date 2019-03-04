using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;

namespace SampleMVVMWPF
{
    public class KeyboardBehaviour
    {
        #region Members

        private static TextPointer m_textPointer;

        #endregion // Members

        #region DependencyProperty

        public static readonly DependencyProperty KeyboardFocusCommandProperty =
           DependencyProperty.RegisterAttached("KeyboardFocusCommand", typeof(ICommand), typeof(KeyboardBehaviour),
           new FrameworkPropertyMetadata(new PropertyChangedCallback(KeyboardFocusCommandChanged)));

        public static readonly DependencyProperty KeyDownCommandProperty =
           DependencyProperty.RegisterAttached("KeyDownCommand", typeof(ICommand), typeof(KeyboardBehaviour),
           new FrameworkPropertyMetadata(new PropertyChangedCallback(KeyDownCommandChanged)));

        #endregion // DependencyProperty

        #region Commands

        public static void KeyboardFocusCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is FrameworkElement element)
            {
                element.GotKeyboardFocus += element_KeyboardFocus;
                Keyboard.Focus(element);
            }
        }

        public static void KeyDownCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is FrameworkElement element)
            {
                element.PreviewKeyDown += element_KeyDown;
            }
        }

        public static void SetKeyboardFocusCommand(UIElement uiElement, ICommand value)
        {
            uiElement.SetValue(KeyboardFocusCommandProperty, value);
        }

        public static void SetKeyDownCommand(UIElement uiElement, ICommand value)
        {
            uiElement.SetValue(KeyDownCommandProperty, value);
        }

        public static ICommand GetKeyboardFocusCommand(UIElement element)
            => (ICommand)element.GetValue(KeyboardFocusCommandProperty);

        public static ICommand GetKeyDownCommand(UIElement element)
          => (ICommand)element.GetValue(KeyDownCommandProperty);

        #endregion // Commands

        #region Event

        public static void element_KeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is FrameworkElement element && element is RichTextBox richTextBox)
            {
                var textPointer = richTextBox.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
                if (m_textPointer != textPointer)
                {
                    m_textPointer = textPointer;
                    var command = GetKeyboardFocusCommand(element);
                    command.Execute(m_textPointer);
                }
            }
        }

        public static void element_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) //Also "Key.Delete" is available.
            {
                if (e.Source is FrameworkElement element && element.DataContext is ApplicationViewModel app)
                {
                    var command = GetKeyDownCommand(element);
                    command.Execute(app.SelectedImage);
                }
            }
        }

        #endregion // Event
    }
}
