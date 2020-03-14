using System;

namespace FinancialStructures.DisplayStructures
{
    public class TwoName : IComparable
    {
        public int CompareTo(object obj)
        {
            if (obj is TwoName value)
            {
                if (PrimaryName == value.PrimaryName)
                {
                    if (SecondaryName == null)
                    {
                        if (value.SecondaryName == null)
                        {
                            return 0;
                        }
                        return 1;
                    }
                    return SecondaryName.CompareTo(value.SecondaryName);
                }

                return PrimaryName.CompareTo(value.PrimaryName);
            }

            return 0;
        }

        public bool IsEqualTo(object obj)
        {
            if (obj is TwoName value)
            {
                if (PrimaryName == value.PrimaryName)
                {
                    if (SecondaryName == value.SecondaryName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private string fPrimaryName;
        public string PrimaryName
        {
            get { return fPrimaryName; }
            set { fPrimaryName = value; }
        }
        private string fSecondaryName;
        public string SecondaryName
        {
            get { return fSecondaryName; }
            set { fSecondaryName = value; }
        }

        public TwoName(string primaryName, string secondaryName)
        {
            fPrimaryName = primaryName;
            fSecondaryName = secondaryName;
        }

        public TwoName(string primaryName)
        {
            fPrimaryName = primaryName;
        }
    }
}
