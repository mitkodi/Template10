using MyTest.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MyTest.Views {
	public sealed partial class MainPage : Page {
		public MainPage() {
			InitializeComponent();
			NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
		}

		// strongly-typed view models enable x:bind
		public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;
	}
}
