using System;
using System.Collections.Generic;
using System.Text;

namespace BaseCore.UI.Controls.KCore.Form
{
    public class MvcFormButtons : MvcBaseControl
    {
        internal override string TagName { get { return "div"; } }
        internal override bool SelfClosing { get { return false; } }
        internal FormButtonsOptions Options { get; private set; }
        public MvcFormButtons(ControlFactory controlFactory, string callBack, bool showCancel , bool showSave) : base(controlFactory)
        {
            Options = new FormButtonsOptions();
            Options.CallBack = callBack;
            Options.ShowCancel = showCancel;
            Options.showSave = showSave;
        }

        internal override MvcControlAttributes GetAttributes()
        {
            return Options;
        }

        internal override string GetContent()
        {
            var res = $@"<hr />
                     <div style='text-align:left'>"                       ;
            if (Options.showSave)
                res += $@"<button 
                          data-submit='ajax'{(!string.IsNullOrEmpty(Options.CallBack)? $"data-callback='{ Options.CallBack }'" : "") } 
                          class='k-button k-button-icontext k-primary k-grid-update' type='submit'>
                          <span class='k-icon k-i-check'></span>ثبت
                            </button>";

            if (Options.ShowCancel)
                res += @"
                         <button type='button' class='k-button k-button-icontext k-grid-cancel'>
                            <span class='k-icon k-i-cancel'></span>لغو
                        </button>";

            return res + "</div>";
        }
    }
}
