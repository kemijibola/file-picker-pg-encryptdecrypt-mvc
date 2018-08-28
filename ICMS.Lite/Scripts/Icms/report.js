function Report ()
{
    this.uploadClass = new Generic();
    this.init();
}

Report.prototype.init = function () {
    this.ViewReports();
};

Report.prototype.OnSuccessful = function ()
{
    window.open('@Url.Action("GeneratedIndentsReports", "Reports", null)', '_blank').focus();
}

Report.prototype.ViewReports = function () {
    $("#btnrptr").click(function () {
        console.log(new Date(''))

    });
}