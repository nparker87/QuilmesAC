﻿namespace QuilmesAC.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Text.RegularExpressions;

    public class Emailer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="fromAddress"></param>
        /// <param name="fromName"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="cc">List of type EmailList</param>
        /// <param name="bcc">List of type EmailList</param>
        public static void SendMsg(string toAddress, string fromAddress, string fromName, string subject, string body, EmailList cc, EmailList bcc)
        {
            var msg = new MailMessage
            {
                Body = body,
                IsBodyHtml = true,
                Priority = MailPriority.Normal,
                Subject = subject
            };
            toAddress = String.IsNullOrWhiteSpace(toAddress) ? "parkerjn2@vcu.edu" : toAddress;

            if (IsValidEmail(toAddress))
                msg.To.Add(new MailAddress(toAddress));
            else
            {
                msg.To.Add(new MailAddress("parkerjn2@vcu.edu"));
                msg.Subject = "ERROR: Invalid To Email Address - " + subject;
            }

            if (IsValidEmail(fromAddress))
                msg.From = new MailAddress(fromAddress, fromName);
            else
            {
                msg.To.Add(new MailAddress("parkerjn2@vcu.edu"));
                msg.Subject = "ERROR: Invalid From Email Address - " + subject;
            }

            // Add any cc'd
            if (cc != null)
                foreach (var email in cc.Emails)
                    if (IsValidEmail(email))
                        msg.CC.Add(new MailAddress(cc.ToString()));

            // Add any bcc'd
            if (bcc != null)
                foreach (var email in bcc.Emails)
                    if (IsValidEmail(email))
                        msg.CC.Add(new MailAddress(bcc.ToString()));

            var smtp = new SmtpClient("");
            smtp.Send(msg);
        }

        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                return false;

            // checks to make sure e-mail is valid
            var reg = new Regex(@"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}\b");
            return reg.IsMatch(email);
        }

        /// <summary>
        /// Used for a list of cc or bcc emails
        /// </summary>
        public class EmailList
        {
            public List<string> Emails { get; set; }
        }
    }
}