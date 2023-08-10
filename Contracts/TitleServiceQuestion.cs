using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCOM.AsyncTitleServiceConsumer.Contracts
{
	[System.Diagnostics.DebuggerDisplay("{QuestionID} ({QuestionType}): {Description}")]
	public class TitleServiceQuestion
	{
		public TitleServiceQuestion()
		{
		}
		public TitleServiceQuestion(TitleServiceQuestion copy)
		{
			QuestionID = copy.QuestionID;
			QuestionType = copy.QuestionType;
			Description = copy.Description;
			DefaultValue = copy.DefaultValue;
			AnswerList = copy.AnswerList == null ? null : new List<string>(copy.AnswerList);
			AnswerListIDs = copy.AnswerListIDs == null ? null : new List<string>(copy.AnswerListIDs);
			Value = copy.Value;
			Required = copy.Required;
			Suppress = copy.Suppress;

			Attributes = copy.Attributes == null ? null : copy.Attributes.Select(x => new TitleServiceQuestionAttribute { Name = x.Name, Type = x.Type, Value = x.Value }).ToList();
		}

		public string QuestionID { get; set; }
		public string QuestionType { get; set; }
		public string Description { get; set; }
		public string DefaultValue { get; set; }
		public List<string> AnswerList { get; set; }
		public List<string> AnswerListIDs { get; set; }
		public string Value { get; set; }
		public bool Required { get; set; }
		public bool? Suppress { get; set; }

		public List<TitleServiceQuestionAttribute> Attributes { get; set; }
	}
	[System.Diagnostics.DebuggerDisplay("{Name} ({Type}): {Value}")]
	public class TitleServiceQuestionAttribute
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
	}
}
