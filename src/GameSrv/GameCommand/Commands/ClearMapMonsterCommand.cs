﻿using GameSrv.Actor;
using GameSrv.Maps;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 清楚指定地图怪物
    /// </summary>
    [Command("ClearMapMonster", "清楚指定地图怪物", "地图号(* 为所有) 怪物名称(* 为所有) 掉物品(0,1)", 10)]
    public class ClearMapMonsterCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            string sMapName = @params.Length > 0 ? @params[0] : "";
            string sMonName = @params.Length > 1 ? @params[1] : "";
            string sItems = @params.Length > 2 ? @params[2] : "";
            if (string.IsNullOrEmpty(sMapName) || string.IsNullOrEmpty(sMonName) || string.IsNullOrEmpty(sItems))
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            bool boKillAll = false;
            bool boKillAllMap = false;
            bool boNotItem = true;
            int nMonCount = 0;
            Envirnoment envir = null;
            if (sMonName == "*")
            {
                boKillAll = true;
            }
            if (sMapName == "*")
            {
                boKillAllMap = true;
            }
            if (sItems == "1")
            {
                boNotItem = false;
            }
            IList<BaseObject> monList = new List<BaseObject>();
            for (int i = 0; i < M2Share.MapMgr.Maps.Count; i++)
            {
                envir = M2Share.MapMgr.Maps[i];
                if (envir != null)
                {
                    if (boKillAllMap || string.Compare(envir.MapName, sMapName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        int monsterCount = M2Share.WorldEngine.GetMapMonster(envir, monList);
                        if (monsterCount > 0)
                        {
                            for (int j = 0; j < monsterCount; j++)
                            {
                                BaseObject baseObject = monList[j];
                                if (baseObject != null)
                                {
                                    if (baseObject.Master != null && baseObject.Race != 135)// 除135怪外，其它宝宝不清除
                                    {
                                        if (baseObject.Master.Race == ActorRace.Play)
                                        {
                                            continue;
                                        }
                                    }
                                    if (boKillAll || string.Compare(sMonName, baseObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        baseObject.NoItem = boNotItem;
                                        baseObject.WAbil.HP = 0;
                                        nMonCount++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (envir == null)
            {
                playObject.SysMsg("输入的地图不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.SysMsg("已清除怪物数: " + nMonCount, MsgColor.Red, MsgType.Hint);
        }
    }
}