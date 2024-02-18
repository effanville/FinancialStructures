using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FinancialStructures.DataStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        private readonly object NotesLock = new object();

        /// <inheritdoc/>
        [XmlIgnore]
        public IReadOnlyList<Note> Notes
        {
            get
            {
                lock (NotesLock)
                {
                    return NotesInternal.ToList();
                }
            }
        }

        /// <summary>
        /// Internal list of all notes for the portfolio.
        /// </summary>
        [XmlArray(ElementName = "Notes")]
        public List<Note> NotesInternal
        {
            get;
            set;
        } = new List<Note>();

        /// <inheritdoc/>
        public void AddNote(DateTime timeStamp, string note)
        {
            lock (NotesLock)
            {
                NotesInternal.Add(new Note(timeStamp, note));
            }

            OnPortfolioChanged(Notes, new PortfolioEventArgs());
        }

        /// <inheritdoc/>
        public bool RemoveNote(Note note)
        {
            bool removed;
            lock (NotesLock)
            {
                removed = NotesInternal.Remove(note);
            }

            if (removed)
            {
                OnPortfolioChanged(Notes, new PortfolioEventArgs());
            }

            return removed;
        }

        /// <inheritdoc/>
        public void RemoveNote(int noteIndex)
        {
            lock (NotesLock)
            {
                NotesInternal.RemoveAt(noteIndex);
            }

            OnPortfolioChanged(Notes, new PortfolioEventArgs());
        }
    }
}
