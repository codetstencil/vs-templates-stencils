using System.Collections.Generic;
using ZeraSystems.CodeNanite.Expansion;

using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.CleanArchitecture
{
    public partial class GenerateEntity
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
            BuildSnippet(null,4);
            BuildSnippet(_public + "class " + _classname+ " : BaseEntity", 4);
            BuildSnippet("{", 4);
            AppendText(GetColumns(), string.Empty); // This is not to allow line feed
            BuildSnippet("}", 4);
            AppendText(BuildSnippet(), string.Empty);
        }

        private string GetColumns()
        {
            foreach (var item in _columns)
                BuildSnippet(_public + item.ColumnType + GetNullSign(item) + " " + item.ColumnName + _getSet);

            BuildSnippet("");
            GetForeignKeyColumns();
            GetRelatedColumns();
            return BuildSnippet();
        }

        private void GetForeignKeyColumns()
        {
            foreach (var item in _foreignKeys)
            {
                if (item.TableName == _table && item.RelatedTable == _table) //A table related to itself
                    BuildSnippet(_public + GetTableString(item.RelatedTable) + " " + item.ColumnName + NavigationLabel() + _getSet);
                else if (item.RelatedTable == _table)
                    BuildSnippet(_public + GetTableString(item.TableName) + " " + item.TableName + _getSet);
                else
                    BuildSnippet(_public + GetTableString(item.RelatedTable) + " " + item.RelatedTable + _getSet);
            }
        }

        private void GetRelatedColumns()
        {
            foreach (var item in _relatedColumns)
            {
                var relatedTable = GetTableString(item.TableName);
                if (IsPrimaryInRelated(_table, relatedTable.Singularize()))
                    continue;
                BuildSnippet(_public + "ICollection<" + relatedTable + "> " + relatedTable.Pluralize() + _getSet +
                             " = new HashSet<" + relatedTable + ">();");
            }
        }

        private string GetTableString(string table)
        {
            return GetExpansionString("MODEL_PREFIX") + table + GetExpansionString("MODEL_SUFFIX");
        }
    }
}