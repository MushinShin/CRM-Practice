function returnRent(executionContext) {
    var parameters = {};
    var rent = {};
    rent.custom_rentid = executionContext.data.entity.getId();
    rent["@odata.type"] = "Microsoft.Dynamics.CRM.custom_rent";
    parameters.Rent = rent;

    var req = new XMLHttpRequest();
    req.open("POST", Xrm.Page.context.getClientUrl() + "/api/data/v9.1/custom_ReturnRent", true);
    req.setRequestHeader("OData-MaxVersion", "4.0");
    req.setRequestHeader("OData-Version", "4.0");
    req.setRequestHeader("Accept", "application/json");
    req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    req.onreadystatechange = function () {
        if (this.readyState === 4) {
            req.onreadystatechange = null;
            if (this.status === 200) {
                var results = JSON.parse(this.response);
                if (results.Result)
                    executionContext.ui.refresh();
            } else {
                Xrm.Utility.alertDialog(this.statusText);
            }
        }
    };
    req.send(JSON.stringify(parameters));
}