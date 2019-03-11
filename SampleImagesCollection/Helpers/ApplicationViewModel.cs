using SampleMVVMWPF.Helpers.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using System;
using System.Windows.Documents;
using System.Windows.Controls;

namespace SampleMVVMWPF
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        #region Members

        private TextPointer m_textPointer;
        private int m_countUIElements;

        #region Commands

        private RelayCommand m_addOpenCommand;
        private RelayCommand m_focusCommand;
        private RelayCommand m_countUIElementsCommand;

        #endregion // Commands

        #endregion // Members

        #region Constructor

        public ApplicationViewModel()
        {
        }

        #endregion // Constructor

        #region Properties

        public RelayCommand AddCommand => m_addOpenCommand ?? (m_addOpenCommand = new RelayCommand(obj =>
        {
            var inlineUIContainer = default(InlineUIContainer);
            using (var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|Bitmaps|*.bmp|PNG files|*.png|JPEG files|*.jpg|GIF files|*.gif|TIFF files|*.tif|All files|*.*"
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var drawingGroup = CreateNewImageWithIndex(openFileDialog.FileName);
                    var drawingImage = new Image
                    {
                        Height = drawingGroup.Bounds.Height,
                        Width = drawingGroup.Bounds.Width,
                        Source = new DrawingImage(drawingGroup)
                    };
                    inlineUIContainer = new InlineUIContainer(drawingImage, m_textPointer);
                }
            }
        }));

        public RelayCommand FocusCommand
        {
            get => m_focusCommand ?? (m_focusCommand = new RelayCommand(obj =>
            {
                if (obj is TextPointer textPointer)
                {
                    m_textPointer = textPointer;
                }
            }));
        }

        public RelayCommand CountUIElements
        {
            get => m_countUIElementsCommand ?? (m_countUIElementsCommand = new RelayCommand(obj =>
            {
                if (obj is int count)
                {
                    m_countUIElements = count;
                }
            }));
        }

        #endregion // Properties

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropetyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion // INotifyPropertyChanged

        #region Heplpers

        private DrawingGroup CreateNewImageWithIndex(string imagePath)
        {
            var image = new System.Drawing.Bitmap(imagePath);
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                var imageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                drawingContext.DrawImage(imageSource, new Rect(0, 0, image.Width > 200 ? 200 : image.Width, image.Height > 200 ? 200 : image.Height));
                var formattedText = new FormattedText($"{m_countUIElements + 1}",
                           CultureInfo.InvariantCulture,
                           System.Windows.FlowDirection.LeftToRight,
                           new Typeface("Segoe UI"),
                           image.Width < 25 ? image.Width / 1.5 : 25,
                           Brushes.Red, 
                           VisualTreeHelper.GetDpi(visual).DpiScaleX);
                drawingContext.DrawText(formattedText, new Point(image.Width > 200 ? 170 : image.Width < 25 ? (image.Width / 1.2) + 5 : image.Width - 25, 0));
            }
            return visual.Drawing;
        }

        #endregion
    }
}
