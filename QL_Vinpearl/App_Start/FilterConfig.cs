﻿using System.Web;
using System.Web.Mvc;

namespace QL_Vinpearl
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
