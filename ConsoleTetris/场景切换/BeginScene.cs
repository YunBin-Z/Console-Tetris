using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTetris;

 class BeginScene:BeginOrEndBaseScene
{
    public BeginScene()
    {
        strTitle ="俄罗斯方块";
        strOne ="开始游戏";
    }
    public override void EnterDoSomething()
    {
        //按Enter键做什么的逻辑
        if (nowSelIndexl == 0)
        {
            Game.ChangeScene(E_SceneType.Gaming);
        }
        else
        {
            Environment.Exit(0);
        }
    }
}