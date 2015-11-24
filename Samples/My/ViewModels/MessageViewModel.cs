using MyTest.Models;
using MyTest.Services.MessageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using Template10.Services.NavigationService;

namespace MyTest.ViewModels {
	public class MessageViewModel : Mvvm.ViewModelBase {
		private MessageService _messageService;
		protected bool Standalone;

		private Message _message;
		public Message Message { get { return _message; } set { Set(ref _message, value); } }

		private DelegateCommand _deleteCommand;
		public ICommand DeleteCommand {
			get {
				return _deleteCommand ?? (_deleteCommand = new DelegateCommand(
					() => {
						if (Message != null) {
							_messageService.DeleteMessage(Message);
						}
						Message = null;
						if (Standalone) {
							NavigationService.GoBack();
						}
						else {
							_deleteCommand.RaiseCanExecuteChanged();
						}
					},
					() => Message != null
				));
			}
		}

		public MessageViewModel() {
			_messageService = new MessageService();
		}

		public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state) {
			string messageId = null;
			if (parameter != null) {
				messageId = parameter.ToString();
			}
			else {
				if (state.ContainsKey("messageId")) {
					messageId = state["messageId"].ToString();
				}
			}

			if (messageId != null) {
				Message = _messageService.GetMessage(messageId);
			}
		}

		public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending) {
			if (suspending) {
				if (Message != null) {
					state["messageId"] = Message.Id;
				}
			}
			return Task.FromResult<object>(null);
		}

		public override void OnNavigatingFrom(NavigatingEventArgs args) {
			args.Cancel = false;
		}
	}
}
