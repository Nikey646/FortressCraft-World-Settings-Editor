using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Simple.World.Settings.Editor.Classes
{
	internal class RegSettings
	{
		private Dictionary<string, object> _values;
		private string _projectName;
		private string _companyName;

		private string _cachedLocation = null;
		private RegistryKey _cachedHive = null;

		public RegSettings(string projectName)
		{
			this.Construct(projectName, "KuroThing");
		}

		public RegSettings(string projectName, string companyName)
		{
			this.Construct(projectName, companyName);
		}

		public object this[string index]
		{
			get { return this.GetValue(index); }
			set { this.SetValue(index, value); }
		}

		public T GetValueAs<T>(string index)
		{
			return (T)this[index];
		}

		public T TryGetValueAs<T>(string index)
		{
			if (this[index] is T)
				return (T)this[index];
			return default(T);
		}

		public void DeleteValue(string index)
		{
			this.TryDeleteValue(index);
		}

		public Boolean TryDeleteValue(string index)
		{
			if (this.ValueExists(index))
			{
				this._cachedHive.DeleteValue(index);
				this._values.Remove(index);
				return true;
			}
			return false;
		}

		public Boolean ValueExists(string index)
		{
			return this._values.ContainsKey(index);
		}

		public void Reload()
		{
			this._values.Clear();
			this.LoadAllValues();
		}

		private object GetValue(string index)
		{
			return this._values.ContainsKey(index) ? this._values[index] : null;
		}

		private void SetValue(string index, object value)
		{
			this._values[index] = value;
			this._cachedHive.SetValue(index, value);
		}

		private void Construct(string projectName, string companyName)
		{
			this._projectName = projectName;
			this._companyName = companyName;
			this._values = new Dictionary<String, Object>();
			this.LoadAllValues();
		}

		private void LoadAllValues()
		{
			if (this._cachedLocation == null)
				this._cachedLocation = $"Software\\{this._companyName}\\{this._projectName}";

			if (this._cachedHive == null)
			{
				var currentUserHive = Registry.CurrentUser;
				if ((this._cachedHive = currentUserHive.OpenSubKey(this._cachedLocation, RegistryKeyPermissionCheck.ReadWriteSubTree)) == null)
					this._cachedHive = currentUserHive.CreateSubKey(this._cachedLocation, RegistryKeyPermissionCheck.ReadWriteSubTree);
			}

			foreach (var keyName in this._cachedHive.GetValueNames())
			{
				var keyValue = this._cachedHive.GetValue(keyName);
				this._values.Add(keyName, keyValue);
			}
		}

	}
}
