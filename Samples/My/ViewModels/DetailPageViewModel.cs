using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace MyTest.ViewModels {
	public class DetailPageViewModel : MyTest.Mvvm.ViewModelBase {
		public DetailPageViewModel() {
			if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
				Value = "Designtime value";
		}

		private string _Value = "Default";
		public string Value { get { return _Value; } set { Set(ref _Value, value); } }

		public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state) {
			if (state.ContainsKey(nameof(Value))) {
				Value = state[nameof(Value)]?.ToString();
				state.Clear();
			}
			else {
				Value = parameter?.ToString();
			}
		}

		public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending) {
			if (suspending)
				state[nameof(Value)] = Value;
			await Task.Yield();
		}

		public override void OnNavigatingFrom(NavigatingEventArgs args) {
			args.Cancel = false;
		}
	}
}

