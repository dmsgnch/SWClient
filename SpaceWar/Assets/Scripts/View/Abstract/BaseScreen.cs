using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace View.Abstract
{
    public abstract class BaseScreen : MonoBehaviour, INotifyPropertyChanged, IDisposable
    {
        public abstract Type ModelType { get; }

        public abstract void Show();
        public abstract void Close();
        public abstract void Bind(object model);

		protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		[CanBeNull] public event PropertyChangedEventHandler PropertyChanged;

		public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private bool _disposed;

		public virtual void Dispose(bool Disposing)
		{
			if (!Disposing || _disposed) return;
			_disposed = true;
		}
	}
}