#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ClassGenerator.ttinclude code generation file.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;

namespace WebPortal	
{
	public partial class BookTable
	{
		private int _bookcode;
		public virtual int bookcode
		{
			get
			{
				return this._bookcode;
			}
			set
			{
				this._bookcode = value;
			}
		}
		
		private string _doccode;
		public virtual string doccode
		{
			get
			{
				return this._doccode;
			}
			set
			{
				this._doccode = value;
			}
		}
		
		private int? _espcode;
		public virtual int? espcode
		{
			get
			{
				return this._espcode;
			}
			set
			{
				this._espcode = value;
			}
		}
		
		private int? _loccode;
		public virtual int? loccode
		{
			get
			{
				return this._loccode;
			}
			set
			{
				this._loccode = value;
			}
		}
		
		private string _bookname;
		public virtual string bookname
		{
			get
			{
				return this._bookname;
			}
			set
			{
				this._bookname = value;
			}
		}
		
		private string _docname;
		public virtual string docname
		{
			get
			{
				return this._docname;
			}
			set
			{
				this._docname = value;
			}
		}
		
		private string _espname;
		public virtual string espname
		{
			get
			{
				return this._espname;
			}
			set
			{
				this._espname = value;
			}
		}
		
		private string _locname;
		public virtual string locname
		{
			get
			{
				return this._locname;
			}
			set
			{
				this._locname = value;
			}
		}
		
		private int? _inter;
		public virtual int? inter
		{
			get
			{
				return this._inter;
			}
			set
			{
				this._inter = value;
			}
		}
		
		private DateTime? _bookstarttime;
		public virtual DateTime? bookstarttime
		{
			get
			{
				return this._bookstarttime;
			}
			set
			{
				this._bookstarttime = value;
			}
		}
		
		private DateTime? _bookendtime;
		public virtual DateTime? bookendtime
		{
			get
			{
				return this._bookendtime;
			}
			set
			{
				this._bookendtime = value;
			}
		}
		
	}
}
#pragma warning restore 1591
