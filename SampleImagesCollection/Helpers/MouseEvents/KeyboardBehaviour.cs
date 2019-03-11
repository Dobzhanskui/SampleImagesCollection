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
                element.GotFocus += element_GotFocus;
                element.LostFocus += element_GotFocus;
                Keyboard.Focus(element);
            }
        }

        public static void SelectedImageCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is FrameworkElement element && element is RichTextBox richTextBox)
            {
                richTextBox.SelectionChanged += element_SelectionChanged;
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

        private static void element_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is RichTextBox richTextBox)
            {
                var position = richTextBox.CaretPosition;
                var command = GetKeyboardFocusCommand(richTextBox);
                command.Execute(position);
            }
        }

        private static void element_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (e.Source is RichTextBox richTextBox)
            {
                foreach (Block block in richTextBox.Document.Blocks)
                {
                    if (block is Paragraph paragraph)
                    {
                        foreach (Inline inline in paragraph.Inlines)
                        {
                            if (inline is InlineUIContainer uiContainer)
                            {
                                if (richTextBox.Selection.Contains(uiContainer.ContentStart))
                                {
                                    var command = GetSelectedImageCommand(richTextBox);
                                    command.Execute(uiContainer);
                                    break;
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
