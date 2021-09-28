using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace SB.Shared.Models.Dynamics
{
	// Do not modify the content of this file.
	// This is an automatically generated file and all 
	// logic should be added in the associated controller class
	// If a controller does not exist, create one that inherits the model.

	public class BookingModel : EntityBase
	{
		// Public static Logical Name
		public const string
			LogicalName = "sb_booking";

		#region Attribute Names
		public static class Fields
		{
			public const string
				Adults = "sb_adults",
				Apartment = "sb_apartmentid",
				ApartmentType = "sb_apartmenttypeid",
				Childrens = "sb_childrens",
				Client = "sb_contactid",
				ClientName = "sb_clientname",
				DateEnd = "sb_dateend",
				DateStart = "sb_datestart",
				Description = "sb_description",
				EmailAddress = "emailaddress",
				FinalPrice = "sb_finalprice",
				ID = "sb_id",
				IsSecurityDeposit = "sb_issecuritydeposit",
				PhoneNumber = "sb_phonenumber",
				Price = "sb_price",
				PrimaryId = "sb_bookingid",
				SecurityDeposit = "sb_securitydeposit",
				Status = "statecode",
				Type = "sb_type";

			public static string[] All => new[] { Adults,
				Apartment,
				ApartmentType,
				Childrens,
				Client,
				ClientName,
				DateEnd,
				DateStart,
				Description,
				EmailAddress,
				FinalPrice,
				ID,
				IsSecurityDeposit,
				PhoneNumber,
				Price,
				PrimaryId,
				SecurityDeposit,
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

		public static class TypeEnum
		{
			public const int
				Новый = 108550000,
				Подтвержденный = 108550001,
				Отмененный = 108550002,
				Оплаченный = 108550003;
		}
		#endregion

		#region Field Definitions
		public int? Adults
		{
			get => (int?)this[Fields.Adults];
			set => this[Fields.Adults] = value; 
		}
		public EntityReference Apartment
		{
			get => (EntityReference)this[Fields.Apartment];
			set => this[Fields.Apartment] = value; 
		}
		public EntityReference ApartmentType
		{
			get => (EntityReference)this[Fields.ApartmentType];
			set => this[Fields.ApartmentType] = value; 
		}
		public int? Childrens
		{
			get => (int?)this[Fields.Childrens];
			set => this[Fields.Childrens] = value; 
		}
		public EntityReference Client
		{
			get => (EntityReference)this[Fields.Client];
			set => this[Fields.Client] = value; 
		}
		public string ClientName
		{
			get => (string)this[Fields.ClientName];
			set => this[Fields.ClientName] = value; 
		}
		public DateTime? DateEnd
		{
			get => (DateTime?)this[Fields.DateEnd];
			set => this[Fields.DateEnd] = value; 
		}
		public DateTime? DateStart
		{
			get => (DateTime?)this[Fields.DateStart];
			set => this[Fields.DateStart] = value; 
		}
		public string Description
		{
			get => (string)this[Fields.Description];
			set => this[Fields.Description] = value; 
		}
		public string EmailAddress
		{
			get => (string)this[Fields.EmailAddress];
			set => this[Fields.EmailAddress] = value; 
		}
		public int? FinalPrice
		{
			get => (int?)this[Fields.FinalPrice];
			set => this[Fields.FinalPrice] = value; 
		}
		public string ID
		{
			get => (string)this[Fields.ID];
			set => this[Fields.ID] = value; 
		}
		public bool? IsSecurityDeposit
		{
			get => (bool?)this[Fields.IsSecurityDeposit];
			set => this[Fields.IsSecurityDeposit] = value; 
		}
		public string PhoneNumber
		{
			get => (string)this[Fields.PhoneNumber];
			set => this[Fields.PhoneNumber] = value; 
		}
		public int? Price
		{
			get => (int?)this[Fields.Price];
			set => this[Fields.Price] = value; 
		}
		public int? SecurityDeposit
		{
			get => (int?)this[Fields.SecurityDeposit];
			set => this[Fields.SecurityDeposit] = value; 
		}
		public new OptionSetValue Status
		{
			get => (OptionSetValue)this[Fields.Status];
			set => this[Fields.Status] = value; 
		}
		public OptionSetValue Type
		{
			get => (OptionSetValue)this[Fields.Type];
			set => this[Fields.Type] = value; 
		}
		#endregion

		#region Constructors
		protected BookingModel()
			: base(LogicalName) { }
		protected BookingModel(IOrganizationService service)
			: base(LogicalName, service) { }
		protected BookingModel(Guid id, ColumnSet columnSet, IOrganizationService service)
			: base(service.Retrieve(LogicalName, id, columnSet), service) { }
		protected BookingModel(Guid id, IOrganizationService service)
			: base(LogicalName, id, service) { }
		protected BookingModel(Entity entity, IOrganizationService service)
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