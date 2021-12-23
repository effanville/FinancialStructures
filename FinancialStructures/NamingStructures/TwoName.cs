using System;

namespace FinancialStructures.NamingStructures
{
    /// <summary>
    /// Contains naming information, allowing for a primary and secondary name.
    /// </summary>
    public class TwoName : IComparable, IComparable<TwoName>, IEquatable<TwoName>
    {
        /// <summary>
        /// The primary name (the company name)
        /// </summary>
        public string Company
        {
            get;
            set;
        }

        /// <summary>
        /// The secondary name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor with both names.
        /// </summary>
        public TwoName(string primaryName, string secondaryName)
        {
            Company = primaryName;
            Name = secondaryName;
        }

        /// <summary>
        /// Allows for construction with just one name.
        /// </summary>
        public TwoName(string primaryName)
        {
            Company = primaryName;
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public TwoName()
        {
        }

        /// <summary>
        /// Display of names.
        /// </summary>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Company) && string.IsNullOrEmpty(Name))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(Company) && !string.IsNullOrEmpty(Name))
            {
                return Name;
            }
            if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Company))
            {
                return Company;
            }

            return $"{Company}-{Name}";
        }

        /// <inheritdoc/>
        public int CompareTo(TwoName other)
        {
            if (Company == other.Company)
            {
                if (Name == null)
                {
                    if (other.Name == null)
                    {
                        return 0;
                    }
                    return 1;
                }
                return Name.CompareTo(other.Name);
            }

            if (Company == null && other.Company != null)
            {
                return -1;
            }

            return Company.CompareTo(other.Company);
        }

        /// <summary>
        /// Compares both names.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is TwoName other)
            {
                return CompareTo(other);
            }

            return 0;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is TwoName otherName)
            {
                return Equals(otherName);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 17;
            hashCode = 23 * hashCode + Company?.GetHashCode() ?? 17;
            hashCode = 23 * hashCode + Name?.GetHashCode() ?? 17;
            return hashCode;
        }

        /// <summary>
        /// Equal if both names are the same.
        /// Can be used in inherited classes to query the uniqueness of the names.
        /// </summary>
        public bool IsEqualTo(object obj)
        {
            if (obj is TwoName otherName)
            {
                return Equals(otherName);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(TwoName other)
        {
            bool companiesEqual = Company?.Equals(other.Company) ?? other.Company == null;
            bool namesEqual = Name?.Equals(other.Name) ?? other.Name == null;
            return companiesEqual && namesEqual;
        }
    }
}
