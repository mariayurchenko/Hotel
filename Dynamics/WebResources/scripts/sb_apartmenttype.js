var page = (function (window, document) {
    /************************************************************************************
    * Variables
    ************************************************************************************/
    var formContext;

    var layout = {
        Fields: {
            CurrentPrice: "sb_currentprice"
        },
        ActionNames: {
            ActionTracking: "sb_ActionTracking",
            GetCurrentPrice: "GetCurrentPrice"
        }
    };

    /************************************************************************************
    * Page events
    ************************************************************************************/

    function onLoad(context) {
        try {
            formContext = context.getFormContext();
            var id = formContext.data.entity.getId();
            if (id !== "" && id !== null) {
                formContext.ui.controls.get(`header_${layout.Fields.CurrentPrice}`).setDisabled(true);
                getAttribute(layout.Fields.CurrentPrice).setRequiredLevel("none");
            } else {
                getAttribute(layout.Fields.CurrentPrice).setRequiredLevel("required");
            }
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

    /************************************************************************************
   * Ribbon events
   ************************************************************************************/

    function onGetCurrentPriceClick(context) {
        try {
            formContext = context;

            Xrm.Utility.showProgressIndicator("Loading...");

            var id = formContext.data.entity.getId().replace(/[{}]/g, '').toLowerCase();
            var request = {
                ActionName: layout.ActionNames.GetCurrentPrice,
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
                    getAttribute(layout.Fields.CurrentPrice).setValue(parseInt(responseBody.Response));
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

    function isGetCurrentPriceVisible(context) {
        formContext = context;
        var id = formContext.data.entity.getId();
        if (id !== "") {
            return true;
        }

        return false;
    };

    /************************************************************************************
    * Helpers
    ************************************************************************************/

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
        onLoad: onLoad,
        onSave: onSave,
        onGetCurrentPriceClick: onGetCurrentPriceClick,
        isGetCurrentPriceVisible: isGetCurrentPriceVisible
    };
})(window, document);