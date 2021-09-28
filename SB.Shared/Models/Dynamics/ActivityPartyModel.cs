using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class ActivityPartyModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "activityparty";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Party = "partyid",
				PrimaryId = "activitypartyid",
				PrimaryName = "partyidname";

			public static string[] All => new[] { Party,
				PrimaryId,
				PrimaryName };
		}
		#endregion

		#region Enums
		
		#endregion

		#region Field Definitions
		public EntityReference Party
		{
			get => (EntityReference)this[Fields.Party];
			set => this[Fields.Party] = value; 
		}
		#endregion

		#region Constructors
		protected ActivityPartyModel()
			: base(LogicalName) { }
		protected ActivityPartyModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected ActivityPartyModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected ActivityPartyModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected ActivityPartyModel(Entity entity, IOrganizationService service)
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