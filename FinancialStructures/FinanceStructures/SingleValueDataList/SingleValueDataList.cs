﻿using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Acts as an overall change of an area of funds.
    /// </summary>
    /// <example>
    /// e.g. FTSE100 or MSCI-Asia
    /// </example>
    public partial class SingleValueDataList : IComparable, ISingleValueDataList
    {
        /// <summary>
        /// Event that controls when data is edited.
        /// </summary>
        public event EventHandler DataEdit;

        internal void OnDataEdit(object edited, EventArgs e)
        {
            DataEdit?.Invoke(edited, e);
        }

        public void SetupEventListening()
        {
            Values.DataEdit += OnDataEdit;
        }

        /// <summary>
        /// The string representation of this list.
        /// </summary>
        public override string ToString()
        {
            return Names.ToString();
        }

        /// <summary>
        /// Method of comparison
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is ICashAccount value)
            {
                return Names.CompareTo(value.Names);
            }

            return 0;
        }

        private NameData fNames;

        /// <summary>
        /// Any name type data associated to this security.
        /// </summary>
        public NameData Names
        {
            get
            {
                return fNames;
            }
            set
            {
                fNames = value;
            }
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public virtual string Name
        {
            get
            {
                return Names.Name;
            }

            set
            {
                Names.Name = value;
            }
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Company
        {
            get
            {
                return Names.Company;
            }

            set
            {
                Names.Company = value;
            }
        }

        public string Url
        {
            get
            {
                return Names.Url;
            }

            set
            {
                Names.Url = value;
            }
        }

        public string Currency
        {
            get
            {
                return Names.Currency;
            }
            set
            {
                Names.Currency = value;
            }
        }

        /// <summary>
        /// The values of the sector.
        /// </summary>
        private TimeList fValues = new TimeList();

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public TimeList Values
        {
            get
            {
                return fValues;
            }

            set
            {
                fValues = value;
            }
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public SingleValueDataList()
        {
            Names = new NameData();
            SetupEventListening();
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public SingleValueDataList(NameData names)
        {
            Names = names;
            SetupEventListening();
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public SingleValueDataList(NameData names, TimeList values)
        {
            Names = names;
            fValues = values;
            SetupEventListening();
        }

        public ISingleValueDataList Copy()
        {
            return new SingleValueDataList(Names, fValues);
        }

        public bool Any()
        {
            return fValues != null && fValues.Any();
        }
    }
}