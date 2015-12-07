using MyTest.Models;
using MyTest.Services.MessageServices;
using MyTest.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Template10.Mvvm;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Template10.Services.NavigationService;

namespace MyTest.ViewModels {
	public class MessagesPageViewModel : Mvvm.ViewModelBase {
		private MessageService _messageService;

		#region ctor

		public MessagesPageViewModel() {
			if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled) {
				_messageService = new MessageService();
			}
		}

		#endregion

		#region ObservableCollection<Message> Messages

		private ObservableCollection<Message> _messages;
		public ObservableCollection<Message> Messages { get { return _messages; } set { Set(ref _messages, value); } }

		#endregion

		#region MessageViewModel Selected

		public MessageViewModel Selected { get; } = new MessageViewModel();

		#endregion

		#region ICommand GoToMessagePageCommand

		private DelegateCommand<string> _goToMessagePageCommand;
		public ICommand GoToMessagePageCommand
		{
			get
			{
				return _goToMessagePageCommand ?? (_goToMessagePageCommand = new DelegateCommand<string>(
					(id) => {
						if (!String.IsNullOrEmpty(id)) {
							NavigationService.Navigate(typeof(MessagePage), id);
						}
					},
					(id) => !String.IsNullOrEmpty(id)
				));
			}
		}

		#endregion

		#region string SearchValue

		private string _searchValue;
		public string SearchValue 
		{
			get { return _searchValue; }
			private set
			{
				if (Set(ref _searchValue, value)) {
					_clearSearchCommand.RaiseCanExecuteChanged();
				}
			}
		}

		#endregion

		#region ICommand SearchCommand

		private DelegateCommand<string> _searchCommand;
		public ICommand SearchCommand
		{
			get
			{
				return _searchCommand ?? (_searchCommand = new DelegateCommand<string>(
					async (value) => {
						if (!String.IsNullOrEmpty(value)) {
							await LoadMessages(value);
						}
					},
					(value) => !String.IsNullOrEmpty(value)
				));
			}
		}

		#endregion

		#region ICommand ClearSearchCommand

		private DelegateCommand _clearSearchCommand;
		public ICommand ClearSearchCommand
		{
			get
			{
				return _clearSearchCommand ?? (_clearSearchCommand = new DelegateCommand(
					async () => {
						await LoadMessages();
					},
					() => !String.IsNullOrEmpty(SearchValue)
				));
			}
		}

		#endregion

		#region Navigation

		public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending) {
			// test
			if (state.ContainsKey("test")) {
				System.Diagnostics.Debug.WriteLine(state["test"]);
			}
			state["test"] = "test";


			if (Selected.Message != null) {
				state[nameof(Selected.Message.Id)] = Selected.Message.Id;
			}
			if (!String.IsNullOrEmpty(SearchValue)) {
				state[nameof(SearchValue)] = SearchValue;
			}


			await Task.Yield();
		}

		public override async void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state) {
			if (state.ContainsKey(nameof(SearchValue))) {
				await LoadMessages(state[nameof(SearchValue)].ToString());
			}
			else {
				await LoadMessages();
			}
			if (state.ContainsKey(nameof(Selected.Message.Id))) {
				Selected.Message = await _messageService.GetMessage(state[nameof(Selected.Message.Id)].ToString());
			}
		}

		public override void OnNavigatingFrom(NavigatingEventArgs args) {
			args.Cancel = false;
		}

		#endregion

		private async Task LoadMessages(string searchValue = null) {
			if (String.IsNullOrEmpty(searchValue)) {
				Messages = await _messageService.GetMessages();
				SearchValue = null;
			}
			else {
				Messages = await _messageService.Search(searchValue);
				SearchValue = searchValue;
			}
		}
	}
}
