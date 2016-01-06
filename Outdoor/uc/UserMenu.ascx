<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserMenu.ascx.cs" Inherits="Outdoor.uc.UserMenu" %>
<div class="my_left" style="height:200px">
    <h2>我的任务</h2>
    <ul class="my_jbo">
        <li id="LiTaskAdd" runat="server"><a href="/monitor/Addtask.aspx">创建任务</a></li>
        <li id="LiScheduleList" runat="server"><a href="/monitor/ScheduleList.aspx">排期列表</a></li>
        <%--       <li id="LiMyTaskList" runat="server"><a href="/monitor/MyTaskList.aspx">我的任务</a></li>--%>
        <li id="LiCustomerList" runat="server"><a href="/monitor/CustomerList.aspx">账号管理</a></li>
        <li id="LiMoneyStatistics" runat="server"><a href="/monitor/MoneyStatistics.aspx">财务统计</a></li>
        <li id="LiTaskStatistics" runat="server"><a href="/monitor/TaskStatistics.aspx">任务统计</a></li>
    </ul>
</div>
