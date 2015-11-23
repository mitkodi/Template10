using MyTest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;

namespace MyTest.Services.MessageServices {
	public partial class MessageService {
		private ObservableCollection<Message> _messages;

		public ObservableCollection<Message> GetMessages() {
			if (_messages == null) {
				_messages = new ObservableCollection<Message>(GetSampleData());
			}

			return _messages;
		}

		public ObservableCollection<Message> Search(string value) => GetMessages()
			.Where(m => m.From.ToLower().Contains(value?.ToLower() ?? String.Empty))
			.Where(m => m.Subject.ToLower().Contains(value?.ToLower() ?? String.Empty))
			.Where(m => m.Body.ToLower().Contains(value?.ToLower() ?? String.Empty))
			.ToObservableCollection();

		public Message GetMessage(string id) => GetMessages().FirstOrDefault(m => m.Id.Equals(id));

		public void DeleteMessage(Message message) {
			GetMessages().Remove(message);
		}
	}
}
