using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class SBCustomSettingsModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_customsettings";

		#region Attribute Names
		public static class Fields
		{
			public const string
				AppId = "sb_appid",
				BookingEmailTemplateName = "sb_bookingemailtemplatename",
				CloseBookingFromDays = "sb_closebookingfromdays",
				DynamicsUrl = "sb_dynamicsurl",
				Mailforreceivingletters = "sb_mailforreceivingletters",
				PrimaryId = "sb_customsettingsid",
				PrimaryName = "sb_name",
				Userforsendingletters = "sb_userforsendingletters";

			public static string[] All => new[] { AppId,
				BookingEmailTemplateName,
				CloseBookingFromDays,
				DynamicsUrl,
				Mailforreceivingletters,
				PrimaryId,
				PrimaryName,
				Userforsendingletters };
		}
		#endregion

		#region Enums
		
		#endregion

		#region Field Definitions
		public string AppId
		{
			get => (string)this[Fields.AppId];
			set => this[Fields.AppId] = value; 
		}
		public string BookingEmailTemplateName
		{
			get => (string)this[Fields.BookingEmailTemplateName];
			set => this[Fields.BookingEmailTemplateName] = value; 
		}
		public int? CloseBookingFromDays
		{
			get => (int?)this[Fields.CloseBookingFromDays];
			set => this[Fields.CloseBookingFromDays] = value; 
		}
		public string DynamicsUrl
		{
			get => (string)this[Fields.DynamicsUrl];
			set => this[Fields.DynamicsUrl] = value; 
		}
		public string Mailforreceivingletters
		{
			get => (string)this[Fields.Mailforreceivingletters];
			set => this[Fields.Mailforreceivingletters] = value; 
		}
		public EntityReference Userforsendingletters
		{
			get => (EntityReference)this[Fields.Userforsendingletters];
			set => this[Fields.Userforsendingletters] = value; 
		}
		#endregion

		#region Constructors
		protected SBCustomSettingsModel()
			: base(LogicalName) { }
		protected SBCustomSettingsModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected SBCustomSettingsModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected SBCustomSettingsModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected SBCustomSettingsModel(Entity entity, IOrganizationService service)
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