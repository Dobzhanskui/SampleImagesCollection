using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;

namespace SampleMVVMWPF
{
    public class KeyboardBehaviour
    {
        #region DependencyProperty

        public static readonly DependencyProperty KeyboardFocusCommandProperty =
           DependencyProperty.RegisterAttached("KeyboardFocusCommand", typeof(ICommand), typeof(KeyboardBehaviour),
           new FrameworkPropertyMetadata(new PropertyChangedCallback(KeyboardFocusCommandChanged)));

        public static readonly DependencyProperty SelectedImageCommandProperty =
           DependencyProperty.RegisterAttached("SelectedImageCommand", typeof(ICommand), typeof(KeyboardBehaviour),
           new FrameworkPropertyMetadata(new PropertyChangedCallback(SelectedImageCommandChanged)));

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

        public static void SelectedImageCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is FrameworkElement element && element is RichTextBox richTextBox)
            {
                element.PreviewMouseUp += element_PreviewMouseUp;
                richTextBox.IsDocumentEnabled = true;
            }
        }

        public static void SetKeyboardFocusCommand(UIElement uiElement, ICommand value)
        {
            uiElement.SetValue(KeyboardFocusCommandProperty, value);
        }

        public static void SetSelectedImageCommand(UIElement uiElement, ICommand value)
        {
            uiElement.SetValue(SelectedImageCommandProperty, value);
        }

        public static ICommand GetKeyboardFocusCommand(UIElement element)
            => (ICommand)element.GetValue(KeyboardFocusCommandProperty);

        public static ICommand GetSelectedImageCommand(UIElement element)
         => (ICommand)element.GetValue(SelectedImageCommandProperty);

        #endregion // Commands

        #region Events

        public static void element_KeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is RichTextBox richTextBox)
            {
                var textPointer = richTextBox.CaretPosition;
                var command = GetKeyboardFocusCommand(richTextBox);
                command.Execute(textPointer);
            }
        }

        private static void element_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element is RichTextBox richTextBox)
            {
                foreach (Block block in richTextBox.Document.Blocks)
                {
                    if (block is Paragraph paragraph)
                    {
                        foreach (Inline inline in paragraph.Inlines)
                        {
                            if (inline is InlineUIContainer inlineUIContainer)
                            {
                                if (richTextBox.Selection.Contains(inlineUIContainer.ContentStart))
                                {
                                    var command = GetSelectedImageCommand(element);
                                    command.Execute(inline);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion // Events
    }
}
