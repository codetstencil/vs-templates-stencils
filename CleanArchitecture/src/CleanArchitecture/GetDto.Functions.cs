using System.Collections.Generic;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.CleanArchitecture
{
    public partial class GetDto
    {
        private string _public = "public ";
        private string _getSet = " { get; set; }";
        private string _classname;
        private string _table;
        private List<ISchemaItem> _columns;
        private List<ISchemaItem> _relatedColumns;
        private List<ISchemaItem> _foreignKeys;

        private void MainFunction()
        {
            _table = Input.Singularize();
            _columns = GetColumns(Input, false);
            _relatedColumns = GetRelatedTables(Input);
            _foreignKeys = GetForeignKeysInTable(Input, true);
            _classname = Singularize(Input) + GetExpansionString("MODEL_SUFFIX") + " ";
            AppendText();
            BuildSnippet(null);
            BuildSnippet(_public + "class " + _classname + "DTO", 4);
            BuildSnippet("{", 4);
            AppendText(GetColumns(), string.Empty); // This is not to allow line feed
            AppendText(AssignDtoColumns(), string.Empty); // This is not to allow line feed
            BuildSnippet("}", 4);
            AppendText(BuildSnippet(), string.Empty);
        }
        private string GetColumns()
        {
            foreach (var item in _columns)
                BuildSnippet(_public + item.ColumnType + GetNullSign(item) + " " + item.ColumnName + _getSet);
            BuildSnippet("");
            return BuildSnippet();
        }

        private string AssignDtoColumns()
        {
            BuildSnippet(_public + "static " + _table + "DTO From" + _table + "(" + _table + " column)", 8);
            BuildSnippet("{", 8);
            BuildSnippet("return new " + _table + "DTO()", 12);
            BuildSnippet("{", 12);

            var count = 0;
            var comma = ",";
            foreach (var item in _columns)
            {
                count++;
                if (count == _columns.Count) comma = string.Empty;
                BuildSnippet(item.ColumnName + " = column." + item.ColumnName + comma, 16);
            }
            BuildSnippet("}", 12);
            BuildSnippet("}", 8);
            return BuildSnippet();
        }

    }
}