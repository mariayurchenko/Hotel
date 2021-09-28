var page = (function (window, document) {
    /************************************************************************************
    * Variables
    ************************************************************************************/
    var formContext;

    var layout = {
        Fields: {},
        ActionNames: {
            ActionTracking: "sb_ActionTracking",
            UpdateBookingHistory: "UpdateBookingHistory"
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

    /************************************************************************************
  * Ribbon events
  ************************************************************************************/

    function onRefreshBookingHistoryClick(context) {
        try {
            formContext = context;

            Xrm.Utility.showProgressIndicator("Loading...");

            var id = formContext.data.entity.getId().replace(/[{}]/g, '').toLowerCase();
            var request = {
                ActionName: layout.ActionNames.UpdateBookingHistory,
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

            Xrm.WebApi.online.execute(request).then(
                function success(result) {
                    Xrm.Utility.closeProgressIndicator();
                    if (result.ok) {
                        Xrm.Page.data.refresh();
                    }
                },
                function (error) {
                    Xrm.Utility.closeProgressIndicator();
                    showErrorMessage(error.message);
                }
            );
        } catch (e) {
            Xrm.Utility.closeProgressIndicator();
            showErrorMessage(error.message);
        }
    };

    function isRefreshBookingHistoryVisible(context) {
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
        isRefreshBookingHistoryVisible: isRefreshBookingHistoryVisible,
        onRefreshBookingHistoryClick: onRefreshBookingHistoryClick,
        onLoad: onLoad,
        onSave: onSave
    };
})(window, document);