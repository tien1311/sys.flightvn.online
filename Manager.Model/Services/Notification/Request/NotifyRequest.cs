using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Model.Services.Notification.Request
{
    public class NotifyLisaRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string MessageType { get; set; }
        public string ExtentData { get; set; }
    }
    public class NotifyLisaAgentCodeRequest : NotifyLisaRequest
    {
        public string ReceiverAgentCode { get; set; }

        //Topup
        public NotifyLisaAgentCodeRequest(string FirstCode, string Amount, string MessageType, string AgentCode, string ReceiverAgentCode, string ExtentData)
        {
            Title = string.Format("{0} - {1} {2} {3} VNĐ", FirstCode.ToUpper(), AgentCode, FirstCode.ToLower(), string.Format("{0:0,0}", Amount));
            Content = string.Format("{0} {1} {2} VNĐ", AgentCode, FirstCode.ToLower(), string.Format("{0:0,0}", Amount));
            this.MessageType = MessageType;
            this.ReceiverAgentCode = ReceiverAgentCode;
            this.ExtentData = ExtentData;
        }
        public NotifyLisaAgentCodeRequest(string FirstCode, string Airline, decimal Amount, string MessageType, string ReceiverAgentCode, string ExtentData)
        {
            Title = string.Format("[{0}] {1} số dư còn {2} VNĐ", FirstCode.ToUpper(), Airline, string.Format("{0:0,0}", Amount));
            Content = string.Format("Hạn mức hãng {0} đang ở mức cảnh báo", Airline);
            this.MessageType = MessageType;
            this.ReceiverAgentCode = ReceiverAgentCode;
            this.ExtentData = ExtentData;
        }
        public NotifyLisaAgentCodeRequest(string Title, string Content, string MessageType, string ReceiverAgentCode, string ExtentData)
        {
            this.Title = Title;
            this.Content = Content;
            this.MessageType = MessageType;
            this.ReceiverAgentCode = ReceiverAgentCode;
            this.ExtentData = ExtentData;
        }
    }
    public class NotifyLisaUserNameRequest : NotifyLisaRequest
    {
        public string ReceiverUsername { get; set; }

        //Topup
        public NotifyLisaUserNameRequest(string FirstCode, string Amount, string MessageType, string AgentCode, string ReceiverUsername, string ExtentData)
        {
            Title = string.Format("{0} - {1} {2} {3} VNĐ", FirstCode.ToUpper(), AgentCode, FirstCode.ToLower(), string.Format("{0:0,0}", Amount));
            Content = string.Format("{0} {1} {2} VNĐ", AgentCode, FirstCode.ToLower(), string.Format("{0:0,0}", Amount));
            this.MessageType = MessageType;
            this.ReceiverUsername = ReceiverUsername;
            this.ExtentData = ExtentData;
        }
        public NotifyLisaUserNameRequest(string FirstCode, string Airline, decimal Amount, string MessageType, string ReceiverUsername, string ExtentData)
        {
            Title = string.Format("[{0}] {1} số dư còn {2} VNĐ", FirstCode.ToUpper(), Airline, string.Format("{0:0,0}", Amount));
            Content = string.Format("Hạn mức hãng {0} đang ở mức cảnh báo", Airline);
            this.MessageType = MessageType;
            this.ReceiverUsername = ReceiverUsername;
            this.ExtentData = ExtentData;
        }
        public NotifyLisaUserNameRequest(string Title, string Content, string MessageType, string ReceiverUsername, string ExtentData)
        {
            this.Title = Title;
            this.Content = Content;
            this.MessageType = MessageType;
            this.ReceiverUsername = ReceiverUsername;
            this.ExtentData = ExtentData;
        }
    }
}
