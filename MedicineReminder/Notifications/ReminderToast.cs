using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MedicineReminder.Notifications
{
    /// <summary>
    /// Represents toast notification for medicine reminder
    /// This toast is based on ToastTemplateType.ToastImageAndText02
    /// </summary>
    class ReminderToast
    {
        XElement xToast; 
        XElement xHeading;
        XElement xText;
        XAttribute xLaunch;

        public String Heading
        {
            get { return xHeading.Value; }
            set { xHeading.Value = value; }
        }

        public String Text
        {
            get { return xText.Value; }
            set { xText.Value = value; }
        }

        /// <summary>
        /// Defines parameter passed to the application when the application is launched via toast notification
        /// </summary>
        public String LaunchParameter
        {
            get { return xLaunch.Value; }
            set { xLaunch.Value = value; }
        }

        public ReminderToast()
        {
            String xml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02).GetXml();
            xToast = XElement.Load(new StringReader(xml));
            xHeading = GetChildElement(xToast, "text", "id", "1");
            xText = GetChildElement(xToast, "text", "id", "2");

            //Setting up default image
            var xImageSrc = GetChildElement(xToast, "image", "id", "1").Attribute("src");
            //xImageSrc.Value = String.Empty; //TODO: set the image to default logo
            xImageSrc.Value = @"ms-appx:///Assets/Logo.png";

            //Setting up default audio
            var xAudio = new XElement("audio");
            xAudio.Add(new XAttribute("src", "ms-winsoundevent:Notification.Looping.Alarm"));
            xAudio.Add(new XAttribute("loop", true));
            xToast.Add(xAudio);

            //Setting toast duration to long
            xToast.Add(new XAttribute("duration", "long"));

            //Setting up application launch parameter
            xLaunch = new XAttribute("launch", String.Empty);
            xToast.Add(xLaunch);
        }

        public ReminderToast(String heading, String text, String launchParameter)
            : this()
        {
            Heading = heading;
            Text = text;
            LaunchParameter = launchParameter;
        }

        private XElement GetChildElement(XElement parent, string name, string attributeName, string attributeValue)
        {
            return (from text in parent.Descendants(name)
                    where text.Attribute(attributeName).Value == attributeValue
                    select text).FirstOrDefault();
        }

        /// <summary>
        /// Returns the xml string for the toast
        /// </summary>
        override public String ToString()
        {
            return xToast.ToString();
        }

        public XmlDocument GetContent()
        {
            var doc = new XmlDocument();
            doc.LoadXml(this.ToString());
            return doc;
        }

    }
}
