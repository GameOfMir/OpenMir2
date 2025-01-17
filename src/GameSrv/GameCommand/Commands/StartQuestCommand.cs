﻿using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("StartQuest", "", "问答名称", 10)]
    public class StartQuestCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sQuestName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sQuestName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            M2Share.WorldEngine.SendQuestMsg(sQuestName);
        }
    }
}