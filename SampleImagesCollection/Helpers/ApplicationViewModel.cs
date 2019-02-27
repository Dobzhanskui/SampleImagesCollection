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

namespace SampleMVVMWPF.Helpers
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        #region Members

        private ImageEdit m_selectedImage;
        private RelayCommand m_addOpenCommand;
        private RelayCommand m_cutCommand;
        private RelayCommand m_pasteCommand;

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
                var image = new Bitmap(openFileDialog.FileName);
                var pixels = new byte[256 * 256 * 4];
                var bitmapSource = BitmapSource.Create(25, 25, 96, 96, PixelFormats.Pbgra32, null, pixels, 256 * 4);
                var visual = new DrawingVisual();
                using (var drawingContext = visual.RenderOpen())
                {
                    var imageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    drawingContext.DrawImage(imageSource, new Rect(0, 0, 300, 300));
                    var text = new FormattedText($"{ImageEditItems.Count + 1}",
                               CultureInfo.InvariantCulture,
                               System.Windows.FlowDirection.LeftToRight,
                               new Typeface("Segoe UI"),
                               32,
                               System.Windows.Media.Brushes.Red);
                    drawingContext.DrawText(text, new System.Windows.Point(300 - 25, 0));
                }
                imageEdit = new ImageEdit { BitImage = new DrawingImage(visual.Drawing) };
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
                    ImageEditItems.Insert(ImageEditItems.Count - 1, imageEdit);
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

        #endregion // Properties

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropetyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion // INotifyPropertyChanged

        #region Heplpers

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        #endregion

    }
}
