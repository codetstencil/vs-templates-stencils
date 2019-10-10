using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeraSystems.CodeNanite.CleanArchitecture
{
    public partial class GetDbSet
    {
        private void MainFunction() => Generate();

        private void Generate()
        {
            //AppendText();
            var thisTable = GetTables();
            foreach (var item in thisTable)
            {
                AppendText( Indent(8) + "public DbSet<" + Singularize(item.TableName) + "> " + Pluralize(item.TableName) + " { get; set; }");
            }

        }

    }
}
