using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace SIS.Principal.Models
{
    public class Utils
    {
        public const bool WithAdditionals = true;

        public const string Error = "error";
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";

        public static Object Deserialize(String Value, Type Class)
        {
            return new JavaScriptSerializer().Deserialize(Value, Class);
        }

        public static void Write(ResponseType Type, Object Message)
        {
            switch (Type)
            {
                case ResponseType.Text:
                    {
                        HttpContext.Current.Response.ContentType = "text/plain;charset=utf-8";
                        HttpContext.Current.Response.Write(Message);
                    }
                    break;
                case ResponseType.JSON:
                    {
                        HttpContext.Current.Response.ContentType = "application/json;charset=utf-8";
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        serializer.MaxJsonLength = Int32.MaxValue;
                        HttpContext.Current.Response.Write(serializer.Serialize(Message));
                    }
                    break;
            }
        }

        public static void WriteMessage(String Message)
        {
            ResponseMessage Response = new ResponseMessage();
            switch (Message.Split('|')[0])
            {
                case Error:
                case Success:
                case Information:
                case Warning:
                    {
                        Response.Id = Message.Split('|')[0].ToString();
                        Response.Message = Message.Split('|')[1].ToString();

                        HttpContext.Current.Response.ContentType = "application/json;charset=utf-8";
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        serializer.MaxJsonLength = Int32.MaxValue;
                        HttpContext.Current.Response.Write(serializer.Serialize(Response));
                    }
                    break;
                default:
                    {
                        throw new Exception("El mensaje no tiene un tipo válido.");
                    }
            }
        }

        public static void WriteMessage(String Message, bool Additionals)
        {
            ResponseMessage Response = new ResponseMessage();
            switch (Message.Split('|')[0])
            {
                case Error:
                case Success:
                case Information:
                case Warning:
                    {
                        Response.Id = Message.Split('|')[0].ToString();
                        Response.Message = Message.Split('|')[1].ToString();
                        Response.Additionals = new List<string>();
                        if (!Response.Id.Equals(Error))
                        {
                            string[] AdditionalsParameters = Message.Split('|')[2].ToString().Split('&');
                            foreach (string AdditionalParameter in AdditionalsParameters)
                            {
                                Response.Additionals.Add(AdditionalParameter);
                            }
                        }

                        HttpContext.Current.Response.ContentType = "application/json;charset=utf-8";
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        serializer.MaxJsonLength = Int32.MaxValue;
                        HttpContext.Current.Response.Write(serializer.Serialize(Response));
                    }
                    break;
                default:
                    {
                        throw new Exception("El mensaje no tiene un tipo válido.");
                    }
            }
        }
    }

    public class ResponseMessage
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public List<string> Additionals { get; set; }
    }

    public enum ResponseType
    {
        Text = 1,
        JSON = 2
    }
}