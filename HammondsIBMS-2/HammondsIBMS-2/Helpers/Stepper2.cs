using System;
using System.Web;
using System.Web.Mvc;
using HammondsIBMS_2.ViewModels.Stepper;

namespace HammondsIBMS_2.Helpers
{
    public static class Stepper2
    {
        private static StepperVM2 StepperStore 
        {
            get
            {
                var store=HttpContext.Current.Session["_viewPager_"] as StepperVM2;
                return store ?? new StepperVM2();
            }
            set { HttpContext.Current.Session["_viewPager_"] = value; }
        }

        public static StepperVM2 StepperModel
        {
            get
            {
                return StepperStore;
            }
        }

        public static void CreatePageStepper(string actionOnCancel,string header)
        {
            CreateStepper(actionOnCancel, header, false, "");
        }

        [Obsolete("this is feature still requires further development")]
        public static void CreateAjaxStepper(int noOfSteps,string actionOnCancel,string header,string updateTargetId)
        {
            throw new Exception("This method should not be called");
            CreateStepper(actionOnCancel, header, true, updateTargetId);
        }

        private static void CreateStepper(string actionOnCancel, string header, bool isAjax,string updateTargetId)
        {
            StepperStore = new StepperVM2
                {
                    ActionOnCancel = actionOnCancel,
                    Header = header,
                    IsAjax = isAjax,
                    UpdateTargetId = updateTargetId
                };
        }

        public static void AddStep(string action,bool isLast=true)
        {
            StepperStore.Steps.Add(new Step{ActionName = action,IsLast = isLast});
        }

        //public static void AddStep(string action, string controller,bool isLast = true)
        //{
        //    StepperStore.Steps.Add(new Step { ActionName = action, Controller = controller,IsLast = isLast});
        //}

        public static string FirstStep()
        {
            return StepperStore.MoveToFirstStep().ActionName;
        }

        public static string  NextStep()
        {
            return StepperStore.MoveToNextStep().ActionName;
        }

        public static string PreviousStep()
        {
            return StepperStore.MoveToPreviousStep().ActionName;
        }

        public static string MoveToStep(int stepNo)
        {
            return StepperStore.MoveToStepNo(stepNo).ActionName;
        }

        public static void DeleteStep(int stepNo)
        {
            StepperStore.Steps.RemoveAt(stepNo-1);
        }

        public static void ClearSteps()
        {
            StepperStore.Steps.Clear();
        }

        public static string Header
        {
            get{return StepperModel.Header;}
            set { StepperModel.Header = value; }
        }


        
    }

    public class Step
    {
        public string ActionName { get; set; }
        public string Controller { get; set; }
        public bool IsLast { get; set; }
    }

}