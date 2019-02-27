using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SampleMVVMWPF.Helpers
{
    public class ImageEdit : INotifyPropertyChanged
    {
        #region Members

        private DrawingImage m_bitImage;

        #endregion // Members

        #region Properties

        public DrawingImage BitImage
        {
            get => m_bitImage;
            set
            {
                if (m_bitImage != value)
                {
                    m_bitImage = value;
                    OnPropertyChanged("Title");
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
