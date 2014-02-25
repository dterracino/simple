using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace isukces.simple
{
    /*
    smartClass
    option NoAdditionalFile
    implement Constructor
    implement Constructor *
    
    property Percent double Progress in percent
    
    property Message string Progress message
    smartClassEnd
    */
    
    public partial class ProgressReporterEventArgs : EventArgs
    {
        #region Methods

        // Public Methods 

        public ProgressReporterEventArgs GetScaled(double min, double max)
        {
            return new ProgressReporterEventArgs()
            {
                Percent = ProgressReporter.Scale(min, max, this.percent),
                Message = this.message
            };
        }

        public ProgressReporterEventArgs GetScaled(double min, double max, string newMessage)
        {
            return new ProgressReporterEventArgs()
            {
                Percent = ProgressReporter.Scale(min, max, this.Percent),
                Message = newMessage
            };
        }

        #endregion Methods
    }
}


// -----:::::##### smartClass embedded code begin #####:::::----- generated 2014-02-25 10:46
// File generated automatically ver 2013-07-10 08:43
// Smartclass.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0c4d5d36fb5eb4ac
namespace isukces.simple
{
    public partial class ProgressReporterEventArgs 
    {
        /*
        /// <summary>
        /// Tworzy instancję obiektu
        /// </summary>
        public ProgressReporterEventArgs()
        {
        }

        Przykłady użycia

        implement INotifyPropertyChanged
        implement INotifyPropertyChanged_Passive
        implement ToString ##Percent## ##Message##
        implement ToString Percent=##Percent##, Message=##Message##
        implement equals Percent, Message
        implement equals *
        implement equals *, ~exclude1, ~exclude2
        */
        #region Constructors
        /// <summary>
        /// Tworzy instancję obiektu
        /// </summary>
        public ProgressReporterEventArgs()
        {
        }

        /// <summary>
        /// Tworzy instancję obiektu
        /// <param name="Percent">Progress in percent</param>
        /// <param name="Message">Progress message</param>
        /// </summary>
        public ProgressReporterEventArgs(double Percent, string Message)
        {
            this.Percent = Percent;
            this.Message = Message;
        }

        #endregion Constructors

        #region Constants
        /// <summary>
        /// Nazwa własności Percent; Progress in percent
        /// </summary>
        public const string PROPERTYNAME_PERCENT = "Percent";
        /// <summary>
        /// Nazwa własności Message; Progress message
        /// </summary>
        public const string PROPERTYNAME_MESSAGE = "Message";
        #endregion Constants

        #region Methods
        #endregion Methods

        #region Properties
        /// <summary>
        /// Progress in percent
        /// </summary>
        public double Percent
        {
            get
            {
                return percent;
            }
            set
            {
                percent = value;
            }
        }
        private double percent;
        /// <summary>
        /// Progress message
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                value = (value ?? String.Empty).Trim();
                message = value;
            }
        }
        private string message = string.Empty;
        #endregion Properties

    }
}
