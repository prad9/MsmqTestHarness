﻿//  <auto-generated>
//      This code was generated by the Voodoo-ass ResourceLocator v2.3 T4 Template.
//		
//		LIC.Services.ITOperations.AnimalGroupMembership.WebApi 0.2.0.0 
//		   
//      Changes to this file will be lost if the code is regenerated.
//  </auto-generated>

using System;

namespace LIC.Services.ITOperations.AnimalGroupMembership.WebApi 
{
	namespace v0 
	{
		public static class ResourceLocator
		{
			public const string ServiceName = "ITOperations.AnimalGroupMembership";
			public static class PurchaseByEid 
			{
				public const string ResourceName = "ITOperations.AnimalGroupMembership.PurchaseByEid";
				public static string Put(Guid eventId)
				{
					var location = "v0/event/purchasebyeid/{eventId}".Replace("{eventId}", eventId.ToString());
					return location.ToLowerInvariant();
				}
			}
		}
	}
}