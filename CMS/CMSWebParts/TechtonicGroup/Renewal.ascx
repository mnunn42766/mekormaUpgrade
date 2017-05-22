  <%@ Control Language="C#" AutoEventWireup="true" CodeFile="Renewal.ascx.cs" Inherits="CMSWebParts_TechtonicGroup_Renewal"  %>
<script src='https://www.google.com/recaptcha/api.js'></script>

<asp:UpdatePanel ID="upAjax" runat="server">
    <ContentTemplate>
 <div class="row">
	  <div class="row">
		 <div class="col-md-12">
			<asp:Label ID="lblErrorMessage" runat="server" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label>
		 </div>
	  </div>
	  <div class="row">
		 <div class="col-md-12" >
			<h1 style="color:green">Renewal Portal</h1>
		 </div>
	  </div>
	  <div class="row">
		 <div class="col-md-12">
			<label>Welcome to the Mekormal Renewal Portal! This provides a fast and easy way to access online <br />
			renewal forms to pay or request a quote for a yearly enhancement. This portal can provide both Partner<br />
			Billed and Direct Billed forms.
		  <a id="hyperLink" href="#" ">  Microsoft Account Numbers </a> are required to access the appropriate form.&nbsp;<a id="infoIcon" href="#"><span  class="glyphicon glyphicon-info-sign"></span></a>
			</label>  
		 </div>
		 <br />
		 <br />
		 <br />
		 <br /> 
	  </div>
	  <div class="row">
		 <div class="col-md-6">
			<label>Are you a Partner renewing an End User's Enhancement Plan?</label>  
		 </div>
		 <div class="col-md-3">             
				  <div class="col-md-3">
			<div class="btn-group" style="font-size:16px;
     width: 100px;" role="group" aria-label="Renewing for a partner?">
				<asp:LinkButton ID="lbNo" runat="server" onClick="lbNo_Click"   CssClass="btn btn-secondary noBtn partnerChoice">No</asp:LinkButton> 
				<asp:LinkButton ID="lbYes" runat="server" OnClick="lbYes_Click" CssClass="btn btn-secondary yesBtn partnerChoice active">Yes</asp:LinkButton> 
				</div>
			</div>
	
		 </div>
	  </div>
	  <br />
	  <br />
	   <div>
           
            
       <!--double form-->
	   <asp:Panel ID="pnlPartnerAccount" runat="server" CssClass="partnerPannel" >
	      <div class="row">
		     <div id="partner-ms">
			    <div class="col-md-3" id="msPartner">
			       <asp:Label ID="lblPartnershow" runat="server" CssClass="form-control txtPartnerAccountNumber">Partner Microsoft Account number</asp:Label>  
			    </div>
			    <div class="col-md-3 ">
			       <asp:TextBox ID="txtPartnerAccountNumber" runat="server" CssClass="form-control txtPartnerAccountNumber"></asp:TextBox>
			    </div>
			    <br />
			    <br />
			    <div class="col-md-3">
			       <label>End User Microsoft Account Number</label>  
			    </div>
			    <div class="col-md-3" id="txtMSAccountNumber">
			       <asp:TextBox ID="txtMicrosoftAccountNumber" runat="server" CssClass="form-control txtMicrosoftAccountNumber"></asp:TextBox>
                    </div>
                 </div>
                 </div>		  
	    </asp:Panel>
        <!--End double form-->
	  <br />

	  
      <!--single form-->
	  <asp:Panel id="pnlAccountNumber" runat="server" CssClass="microsoftPannel" >
	  <div class="row">
		 <div>
			<div class="col-md-3" id="nonpartner-ms">
			   <label>Your Microsoft Account Number</label>  
			</div>
			<div class="col-md-3">
			   <asp:TextBox ID="txtYourMicrosoftNumber" runat="server" CssClass="form-control txtYourMicrosoftNumber"></asp:TextBox>
				</div>
                </div>
          </div>
          </asp:Panel>
                <div class="row">
                <div class ="col-md-8">
                     
                <asp:LinkButton ID="LinkButton2" OnClick="lbRenew_Click" CssClass="FormButton btn btn-primary" runat="server">Submit</asp:LinkButton>
                </div>
               </div>
    <!--end single form-->

				<div class="row">
					
			</div>
		 </div>
	   <br />
   </div>

	</div>
</div>

