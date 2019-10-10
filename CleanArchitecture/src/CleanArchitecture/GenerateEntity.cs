using System.Collections.Generic;
using System.ComponentModel.Composition;
using ZeraSystems.CodeNanite.Expansion;
using ZeraSystems.CodeStencil.Contracts;

namespace ZeraSystems.CodeNanite.CleanArchitecture
{
    /// <summary>
    /// There are 10 elements in thHhe String Array used by the
    /// 0  - This is the name of the publisher
    /// 1  - This is the title of the Code Nanite
    /// 2  - This is the description
    /// 3  - Version Number
    /// 4  - Label of the Code Nanite
    /// 5  - Namespace
    /// 6  - Release Date
    /// 7  - Name to use for Expander Label
    /// 9  - RESERVED
    /// 10 - RESERVED
    /// </summary>
    [Export(typeof(ICodeStencilCodeNanite))]
    [CodeStencilCodeNanite(new[]
    {
        "Zera Systems Inc.",
        "Entity Generator",
        "This Nanite generates Models/Entities from the Schema attached to this stencil."+
        "It inherits from the BaseEntity Abstract class, therefore the primary key does not have to be defined or generated in this class",
        "1.0",
        "GenerateEntity",
        "ZeraSystems.CodeNanite.CleanArchitecture",
        "09/01/2019",
        "CA_GENERATE_ENTITY",
        "1",
        "",
        ""
    })]
    public partial class GenerateEntity : ExpansionBase, ICodeStencilCodeNanite
    {
        public string Input { get; set; }
        public string Output { get; set; }
        public int Counter { get; set; }
        public List<string> OutputList { get; set; }
        public List<ISchemaItem> SchemaItem { get; set; }
        public List<IExpander> Expander { get; set; }
        public List<string> InputList { get; set; }

        //public string ErrorMessage { get; set; }

        public void ExecutePlugin()
        {
            OutputList = new List<string>();
            if (Input == null)
            {
                //OutputList.Add("**ERROR : Input = null, Missing Table Name");
                return;
            }

            Initializer(SchemaItem, Expander);
            MainFunction();
            Output = ExpandedText.ToString();
        }
    }
}