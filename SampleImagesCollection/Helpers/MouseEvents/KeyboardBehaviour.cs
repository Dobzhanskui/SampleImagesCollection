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

        public static void SetKeyboardFocusCommand(UIElement uiElement, ICommand value)
        {
            uiElement.SetValue(KeyboardFocusCommandProperty, value);
        }

        public static ICommand GetKeyboardFocusCommand(UIElement element)
            => (ICommand)element.GetValue(KeyboardFocusCommandProperty);

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

        #endregion // Event
    }
}
