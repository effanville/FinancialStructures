using System;
using System.Xml.Serialization;

namespace FinancialStructures.DataStructures
{
    /// <summary>
    /// Contains information about a note.
    /// </summary>
    public sealed class Note
    {
        /// <summary>
        /// The time the note was written.
        /// </summary>
        [XmlAttribute(AttributeName = "D")]
        public DateTime TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// The text of the note.
        /// </summary>
        [XmlAttribute(AttributeName = "T")]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Note()
        {
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public Note(DateTime timeStamp, string text)
        {
            TimeStamp = timeStamp;
            Text = text;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Note: {TimeStamp}-{Text}";
        }
    }
}
