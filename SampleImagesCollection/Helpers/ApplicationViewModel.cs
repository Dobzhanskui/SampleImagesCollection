﻿using SampleMVVMWPF.Helpers.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using System;
using SampleMVVMWPF.Helpers;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Input;

namespace SampleMVVMWPF
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        #region Members

        private InlineUIContainer m_selectedImage;
        private TextPointer m_textPointer;

        #region Commands

        private RelayCommand m_addOpenCommand;
        private RelayCommand m_cutCommand;
        private RelayCommand m_pasteCommand;
        private RelayCommand m_deleteCommand;
        private RelayCommand m_keyboardFocusCommand;
        private RelayCommand m_selectedTextEditImage;

        #endregion // Commands

        #endregion // Members

        #region Constructor

        public ApplicationViewModel()
        {
            ImageEditItems = new ObservableCollection<InlineUIContainer>();
        }

        #endregion // Constructor

        #region Properties

        public ObservableCollection<InlineUIContainer> ImageEditItems { get; set; }

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

            if (inlineUIContainer != null)
            {
                ImageEditItems.Add(inlineUIContainer);
            }
        }));

        public RelayCommand CutCommand
        {
            get => m_cutCommand ?? (m_cutCommand = new RelayCommand(obj =>
            {
                if (obj is InlineUIContainer inlineUIContainer)
                {
                    m_textPointer.Paragraph?.Inlines?.Remove(inlineUIContainer);
                    ImageEditItems.Remove(inlineUIContainer);
                }
            },
            (obj) => obj != null && ImageEditItems.Count > 0));
        }

        public RelayCommand PasteCommand
        {
            get => m_pasteCommand ?? (m_pasteCommand = new RelayCommand(obj =>
            {
                if (obj is InlineUIContainer inlineUIContainer)
                {
                    if (m_textPointer.Paragraph?.Inlines?.Contains(inlineUIContainer) == true)
                        return;

                    if (m_textPointer.Paragraph?.Inlines?.Count == 0)
                    {
                        m_textPointer.Paragraph?.Inlines.Add(inlineUIContainer);
                    }
                    else
                    {
                        try
                        {
                            m_textPointer.Paragraph?.Inlines?.InsertAfter(m_textPointer.Paragraph?.Inlines?.LastInline, inlineUIContainer);
                        }
                        catch (Exception)
                        {
                        }

                    }

                    ImageEditItems.Insert(ImageEditItems.Count == 0 ? 0 : ImageEditItems.Count - 1, inlineUIContainer);
                    SelectedImage = null;
                }
            },
            (obj) => obj != null));
        }

        public RelayCommand DeleteCommand
        {
            get => m_deleteCommand ?? (m_deleteCommand = new RelayCommand(obj =>
            {
                if (obj is InlineUIContainer inlineUIContainer)
                {
                    ImageEditItems.Remove(inlineUIContainer);
                    m_textPointer.Paragraph?.Inlines?.Remove(inlineUIContainer);
                    SelectedImage = null;
                }
            },
            (obj) => obj != null && ImageEditItems.Count > 0));
        }

        public RelayCommand SelectedTextEditImage
        {
            get => m_selectedTextEditImage ?? (m_selectedTextEditImage = new RelayCommand(obj =>
            {
                if (obj is InlineUIContainer inlineUIContainer)
                {
                    SelectedImage = inlineUIContainer;
                }
            },
            (obj) => obj != null && ImageEditItems.Count > 0));
        }

        public InlineUIContainer SelectedImage
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
                if (obj is TextPointer textPointer)
                {
                    m_textPointer = textPointer;
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

        public DrawingGroup CreateNewImageWithIndex(string imagePath)
        {
            var image = new System.Drawing.Bitmap(imagePath);
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                var imageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                drawingContext.DrawImage(imageSource, new Rect(0, 0, image.Width > 200 ? 200 : image.Width, image.Height > 200 ? 200 : image.Height));
                var formattedText = new FormattedText($"{ImageEditItems.Count + 1}",
                           CultureInfo.InvariantCulture,
                           System.Windows.FlowDirection.LeftToRight,
                           new Typeface("Segoe UI"),
                           image.Width < 25 ? image.Width / 1.5 : 25,
                           Brushes.Red, 
                           VisualTreeHelper.GetDpi(visual).PixelsPerDip);
                drawingContext.DrawText(formattedText, new Point(image.Width > 200 ? 170 : image.Width < 25 ? (image.Width / 1.2) + 5 : image.Width - 25, 0));
            }
            return visual.Drawing;
        }

        #endregion
    }
}
