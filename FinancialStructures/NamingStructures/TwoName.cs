using System;

namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// Contains naming information, allowing for a primary and secondary name.
    /// </summary>
    public class TwoName : IComparable
    {
        /// <summary>
        /// Display of names.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "'" + Company + "' - '" + Name + "'";
        }

        /// <summary>
        /// Compares both names.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is TwoName value)
            {
                if (Company == value.Company)
                {
                    if (Name == null)
                    {
                        if (value.Name == null)
                        {
                            return 0;
                        }
                        return 1;
                    }
                    return Name.CompareTo(value.Name);
                }
                if (Company == null && value.Company != null)
                {
                    return -1;
                }
                return Company.CompareTo(value.Company);
            }

            return 0;
        }

        /// <summary>
        /// Equal if both names are the same.
        /// </summary>
        public bool IsEqualTo(object obj)
        {
            if (obj is TwoName value)
            {
                if (Company == value.Company)
                {
                    if (Name == value.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private string fCompany;

        /// <summary>
        /// The primary name (the company name)
        /// </summary>
        public virtual string Company
        {
            get { return fCompany; }
            set { fCompany = value; }
        }

        private string fName;

        /// <summary>
        /// The secondary name.
        /// </summary>
        public virtual string Name
        {
            get { return fName; }
            set { fName = value; }
        }

        /// <summary>
        /// Constructor with both names.
        /// </summary>
        public TwoName(string primaryName, string secondaryName)
        {
            fCompany = primaryName;
            fName = secondaryName;
        }

        /// <summary>
        /// Allows for construction with just one name.
        /// </summary>
        public TwoName(string primaryName)
        {
            fCompany = primaryName;
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public TwoName()
        {
        }
    }
}
