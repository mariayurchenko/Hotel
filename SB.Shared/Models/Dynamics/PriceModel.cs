using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class PriceModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_price";

		#region Attribute Names
		public static class Fields
		{
			public const string
				ApartmentType = "sb_apartmenttypeid",
				DateStart = "sb_datestart",
				Name = "sb_name",
				PriceValue = "sb_pricevalue",
				PrimaryId = "sb_priceid";

			public static string[] All => new[] { ApartmentType,
				DateStart,
				Name,
				PriceValue,
				PrimaryId };
		}
		#endregion

		#region Enums
		
		#endregion

		#region Field Definitions
		public EntityReference ApartmentType
		{
			get => (EntityReference)this[Fields.ApartmentType];
			set => this[Fields.ApartmentType] = value; 
		}
		public DateTime? DateStart
		{
			get => (DateTime?)this[Fields.DateStart];
			set => this[Fields.DateStart] = value; 
		}
		public string Name
		{
			get => (string)this[Fields.Name];
			set => this[Fields.Name] = value; 
		}
		public int? PriceValue
		{
			get => (int?)this[Fields.PriceValue];
			set => this[Fields.PriceValue] = value; 
		}
		#endregion

		#region Constructors
		protected PriceModel()
			: base(LogicalName) { }
		protected PriceModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected PriceModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected PriceModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected PriceModel(Entity entity, IOrganizationService service)
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