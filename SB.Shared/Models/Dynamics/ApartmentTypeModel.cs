using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class ApartmentTypeModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_apartmenttype";

		#region Attribute Names
		public static class Fields
		{
			public const string
				CurrentPrice = "sb_currentprice",
				Description = "sb_description",
				MainImage = "sb_imageid",
				Name = "sb_name",
				PrimaryId = "sb_apartmenttypeid",
				Status = "statecode";

			public static string[] All => new[] { CurrentPrice,
				Description,
				MainImage,
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
		public int? CurrentPrice
		{
			get => (int?)this[Fields.CurrentPrice];
			set => this[Fields.CurrentPrice] = value; 
		}
		public string Description
		{
			get => (string)this[Fields.Description];
			set => this[Fields.Description] = value; 
		}
		public EntityReference MainImage
		{
			get => (EntityReference)this[Fields.MainImage];
			set => this[Fields.MainImage] = value; 
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
		protected ApartmentTypeModel()
			: base(LogicalName) { }
		protected ApartmentTypeModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected ApartmentTypeModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected ApartmentTypeModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected ApartmentTypeModel(Entity entity, IOrganizationService service)
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