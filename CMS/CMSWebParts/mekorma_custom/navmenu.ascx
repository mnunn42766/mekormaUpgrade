<%@ Control Language="C#" AutoEventWireup="true" CodeFile="navmenu.ascx.cs" Inherits="CMSWebParts_mekorma_custom_navmenu" %>
   

<asp:Literal runat="server" ID="menu_render"></asp:Literal>
<input class="currentpageId" type="hidden" id="currentpageId" runat="server" />
<script type="text/javascript">
    $(document).ready(function () {

        //Highlight static navigation links on top of the webpages
        $("#quicklinks .link").each(function () {
            if ($(this).attr("data-pageid") == $(".currentpageId").val()) {
                $(this).addClass("menuUnderline");
            }
        });

        //Highlight static navigation links in mobile
        $(".mobquicklinks").each(function () {
            if ($(this).attr("data-pageid") == $(".currentpageId").val()) {
                $(this).addClass("pageSelected");
            }
        });

        //Highlight main navigation - highlight parent link when navigated to sub-pages
        $("li.parent-link").each(function (i) {
            var parentPage = $(this);
            $("li.child-link").each(function (j) {
                var childPage = $(this);
                if (childPage.attr("data-pageid") == $(".currentpageId").val()) {
                    if (childPage.attr("data-parentpageid") == parentPage.attr("data-pageid")) {
                        parentPage.addClass("pageSelected");
                        childPage.addClass("pageSelected");
                    }
                }
            });
        });

        //the mobile hamburger icon for menu
        $(".menu-collapse").click(function () {
            $("#navmenu").slideToggle();
        });

       
    });
</script>