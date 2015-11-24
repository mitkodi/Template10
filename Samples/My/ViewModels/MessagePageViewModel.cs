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
	public class MessagePageViewModel : Mvvm.ViewModelBase {
		private MessageService _messageService;

		private ObservableCollection<Message> _messages;
		public ObservableCollection<Message> Messages { get { return _messages; } set { Set(ref _messages, value); } }

		public MessageViewModel SelectedMessageViewModel { get; } = new MessageViewModel();

		private DelegateCommand<string> _goToMessagePageCommand;
		public ICommand GoToMessagePageCommand {
			get {
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

		private string _searchValue;
		public string SearchValue {
			get { return _searchValue; }
			set {
				if (Set(ref _searchValue, value)) {
					_searchCommand.RaiseCanExecuteChanged();
				}

			}
		}

		private DelegateCommand _searchCommand;
		public ICommand SearchCommand {
			get {
				return _searchCommand ?? (_searchCommand = new DelegateCommand(
					() => {
						if (!String.IsNullOrEmpty(SearchValue)) {
							Messages = _messageService.Search(SearchValue);
							_clearSearchCommand.RaiseCanExecuteChanged();
						}
					},
					() => !String.IsNullOrEmpty(SearchValue)
				));
			}
		}

		private DelegateCommand _clearSearchCommand;
		public ICommand ClearSearchCommand {
			get {
				return _clearSearchCommand ?? (_clearSearchCommand = new DelegateCommand(
					() => {
						if (!String.IsNullOrEmpty(SearchValue)) {
							Messages = _messageService.GetMessages();
							SearchValue = null;
							_searchCommand.RaiseCanExecuteChanged();
						}
					},
					() => !String.IsNullOrEmpty(SearchValue)
				));
			}
		}

		public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state) {
			base.OnNavigatedTo(parameter, mode, state);
		}

		public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending) {
			if (SelectedMessageViewModel.Message != null) {
				state["messageId"] = SelectedMessageViewModel.Message.Id;
			}
			if (!String.IsNullOrEmpty(SearchValue)) {
				state["searchValue"] = SearchValue;
			}

			return Task.FromResult<object>(null);
		}

		public override void OnNavigatingFrom(NavigatingEventArgs args) {
			args.Cancel = false;
		}
	}
}
