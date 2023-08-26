using System.Collections;
using System.Security;

namespace Mono.Xml
{
	public class SecurityParser : MiniParser, MiniParser.IHandler, MiniParser.IReader
	{
		private SecurityElement _root;

		private string _xmldoc;

		private int _pos;

		private SecurityElement _current;

		private Stack stack;

		public SecurityParser()
		{
			stack = new Stack();
		}

		public void LoadXml(string xml)
		{
			_root = null;
			_xmldoc = xml;
			_pos = 0;
			stack.Clear();
			Parse(this, this);
		}

		public SecurityElement ToXml()
		{
			return _root;
		}

		public int Read()
		{
			if (_pos >= _xmldoc.Length)
			{
				return -1;
			}
			return _xmldoc[_pos++];
		}

		public void OnStartParsing(MiniParser parser)
		{
		}

		public void OnStartElement(string name, IAttrList attrs)
		{
			SecurityElement securityElement = new SecurityElement(name);
			if (_root == null)
			{
				_root = securityElement;
				_current = securityElement;
			}
			else
			{
				SecurityElement securityElement2 = (SecurityElement)stack.Peek();
				securityElement2.AddChild(securityElement);
			}
			stack.Push(securityElement);
			_current = securityElement;
			int length = attrs.Length;
			for (int i = 0; i < length; i++)
			{
				_current.AddAttribute(attrs.GetName(i), attrs.GetValue(i));
			}
		}

		public void OnEndElement(string name)
		{
			_current = (SecurityElement)stack.Pop();
		}

		public void OnChars(string ch)
		{
			_current.Text = ch;
		}

		public void OnEndParsing(MiniParser parser)
		{
		}
	}
}
