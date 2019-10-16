using System;
using System.Collections.Generic;
using HammondsIBMS_2.Helpers;

namespace HammondsIBMS_2.ViewModels.Stepper
{
    public  class StepperVM2
    {
        public List<Step> Steps { get; set; }
        public string Header { get; set; }
        public int CurrentStepNo { get; set; }
        public int NoOfSteps
        {
            get { return Steps.Count; }
        }

        public Step CurrentStep
        {
            get
            {
                if(Steps.Count==0)
                    throw new ApplicationException("No steps defined");
                if(Steps.Count<CurrentStepNo)
                    throw new ApplicationException("Current step number is greater than number of existing steps");
                if (CurrentStepNo == 0)
                    CurrentStepNo = 1;
                return Steps[CurrentStepNo-1];
            }
        }

        public Step MoveToNextStep()
        {
            CurrentStepNo++;
            return CurrentStep;
        }

        public Step MoveToPreviousStep()
        {
            CurrentStepNo--;
            return CurrentStep;
        }

        public Step MoveToFirstStep()
        {
            CurrentStepNo = 1;
            return CurrentStep;
        }

        public Step MoveToStepNo(int stepNo)
        {
            CurrentStepNo = stepNo;
            return CurrentStep;
        }

        public string PreviousStepAction { get; set; }
        public string ActionOnCancel { get; set; }
        public bool IsAjax { get; set; }
        public string UpdateTargetId { get; set; }

        public StepperVM2()
        {
            CurrentStepNo = 0;
            Steps=new List<Step>();
        }

      
    }
}