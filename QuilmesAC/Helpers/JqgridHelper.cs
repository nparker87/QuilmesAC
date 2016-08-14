namespace QuilmesAC.Helpers
{
	public class JqgridHelper
	{
		public class JqgridRow
		{
			public long ID;
			public string[] Cell;
		}

		// For use with jqGrid Advanced Searching. These classes map the filter options.
		public class FilterSettings
		{
			// AND or OR
			public string GroupOp { get; set; }

			public FilterRule[] Rules { get; set; }
		}

		public class FilterRule
		{
			// database field name
			public string Field { get; set; }

			// operation abbreviation enum
			public string Op { get; set; }

			// search string
			public string Data { get; set; }
		}
	}
}