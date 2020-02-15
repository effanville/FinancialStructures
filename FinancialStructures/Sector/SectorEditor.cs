using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using System;
using System.Collections.Generic;

namespace SectorHelperFunctions
{
    /// <summary>
    /// Helper class to edit the sectors held in the global database.
    /// </summary>
    public static class SectorEditor
    {
        public static List<NameData> GetSectorNames(List<Sector> sectors)
        {
            var outputs = new List<NameData>();
            if (sectors != null)
            {
                foreach (Sector thing in sectors)
                {
                    outputs.Add(new NameData(thing.GetName(), string.Empty, string.Empty, thing.GetUrl(), false));
                }
            }
            return outputs;
        }

        public static bool TryAddSector(List<Sector> sectors, NameData name, ErrorReports reports)
        { 
            return TryAddSector(sectors, name.Name, name.Url, reports);
        }
        /// <summary>
        /// Tries to add a sector to the underlying global database
        /// </summary>
        public static bool TryAddSector(List<Sector> sectors, string name, string url, ErrorReports reports)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    reports.AddError($"Sector with name {name} already exists.", Location.AddingData);
                    return false;
                }
            }
            Sector newSector = new Sector(name, url);
            reports.AddReport($"Added sector with name {name}.", Location.AddingData);
            sectors.Add(newSector);
            return true;
        }

        /// <summary>
        /// Returns a sector from the database with specified name.
        /// </summary>
        public static bool TryGetSector(List<Sector> sectors, string name, out Sector Desired)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    Desired = sector.Copy();
                }
            }
            Desired = null;
            return false;
        }

        public static bool TryGetSectorData(List<Sector> sectors, string name, out List<AccountDayDataView> data)
        {
            data = new List<AccountDayDataView>();
            foreach (Sector sec in sectors)
            {
                if (sec.GetName() == name)
                {
                    data = sec.GetDataForDisplay();
                    return true;
                }
            }

            return false;
        }

        public static List<AccountDayDataView> SectorData(List<Sector> sectors, NameData name, ErrorReports reports)
        {
            foreach (Sector sec in sectors)
            {
                if (sec.GetName() == name.Name)
                {
                    return sec.GetDataForDisplay();
                }
            }
            reports.AddError($"Sector {name.Name} does not exist.", Location.DatabaseAccess);
            return new List<AccountDayDataView>();
        }

        public static bool TryAddDataToSector(List<Sector> sectors, NameData name, AccountDayDataView value, ErrorReports reports)
        {
            return TryAddDataToSector(sectors, name.Name, value.Date, value.Amount, reports);
        }

        /// <summary>
        /// Attempts to add data to the sector. Fails if data already exists
        /// </summary>
        public static bool TryAddDataToSector(List<Sector> sectors, string name, DateTime date, double value, ErrorReports reports)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    return sector.TryAddData(date, value, reports);
                }
            }
            reports.AddError($"Could not find sector {name}", Location.AddingData);
            return false;
        }

        public static bool TryEditSector(List<Sector> sectors, NameData name, AccountDayDataView oldData, AccountDayDataView newData, ErrorReports reports)
        {
            return TryEditSector(sectors, name.Name, oldData.Date, newData.Date, newData.Amount, reports);
        }

        public static bool TryEditSector(List<Sector> sectors, string name, DateTime oldDate, DateTime date, double value, ErrorReports reports)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    return sector.TryEditData(oldDate, date, value, reports);
                }
            }

            return false;
        }

        public static bool TryDeleteSectorData(List<Sector> sectors, NameData name, AccountDayDataView value, ErrorReports reports)
        {
            return TryDeleteSectorData(sectors, name.Name, value.Date, value.Amount, reports);
        }

        public static bool TryDeleteSectorData(List<Sector> sectors, string name, DateTime date, double value, ErrorReports reports)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    return sector.TryDeleteData(date, value, reports);
                }
            }

            return false;
        }

        public static bool TryEditSectorName(List<Sector> sectors, NameData oldName, NameData newName, ErrorReports reports)
        { 
            return TryEditSectorName(sectors, oldName.Name, newName.Name, newName.Url, reports); 
        }

        public static bool TryEditSectorName(List<Sector> sectors, string oldName, string newName, string url, ErrorReports reports)
        {
            foreach (var sector in sectors)
            {
                if (sector.GetName() == oldName)
                {
                    sector.TryEditNameUrl(newName, url);
                    reports.AddReport($"Renamed sector {oldName} with new name {newName}.", Location.EditingData);
                    return true;
                }
            }

            reports.AddError($"Could not rename sector {oldName} with new name {newName}.", Location.EditingData);
            return false;
        }

        /// <summary>
        /// Returns true if sector with given name exists.
        /// </summary>
        public static bool DoesSectorExist(List<Sector> sectors, string name)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryDeleteSector(List<Sector> sectors, NameData name, ErrorReports reports)
        { 
            return TryDeleteSector(sectors, name.Name, reports);
        }

        /// <summary>
        /// Deletes sector if sector exists. Does nothing otherwise.
        /// </summary>
        public static bool TryDeleteSector(List<Sector> sectors, string name, ErrorReports reports)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    reports.AddReport($"Removed sector {name}", Location.DeletingData);
                    sectors.Remove(sector);
                    return true;
                }
            }

            return false;
        }
    }
}
