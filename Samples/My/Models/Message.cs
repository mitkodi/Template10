using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace MyTest.Models {
	public class Message : BindableBase {
		private string _id = default(string);
		public string Id { get { return _id; } set { Set(ref _id, value); } }

		private string _from = default(string);
		public string From { get { return _from; } set { Set(ref _from, value); } }

		private string _to = default(string);
		public string To { get { return _to; } set { Set(ref _to, value); } }

		private string _subject = default(string);
		public string Subject { get { return _subject; } set { Set(ref _subject, value); } }

		private string _body = default(string);
		public string Body { get { return _body; } set { Set(ref _body, value); } }

		private DateTime _date = default(DateTime);
		public DateTime Date { get { return _date; } set { Set(ref _date, value); } }

		private Boolean _isRead = false;
		public Boolean IsRead { get { return _isRead; } set { Set(ref _isRead, value); } }
	}
}
