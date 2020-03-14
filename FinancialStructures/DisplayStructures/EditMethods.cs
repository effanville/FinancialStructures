using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialStructures.GUIFinanceStructures
{
    public enum FunctionType
    {
        Download,
        NameUpdate,
        Create,
        Edit,
        Delete,
        SelectData,
        AddData,
        EditData,
        DeleteData
    }

    public class EditMethods
    {
        public async Task<object> ExecuteFunction(FunctionType functionType, params object[] functionInputs)
        {
            if (functionInputs.Length < 2)
            {
                return null;
            }
            object output = null;
            var portfolio = (Portfolio)functionInputs[0];
            var sectors = (List<Sector>)functionInputs[1];
            NameData name = functionInputs.Length > 2 ? (NameData)functionInputs[2] : null;

            switch (functionType)
            {
                case (FunctionType.Download):
                    {
                        if (functionInputs.Length != 4)
                        {
                            break;
                        }

                        await DownloadMethod(portfolio, sectors, (NameData)functionInputs[2], (Action<string, string, string>)functionInputs[3]).ConfigureAwait(false);
                        break;
                    }
                case (FunctionType.NameUpdate):
                    {
                        if (functionInputs.Length != 2)
                        {
                            break;
                        }

                        output = UpdateNameMethod(portfolio, sectors);
                        break;
                    }
                case (FunctionType.Create):
                    {
                        if (functionInputs.Length != 4)
                        {
                            break;
                        }
                        output = CreateMethod(portfolio, sectors, name, (Action<string, string, string>)functionInputs[3]);
                        break;
                    }
                case (FunctionType.Edit):
                    {
                        if (functionInputs.Length != 5)
                        {
                            break;

                        }
                        output = EditMethod(portfolio, sectors, name, (NameData)functionInputs[3], (Action<string, string, string>)functionInputs[4]);
                        break;
                    }
                case (FunctionType.Delete):
                    {
                        if (functionInputs.Length != 4)
                        {
                            break;
                        }
                        output = DeleteMethod(portfolio, sectors, name, (Action<string, string, string>)functionInputs[3]);
                        break;
                    }
                case (FunctionType.SelectData):
                    {
                        if (functionInputs.Length != 4)
                        {
                            break;
                        }
                        output = SelectedDataMethod(portfolio, sectors, name, (Action<string, string, string>)functionInputs[3]);
                        break;
                    }
                case (FunctionType.AddData):
                    {
                        if (functionInputs.Length != 5)
                        {
                            break;
                        }
                        output = AddDataMethod(portfolio, sectors, name, (DayValue_ChangeLogged)functionInputs[3], (Action<string, string, string>)functionInputs[4]);
                        break;
                    }
                case (FunctionType.EditData):
                    {
                        if (functionInputs.Length != 6)
                        {
                            break;
                        }
                        output = EditDataMethod(portfolio, sectors, name, (DayValue_ChangeLogged)functionInputs[3], (DayValue_ChangeLogged)functionInputs[4], (Action<string, string, string>)functionInputs[5]);
                        break;
                    }
                case (FunctionType.DeleteData):
                    {
                        if (functionInputs.Length != 5)
                        {
                            break;
                        }
                        output = DeleteDataMethod(portfolio, sectors, name, (DayValue_ChangeLogged)functionInputs[3], (Action<string, string, string>)functionInputs[4]);
                        break;
                    }
                default:
                    break;
            }
            return output;
        }

        private readonly Func<Portfolio, List<Sector>, NameData, Action<string, string, string>, Task> DownloadMethod;
        private readonly Func<Portfolio, List<Sector>, List<NameCompDate>> UpdateNameMethod;

        private readonly Func<Portfolio, List<Sector>, NameData, Action<string, string, string>, bool> CreateMethod;

        private readonly Func<Portfolio, List<Sector>, NameData, NameData, Action<string, string, string>, bool> EditMethod;

        private readonly Func<Portfolio, List<Sector>, NameData, Action<string, string, string>, bool> DeleteMethod;

        private readonly Func<Portfolio, List<Sector>, NameData, Action<string, string, string>, List<DayValue_ChangeLogged>> SelectedDataMethod;

        private readonly Func<Portfolio, List<Sector>, NameData, DayValue_ChangeLogged, Action<string, string, string>, bool> AddDataMethod;

        private readonly Func<Portfolio, List<Sector>, NameData, DayValue_ChangeLogged, DayValue_ChangeLogged, Action<string, string, string>, bool> EditDataMethod;

        private readonly Func<Portfolio, List<Sector>, NameData, DayValue_ChangeLogged, Action<string, string, string>, bool> DeleteDataMethod;

        public EditMethods(
            Func<Portfolio, List<Sector>, NameData, Action<string, string, string>, Task> downloadMethod,
            Func<Portfolio, List<Sector>, List<NameCompDate>> updateNameMethod = null,
            Func<Portfolio, List<Sector>, NameData, Action<string, string, string>, bool> createMethod = null,
            Func<Portfolio, List<Sector>, NameData, NameData, Action<string, string, string>, bool> editMethod = null,
            Func<Portfolio, List<Sector>, NameData, Action<string, string, string>, bool> deleteMethod = null,
            Func<Portfolio, List<Sector>, NameData, Action<string, string, string>, List<DayValue_ChangeLogged>> selectedDataMethod = null,
            Func<Portfolio, List<Sector>, NameData, DayValue_ChangeLogged, Action<string, string, string>, bool> addDataMethod = null,
            Func<Portfolio, List<Sector>, NameData, DayValue_ChangeLogged, DayValue_ChangeLogged, Action<string, string, string>, bool> editDataMethod = null,
            Func<Portfolio, List<Sector>, NameData, DayValue_ChangeLogged, Action<string, string, string>, bool> deleteDataMethod = null)
        {
            DownloadMethod = downloadMethod;
            UpdateNameMethod = updateNameMethod;
            CreateMethod = createMethod;
            EditMethod = editMethod;
            DeleteMethod = deleteMethod;
            SelectedDataMethod = selectedDataMethod;
            AddDataMethod = addDataMethod;
            EditDataMethod = editDataMethod;
            DeleteDataMethod = deleteDataMethod;
        }

        public EditMethods()
        { }
    }
}
