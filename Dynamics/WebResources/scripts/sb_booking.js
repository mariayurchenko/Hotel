var page = (function (window, document) {
    /************************************************************************************
    * Variables
    ************************************************************************************/
    var formContext;

    var layout = {
        Fields: {
            Apartment: "sb_apartmentid",
            ApartmentType: "sb_apartmenttypeid",
            Client: "sb_contactid",
            PhoneNumber: "sb_phonenumber",
            ClientName: "sb_clientname",
            Price: "sb_price",
            DateStart: "sb_datestart",
            DateEnd: "sb_dateend",
            EmailAddress: "emailaddress"
        },
        Entities: {
            Apartment: "sb_apartment",
            Contact: {
                LogicalName: "contact",
                Fields: {
                    MobilePhone: "mobilephone",
                    FullName: "fullname",
                    Email: "emailaddress1"
                }
            }
        },
        ActionNames: {
            ActionTracking: "sb_ActionTracking",
            CalculatePrice: "CalculatePrice"
        }
    };

    /************************************************************************************
    * Page events
    ************************************************************************************/

    function onLoad(context) {
        try {
            formContext = context.getFormContext();
        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }   
    };

    function onSave(context) {
        try {
            formContext = context.getFormContext();
        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    };

    /************************************************************************************
     * Field events
     ************************************************************************************/

    function onApartmentChange(context) {
        try {
            formContext = context.getFormContext();

            var apartment = getAttribute(layout.Fields.Apartment).getValue();

            if (apartment !== null) {
                if (formContext.ui.controls.get(layout.Fields.Apartment).getDisabled()) {
                    formContext.ui.controls.get(layout.Fields.Apartment).setDisabled(false);
                } else {
                    formContext.ui.controls.get(layout.Fields.ApartmentType).setDisabled(true);
                    var id = apartment[0].id.replace(/[{}]/g, '').toLowerCase();
                    setApartmentTypeByApartmentId(id);
                }
            } else {
                formContext.ui.controls.get(layout.Fields.ApartmentType).setDisabled(false);
                getAttribute(layout.Fields.ApartmentType).setValue(null);
            }
        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    }

    function onApartmentTypeChange(context) {
        try {
            formContext = context.getFormContext();

            var apartmentType = getAttribute(layout.Fields.ApartmentType).getValue();

            if (apartmentType !== null) {
                if (formContext.ui.controls.get(layout.Fields.ApartmentType).getDisabled()) {
                    formContext.ui.controls.get(layout.Fields.ApartmentType).setDisabled(false);
                } else {
                    formContext.ui.controls.get(layout.Fields.Apartment).setDisabled(true);
                }
            } else {
                formContext.ui.controls.get(layout.Fields.Apartment).setDisabled(false);
                getAttribute(layout.Fields.Apartment).setValue(null);
            }
        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    }

    function onClientChange(context) {
        try {
            formContext = context.getFormContext();

            var client = getAttribute(layout.Fields.Client).getValue();

            if (client !== null) {
                if (formContext.ui.controls.get(layout.Fields.Client).getDisabled()) {
                    formContext.ui.controls.get(layout.Fields.Client).setDisabled(true);
                    return;
                }
                formContext.ui.controls.get(layout.Fields.ClientName).setDisabled(true);
                formContext.ui.controls.get(layout.Fields.PhoneNumber).setDisabled(true);
                formContext.ui.controls.get(layout.Fields.EmailAddress).setDisabled(true);
                var id = client[0].id.replace(/[{}]/g, '').toLowerCase();
                setPhoneNumberAndClientNameByContactId(id);
            } else {
                formContext.ui.controls.get(layout.Fields.ClientName).setDisabled(false);
                formContext.ui.controls.get(layout.Fields.PhoneNumber).setDisabled(false);
                formContext.ui.controls.get(layout.Fields.EmailAddress).setDisabled(false);
                getAttribute(layout.Fields.ClientName).setValue(null);
                getAttribute(layout.Fields.PhoneNumber).setValue(null);
                getAttribute(layout.Fields.EmailAddress).setValue(null);
            }
        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    }

    function onPhoneNumberChange(context) {
        try {
            formContext = context.getFormContext();
            setDisabledForClient();
        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    }

    function onClientNameChange(context) {
        try {
            formContext = context.getFormContext();
            setDisabledForClient();
        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    }

    function onEmailAddressChange(context) {
        try {
            formContext = context.getFormContext();
            setDisabledForClient();
        } catch (e) {
            console.log(e);
            showErrorMessage(e.message);
        }
    }

    /************************************************************************************
  * Ribbon events
  ************************************************************************************/

    function onCalculatePriceClick(context) {
        try {
            formContext = context;

            Xrm.Utility.showProgressIndicator("Loading...");

            var id = formContext.data.entity.getId().replace(/[{}]/g, '').toLowerCase();
            var request = {
                ActionName: layout.ActionNames.CalculatePrice,
                Parameters: id,

                getMetadata: function () {
                    return {
                        boundParameter: null,
                        parameterTypes: {
                            "ActionName": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            },
                            "Parameters": {
                                "typeName": "Edm.String",
                                "structuralProperty": 1
                            }
                        },
                        operationType: 0,
                        operationName: layout.ActionNames.ActionTracking
                    };
                }
            };

            Xrm.WebApi.online.execute(request)
                .then(function (response) {
                    return response.json();
                }
                )
                .then(function (responseBody) {
                    Xrm.Utility.closeProgressIndicator();
                    if (responseBody.Response == null) {
                        throw new Error("Response return null");
                    }
                    getAttribute(layout.Fields.Price).setValue(parseInt(responseBody.Response));
                })
                .catch(function (error) {
                    Xrm.Utility.closeProgressIndicator();
                    showErrorMessage(error.message);
                });
        } catch (e) {
            Xrm.Utility.closeProgressIndicator();
            showErrorMessage(error.message);
        }
    };

    function isCalculatePriceVisible(context) {
        formContext = context;
        var id = formContext.data.entity.getId();
        var dateStart = getAttribute(layout.Fields.DateStart).getValue();
        var dateEnd = getAttribute(layout.Fields.DateEnd).getValue();
        var type = getAttribute(layout.Fields.ApartmentType).getValue();
        if (id !== "" && dateStart !== null && dateEnd !== null && type !== null) {
            return true;
        }

        return false;
    };

    /************************************************************************************
    * Helpers
    ************************************************************************************/

    function setDisabledForClient() {
        var name = getAttribute(layout.Fields.ClientName).getValue();
        var phoneNumber = getAttribute(layout.Fields.PhoneNumber).getValue();
        var email = getAttribute(layout.Fields.EmailAddress).getValue();
        var client = getAttribute(layout.Fields.Client).getValue();
        if (name !== null || phoneNumber !== null || email !== null) {
            if (formContext.ui.controls.get(layout.Fields.PhoneNumber).getDisabled() ||
                formContext.ui.controls.get(layout.Fields.ClientName).getDisabled() ||
                formContext.ui.controls.get(layout.Fields.EmailAddress).getDisabled()) {
                formContext.ui.controls.get(layout.Fields.ClientName).setDisabled(false);
                formContext.ui.controls.get(layout.Fields.PhoneNumber).setDisabled(false);
                formContext.ui.controls.get(layout.Fields.EmailAddress).setDisabled(false);
                return;
            }

            formContext.ui.controls.get(layout.Fields.Client).setDisabled(true);
            
        } else {
            formContext.ui.controls.get(layout.Fields.Client).setDisabled(false);
        }
        if (client != null) {
            getAttribute(layout.Fields.Client).setValue(null);
        }
    }

    function setPhoneNumberAndClientNameByContactId(id) {
        Xrm.WebApi.online.retrieveRecord(layout.Entities.Contact.LogicalName, id, `?$select=${layout.Entities.Contact.Fields.FullName},${layout.Entities.Contact.Fields.MobilePhone},${layout.Entities.Contact.Fields.Email}`).then(
            function success(result) {
                getAttribute(layout.Fields.ClientName).setValue(result[layout.Entities.Contact.Fields.FullName]);
                getAttribute(layout.Fields.PhoneNumber).setValue(result[layout.Entities.Contact.Fields.MobilePhone]);
                getAttribute(layout.Fields.EmailAddress).setValue(result[layout.Entities.Contact.Fields.Email]);
            },
            function (error) {
                Xrm.Utility.alertDialog(error.message);
            }
        );
    }

    function setApartmentTypeByApartmentId(id) {
        Xrm.WebApi.online.retrieveRecord(layout.Entities.Apartment, id, `?$select=_sb_apartmenttypeid_value`).then(
            function success(result) {
                var id = result[`_sb_apartmenttypeid_value`];
                var name = result[`_sb_apartmenttypeid_value@OData.Community.Display.V1.FormattedValue`];
                var logicalName = result[`_sb_apartmenttypeid_value@Microsoft.Dynamics.CRM.lookuplogicalname`];
                var lookupValue = [];
                lookupValue[0] = {};
                lookupValue[0].id = id;
                lookupValue[0].name = name;
                lookupValue[0].entityType = logicalName;
                getAttribute(layout.Fields.ApartmentType).setValue(lookupValue);
            },
            function (error) {
                Xrm.Utility.alertDialog(error.message);
            }
        );
    }

    function getAttribute(attributeName) {
        /// <summary> Returns attribute. </summary>
        /// <param name="attributeName" type="string"> Attribute name. </param>
        /// <returns type="Object"> Return the attribute. </returns>

        var attribute = formContext.getAttribute(attributeName);
        if (attribute == null) {
            throw new Error("Data Field " + attributeName + " not found.");
        }

        return attribute;
    }

    function showErrorMessage(message) {
        var guid = Date.now().toString();

        formContext.ui.setFormNotification(message, "ERROR", guid);

        setTimeout(clearMessage, 5000, guid);
    }

    function clearMessage(guid) {
        try {
            formContext.ui.clearFormNotification(guid);
        } catch (e) {
            Xrm.Navigation.openAlertDialog({ confirmButtonLabel: "Ok", text: e.message });
        }
    }

    return {
        onApartmentChange: onApartmentChange,
        onApartmentTypeChange: onApartmentTypeChange,
        onPhoneNumberChange: onPhoneNumberChange,
        onClientChange: onClientChange,
        onClientNameChange: onClientNameChange,
        onEmailAddressChange: onEmailAddressChange,
        onLoad: onLoad,
        onSave: onSave,
        onCalculatePriceClick: onCalculatePriceClick,
        isCalculatePriceVisible: isCalculatePriceVisible
    };
})(window, document);