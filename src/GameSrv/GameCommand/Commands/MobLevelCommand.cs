﻿using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 显示你屏幕上你近处所有怪与人的详细情况
    /// </summary>
    [Command("MobLevel", "显示你屏幕上你近处所有怪与人的详细情况", 10)]
    public class MobLevelCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            PlayObject.Envir.GetRangeBaseObject(PlayObject.CurrX, PlayObject.CurrY, 2, true, BaseObjectList);
            for (int i = 0; i < BaseObjectList.Count; i++) {
                PlayObject.SysMsg(BaseObjectList[i].GetBaseObjectInfo(), MsgColor.Green, MsgType.Hint);
            }
            BaseObjectList.Clear();
        }
    }
}