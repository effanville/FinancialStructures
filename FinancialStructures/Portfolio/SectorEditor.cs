using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
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
                    outputs.Add(new NameData(string.Empty, thing.GetName(), string.Empty, thing.GetUrl(), false));
                }
            }
            return outputs;
        }

        public static bool TryAddSector(List<Sector> sectors, NameData name, Action<string, string, string> reportLogger)
        {
            return TryAddSector(sectors, name.Name, name.Url, reportLogger);
        }
        /// <summary>
        /// Tries to add a sector to the underlying global database
        /// </summary>
        public static bool TryAddSector(List<Sector> sectors, string name, string url, Action<string, string, string> reportLogger)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    reportLogger("Error", "AddingData", $"Sector with name {name} already exists.");
                    return false;
                }
            }
            Sector newSector = new Sector(name, url);
            reportLogger("Report", "AddingData", $"Added sector with name {name}.");
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

        public static bool TryGetSectorData(List<Sector> sectors, string name, out List<DayValue_ChangeLogged> data)
        {
            data = new List<DayValue_ChangeLogged>();
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

        public static List<DayValue_ChangeLogged> SectorData(List<Sector> sectors, NameData name, Action<string, string, string> reportLogger)
        {
            foreach (Sector sec in sectors)
            {
                if (sec.GetName() == name.Name)
                {
                    return sec.GetDataForDisplay();
                }
            }
            reportLogger("Error", "DatabaseAccess", $"Sector {name.Name} does not exist.");
            return new List<DayValue_ChangeLogged>();
        }

        public static bool TryAddDataToSector(List<Sector> sectors, NameData name, DayValue_ChangeLogged value, Action<string, string, string> reportLogger)
        {
            return TryAddDataToSector(sectors, name.Name, value.Day, value.Value, reportLogger);
        }

        /// <summary>
        /// Attempts to add data to the sector. Fails if data already exists
        /// </summary>
        public static bool TryAddDataToSector(List<Sector> sectors, string name, DateTime date, double value, Action<string, string, string> reportLogger)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    return sector.TryAddData(date, value, reportLogger);
                }
            }
            reportLogger("Error", "AddingData", $"Could not find sector {name}");
            return false;
        }

        public static bool TryEditSector(List<Sector> sectors, NameData name, DayValue_ChangeLogged oldData, DayValue_ChangeLogged newData, Action<string, string, string> reportLogger)
        {
            return TryEditSector(sectors, name.Name, oldData.Day, newData.Day, newData.Value, reportLogger);
        }

        public static bool TryEditSector(List<Sector> sectors, string name, DateTime oldDate, DateTime date, double value, Action<string, string, string> reportLogger)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    return sector.TryEditData(oldDate, date, value, reportLogger);
                }
            }

            return false;
        }

        public static bool TryDeleteSectorData(List<Sector> sectors, NameData name, DayValue_ChangeLogged value, Action<string, string, string> reportLogger)
        {
            return TryDeleteSectorData(sectors, name.Name, value.Day, value.Value, reportLogger);
        }

        public static bool TryDeleteSectorData(List<Sector> sectors, string name, DateTime date, double value, Action<string, string, string> reportLogger)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    return sector.TryDeleteData(date, reportLogger);
                }
            }

            return false;
        }

        public static bool TryEditSectorName(List<Sector> sectors, NameData oldName, NameData newName, Action<string, string, string> reportLogger)
        {
            return TryEditSectorName(sectors, oldName.Name, newName.Name, newName.Url, reportLogger);
        }

        public static bool TryEditSectorName(List<Sector> sectors, string oldName, string newName, string url, Action<string, string, string> reportLogger)
        {
            foreach (var sector in sectors)
            {
                if (sector.GetName() == oldName)
                {
                    sector.EditNameData(string.Empty, newName, url);
                    reportLogger("Report", "EditingData", $"Renamed sector {oldName} with new name {newName}.");
                    return true;
                }
            }

            reportLogger("Error", "EditingData", $"Could not rename sector {oldName} with new name {newName}.");
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

        public static bool TryDeleteSector(List<Sector> sectors, NameData name, Action<string, string, string> reportLogger)
        {
            return TryDeleteSector(sectors, name.Name, reportLogger);
        }

        /// <summary>
        /// Deletes sector if sector exists. Does nothing otherwise.
        /// </summary>
        public static bool TryDeleteSector(List<Sector> sectors, string name, Action<string, string, string> reportLogger)
        {
            foreach (var sector in sectors)
            {
                if (name == sector.GetName())
                {
                    reportLogger("Report", "DeletingData", $"Removed sector {name}");
                    sectors.Remove(sector);
                    return true;
                }
            }

            return false;
        }
    }
}
