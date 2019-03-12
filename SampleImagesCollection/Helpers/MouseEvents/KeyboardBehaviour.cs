using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Documents;

namespace SampleMVVMWPF
{
    public class KeyboardBehaviour
    {
        #region DependencyProperty

        public static readonly DependencyProperty FocusCommandProperty =
           DependencyProperty.RegisterAttached("FocusCommand", typeof(ICommand), typeof(KeyboardBehaviour),
           new FrameworkPropertyMetadata(new PropertyChangedCallback(FocusCommandChanged)));

        public static readonly DependencyProperty CountUIElementsCommandProperty =
          DependencyProperty.RegisterAttached("CountUIElementsCommand", typeof(ICommand), typeof(KeyboardBehaviour),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(CountUIElementsCommandChanged)));

        #endregion // DependencyProperty

        #region Commands

        public static void FocusCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is FrameworkElement element)
            {
                element.GotFocus += element_GotFocus;
                element.LostFocus += element_GotFocus;
                Keyboard.Focus(element);
            }
        }

        public static void CountUIElementsCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is RichTextBox richTextBox)
            {
                richTextBox.TextChanged += richTextBox_TextChanged;
            }
        }       

        public static void SetFocusCommand(UIElement uiElement, ICommand value)
        {
            uiElement.SetValue(FocusCommandProperty, value);
        }

        public static void SetCountUIElementsCommand(UIElement uiElement, ICommand value)
        {
            uiElement.SetValue(CountUIElementsCommandProperty, value);
        }

        public static ICommand GetFocusCommand(UIElement element)
           => (ICommand)element.GetValue(FocusCommandProperty);

        public static ICommand GetCountUIElementsCommand(UIElement element)
           => (ICommand)element.GetValue(CountUIElementsCommandProperty);

        #endregion // Commands

        #region Events

        private static void element_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is RichTextBox richTextBox)
            {
                var position = richTextBox.CaretPosition;
                var command = GetFocusCommand(richTextBox);
                command.Execute(position);
            }
        }

        private static void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var countUIElements = 0;
            if (sender is RichTextBox richTextBox)
            {
                foreach (Block block in richTextBox.Document.Blocks)
                {
                    if (block is Paragraph paragraph)
                    {
                        foreach (Inline inline in paragraph.Inlines)
                        {
                            if (inline is InlineUIContainer uiContainer)
                            {
                                countUIElements++;
                            }
                        }
                    }
                }

                var command = GetCountUIElementsCommand(richTextBox);
                command.Execute(countUIElements);
            }
        }

        #endregion // Events
    }
}
