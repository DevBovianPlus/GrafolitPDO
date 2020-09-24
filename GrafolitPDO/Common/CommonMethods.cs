﻿using DatabaseWebService.ModelsPDO.Inquiry;
using GrafolitPDO.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace GrafolitPDO.Common
{
    public static class CommonMethods
    {
        public static bool IsNumeric(this string text)
        {
            double test;
            return double.TryParse(text, out test);
        }

        public static int ParseInt(object param)
        {
            int num = 0;

            if (param != null)
            {
                int.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static Nullable<int> ParseNullableInt(object param)
        {
            int num = 0;

            if (param != null)
            {
                int.TryParse(param.ToString(), out num);

                if (num < 0)
                    return null;

                return num;
            }
            else
                return null;
        }

        public static decimal ParseDecimal(object param)
        {
            decimal num = 0;
            if (param != null)
            {
                decimal.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static double ParseDouble(object param)
        {
            double num = 0;
            if (param != null)
            {
                double.TryParse(param.ToString(), out num);

                if (num < 0)
                    num = 0;
            }

            return num;
        }

        public static bool ParseBool(string param)
        {
            bool value = false;
            bool.TryParse(param, out value);

            return value;
        }

        public static bool ParseBool(object param)
        {
            bool variable = false;
            if (param != null)
            {
                bool.TryParse(param.ToString(), out variable);
            }

            return variable;
        }

        public static string PreveriZaSumnike(string _crka)
        {
            char crkaC = ' ';
            string novS = "";

            _crka = _crka.ToUpper();

            foreach (char item in _crka)
            {
                switch (item)
                {
                    case 'Č':
                        crkaC = 'C';
                        break;
                    case 'Š':
                        crkaC = 'S';
                        break;
                    case 'Ž':
                        crkaC = 'Z';
                        break;
                    case 'Đ':
                        crkaC = 'D';
                        break;
                    default:
                        crkaC = item;
                        break;
                }

                novS += crkaC.ToString();
            }

            return novS;
        }

        public static string Trim(string sTrim)
        {
            return String.IsNullOrEmpty(sTrim) ? "" : sTrim.Trim();
        }

        public static void LogThis(string message)
        {
            bool isLoggingEnabled = CommonMethods.ParseBool(ConfigurationManager.AppSettings["EnableLogging"]);

            if (isLoggingEnabled)
            {
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                File.AppendAllText(directory + "log.txt", DateTime.Now + " " + message + Environment.NewLine);
            }
        }

        public static bool SendEmailToDeveloper(string displayName, string subject, string body)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;//Port 465 (SSL required)
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("bovianplus@gmail.com", "Geslo123.");
                client.Timeout = 6000;

                MailMessage message;

                message = new MailMessage();
                message.To.Add(new MailAddress("martin@bovianplus.si"));
                message.To.Add(new MailAddress("boris.dolinsek@bovianplus.si"));

                message.Sender = new MailAddress("bovianplus@gmail.com");
                message.From = new MailAddress("bovianplus@gmail.com", displayName);
                message.Subject = subject;
                message.IsBodyHtml = false;
                message.Body = body;
                message.BodyEncoding = Encoding.UTF8;

                client.Send(message);

            }
            catch (SmtpFailedRecipientsException ex)
            {
                LogThis(ex.Message + "\r\n " + ex.Source + "\r\n " + ex.StackTrace);
                return false;
            }
            catch (SmtpException ex)
            {
                LogThis(ex.Message + "\r\n " + ex.Source + "\r\n " + ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                LogThis(ex.Message + "\r\n " + ex.Source + "\r\n " + ex.StackTrace);
                return false;
            }

            return true;
        }

        public static void getError(Exception e, ref string errors)
        {
            if (e.GetType() != typeof(HttpException)) errors += " -------- " + e.ToString();
            if (e.InnerException != null) getError(e.InnerException, ref errors);
        }

        public static bool IsCallbackRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            var context = HttpContext.Current;
            var isCallbackRequest = false;// callback requests are ajax requests
            if (context != null && context.CurrentHandler != null && context.CurrentHandler is System.Web.UI.Page)
            {
                isCallbackRequest = ((System.Web.UI.Page)context.CurrentHandler).IsCallback;
            }
            return isCallbackRequest || (request["X-Requested-With"] == "XMLHttpRequest") || (request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }

        public static string GetTimeStamp()
        {
            return PrincipalHelper.GetUserPrincipal().firstName + "_" + PrincipalHelper.GetUserPrincipal().lastName + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm");
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static byte[] GetZipMemmoryStream(List<FileToDownload> fileList)
        {
            using (var ms = new MemoryStream())
            {//Create an archive and store the stream in memory.
                using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var item in fileList)
                    {
                        //Create a zip entry for each attachment
                        var zipEntry = zipArchive.CreateEntry(item.Name + item.Extension);

                        //Get the stream of the attachment
                        using (var originalFileStream = new MemoryStream(item.Content))
                        {
                            using (var zipEntryStream = zipEntry.Open())
                            {
                                //Copy the attachment stream to the zip entry stream
                                originalFileStream.CopyTo(zipEntryStream);
                            }
                        }
                    }
                }

                return ms.ToArray();
            }
        }

        public static decimal CalculateSheetInKg(hlpCalculateWeight hw, decimal qnt)
        {
            decimal calc = 0;

            if (hw != null && hw.Weight > 0 && hw.SizeA > 0 && hw.SizeB > 0)
            {
                decimal pG = hw.Weight * (decimal)0.001;
                decimal sA = hw.SizeA * (decimal)0.01;
                decimal sB = hw.SizeB * (decimal)0.01;

                calc = pG * sA * sB * qnt;
            }

            return Convert.ToDecimal(calc);
        }


        public static hlpCalculateWeight GetCalculateWeight(string sNazivArtikla)
        {
            hlpCalculateWeight hlpWeight = new hlpCalculateWeight();

            if (sNazivArtikla != null)
            {
                sNazivArtikla = sNazivArtikla.ToUpper();

                string[] split = sNazivArtikla.Split(' ');
                foreach (var item in split)
                {
                    // weight
                    if (item.Contains("g") || item.Contains("G"))
                    {
                        string[] splWeight = item.Split('G');
                        if (splWeight.Length == 2 && CommonMethods.IsNumeric(splWeight[0].ToString()))
                        {
                            hlpWeight.Weight = Convert.ToInt32(splWeight[0]);
                        }
                    }

                    // size
                    if (item.Contains("x") || item.Contains("X"))
                    {

                        string[] splSize = item.Split('X');
                        if (splSize.Length == 2 && CommonMethods.IsNumeric(splSize[0].ToString()) && CommonMethods.IsNumeric(splSize[1].ToString()))
                        {
                            string sSize1 = splSize[0];
                            string sSize2 = splSize[1];

                            sSize1 = sSize1.Replace(",", ".");
                            sSize2 = sSize2.Replace(",", ".");

                            hlpWeight.SizeA = CommonMethods.ParseDecimal(sSize1);
                            hlpWeight.SizeB = CommonMethods.ParseDecimal(sSize2);
                        }
                    }
                }
            }
            return hlpWeight;
        }
    }
}