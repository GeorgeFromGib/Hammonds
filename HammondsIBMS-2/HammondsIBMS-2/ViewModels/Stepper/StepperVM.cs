using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HammondsIBMS_2.ViewModels.Stepper
{
    public  class StepperVM
    {
        public string Header { get; set; }
        public int CurrentStep { get; set; }
        public int NoOfSteps { get; set; }
        public string PreviousStepAction { get; set; }
        public string ActionOnCancel { get; set; }
        public bool IsAjax { get; set; }
        public string UpdateTargetId { get; set; }
    }
}