using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SampleMVVMWPF.Helpers
{
    public class ImageEditViewModel : INotifyPropertyChanged
    {
        #region Members

        private string m_title;
        private ImageEdit m_imageEdit;

        #endregion // Members

        #region Properties

        public string Title
        {
            get => m_title;
            set
            {
                if (m_title != value)
                {
                    m_title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        #endregion // Properties

        #region Constructor

        public ImageEditViewModel(ImageEdit image)
        {
            m_imageEdit = image;
        }

        #endregion //Constructor

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion // INotifyPropertyChanged
    }
}
