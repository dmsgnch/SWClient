using System.ComponentModel;
using UnityEngine;

namespace ViewModels.Abstract
{
    public abstract class ViewModelBase : IViewModel
    {
        public virtual void Dispose()
        {
            
        }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}