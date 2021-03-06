﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <link href="http://localhost:1745/Content/Bootstrap/css/bootstrap-responsive.css" rel="stylesheet" />
    <link href="http://localhost:1745/Content/Bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="http://localhost:1745/Content/Site/css/Site.css" rel="stylesheet" />
    <title></title>

</head>

<body>
    <p style="text-align: center;"><strong><span style="text-decoration: underline;">Sales Form</span></strong></p>
    <fieldset>
        <legend>Customer Details</legend>
        <table>
            <tbody>
                <tr>
                    <td>
                        <table style="width: 100%;">
                            <tbody>
                                <tr>
                                    <td style="width: 97px;">Customer Id :</td>
                                    <td>1</td>
                                </tr>
                                <tr>
                                    <td>Name :</td>
                                    <td>$Customer.NameSurname$</td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top;">Address :</td>
                                    <td>$Customer.Address$<br />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <td style="vertical-align: top;">
                        <table style="width: 100%;">
                            <tbody>
                                <tr>
                                    <td style="width: 87px;"></td>
                                </tr>
                                <tr>
                                    <td>Tel(1) :</td>
                                    <td>$Customer.Tel1$</td>
                                </tr>
                                <tr>
                                    <td>Tel(2) :</td>
                                    <td>$Customer.Tel2$</td>
                                </tr>
                                <tr>
                                    <td>Mobile :</td>
                                    <td>$Customer.Mobile1$</td>
                                </tr>
                                <tr>
                                    <td>Email :</td>
                                    <td>$Customer.Email$</td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </fieldset>
    <fieldset>
        <legend>Delivery</legend>
        <table>
            <tbody>
                <tr style="vertical-align: top;">
                    <td>
                        <table style="width: 100%;">
                            <tbody>
                                <tr>
                                    <td style="width: 97px; vertical-align: top;">Instructions :</td>
                                    <td>$Instructions$</td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                    <td>
                        <table>
                            <tbody>
                                <tr>
                                    <td style="width: 97px; vertical-align: top;">Address :</td>
                                    <td>$Alternate.Address$<br />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </fieldset>
    <fieldset>
        <legend>Product Details</legend>
        $ProductDetails$
    </fieldset>
     <fieldset>
        <legend>Contact Details</legend>
        $ContractDetails$
    </fieldset>
</body>
</html>
