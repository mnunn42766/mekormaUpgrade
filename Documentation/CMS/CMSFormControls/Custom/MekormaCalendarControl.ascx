<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/CMSFormControls/Custom/MekormaCalendarControl.ascx.cs"
    Inherits="CMSFormControls_Custom_MekormaCalendarControl" %>


<link type="text/css" rel="stylesheet" href="/CMSScripts/mekorma/jquery-ui/jquery-ui.css">
<script type="text/javascript" src="/CMSScripts/mekorma/jquery-ui/jquery-ui.js"></script>
<asp:HiddenField ClientIDMode="Static" id="noWeekendHdn" runat="server" />
<asp:HiddenField ClientIDMode="Static" id="monthAndDayHdn" runat="server" />
<asp:HiddenField ClientIDMode="Static" id="onlyFutureDatesHdn" runat="server" />

<script type="text/javascript">
    $(document).ready(function () {

        var monthAndDayHdn = $("#monthAndDayHdn").val();
        var dateFormat = "";
        if (monthAndDayHdn == "true") {
            dateFormat = "mm/dd";
        }
        else {
            dateFormat = "mm/dd/yy";
        }

        var onlyFutureDatesHdn = $("#onlyFutureDatesHdn").val();
        var minDate = null;
        if (onlyFutureDatesHdn == "true") {
            minDate = new Date();
        }
        else {
            minDate = null;
        }

        function noWeekends(date) {
            var noWeekendHdn = $("#noWeekendHdn").val();
            if (noWeekendHdn == "true") {
                var noWeekend = jQuery.datepicker.noWeekends(date);
                return noWeekend;
            }
            else {
                return "true";
            }
        }

        $("#mk_datepicker").datepicker({
            changeMonth: true,
            changeYear: true,
            buttonImage: "/CMSScripts/mekorma/jquery-ui/images/calendar.png",
            buttonImageOnly: true,
            showOn: "both",
            minDate: minDate,
            beforeShowDay: noWeekends,
            dateFormat: dateFormat
        });

        //hide year selection if flag for only month and day is set. 
        $("#mk_datepicker").on("focus", function () {
            setTimeout(function () {
                if (monthAndDayHdn == "true") {
                    $(".ui-datepicker-year").hide();
                }
            }, 100);
        });

        //hide the datepicker icon if the date textbox is disabled
        if ($("#mk_datepicker").is(':disabled')) {
            $('.ui-datepicker-trigger').hide();
        }


    });
</script>
<asp:TextBox ID="mk_datepicker" class="left" type="text" runat="server" ClientIDMode="Static"></asp:TextBox>


