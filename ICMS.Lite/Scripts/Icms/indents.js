function Indent() {
    this.uploadClass = new Generic();
    this.alertClass = new Alert();
    this.init();    
}

Indent.prototype.init = function () {
    this.TriggerFileUpload();
    this.InputFileChange();
    this.UploadExcel();
    this.ConfirmUpload();
    this.GenerateIndents();
    this.EncryptionProcess();

};

Indent.prototype.TriggerFileUpload = function ()
{
    $("#btnUpload").click(function () {
        $("#files").val('');
        $("#files").trigger("click");
    });
}

Indent.prototype.InputFileChange = function () {
    $("#files").change(function (e) {
        var fileName = e.target.files[0].name;
        $("#selected").val(fileName);
    });
}

Indent.prototype.UploadExcel = function () {

    var self = this;

    $("#btnUploadExcel").click(function () {
        
        $('#Uploadmodal').modal('hide');
        var form = document.forms.namedItem("mfbTempForm");
        var oData = new FormData(form);
        console.log(oData);
        self.uploadClass.ajaxCall("../Indents/Upload", "Post", oData, function (err, response) {
            if (err != undefined && err != null) {
                theme = "red";
                self.alertClass.jInfoAlert('Error', err, theme, 'white');
            } else {
                if (response.isSuccessful) {

                    console.log(response);
                    theme = "green";
                    self.alertClass.jInfoAlert('Success', response.Message + ' indent(s) was uploaded!', theme, 'white');
                }
                if (!response.isSuccessful)
                {
                    theme = "blue";
                    self.alertClass.jInfoAlert('Information', response.Message, theme, 'white');
                }

            }
        })
    });

};

Indent.prototype.ConfirmUpload = function () {

    $("#btnUploadFile").click(function () {

        confirmGenerate("Are you sure you want to upload file?", "btnUploadExcel");

    });
}

Indent.prototype.GenerateIndents = function ()
{
    $("#btngenerate").click(function () {

        confirmGenerate("Are you sure you want to generate indents?", "btnProcessEncryption");
     
    }); 
}

Indent.prototype.EncryptionProcess = function ()
{
    var self = this;
    fetchData();

    $("#btnProcessEncryption").click(function () {

        $("#btngenerate").text("Processing...");
        $("#btngenerate").attr("disabled", "disabled");

        self.uploadClass.ajaxCall("../Indents/ProcessIndentsGeneration", "Get", null, function (err, response) {
            if (err != undefined && err != null) {
                theme = "red";
                self.alertClass.jInfoAlert('Error', err, theme, 'white');

                $("#btngenerate").text("Generate");
                $("#btngenerate").removeAttr("disabled", "disabled");
            }
            else {
                if (response.isSuccessful)
                {
                    theme = "green";
                    fetchData();
                    self.alertClass.jInfoAlert('Success', response.Message + ' indents was processed!', theme, 'white');

                    $('#tblIndents').DataTable().destroy();
                    $("#tblIndents tbody").empty();
                   
                    $("#btngenerate").text("Generate");
                    $("#btngenerate").removeAttr("disabled", "disabled");
                }
                if(!response.isSuccessful)
                {
                    theme = "blue";
                    fetchData();
                    self.alertClass.jInfoAlert('Information', response.Message, theme, 'white');

                    $("#btngenerate").text("Generate");
                    $("#btngenerate").removeAttr("disabled", "disabled");
                }
            }
        })

    });
}

    
      
