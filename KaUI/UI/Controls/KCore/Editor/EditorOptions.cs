using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace KaUI.UI.Controls.KCore.Editor
{
    public class EditorOptions : MvcControlAttributes
    {
        public EditorOptions()
        {
            Tools = new List<Tool>();
            Array.ForEach(BaseTools, name => { Tools.Add(new Tool { Name = name }); });
            
            Tools.Add(new Tool
            {
                Name = "insertInlineImage",
                Tooltip = "درج تصویر",
                Exec = new JRaw("kendeHandlers.editorInsertInlineImage")
            });
            //Tools.Add(new Tool
            //{
            //    Name = "insertInlineFile",
            //    Tooltip = "درج فایل",
            //    Exec = new JRaw("kendeHandlers.editorInsertInlineFile")
            //});
        }

        private static string[] BaseTools = {
        "bold",
                    "italic",
                    "underline",
                    "undo",
                    "redo",
                    "justifyLeft",
                    "justifyCenter",
                    "justifyFull",
                    "justifyRight",
                    "insertUnorderedList",
                    "createLink",
                    "unlink",
                    "insertImage",
                    "tableWizard",
                    "createTable",
                    "addRowAbove",
                    "addRowBelow",
                    "addColumnLeft",
                    "addColumnRight",
                    "deleteRow",
                    "deleteColumn",
                    "mergeCellsHorizontally",
                    "mergeCellsVertically",
                    "splitCellHorizontally",
                    "splitCellVertically",
                    "formatting",
                   
                    "fontSize",
                    "foreColor",
                    "backColor",
        };
        internal List<Tool> Tools { get; set; }
        internal override string Role { get { return "editor"; } }

        internal override void GetAttributes(Dictionary<string, string> attributes)
        {
            attributes["role"] = Role;
            attributes["tools"] = JsonConvert.SerializeObject(Tools);
            attributes["stylesheets"] = "[\"/Kendo/css/Kendo.Editor.css\"]";
        }
    }
}
