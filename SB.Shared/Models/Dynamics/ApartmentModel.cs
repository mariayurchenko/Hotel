using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class ApartmentModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_apartment";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Address = "sb_address",
				CurrentBooking = "sb_currentbooking",
				LastBooking = "sb_lastbooking",
				Name = "sb_name",
				NextBooking = "sb_nextbooking",
				PrimaryId = "sb_apartmentid",
				Status = "statecode",
				Type = "sb_apartmenttypeid";

			public static string[] All => new[] { Address,
				CurrentBooking,
				LastBooking,
				Name,
				NextBooking,
				PrimaryId,
				Status,
				Type };
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
		public string Address
		{
			get => (string)this[Fields.Address];
			set => this[Fields.Address] = value; 
		}
		public EntityReference CurrentBooking
		{
			get => (EntityReference)this[Fields.CurrentBooking];
			set => this[Fields.CurrentBooking] = value; 
		}
		public EntityReference LastBooking
		{
			get => (EntityReference)this[Fields.LastBooking];
			set => this[Fields.LastBooking] = value; 
		}
		public string Name
		{
			get => (string)this[Fields.Name];
			set => this[Fields.Name] = value; 
		}
		public EntityReference NextBooking
		{
			get => (EntityReference)this[Fields.NextBooking];
			set => this[Fields.NextBooking] = value; 
		}
		public new OptionSetValue Status
		{
			get => (OptionSetValue)this[Fields.Status];
			set => this[Fields.Status] = value; 
		}
		public EntityReference Type
		{
			get => (EntityReference)this[Fields.Type];
			set => this[Fields.Type] = value; 
		}
		#endregion

		#region Constructors
		protected ApartmentModel()
			: base(LogicalName) { }
		protected ApartmentModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected ApartmentModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected ApartmentModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected ApartmentModel(Entity entity, IOrganizationService service)
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