using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCOM.AsyncTitleServiceConsumer.Contracts
{
	public enum AysncTitleProviderMessageType : int
	{
		NewOrder = 0,
		Document = 1,
		Note = 2,
		ScheduleSigning = 3
	}

	public class AysncTitleProviderMessage
	{

		public AysncTitleProviderMessageType MessageType { get; set; }
		public AysncTitleProviderMessageNewOrder MessageNewOrder { get; set; }
		public AysncTitleProviderMessageDocument MessageDocument { get; set; }
		public AysncTitleProviderMessageNote MessageNote { get; set; }
		public AysncTitleProviderMessageScheduleSigning MessageScheduleSigning { get; set; }

	}

	public class AysncTitleProviderMessageNewOrder
	{
		public int BorrowerID { get; set; }
		public string[] Endorsements { get; set; }
		public List<TitleServiceQuestion> AnsweredQuestions { get; set; }
		public string TitleOrderNote { get; set; }
		public string TitleRefID { get; set; }

	}

	public class AysncTitleProviderMessageDocument
	{

		public int BorrowerID { get; set; }
		public string Comment { get; set; }
		public int DocumentID { get; set; }

	}

	public class AysncTitleProviderMessageNote
	{
		public int BorrowerID { get; set; }
		public int UserID { get; set; }
		public string Subject { get; set; }
		public string Note { get; set; }
		public int OrderNoteID { get; set; }

	}

	public class AysncTitleProviderMessageScheduleSigning
	{
		public int BorrowerID { get; set; }
		public int CurrentUserID { get; set; }

	}
}
