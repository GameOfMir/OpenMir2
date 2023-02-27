﻿using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 进入/退出隐身模式(进入模式后别人看不到自己)(支持权限分配)
    /// </summary>
    [Command("ChangeObMode", "进入/退出隐身模式(进入模式后别人看不到自己)", 10)]
    public class ChangeObModeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            bool boFlag = !PlayObject.ObMode;
            if (boFlag) {
                PlayObject.SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");// 发送刷新数据到客户端，解决隐身有影子问题
            }
            PlayObject.ObMode = boFlag;
            if (PlayObject.ObMode) {
                PlayObject.SysMsg(Settings.ObserverMode, MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayObject.SysMsg(Settings.ReleaseObserverMode, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}