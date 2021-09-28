using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class UserModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "systemuser";

		#region Attribute Names
		public static class Fields
		{
			public const string
				PrimaryId = "systemuserid",
				PrimaryName = "fullname";

			public static string[] All => new[] { PrimaryId,
				PrimaryName };
		}
		#endregion

		#region Enums
		
		#endregion

		#region Field Definitions
		
		#endregion

		#region Constructors
		protected UserModel()
			: base(LogicalName) { }
		protected UserModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected UserModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected UserModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected UserModel(Entity entity, IOrganizationService service)
			: base(entity, service) { }
		#endregion

		#region Public Methods
		public override string GetPrimaryAttribute()
        {
            return Fields.PrimaryId;
        }
		#endregion
	}
}