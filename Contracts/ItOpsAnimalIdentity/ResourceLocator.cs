﻿//  <auto-generated>
//      This code was generated by the Voodoo-ass ResourceLocator v2.3 T4 Template.
//		
//		LIC.Services.ITOperations.AnimalIdentity.WebApi 0.1.2.0 
//		   
//      Changes to this file will be lost if the code is regenerated.
//  </auto-generated>

using System;

namespace LIC.Services.ITOperations.AnimalIdentity.WebApi 
{
	namespace v0 
	{
		public static class ResourceLocator
		{
			public const string ServiceName = "ITOperations.AnimalIdentity";
			public static class AssignEid 
			{
				public const string ResourceName = "ITOperations.AnimalIdentity.AssignEid";
				public static string Put(Guid eventId)
				{
					var location = "v0/event/assigneid/{eventId}".Replace("{eventId}", eventId.ToString());
					return location.ToLowerInvariant();
				}
			}
		}
	}
}