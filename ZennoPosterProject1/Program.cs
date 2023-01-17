using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Resources;
using System.Text;
using System.Threading;
using ZennoLab.CommandCenter;
using ZennoLab.Emulation;
using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoLab.InterfacesLibrary.ProjectModel.Enums;
using ZennoPosterEmulation;
using UserParser;
using ZennoPosterProject1.Parsers.ParserUsers;

namespace ZennoPosterProject1
{
    /// <summary>
    /// Класс для запуска выполнения скрипта
    /// </summary>
    public class Program : IZennoExternalCode
    {
        /// <summary>
        /// Метод для запуска выполнения скрипта
        /// </summary>
        /// <param name="instance">Объект инстанса выделеный для данного скрипта</param>
        /// <param name="project">Объект проекта выделеный для данного скрипта</param>
        /// <returns>Код выполнения скрипта</returns>		
        public int Execute(Instance instance, IZennoPosterProjectModel project)
        {
            new UsersParser(project).StartListGenerator();












            //SwipeAndClick swipeAndClick = new SwipeAndClick(instance, project);
            //var path = project.Directory + @"/InputFile/Links.txt"; // Путь к произвольному текстовому файлу
            //var myList = File.ReadAllLines(path);





            //foreach (var VARIABLE in myList)
            //{
            //    instance.ActiveTab.Navigate(VARIABLE);
            //    instance.ActiveTab.WaitDownloading();
            //    Thread.Sleep(2000);
            //    HtmlElement he =
            //        instance.ActiveTab.FindElementByXPath("//button[contains(@class, 'freeVoteButton')]", 0);
            //    HtmlElement he2 = instance.ActiveTab.FindElementByXPath("//div[contains(@class, 'modelName')]", 0);

            //    if (he.InnerHtml.Contains("Проголосовали"))
            //    {
            //        project.SendInfoToLog("Уже голосовали за этого!");
            //    }
            //    else
            //    {
            //        swipeAndClick.SwipeAndClickToElement(he);
            //        swipeAndClick.SwipeAndClickToElement(he2);
            //        Thread.Sleep(3000);
            //        HtmlElement he3 = instance.ActiveTab.FindElementByXPath("//a[contains(@class, 'actionBtn sendMsgBtn updatedStyledBtn')]", 0);
            //        swipeAndClick.SwipeAndClickToElement(he3);
            //        instance.ActiveTab.WaitDownloading();
            //        Thread.Sleep(3000);
            //        HtmlElement he4 = instance.ActiveTab.FindElementByXPath("//textarea[@id='postMsgInput']", 0);
            //        HtmlElement he5 = instance.ActiveTab.FindElementByXPath("//button[@id='postButton']", 0);
            //        swipeAndClick.SetText(he4,"Сообщение",true);
            //        swipeAndClick.SwipeAndClickToElement(he5);

            //    }
            //}
            return 0;
        }
    }
}