<!--recapta-->
<div class="col-md-8" > 
	
	<br/>
   
	<%--<asp:Panel ID="recapture" runat="server" class="g-recaptcha" data-sitekey="6LdFkgsUAAAAAF5vsZhUT82jm22LkhYrUMWyWjcH"> </asp:Panel>--%>
</div>
<!--recapta-->

<div class="modal fade bs-example-modal-lg" id="myModal" tabindex="-1" role="dialog" >
   <div class="modal-dialog modal-lg">
	  <div class="modal-content">
	 
	   <div class="modal-header">
				<button style="float:right" type="button" class="close" data-dismiss="modal" aria-hidden="true" aria-labelledby="myLargeModalLabel">×</button>
				<h2>  How to find your Microsoft Account Number:</h2>
			</div>
			<div class="modal-body">
				<h4>
					Your Microsoft Account Number can be found within your Dynamics GP system. Administrative access maybe required to view this screen. 
				</h4>
				<br />
				<a>
					Microsoft Dynamics GP > Setup > Systems > Mekorma Products Registration
				</a>
				<br />
				<br />
				<img src="/App_Themes/CommunitySite/Images/TechtonicGroup/MekormaProductsRegistrationInfo.png" />
			</div>
	  </div>
   </div>
</div>
        </ContentTemplate>
    </asp:UpdatePanel>
<!--=== End Content Part ===-->
<link href="/App_Themes/TechtonicGroup/css/bootstrap.css" rel="stylesheet" />
<script src="/App_Themes/TechtonicGroup/js/bootstrap.js"></script>
    


<script type ="text/javascript">

    $(function () {
        var $partnerNumberTxtBox = $('#p_lt_ctl02_Renewal_txtPartnerAccountNumber');
        var $microsoftNumberTxtBox = $('#p_lt_ctl02_Renewal_txtMicrosoftAccountNumber');
        var $msAccountNumber = $('#msAccountNumber');
        var $txtYourMicrosoftNumber = $('.txtYourMicrosoftAccountNumber');
        var $lbNo = $('.noBtn');
        var $lbYes = $('.yesBtn');

        var $YourPartnerNumber = $('#nonpartner-ms');
        var $yesAndNoBtns = $('.partnerChoice');
        var $submitBtn = $('.FormButton');
        var $msPartner = $('#msPartner');
        $submitBtn.hide();
        if ($lbYes.hasClass('active')) {

        }
        $partnerNumberTxtBox.keyup(function () {
          
            if ($partnerNumberTxtBox.val().length > 2 && $microsoftNumberTxtBox.val().length > 2) {
               
                    $submitBtn.show();
                }
                else {
                $submitBtn.hide();
                }
            }
        );

        $microsoftNumberTxtBox.keyup(function () {
            if ($microsoftNumberTxtBox.val().length > 2 ) {
                
                $submitBtn.show();
            }
            else {
                $submitBtn.hide();
            }
        }
        );

        $yesAndNoBtns.click(function (event) {
            event.preventDefault();
            $yesAndNoBtns.removeClass("active");
            debugger;
            $(this).addClass("active");
            if ($(this).hasClass('noBtn')) {
                $msPartner.hide();
                $partnerNumberTxtBox.hide();
                $msAccountNumber.hide()
                $microsoftNumberTxtBox.show();
                $txtYourMicrosoftNumber.show();
                 } 
            else {
                $msPartner.show();
                $partnerNumberTxtBox.show();
            }

        });
    })

    $('#pnlAccountNumber').hide();

    $("#hyperLink").click(function () {
        $('#myModal').modal('show');
    });
    $("#infoIcon").click(function () {
        $('#myModal').modal('show');
    });

 

</script>

<style>
   .btn.active{
       background-color: green;
        color: white;
         
    }

   iframe {
    
    max-height: 100%;
    border: 1px solid #4b4b4c;
}

    .btn-group > .btn:active {
        background-color: green;
        color: white;
    }
     
    .btn-primary, .btn-primary:hover, .btn-prmary:active {
        background-color: green;
    }
    label {
        color: #58585b;
        font-family: 'AverageSans';
        font-size: 13px;
    }
    .btn-secondary, .btn-secondary:hover, .btn-secondary:active {
        border-color: green;
    }
    .btn-group > .btn:first-child:not(:last-child):not(.dropdown-toggle)
     {
        border-color: green;
       }
    .btn-group > .btn:last-child:not(:first-child) {
        border-color: green;
    }
    .FormButton{
        margin:10px auto;
    }
</style>