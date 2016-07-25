using System.ComponentModel;

namespace Generals_Manager
{
    public class CheckBoxListViewItem : INotifyPropertyChanged
    {
        private bool isBad;
        private bool isChecked;
        private string text;

        public CheckBoxListViewItem(Map map)
        {
            Text = map.Name;
            IsChecked = map.InGame;
            IsBad = map.Bad;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBad
        {
            get { return isBad; }
            set
            {
                if (isBad != value)
                {
                    isBad = value;
                    RaisePropertyChanged("IsBad");
                }
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    RaisePropertyChanged("IsChecked");
                }
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    RaisePropertyChanged("Text");
                }
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}