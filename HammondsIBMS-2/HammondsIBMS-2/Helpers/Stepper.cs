using System;
using System.Diagnostics;
using System.Web;
using HammondsIBMS_2.ViewModels.Stepper;

namespace HammondsIBMS_2.Helpers
{
    public static class Stepper
    {
        private static StepperVM StepperStore 
        {
            get
            {
                var store=HttpContext.Current.Session["_viewPager_"] as StepperVM;
                return store ?? new StepperVM();
            }
            set { HttpContext.Current.Session["_viewPager_"] = value; }
        }

        public static StepperVM StepperModel
        {
            get
            {
                return StepperStore;
            }
        }

        public static void CreatePageStepper(int noOfSteps,string actionOnCancel,string header)
        {
            CreateStepper(noOfSteps, actionOnCancel, header, false, "");
        }

        [Obsolete("this is feature still requires further development")]
        public static void CreateAjaxStepper(int noOfSteps,string actionOnCancel,string header,string updateTargetId)
        {
            throw new Exception("This method should not be called");
            CreateStepper(noOfSteps, actionOnCancel, header, true, updateTargetId);
        }

        private static void CreateStepper(int noOfSteps, string actionOnCancel, string header, bool isAjax,string updateTargetId)
        {
            StepperStore = new StepperVM
                {
                    NoOfSteps = noOfSteps,
                    ActionOnCancel = actionOnCancel,
                    Header = header,
                    IsAjax = isAjax,
                    UpdateTargetId = updateTargetId
                };
        }


        public static void Step(int stepNo,string previousStepAction=null)
        {
            var stepper = StepperStore;
            stepper.CurrentStep = stepNo;
            stepper.PreviousStepAction = previousStepAction;
        }

        public static string Header
        {
            get{return StepperModel.Header;}
            set { StepperModel.Header = value; }
        }
        
    }
}