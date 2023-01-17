using System;
using ZennoLab.CommandCenter;
using ZennoLab.InterfacesLibrary.ProjectModel;
using System.Threading;
using ZennoPosterProject1;



namespace ZennoPosterEmulation
{
    class SwipeAndClick
    {
        private int ElementPosition { get; set; }
        private int InstanceHeight { get; set; }
        public int LatencyKeySetText { get { return EmulationValue.LatencyKey.ParseRangeValueInt().ValueRandom; } }
        readonly Instance instance;
        readonly IZennoPosterProjectModel project;
        CreateTouchAndSwipeParametr CreatTuchParametrs;

        public SwipeAndClick(Instance instance, IZennoPosterProjectModel project)
        {
            this.instance = instance;
            this.project = project;
            this.CreatTuchParametrs = new CreateTouchAndSwipeParametr(project);
        }

        public void SetText(HtmlElement HtmlElem, string text, bool True_SwipeAndClick_False_Click)
        {
            project.SendInfoToLog("Вводим текст: " + text);

            try
            {
                if (True_SwipeAndClick_False_Click)
                {
                    SwipeAndClickToElement(HtmlElem);
                }
                else
                {
                    ClickToElement(HtmlElem);
                }

                char[] InputText = text.ToCharArray();

                foreach (char InputChar in InputText)
                {
                    instance.SendText(Convert.ToString(InputChar), 0);
                    Thread.Sleep(LatencyKeySetText);
                }



            }
            catch (Exception ex)
            {
                project.SendWarningToLog("Ввод текста не был совершен так как не был сделан свайп или клик. " + ex.Message);
            }
        }//Ввод текста
        public void SwipeAndClickToElement(HtmlElement HtmlElem)
        {

            try
            {
                SwipeToElement(HtmlElem);
                ClickToElement(HtmlElem);
            }

            catch (Exception ex)
            {
                project.SendWarningToLog("Не удалось сделать свайп или клик: " + ex.Message, true);
            }
        }//Свайп до элемента и клик по нему       
        public void SwipeToElement(HtmlElement HtmlElem)
        {
            instance.ActiveTab.Touch.SetTouchEmulationParameters(CreatTuchParametrs.CreateTouchParametrs());
            int CounterAttemptSwipe = 0;

            do
            {
SwipeMoreTime:
            Thread.Sleep(1000);
            if (CounterAttemptSwipe == 10)
                {
                    break;
                }

                instance.ActiveTab.Touch.SwipeIntoView(HtmlElem);

                if (String.IsNullOrEmpty(HtmlElem.GetAttribute("topInTab")))
                {
                    CounterAttemptSwipe++;
goto SwipeMoreTime;
                }

                ElementPosition = Convert.ToInt32(HtmlElem.GetAttribute("topInTab"));
                InstanceHeight = Convert.ToInt32(instance.ActiveTab.MainDocument.EvaluateScript("return window.innerHeight"));
                CounterAttemptSwipe++;
            }
            while (ElementPosition > InstanceHeight || ElementPosition < 0);

        }//Свайп до элемента
        public void ClickToElement(HtmlElement HtmlElem)
        {

            if (HtmlElem.IsVoid)
            {
                project.SendErrorToLog("HtmlElement элемент для клика не найден.", true);
            }

            instance.ActiveTab.Touch.SetTouchEmulationParameters(CreatTuchParametrs.CreateTouchParametrs());
            instance.ActiveTab.Touch.Touch(HtmlElem);
        }//Клик по элементу



        public void LongTuch(HtmlElement HtmlElem)
        {

            if (HtmlElem.IsVoid)
            {
                project.SendErrorToLog("HtmlElement элемент для лонгтача не найден.", true);
            }

            instance.ActiveTab.Touch.SetTouchEmulationParameters(CreatTuchParametrs.CreateTouchParametrs());
            instance.ActiveTab.Touch.LongTouch(HtmlElem);
        }//Клик по элементу






    }
}