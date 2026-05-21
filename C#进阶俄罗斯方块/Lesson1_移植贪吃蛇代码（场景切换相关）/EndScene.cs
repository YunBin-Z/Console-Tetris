using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_进阶俄罗斯方块;

 class EndScene:BeginOrEndBaseScene
{
     public EndScene()
    {
        strTitle = "游戏结束";
        strOne = "重新开始游戏";
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