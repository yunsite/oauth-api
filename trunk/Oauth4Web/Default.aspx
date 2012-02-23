<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Default.aspx.cs"
    Inherits="Oauth4Web.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微博App授权0.1</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <h3>
                微博应用授权</h3>
        </div>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:DropDownList ID="drpSite" runat="server">
                            <asp:ListItem Value="5">新浪微博2.0</asp:ListItem>
                            <asp:ListItem Value="0">新浪微博1.0</asp:ListItem>
                            <asp:ListItem Value="1">腾讯微博</asp:ListItem>
                            <asp:ListItem Value="2">网易微博</asp:ListItem>
                            <asp:ListItem Value="3">搜狐微博</asp:ListItem>
                            <%--<asp:ListItem Value="4">开心网</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:Button ID="butReset" runat="server" Text="清空" OnClick="butReset_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        AppKey:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppKey" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        AppSecret:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppSecret" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        微博帐号:
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        微博密码:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnGo" runat="server" Text="提交" OnClick="btnGo_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <h4>
                            授权结果：</h4>
                    </td>
                </tr>
                <tr>
                    <td>
                        Token:
                    </td>
                    <td>
                        <asp:TextBox ID="txtToken" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        TokenSecret:
                    </td>
                    <td>
                        <asp:TextBox ID="txtTokenSecret" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnSave" runat="server" Text="保存" onclick="btnSave_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                        --------------------------测试API---------------------------
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        api:
                    </td>
                    <td>
                        <asp:TextBox ID="txtApi" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        api参数:
                    </td>
                    <td>
                        <asp:TextBox ID="txtApiParameter" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        图片:
                    </td>
                    <td>
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="drpMethod" runat="server">
                            <asp:ListItem Value="0">GET</asp:ListItem>
                            <asp:ListItem Value="1">POST</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnGet" runat="server" Text="测试API" OnClick="btnGet_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Width="493px" Height="500px"
                            EnableViewState="False"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
