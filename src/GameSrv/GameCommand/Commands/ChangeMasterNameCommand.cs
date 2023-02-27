﻿using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家师傅名称
    /// </summary>
    [Command("ChangeMasterName", "调整指定玩家师傅名称", "人物名称 师徒名称(如果为 无 则清除)", 10)]
    public class ChangeMasterNameCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sMasterName = @Params.Length > 1 ? @Params[1] : "";
            string sIsMaster = @Params.Length > 2 ? @Params[2] : "";
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sMasterName)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null) {
                if (string.Compare(sMasterName, "无", StringComparison.OrdinalIgnoreCase) == 0) {
                    m_PlayObject.MasterName = "";
                    m_PlayObject.RefShowName();
                    m_PlayObject.IsMaster = false;
                    PlayObject.SysMsg(sHumanName + " 的师徒名清除成功。", MsgColor.Green, MsgType.Hint);
                }
                else {
                    m_PlayObject.MasterName = sMasterName;
                    if (!string.IsNullOrEmpty(sIsMaster) && sIsMaster[0] == '1') {
                        m_PlayObject.IsMaster = true;
                    }
                    else {
                        m_PlayObject.IsMaster = false;
                    }
                    m_PlayObject.RefShowName();
                    PlayObject.SysMsg(sHumanName + " 的师徒名更改成功。", MsgColor.Green, MsgType.Hint);
                }
            }
            else {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}