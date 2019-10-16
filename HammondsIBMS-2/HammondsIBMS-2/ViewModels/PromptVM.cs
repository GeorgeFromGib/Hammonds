using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Telerik.Web.Mvc.Infrastructure;

namespace HammondsIBMS_2.ViewModels
{
    public class PromptVM
    {
        [Flags]
        public enum DialogButtons
        {
            Ok=1,
            Cancel=2,
            ConfirmOk=4,
            All=255
        }

        [Key]
        public int RecordIndex { get; set; }
        public int SecondIndex { get; set; }
        public List<DialogPrompt> Prompts { get; set; }
        public DialogButtons Buttons { get; set; }

        public PromptVM()
        {
            Buttons = DialogButtons.Ok | DialogButtons.Cancel;
            Prompts=new List<DialogPrompt>();
        }

        public PromptVM(int recordIndex, DialogPrompt dialogPrompt) : this()
        {
            RecordIndex = recordIndex;
            Prompts = new List<DialogPrompt>{dialogPrompt};
        }

        public PromptVM AddPrompt(DialogPrompt dialogPrompt)
        {
            Prompts = Prompts ?? new List<DialogPrompt>();
            Prompts.Add(dialogPrompt);
            return this;
        }

        public PromptVM SetButtonOptions(DialogButtons dialogButtons)
        {
            Buttons = dialogButtons;
            return this;
        }


    }

    public class DialogPrompt
    {
        public PromptMessageAlertLevel AlertLevel { get; set; }
        public string Message { get; set; }

        public DialogPrompt(string message, PromptMessageAlertLevel alertLevel=PromptMessageAlertLevel.Warning)
        {
            AlertLevel = alertLevel;
            Message = message;
        }
    }

    public enum PromptMessageAlertLevel
    {
        Info,
        Warning,
        Danger
    }
}