using System;
using Windows.UI.Xaml;

namespace MyTest.Services.SettingsServices {
	public interface ISettingsService {
		bool UseShellBackButton { get; set; }
		ApplicationTheme AppTheme { get; set; }
		TimeSpan CacheMaxDuration { get; set; }
	}
}
