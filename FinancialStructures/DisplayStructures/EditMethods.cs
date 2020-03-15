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
            if (functionInputs.Length < 1)
            {
                return null;
            }
            object output = null;
            var portfolio = (Portfolio)functionInputs[0];
            NameData name = functionInputs.Length > 2 ? (NameData)functionInputs[2] : null;

            switch (functionType)
            {
                case (FunctionType.Download):
                    {
                        if (functionInputs.Length != 3)
                        {
                            break;
                        }

                        await DownloadMethod(portfolio, (NameData)functionInputs[2], (Action<string, string, string>)functionInputs[3]).ConfigureAwait(false);
                        break;
                    }
                case (FunctionType.NameUpdate):
                    {
                        if (functionInputs.Length != 1)
                        {
                            break;
                        }

                        output = UpdateNameMethod(portfolio);
                        break;
                    }
                case (FunctionType.Create):
                    {
                        if (functionInputs.Length != 3)
                        {
                            break;
                        }
                        output = CreateMethod(portfolio, name, (Action<string, string, string>)functionInputs[3]);
                        break;
                    }
                case (FunctionType.Edit):
                    {
                        if (functionInputs.Length != 4)
                        {
                            break;

                        }
                        output = EditMethod(portfolio, name, (NameData)functionInputs[3], (Action<string, string, string>)functionInputs[4]);
                        break;
                    }
                case (FunctionType.Delete):
                    {
                        if (functionInputs.Length != 3)
                        {
                            break;
                        }
                        output = DeleteMethod(portfolio, name, (Action<string, string, string>)functionInputs[3]);
                        break;
                    }
                case (FunctionType.SelectData):
                    {
                        if (functionInputs.Length != 3)
                        {
                            break;
                        }
                        output = SelectedDataMethod(portfolio, name, (Action<string, string, string>)functionInputs[3]);
                        break;
                    }
                case (FunctionType.AddData):
                    {
                        if (functionInputs.Length != 4)
                        {
                            break;
                        }
                        output = AddDataMethod(portfolio, name, (DayValue_ChangeLogged)functionInputs[3], (Action<string, string, string>)functionInputs[4]);
                        break;
                    }
                case (FunctionType.EditData):
                    {
                        if (functionInputs.Length != 5)
                        {
                            break;
                        }
                        output = EditDataMethod(portfolio, name, (DayValue_ChangeLogged)functionInputs[3], (DayValue_ChangeLogged)functionInputs[4], (Action<string, string, string>)functionInputs[5]);
                        break;
                    }
                case (FunctionType.DeleteData):
                    {
                        if (functionInputs.Length != 4)
                        {
                            break;
                        }
                        output = DeleteDataMethod(portfolio, name, (DayValue_ChangeLogged)functionInputs[3], (Action<string, string, string>)functionInputs[4]);
                        break;
                    }
                default:
                    break;
            }
            return output;
        }

        private readonly Func<Portfolio, NameData, Action<string, string, string>, Task> DownloadMethod;
        private readonly Func<Portfolio, List<NameCompDate>> UpdateNameMethod;

        private readonly Func<Portfolio, NameData, Action<string, string, string>, bool> CreateMethod;

        private readonly Func<Portfolio, NameData, NameData, Action<string, string, string>, bool> EditMethod;

        private readonly Func<Portfolio, NameData, Action<string, string, string>, bool> DeleteMethod;

        private readonly Func<Portfolio, NameData, Action<string, string, string>, List<DayValue_ChangeLogged>> SelectedDataMethod;

        private readonly Func<Portfolio, NameData, DayValue_ChangeLogged, Action<string, string, string>, bool> AddDataMethod;

        private readonly Func<Portfolio, NameData, DayValue_ChangeLogged, DayValue_ChangeLogged, Action<string, string, string>, bool> EditDataMethod;

        private readonly Func<Portfolio, NameData, DayValue_ChangeLogged, Action<string, string, string>, bool> DeleteDataMethod;

        public EditMethods(
            Func<Portfolio, NameData, Action<string, string, string>, Task> downloadMethod,
            Func<Portfolio, List<NameCompDate>> updateNameMethod = null,
            Func<Portfolio, NameData, Action<string, string, string>, bool> createMethod = null,
            Func<Portfolio, NameData, NameData, Action<string, string, string>, bool> editMethod = null,
            Func<Portfolio, NameData, Action<string, string, string>, bool> deleteMethod = null,
            Func<Portfolio, NameData, Action<string, string, string>, List<DayValue_ChangeLogged>> selectedDataMethod = null,
            Func<Portfolio, NameData, DayValue_ChangeLogged, Action<string, string, string>, bool> addDataMethod = null,
            Func<Portfolio, NameData, DayValue_ChangeLogged, DayValue_ChangeLogged, Action<string, string, string>, bool> editDataMethod = null,
            Func<Portfolio, NameData, DayValue_ChangeLogged, Action<string, string, string>, bool> deleteDataMethod = null)
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
