using SampleMVVMWPF.Helpers.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using System;
using SampleMVVMWPF.Helpers;
using System.Text;

namespace SampleMVVMWPF
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        #region Members

        private ImageEdit m_selectedImage;
        private Rect m_rect;

        #region Commands

        private RelayCommand m_addOpenCommand;
        private RelayCommand m_cutCommand;
        private RelayCommand m_pasteCommand;
        private RelayCommand m_deleteCommand;
        private RelayCommand m_keyboardFocusCommand;

        #endregion // Commands

        #endregion // Members

        #region Constructor

        public ApplicationViewModel()
        {
            ImageEditItems = new ObservableCollection<ImageEdit>();
        }

        #endregion // Constructor

        #region Properties

        public ObservableCollection<ImageEdit> ImageEditItems { get; set; }

        public RelayCommand AddCommand => m_addOpenCommand ?? (m_addOpenCommand = new RelayCommand(obj =>
        {
            var imageEdit = default(ImageEdit);
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|Bitmaps|*.bmp|PNG files|*.png|JPEG files|*.jpg|GIF files|*.gif|TIFF files|*.tif|All files|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imageEdit = new ImageEdit { BitImage = new DrawingImage(CreateNewImage(openFileDialog.FileName)) };
            }

            if (imageEdit != null)
            {
                ImageEditItems.Add(imageEdit);
                SelectedImage = imageEdit;
            }
        }));

        public RelayCommand CutCommand
        {
            get => m_cutCommand ?? (m_cutCommand = new RelayCommand(obj =>
            {
                if (obj is ImageEdit imageEdit)
                {
                    ImageEditItems.Remove(imageEdit);
                }
            }));
        }

        public RelayCommand PasteCommand
        {
            get => m_pasteCommand ?? (m_pasteCommand = new RelayCommand(obj =>
            {
                if (obj is ImageEdit imageEdit)
                {
                    ImageEditItems.Insert(ImageEditItems.Count == 0 ? 0 : ImageEditItems.Count - 1, imageEdit);
                }
            }));
        }

        public RelayCommand DeleteCommand
        {
            get => m_deleteCommand ?? (m_deleteCommand = new RelayCommand(obj =>
            {
                if (obj is ImageEdit imageEdit)
                {
                    ImageEditItems.Remove(imageEdit);
                }
            }));
        }

        public ImageEdit SelectedImage
        {
            get => m_selectedImage;
            set
            {
                m_selectedImage = value;
                OnPropetyChanged("SelectedImage");
            }
        }

        public RelayCommand KeyboardFocusCommand
        {
            get => m_keyboardFocusCommand ?? (m_keyboardFocusCommand = new RelayCommand(obj =>
            {
                if (obj is Rect rect)
                {
                    if (m_rect != rect)
                    {
                        m_rect = rect;
                    }
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

        public DrawingGroup CreateNewImage(string imagePath)
        {
            var image = new Bitmap(imagePath);
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                var imageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                drawingContext.DrawImage(imageSource, new Rect(5, 5, image.Width > 300 ? 300 : image.Width, image.Height > 300 ? 300 : image.Height));
                var formattedText = new FormattedText($"{ImageEditItems.Count + 1}",
                           CultureInfo.InvariantCulture,
                           System.Windows.FlowDirection.LeftToRight,
                           new Typeface("Segoe UI"),
                           32,
                           System.Windows.Media.Brushes.Red, 
                           VisualTreeHelper.GetDpi(visual).PixelsPerDip);
                drawingContext.DrawText(formattedText, new System.Windows.Point(image.Width > 275 ? 275 : 25, 0));
            }
            return visual.Drawing;
        }

        #endregion
    }
}
