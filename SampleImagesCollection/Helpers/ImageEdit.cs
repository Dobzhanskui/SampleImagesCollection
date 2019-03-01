using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace SampleMVVMWPF.Helpers
{
    public class ImageEdit : INotifyPropertyChanged
    {
        #region Members

        private Image m_image;

        #endregion // Members

        #region Properties

        public Image Image
        {
            get => m_image;
            set
            {
                if (m_image != value)
                {
                    m_image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        #endregion // Properties

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion // INotifyPropertyChanged
    }
}
