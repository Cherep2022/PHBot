using ZennoLab.InterfacesLibrary.ProjectModel;
using ZennoLab.CommandCenter;
using ZennoPosterEmulation;

namespace ZennoPosterProject1
{
    class InputSettings
    {
        readonly IZennoPosterProjectModel project;
        readonly Instance instance;
        public InputSettings(Instance instance, IZennoPosterProjectModel project)
        {
            this.instance = instance;
            this.project = project;
            InitializationInputValue();
        }

        public void InitializationInputValue()
        {
            project.SendInfoToLog("Инициализируем входные настройки");
            new EmulationValue(project);
        }//Считывание входных настроек
    }
}