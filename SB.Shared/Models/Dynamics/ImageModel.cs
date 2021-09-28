using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class ImageModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_image";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Apartment = "sb_apartmentid",
				Name = "sb_name",
				PrimaryId = "sb_imageid",
				Status = "statecode";

			public static string[] All => new[] { Apartment,
				Name,
				PrimaryId,
				Status };
		}
		#endregion

		#region Enums
		public static class StatusEnum
		{
			public const int
				Active = 0,
				Inactive = 1;
		}
		#endregion

		#region Field Definitions
		public EntityReference Apartment
		{
			get => (EntityReference)this[Fields.Apartment];
			set => this[Fields.Apartment] = value; 
		}
		public string Name
		{
			get => (string)this[Fields.Name];
			set => this[Fields.Name] = value; 
		}
		public new OptionSetValue Status
		{
			get => (OptionSetValue)this[Fields.Status];
			set => this[Fields.Status] = value; 
		}
		#endregion

		#region Constructors
		protected ImageModel()
			: base(LogicalName) { }
		protected ImageModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected ImageModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected ImageModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected ImageModel(Entity entity, IOrganizationService service)
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