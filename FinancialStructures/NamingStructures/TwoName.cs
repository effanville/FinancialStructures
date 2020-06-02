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
            //both name and company cannot be null so this is all cases.
            if (string.IsNullOrEmpty(Company) && !string.IsNullOrEmpty(Name))
            {
                return Name;
            }
            if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Company))
            {
                return Company;
            }

            return Company + "-" + Name;
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
        /// Returns whether another object is the same as this one.
        /// </summary>
        public override bool Equals(object obj)
        {
            return EqualityMethod(obj);

        }

        public override int GetHashCode()
        {
            int companyVal = Company != null ? Company.GetHashCode() * 365 : 0;
            int nameVal = Name != null ? Name.GetHashCode() : 0;
            return companyVal + nameVal;
        }

        /// <summary>
        /// Equal if both names are the same.
        /// Can be used in inherited classes to query the uniqueness of the names.
        /// </summary>
        public bool IsEqualTo(object obj)
        {
            return EqualityMethod(obj);
        }

        private bool EqualityMethod(object obj)
        {
            if (obj is TwoName otherName)
            {
                if (otherName == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(Company) && string.IsNullOrEmpty(Name))
                {
                    if (string.IsNullOrEmpty(otherName.Company) && string.IsNullOrEmpty(otherName.Name))
                    {
                        return true;
                    }

                    return false;
                }
                if (string.IsNullOrEmpty(Company))
                {
                    if (string.IsNullOrEmpty(otherName.Company))
                    {
                        return Name.Equals(otherName.Name);
                    }

                    return false;
                }

                if (string.IsNullOrEmpty(Name))
                {
                    if (string.IsNullOrEmpty(otherName.Name))
                    {
                        return Company.Equals(otherName.Company);
                    }

                    return false;
                }

                if (Company.Equals(otherName.Company) && Name.Equals(otherName.Name))
                {
                    return true;
                }
            }

            return false;
        }

        private string fCompany;

        /// <summary>
        /// The primary name (the company name)
        /// </summary>
        public string Company
        {
            get
            {
                return fCompany;
            }
            set
            {
                fCompany = value;
            }
        }

        private string fName;

        /// <summary>
        /// The secondary name.
        /// </summary>
        public string Name
        {
            get
            {
                return fName;
            }
            set
            {
                fName = value;
            }
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
